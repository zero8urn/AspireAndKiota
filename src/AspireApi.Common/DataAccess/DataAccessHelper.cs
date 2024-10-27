using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace AspireApi.Common.DataAccess;

public class DataAccessHelper<TConnection> : IDataAccessHelper where TConnection : IDbConnection, new()
{
    private readonly string connectionString;

    private IDbConnection Conn => new TConnection { ConnectionString = connectionString };

    public DataAccessHelper(IOptionsMonitor<DataAccessOptions> dataAccessOptions)
    {
        connectionString = dataAccessOptions.CurrentValue.ConnectionString;
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, CommandType? commandType = null)
    {
        using var cnn = Conn;
        return await cnn.QueryAsync<T>(sql, param, commandType: commandType);
    }

    public async Task<int> ExecuteAsync(string sql, object param = null, CommandType? commandType = null)
    {
        using var cnn = Conn;
        return await cnn.ExecuteAsync(sql, param, commandType: commandType);
    }
}
