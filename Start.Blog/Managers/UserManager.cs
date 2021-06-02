using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Start.Blog.Extensions;
using Start.Blog.Helpers;

namespace Start.Blog.Managers
{
    public class UserManager<T> : IUserManager<T>
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
            if (name.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(name), "Invalid parameters");
            var parameters = new DynamicParameters();
            parameters.Add("Name", name);
            var user = await _sqlHelper.GetAsync(parameters);
            return user;
        }
    }
}
