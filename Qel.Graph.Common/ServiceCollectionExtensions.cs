using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Qel.Graph.Common;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureNpgsqlDatabase<T>(
        this IServiceCollection collection,
        HostBuilderContext hostingcontext) where T : DbContext
    {
        collection.AddDbContext<T, T>((options) =>
        {
            var connectionString = hostingcontext.Configuration.GetConnectionString(typeof(T).Name);
            ArgumentException.ThrowIfNullOrEmpty(connectionString, $"Строка соединения {typeof(T).Name} не задана в конфигурации");
            options.UseNpgsql(
                connectionString,
                server => server.MigrationsAssembly(typeof(T).Assembly.FullName));
        }
        , ServiceLifetime.Transient
        , ServiceLifetime.Transient);
        return collection;
    }
}
