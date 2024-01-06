using Microsoft.AspNetCore.Mvc;
using Qel.Graph.Dal;
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

        string? filePath = string.Empty;

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
                GraphEdges = new Dal.Entities.GraphEdgesCollection //TODO: Дописать и сделать одннобразными json'ы ходящие во фронтенде
                {
                    Edge = new Dal.Entities.Edge
                    {
                        FromNode = new Dal.Entities.Node
                        {
                            Color = "",
                            Text = "",
                            Label = "",
                            Shape = "",
                        },
                        ToNode = new Dal.Entities.Node
                        {
                            Color = "",
                            Text = "",
                            Label = "",
                            Shape = "",
                        },
                        Color = "",
                        Label = "",
                    }
                },
                IsDirected = entity!.IsDirected,
                CreationDateTime = DateTime.UtcNow,
                ModifyDateTime = DateTime.UtcNow,
            });
            await db.SaveChangesAsync();
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
        IActionResult? result;
        var parsedGuid = Guid.Parse(guid);
        try
        {
            var db = DbContextMain.CreateContext();

            var entity = await db.Files.FindAsync(parsedGuid);

            if (entity?.IsDeleted == false && entity != null) 
            {
                result = Json(new
                {
                    project_guid = entity?.Id,
                    edges = entity?.Edges,
                    text = entity?.Text,
                    edges_color = entity?.EdgesColor,
                    nodes_color = entity?.NodesColor,
                    directed = entity?.IsDirected,
                });
            }
            else
            {
                return Content(null!);
            }
            
        }
        catch
        {
            return Content(null!);
        }
        return result;
    }
}
