using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using TotemPWA.Data;

var builder = WebApplication.CreateBuilder(args);

// Adiciona suporte a controllers com views (MVC)
builder.Services.AddControllersWithViews();

// Configuração do banco de dados SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SQLLiteConnection")));

// Configuração da autenticação por cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Administrador/Login";
        options.LogoutPath = "/Administrador/Logout";
        options.AccessDeniedPath = "/Administrador/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.Cookie.Name = "TotemPWAAuth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.SameSite = SameSiteMode.Lax;
    });

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Middleware para servir arquivos estáticos
app.UseStaticFiles();

// Middleware de roteamento
app.UseRouting();

// IMPORTANTE: A ordem é crucial!
// UseAuthentication deve vir ANTES de UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

// Define a rota padrão: HomeController -> Index()
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();