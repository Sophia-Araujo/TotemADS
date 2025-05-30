using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TotemPWA.Models;

namespace TotemPWA.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult telaPedidos()
    {
        return View();
    }

    public IActionResult telaOpcoes()
    {
        return View();
    }

    public IActionResult carregamento()
    {
        return View();
    }


    public IActionResult telaCpf()
    {
        return View();
    }



     public IActionResult telaNome()
     {
        return View();
    }


    public IActionResult telaItensPedidos()
    {
        return View();
    }

    public IActionResult telaTipoPedido()
    {
        return View();
    }


    public IActionResult modalCupom()
    {
        return View();
    }

    public IActionResult Pagamentos()
      {
        return View();
    }





    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}