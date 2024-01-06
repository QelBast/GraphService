using Qel.Graph.Common;
using Qel.Graph.Dal;

namespace Qel.Graph.Web;

/// <summary>
/// Точка входа в веб-сервис
/// </summary>
public class Program
{
    /// <summary>
    /// Настройка и запуск модуля веб-интерфейса
    /// </summary>
    /// <param name="args">Принимаемые аргументы</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.ConfigureNpgsqlDatabase<DbContextMain>(builder.Configuration)
                .AddControllersWithViews();
        builder.Services.AddSignalR();

        var app = builder.Build();

        // устанавливаем сопоставление маршрутов с контроллерами
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        //app.MapHub<ChatHub>("/chatHub");

        //app.MapPost("/uploadstream", async (IConfiguration config, HttpContext context) =>
        //{
        //    var filePath = Path.Combine(Path.GetRandomFileName(), ".json");
        //    await using var writeStream = File.Create(filePath);
        //    await context.Request.Body.CopyToAsync(writeStream);
        //});
        
        app.Run();
    }
}
