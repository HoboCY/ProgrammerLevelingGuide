using System.Threading.Tasks;
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
        private readonly IUserService _userService;

        public UserController(IUserManager<User> userManager,IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        [HttpPost("api/[controller]/Login")]
        public async Task<IActionResult> LoginAsync(LoginInput input)
        {
            var user = await _userManager.FindByNameAsync(input.Username);
            if (user == null) return NotFound($"Not found with name：{input.Username}");
            var isCorrect = await _userManager.CheckPasswordAsync(user,input.Password,BlogConsts.Salt);
            return isCorrect ? Ok(_userService.GenerateJwtToken(user.Id,user.Name)) : BadRequest("Invalid password");
        }

        [HttpPost("api/[controller]/Register")]
        public async Task<IActionResult> RegisterAsync(RegisterInput input)
        {
            var user = await _userManager.FindByNameAsync(input.Username);
            if (user != null) return BadRequest("Username was existed");
            await _userManager.RegisterAsync(input.Username,input.Password,BlogConsts.Salt);
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
