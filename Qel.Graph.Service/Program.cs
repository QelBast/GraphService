using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Qel.Graph.Common;
using Qel.Graph.Dal;
using System.Reflection;

namespace Qel.Graph;

internal class Program
{
    static void Main(string[] args)
    {
        string? exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        Directory.SetCurrentDirectory(exeDir!);

        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostingContext, services) => 
            {
                services.ConfigureNpgsqlDatabase<DbContextMain>(hostingContext)
                    .AddHostedService<WorkService>();
            })
            .Build();

        host.Run();
    }
}
