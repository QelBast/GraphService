using Microsoft.AspNetCore.Mvc;
using Qel.Graph.Web.Processing;
using static System.Net.Mime.MediaTypeNames;

namespace Qel.Graph.Web.Controllers;

[Controller]
public class HomeController(ILogger<HomeController> logger, IWebHostEnvironment env) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    private readonly IWebHostEnvironment _env = env;
    public IActionResult Index()
    {
        return View();
    }

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
