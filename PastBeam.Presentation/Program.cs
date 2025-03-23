using Microsoft.EntityFrameworkCore;
using PastBeam.Infrastructure.DataBase;
using PastBeam.Core.Library.Interfaces;
using PastBeam.Infrastructure.Library.Repositories;
using PastBeam.Application.Library.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Налаштування бази даних
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Реєстрація сервісів і репозиторіїв
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<ArticleService>();

// Додаємо MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Налаштування middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Маршрутизація за замовчуванням
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

app.Run();