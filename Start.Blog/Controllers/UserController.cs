using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Start.Blog.Helpers;
using Start.Blog.Models;

namespace Start.Blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ISqlHelper<User> _sqlHelper;

        public UserController(ISqlHelper<User> sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
    }
}
