using System.ComponentModel.DataAnnotations;

namespace VideoHosting.Core.Models
{
    public class UserEnterModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}