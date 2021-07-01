using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;

namespace Start.Blog.Helpers
{
    public interface ISqlHelper<T>
    {
        Task<T> GetAsync(int id);

        Task<T> GetAsync(DynamicParameters parameters);

        Task<IEnumerable<T>> GetListAsync(DynamicParameters parameters);

        Task<IQueryable<T>> GetAsync(Func<T, bool> expression = null);

        Task<IEnumerable<TResult>> GetListAsync<TResult>(string sql, DynamicParameters parameters = null);

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int id);
    }
}
