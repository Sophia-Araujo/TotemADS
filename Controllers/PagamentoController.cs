using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TotemPWA.Models;
using TotemPWA.Data;

namespace TotemPWA.Controllers
{
    public class PagamentoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagamentoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PagamentoFinal()
        {
            return View();
        }

    }
}