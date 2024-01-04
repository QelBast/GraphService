using Microsoft.AspNetCore.Mvc;
using Qel.Graph.Web.Processing;

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
    /// <returns>Путь к файлу или ошибку</returns>
    [HttpPost]
    public async Task<IActionResult> WorkWithInputJson(string inputJson)
    {
        var rabbit = new RabbitMqProvider();
        rabbit.Produce(inputJson, "jsonToGraph", "json", "amq.topic");

        string? filePath = null;

        while (filePath == null) 
        {
            filePath = await rabbit.Consume("graphToWeb");
        }
        var path = Path.Combine(_env.WebRootPath, @"imgs");
        Directory.CreateDirectory(path);

        string pathOnWww = Path.Combine(
            path, 
            Path.GetFileName(filePath));

        System.IO.File.Copy(filePath, pathOnWww);

        if(System.IO.File.Exists(pathOnWww))
        {
            return Content(pathOnWww);
        }
        else
        {
            return Content("Error!");
        }
    }
}
