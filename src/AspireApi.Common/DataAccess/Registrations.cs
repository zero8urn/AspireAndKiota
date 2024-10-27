using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace AspireApi.Common.DataAccess;

public static class Registrations
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration config)
    {
        var dbAccessConfig = config.GetSection(nameof(DataAccessOptions));

        services.Configure<DataAccessOptions>(dbAccessConfig);

        services.AddSingleton<IDataAccessHelper, DataAccessHelper<NpgsqlConnection>>();

        services.AddSingleton<ITodoDataAccess, TodoDataAccess>();

        return services;
    }
}
