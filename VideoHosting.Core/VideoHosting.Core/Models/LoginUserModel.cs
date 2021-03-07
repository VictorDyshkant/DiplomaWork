using System.ComponentModel.DataAnnotations;


namespace VideoHosting.Core.Models
{
    public class LoginUserModel
    {
        [Required]
        public string Email { get; set; }
    }
}