using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Start.Blog.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string Title{ get; set; }

        public string Content { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.Now;

        public int UserId { get; set; }
    }
}
