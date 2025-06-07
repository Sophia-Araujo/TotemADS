using Microsoft.AspNetCore.Mvc;
using SeuProjeto.Models;
using System.Collections.Generic;

namespace SeuProjeto.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var menu = new List<MenuItem>
            {
                new MenuItem { Nome = "Tela inicial", Link = "#" },
                new MenuItem { Nome = "Tabelas", Link = "#" },
                new MenuItem { Nome = "Produtos do Sistema", Link = "#" },
                new MenuItem { Nome = "Novo produto", Link = "#" },
                new MenuItem { Nome = "Novo cupom", Link = "#" },
                new MenuItem { Nome = "Novo usuários", Link = "#" },
                new MenuItem { Nome = "Nova Tabela", Link = "#" }
            };

            return View(menu);
        }
    }
}
