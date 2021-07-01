using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Start.Blog.ViewModels
{
    public class PostDetailViewModel
    {
        public int Id{ get; set; }
        public string Title { get; set; }

        public string Content { get; set; }

        public List<CommentViewModel> Comments { get; set; }
    }

    public class CommentViewModel
    {
        public string Content { get; set; }

        public DateTime CreationTime { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }
    }
}
