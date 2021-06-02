using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Components.Forms;
using Start.Blog.Extensions;
using Start.Blog.Helpers;
using Start.Blog.Models;

namespace Start.Blog.Managers
{
    public class UserManager<T> : IUserManager<T> where T : IUser, new()
    {
        private readonly ISqlHelper<T> _sqlHelper;

        public UserManager(ISqlHelper<T> sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        public async Task<T> FindByIdAsync(int id)
        {
            var user = await _sqlHelper.GetAsync(id);
            return user;
        }

        public async Task<T> FindByNameAsync(string name)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Name", name);
            var user = await _sqlHelper.GetAsync(parameters);
            return user;
        }

        public Task<bool> CheckPasswordAsync(T t, string password, string salt = null)
        {
            var input = salt.IsNullOrWhiteSpace() ? password : password + salt;
            var result = t.Password == input.ToMd5();
            return Task.FromResult(result);
        }

        public async Task RegisterAsync(string name, string password, string salt = null)
        {
            var inputPassword = salt.IsNullOrWhiteSpace() ? password : password + salt;
            var user = new T {Name = name, Password = inputPassword.ToMd5()};
            await _sqlHelper.AddAsync(user);
        }
    }
}
