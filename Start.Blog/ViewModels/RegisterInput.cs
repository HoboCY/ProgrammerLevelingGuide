using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Start.Blog.ViewModels
{
    public class RegisterInput
    {
        [Required]
        [MinLength(6)]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(50)]
        public string Password { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(50)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
