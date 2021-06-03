using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Start.Blog.Extensions;
using Start.Blog.Helpers;
using Start.Blog.Managers;
using Start.Blog.Models;
using Start.Blog.ViewModels;

namespace Start.Blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserManager<User> _userManager;

        public UserController(IUserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(LoginInput input)
        {
            var user = await _userManager.FindByNameAsync(input.Username);
            if (user == null) return NotFound($"Not found with name:{input.Username}");
            var isCorrect = await _userManager.CheckPasswordAsync(user, input.Password, BlogConsts.Salt);
            return isCorrect ? Ok("Login Success") : BadRequest("Invalid password");
        }
    }
}
