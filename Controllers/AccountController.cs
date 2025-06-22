// using Microsoft.AspNetCore.Mvc;
// using TotemPWA.Models;

// namespace TotemPWA.Controllers
// {
//     public class AdministradorController : Controller // Alterado de AccountController para AdministradorController
//     {
//         public IActionResult Login()
//         {
//             return View();
//         }

//         [HttpPost]
//         public IActionResult Login(LoginViewModel model)
//         {
//             if (ModelState.IsValid)
//             {
//                 if (model.Username == "admin" && model.Password == "123")
//                 {
//                     return RedirectToAction("Index", "Dashboard");
//                 }

//                 ModelState.AddModelError("", "Usuário ou senha inválidos");
//             }

//             return View(model);
//         }
//     }
// }