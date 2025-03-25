using Microsoft.EntityFrameworkCore;
using PastBeam.Infrastructure.DataBase;
using PastBeam.Core.Library.Interfaces;
using PastBeam.Infrastructure.Library.Repositories;
using PastBeam.Application.Library.Services;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ������������ ���� �����
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// ��������� ������ � ����������
builder.Services.AddScoped<PastBeam.Infrastructure.Library.Logger.ILogger, PastBeam.Infrastructure.Library.Logger.Logger>();

builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<ArticleService>();

builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<CourseService>();

// ������ MVC
builder.Services.AddControllersWithViews();

builder.Host.UseSerilog();


var app = builder.Build();

// ������������ middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// ������������� �� �������������
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();