using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace AspireApi.Common.DataAccess;

public interface IDataAccessHelper
{
    Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, CommandType? commandType = null);
    Task<int> ExecuteAsync(string sql, object param = null, CommandType? commandType = null);
}
