using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Start.Blog.Extensions;
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
        private readonly ISqlHelper<Comment> _commentSqlHelper;
        private readonly IUserService _userService;

        public PostController(ISqlHelper<Post> sqlHelper, IUserService userService, ISqlHelper<Comment> commentSqlHelper)
        {
            _sqlHelper = sqlHelper;
            _userService = userService;
            _commentSqlHelper = commentSqlHelper;
        }

        [AllowAnonymous]
        [HttpGet("[controller]/Index")]
        public async Task<IActionResult> Index()
        {
            var posts = await _sqlHelper.GetAsync();
            return View(posts.ToList());
        }

        [HttpGet("[controller]/manage")]
        public async Task<IActionResult> Manage()
        {
            var currentUserId = _userService.GetUserId();
            var parameters = new DynamicParameters();
            parameters.Add("UserId", currentUserId);
            var posts = await _sqlHelper.GetListAsync(parameters);
            return View(posts.ToList());
        }

        [HttpGet("[controller]/Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet("[controller]/Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var post = await _sqlHelper.GetAsync(id);
            if(post == null) return NotFound($"The post not found with id:{id}");

            var currentUserId = _userService.GetUserId();
            return post.UserId != currentUserId ? BadRequest("Can't edit other people's posts") : View(post);
        }

        [AllowAnonymous]
        [HttpGet("[controller]/Detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var post = await _sqlHelper.GetAsync(id);
            if(post == null) return NotFound($"The post not found with id:{id}");

            var sql = "SELECT u.Id,c.Content AS Content,u.Name AS Username,c.CreationTime AS CreationTime FROM User u LEFT JOIN Comment c ON u.Id = c.UserId WHERE c.PostId = @PostId";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("PostId", post.Id);

            var comments = await _sqlHelper.GetListAsync<CommentViewModel>(sql, parameters);

            var postDetail = new PostDetailViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                Comments = comments.ToList()
            };

            return View(postDetail);
        }

        [HttpPost("api/[controller]")]
        public async Task<IActionResult> CreateAsync(CreatePostInput input)
        {
            if(input.Title.IsNullOrWhiteSpace() || input.Content.IsNullOrWhiteSpace()) return BadRequest("Fields can not empty.");
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

        [HttpDelete("api/[controller]/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var post = await _sqlHelper.GetAsync(id);
            if(post == null) return NotFound($"The post not found with id:{id}");

            var currentUserId = _userService.GetUserId();

            if(post.UserId != currentUserId) return BadRequest("Can't delete other people's posts");

            await _sqlHelper.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("api/[controller]/{id}")]
        public async Task<IActionResult> UpdateAsync(int id, UpdatePostInput input)
        {
            if(input.Title.IsNullOrWhiteSpace() || input.Content.IsNullOrWhiteSpace()) return BadRequest("Fields can not empty.");
            var post = await _sqlHelper.GetAsync(id);
            if(post == null) return NotFound($"The post not found with id:{id}");

            post.Title = input.Title;
            post.Content = input.Content;

            var currentUserId = _userService.GetUserId();
            if(currentUserId != post.UserId) return BadRequest("Can't edit other people's posts");

            await _sqlHelper.UpdateAsync(post);
            return NoContent();
        }

        [HttpPost("api/[controller]/{id}/comment")]
        public async Task<IActionResult> CommentAsync(int id, CommentPostInput input)
        {
            if (input.Content.IsNullOrWhiteSpace()) return BadRequest("Content can't empty");
            
            var post = await _sqlHelper.GetAsync(id);
            if (post == null) return NotFound($"The post not found with id:{id}");

            var currentUserId = _userService.GetUserId();

            var comment = new Comment
            {
                Content = input.Content,
                PostId = id,
                UserId = currentUserId,
                CreationTime = DateTime.Now
            };

            await _commentSqlHelper.AddAsync(comment);
            return NoContent();
        }
    }
}
