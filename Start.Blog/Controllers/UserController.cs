using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Start.Blog.Managers;
using Start.Blog.Models;
using Start.Blog.Services;
using Start.Blog.ViewModels;

namespace Start.Blog.Controllers
{
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserManager<User> _userManager;

        public UserController(IUserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("api/[controller]/Login")]
        public async Task<IActionResult> LoginAsync(LoginInput input)
        {
            var user = await _userManager.FindByNameAsync(input.Username);
            if(user == null) return NotFound($"Not found with name：{input.Username}");
            var isCorrect = await _userManager.CheckPasswordAsync(user, input.Password, BlogConsts.Salt);
            if(!isCorrect) return BadRequest("Invalid password");
            var claimsPrincipal = new ClaimsPrincipal();

            var claims = new List<Claim>
            {
                new Claim("sub", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Id.ToString())

            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return Ok("Login Success");
        }

        [HttpPost("api/[controller]/Register")]
        public async Task<IActionResult> RegisterAsync(RegisterInput input)
        {
            var user = await _userManager.FindByNameAsync(input.Username);
            if(user != null) return BadRequest("Username was existed");
            await _userManager.RegisterAsync(input.Username, input.Password, BlogConsts.Salt);
            return Ok("Register Success");
        }

        [HttpGet("[controller]/Login")]
        public IActionResult LoginAsync()
        {
            return View();
        }

        [HttpGet("[controller]/Register")]
        public IActionResult RegisterAsync()
        {
            return View();
        }
    }
}
