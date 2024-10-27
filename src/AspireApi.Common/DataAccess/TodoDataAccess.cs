using System;
using System.Linq;
using System.Threading.Tasks;

namespace AspireApi.Common.DataAccess;

public class TodoDataAccess : ITodoDataAccess
{
    private readonly IDataAccessHelper dataAccessHelper;

    public TodoDataAccess(IDataAccessHelper dataAccessHelper)
    {
        this.dataAccessHelper = dataAccessHelper;
    }

    public async Task<TodoItem> Add<TodoItem>(TodoItem entity)
    {
        var sql = @"
            insert into todo.items(id, name)
            values(@Id, @Name)
            returning *
        ";

        return (await dataAccessHelper.QueryAsync<TodoItem>(sql, entity)).FirstOrDefault();
    }

    public Task<int> Delete<TodoItem>(TodoItem entity)
    {
        throw new NotImplementedException();
    }

    public async Task<TodoItem> GetById<TodoItem>(Guid id)
    {
        var sql = @"
            select id, name
            from todo.items
            where id = @id
        ";

        var param = new { id };

        return (await dataAccessHelper.QueryAsync<TodoItem>(sql, param)).FirstOrDefault();
    }

    public Task<TodoItem> Update<TodoItem>(TodoItem entity)
    {
        throw new NotImplementedException();
    }
}
