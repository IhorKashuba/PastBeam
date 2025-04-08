using Microsoft.EntityFrameworkCore;
using PastBeam.Infrastructure.DataBase;
using PastBeam.Core.Library.Interfaces;
using PastBeam.Infrastructure.Library.Repositories;
using PastBeam.Application.Library.Services;
using Serilog;
using PastBeam.Application.Library.Interfaces;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Налаштування бази даних
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Реєстрація сервісів і репозиторіїв
builder.Services.AddScoped<PastBeam.Infrastructure.Library.Logger.ILogger, PastBeam.Infrastructure.Library.Logger.Logger>();

builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IArticleService,ArticleService>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<CourseService>();

builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();

builder.Services.AddScoped<IBookmarkRepository, BookmarkRepository>();
builder.Services.AddScoped<IFolderRepository, FolderRepository>();
builder.Services.AddScoped<IUserCourseRepository, UserCourseRepository>();

// Додаємо MVC
builder.Services.AddControllersWithViews();

builder.Host.UseSerilog();


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

app.Run();