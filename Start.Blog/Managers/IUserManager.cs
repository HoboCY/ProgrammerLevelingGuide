using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Start.Blog.Models;

namespace Start.Blog.Managers
{
    public interface IUserManager<T>
    {
        Task<T> FindByIdAsync(int id);

        Task<T> FindByNameAsync(string name);
    }
}
