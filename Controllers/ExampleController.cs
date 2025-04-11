using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TotemPWA.Models;

namespace TotemPWA.Controllers;

public class ExemploController : Controller
{
    private readonly ILogger<ExemploController> _logger;

    public ExemploController(ILogger<ExemploController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Receive(string name)
    {
        ViewBag.nome = name;
        return View();
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}