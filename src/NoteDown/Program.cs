using MySql.Data.MySqlClient;
using NoteDown.Data.IModels;
using NoteDown.Data.Models;
using System.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");

// Add services to the container.
builder.Logging.AddConsole();  // Adiciona o provedor de log do console
builder.Services.AddControllersWithViews();
// Adiciona a configuração do IDbConnection
builder.Services.AddScoped<IDbConnection>(provider => new MySqlConnection(builder.Configuration.GetConnectionString("MySqlConnection")));
builder.Services.AddScoped<IDataModel, DataModel>();
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
