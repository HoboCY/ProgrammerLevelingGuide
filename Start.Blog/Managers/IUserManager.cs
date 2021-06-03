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

        Task<bool> CheckPasswordAsync(T t, string password, string salt = null);

        Task RegisterAsync(string name, string password, string salt = null);
    }
}
