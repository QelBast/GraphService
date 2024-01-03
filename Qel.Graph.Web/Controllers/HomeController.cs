using Microsoft.AspNetCore.Mvc;
using Qel.Graph.Domain.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Qel.Graph.Web.Controllers;

[Controller]
public class HomeController(ILogger<HomeController> logger) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
    public IActionResult WorkWithInputJson([FromBody] CustomGraph inputJson)
    {
        //this.Request.Body.Read();
        if (ModelState.IsValid)
        {
            // Add user to server  

            return Json(new { success = true, message = "Operation was successful" });
        }
        else
        {
            return Json(new { success = false, message = "SOSNOOLEY" });
        }
        var test1 = inputJson.edges;
    }

    [HttpGet]
    public FileResult DowloadFile(string filePath) 
    {
        string path = filePath;
        byte[] fileBytes = System.IO.File.ReadAllBytes(path);
        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, path);
    }
}
