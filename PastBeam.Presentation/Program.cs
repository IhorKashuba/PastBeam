using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using PastBeam.Core.Interfaces;
using PastBeam.Infrastructure.Repositories;
using PastBeam.Application.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ������������ ���� �����
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// ��������� ������ � ����������
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<ArticleService>();

// ������ MVC
builder.Services.AddControllersWithViews();

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

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

app.Run();