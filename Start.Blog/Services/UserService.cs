using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using static System.Int32;

namespace Start.Blog.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            var sub = _httpContextAccessor?.HttpContext?.User.FindFirst(u => u.Type == "sub")?.Value;
            TryParse(sub, out var userId);
            return userId == 0 ? throw new ArgumentException("UserId Error,Please login again") : userId;
        }
    }
}
