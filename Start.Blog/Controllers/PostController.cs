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

        public PostController(ISqlHelper<Post> sqlHelper, IUserService userService)
        {
            _sqlHelper = sqlHelper;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet("[controller]/Index")]
        public async Task<IActionResult> Index()
        {
            var posts = await _sqlHelper.GetAsync();
            return View(posts.ToList());
        }

        [HttpPost("api/[controller]")]
        public async Task<IActionResult> CreateAsync(CreateBlogInput input)
        {
            var currentUserId = _userService.GetUserId();
            await _sqlHelper.AddAsync(new Post
            {
                Title = input.Title,
                Content = input.Content,
                CreationTime = DateTime.Now,
                UserId = currentUserId
            });
            return NoContent();
        }

        [HttpDelete("api/[controller]/id")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var post = await _sqlHelper.GetAsync(id);
            if(post == null) return NotFound();

            var currentUserId = _userService.GetUserId();

            if(post.UserId != currentUserId) return BadRequest("Can't delete other people's posts");

            await _sqlHelper.DeleteAsync(id);
            return NoContent();
        }
    }
}
