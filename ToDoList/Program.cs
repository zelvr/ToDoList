using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using Prometheus.SystemMetrics;
using Serilog;
using ToDoList;
using ToDoList.Lib;
using ToDoList.Lib.Database;
using ToDoList.Lib.FileService;
using ToDoList.Lib.Logging;
using ToDoList.Lib.Telegram;
using ToDoList.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var connString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ToDoListDbContext>(options => options.UseSqlite(connString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => 
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 1;
    options.Password.RequiredUniqueChars = 1;
}).AddEntityFrameworkStores<ToDoListDbContext>();

builder.Services.AddSingleton<UserStatistics>();
builder.Services.AddHostedService<BackgroundStatistics>();
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "local";
});

//var environment = builder.Environment;
//builder.Logging.AddFile(Path.Combine(environment.WebRootPath, "files", "logging", "logs.txt"));
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddSystemMetrics();
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddTransient<TelegramNotifier>();
builder.Services.AddScoped(typeof(IDatabaseService<>), typeof(DatabaseService<>));
builder.Services.AddScoped(typeof(IFileService), typeof(FileService));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();
app.UseHttpMetrics();
app.MapMetrics();
app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
