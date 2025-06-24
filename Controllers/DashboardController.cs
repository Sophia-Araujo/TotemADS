using Microsoft.AspNetCore.Mvc;
using TotemPWA.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace TotemPWA.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {


            return View();
        }
    }
}
