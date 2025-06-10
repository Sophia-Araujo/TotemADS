using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Adiciona suporte a controllers com views (MVC)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware para servir arquivos estáticos (wwwroot, etc.)
app.UseStaticFiles();

// Middleware de roteamento
app.UseRouting();

// Define a rota padrão: HomeController -> Index()
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
