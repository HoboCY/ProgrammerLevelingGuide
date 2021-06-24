using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Start.Blog.Helpers;
using Start.Blog.Models;
using Start.Blog.Services;
using Start.Blog.ViewModels;

namespace Start.Blog.Controllers
{
    [ApiController]
    [Authorize]
    public class PostController : Controller
    {
        private readonly ISqlHelper<Post> _sqlHelper;
        private readonly IUserService _userService;

        public PostController(ISqlHelper<Post> sqlHelper,IUserService userService)
        {
            _sqlHelper = sqlHelper;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet("[controller]/Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("api/[controller]")]
        public async Task<IActionResult> CreateAsync(CreateBlogInput input)
        {
            await _sqlHelper.AddAsync(new Post
            {
                Title = input.Title,
                Content = input.Content,
                CreationTime = DateTime.Now,
                UserId = _userService.GetUserId()
            });
            return NoContent();
        }
    }
}
