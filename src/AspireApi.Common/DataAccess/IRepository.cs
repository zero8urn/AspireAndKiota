using System;
using System.Threading.Tasks;

namespace AspireApi.Common.DataAccess;

public interface IRepository
{
    Task<T> GetById<T>(Guid id);
    Task<T> Add<T>(T entity);
    Task<T> Update<T>(T entity);
    Task<int> Delete<T>(T entity);
}
