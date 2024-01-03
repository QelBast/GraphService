namespace Qel.Graph.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
                //.AddRazorPages()
                

            var app = builder.Build();

            // устанавливаем сопоставление маршрутов с контроллерами
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapPost("/uploadstream", async (IConfiguration config, HttpContext context) =>
            {
                var filePath = Path.Combine(Path.GetRandomFileName(), ".json");
                await using var writeStream = File.Create(filePath);
                await context.Request.Body.CopyToAsync(writeStream);
            });
            
            app.Run();
        }
    }
}
