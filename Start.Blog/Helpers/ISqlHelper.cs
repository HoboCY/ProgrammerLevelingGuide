using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Start.Blog.Helpers
{
    public interface ISqlHelper<T>
    {
        Task<T> GetAsync(int id);

        Task<IEnumerable<T>> GetAsync(Func<T, bool> expression);

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int id);
    }
}
