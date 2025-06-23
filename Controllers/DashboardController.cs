using Microsoft.AspNetCore.Mvc;
using TotemPWA.Models;
using System.Collections.Generic;

namespace TotemPWA.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {


            return View();
        }
    }
}
