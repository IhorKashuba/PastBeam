using Microsoft.EntityFrameworkCore;
using PastBeam.Infrastructure.DataBase;
using PastBeam.Core.Library.Interfaces;
using PastBeam.Infrastructure.Library.Repositories;
using PastBeam.Application.Library.Services;
using Serilog;
using PastBeam.Application.Library.Interfaces;
using Microsoft.AspNetCore.Identity;
using PastBeam.Core.Library.Entities;
using Microsoft.EntityFrameworkCore.Design;

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

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

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
app.UseAuthentication();
app.UseAuthorization();

// Маршрутизація за замовчуванням
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//temp user
//delete when registration done

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.SeedTestUserAsync(services);
}

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var context = services.GetRequiredService<ApplicationDbContext>();

//    // Створення тестової статті
//    var testArticle = new Article
//    {
//        Title = "Test Article",
//        Content = "This is a test article content.",
//        CreatedAt = DateTime.UtcNow,
//        UpdatedAt = DateTime.UtcNow
//    };
//    context.Articles.Add(testArticle);
//    await context.SaveChangesAsync();
//}

//temp user
//delete when registration done

app.Run();