using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Start.Blog.Models
{
    public class Comment
    {
        public int Id { get; set; }


        public string Content { get; set; }

        public DateTime CreationTime { get; set; }

        public int UserId { get; set; }

        public int PostId { get; set; }
    }
}
