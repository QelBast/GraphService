using Microsoft.AspNetCore.Mvc;
using Qel.Graph.Dal;
using Qel.Graph.Dal.Entities;
using Qel.Graph.Domain.Models;
using Qel.Graph.Web.Processing;
using System.Text.Json;

namespace Qel.Graph.Web.Controllers;

/// <summary>
/// Основной контроллер
/// </summary>
/// <param name="logger">Логгер</param>
/// <param name="env">Среда веб-хоста</param>
[Controller]
public class HomeController(ILogger<HomeController> logger, IWebHostEnvironment env) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    private readonly IWebHostEnvironment _env = env;

    /// <summary>
    /// Создание представления <see cref="Index"/> контроллером
    /// </summary>
    /// <returns>Представление <see cref="Index"/></returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Осуществляет передачу в модуль отрисовки Json-файла принимаемого от клиента, 
    /// получает от модуля путь к этому файлу и записывает его в корневую директорию статических файлов
    /// </summary>
    /// <param name="inputJson">Json-файл клиента</param>
    /// <returns>Путь к файлу или null</returns>
    [HttpPost]
    public async Task<IActionResult> WorkWithInputJson(string inputJson)
    {
        var work = new WorkService();

        var tcs = new TaskCompletionSource<string>();

        string? filePath;

        try
        {
            filePath = await work.Process(inputJson);
            tcs.TrySetResult(filePath);
        }
        catch (Exception ex)
        {
            tcs.TrySetException(ex);
        }

        var path = Path.Combine(_env.WebRootPath, @"imgs");
        Directory.CreateDirectory(path);

        filePath = tcs.Task.Result;
        var pathOnWww = Path.Combine(
            path,
            Path.GetFileName(filePath));

        if(System.IO.File.Exists(pathOnWww))
        {
            System.IO.File.Delete(pathOnWww);
        }
        System.IO.File.Move(filePath, pathOnWww, true);

        if (System.IO.File.Exists(pathOnWww))
        {
            return Content(Path.Combine("imgs", Path.GetFileName(pathOnWww)));
        }
        else
        {
            return Content(null!);
        }
    }

    /// <summary>
    ///  Осуществляет сохранение в базу данных созданного пользователем проекта
    /// </summary>
    /// <param name="saveJson">Данные с клиента о сохраняемом проекте</param>
    /// <returns>Ответ о статусе прошедшей операции или null</returns>
    [HttpPost]
    public async Task<IActionResult> WorkWithSaveJson(string saveJson)
    {
        try
        {
             var entity = JsonSerializer.Deserialize<Domain.File>(saveJson);

            var edgeCollection = new List<Edge>();
            foreach (var edge in entity!.Edges)
            {
                var edgeToDb = new Edge
                {
                    ToNode = new Node
                    {
                        Color = edge.To!.Color,
                        Label = edge.To.Label,
                        Shape = edge.To.Shape,
                        Text = edge.To.Text,
                    },
                    FromNode = new Node
                    {
                        Color = edge.From!.Color,
                        Label = edge.From.Label,
                        Shape = edge.From.Shape,
                        Text = edge.From.Text,
                    },
                    Color = edge.Color,
                    Label = edge.Label,
                    FileId = entity.Id
                };
                edgeCollection.Add(edgeToDb);
            }

            var db = DbContextMain.CreateContext();

            var existEntity = await db.Files.FindAsync(entity!.Id);
            if (existEntity != null) 
            { 
                db.Remove(existEntity);
            }

            await db.Files.AddAsync(new Dal.Entities.File
            {
                Id = entity!.Id,
                Text = entity!.Text,
                Edges = edgeCollection,
                IsDirected = entity!.IsDirected,
                CreationDateTime = DateTime.UtcNow,
                ModifyDateTime = DateTime.UtcNow,
            });
            await db.SaveChangesAsync();
    //    project_guid: "",
    //directed: "",
    //edges:
    //        [
    //    {
    //        color: "",
    //        label: "",
    //        from:
    //            {
    //            text: "",
    //            shape: "",
    //            label: "",
    //            color: ""
    //        },
    //        to:
    //            {
    //            text: "",
    //            shape: "",
    //            label: "",
    //            color: ""
    //        }
    //        }
    //]
    //not_edged_nodes:
    //        [
    //    {
    //        text: "",
    //        shape: "",
    //        label: "",
    //        color: ""
    //    }
    //]
        }
        catch
        {
            return Content(null!);
        }
        return Content("OK");
    }

    /// <summary>
    /// Загрузка файла из базы данных и передача на сторону клиента
    /// </summary>
    /// <returns>Json файл с данными о загруженном проекте или null</returns>
    [HttpPost]
    public async Task<IActionResult?> WorkWithLoadJson(string guid)
    {
        IActionResult? result = null;
        var parsedGuid = Guid.Parse(guid);
        try
        {
            var db = DbContextMain.CreateContext();

            var entity = await db.Files.FindAsync(parsedGuid);

            if (entity != null) 
            {
                var edgesCollection = new List<CustomEdge>();
                foreach (var edge in entity.Edges!)
                {
                    var edgeForLoad = new CustomEdge
                    {
                        From = new CustomNode 
                        {
                            Text = edge.FromNode!.Text,
                            Color = edge.FromNode.Color,
                            Label = edge.FromNode.Label,
                            Shape = edge.FromNode.Shape,
                        },
                        To = new CustomNode
                        {
                            Text = edge.ToNode!.Text,
                            Color = edge.ToNode.Color,
                            Label = edge.ToNode.Label,
                            Shape = edge.ToNode.Shape,
                        },
                        Color = edge.ToNode.Color,
                        Label = edge.Label!
                    };
                    edgesCollection.Add(edgeForLoad);
                }

                var finalEntity = new Domain.File
                {
                    Id = entity!.Id,
                    IsDeleted = entity.IsDeleted,
                    IsDirected = true,
                    Text = entity.Text,
                    Edges = edgesCollection
                };

                if (entity?.IsDeleted == false)
                {
                    result = Json(new
                    {
                        project_guid = entity?.Id,
                        edges = JsonSerializer.Serialize(entity!.Edges),
                        text = entity?.Text,
                        directed = entity?.IsDirected,
                    });
                }
                else
                {
                    return Content(null!);
                }
            }
        }
        catch (Exception ex)
        {
            throw;
            return Content(null!);
        }
        return result;
    }
}
