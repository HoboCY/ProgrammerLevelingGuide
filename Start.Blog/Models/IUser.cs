using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Start.Blog.Models
{
    public interface IUser
    {
        string Name { get; set; }

        string Password { get; set; }
    }
}
