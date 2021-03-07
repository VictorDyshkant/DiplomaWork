using System.ComponentModel.DataAnnotations;


namespace VideoHosting.Core.Models
{
    public class ResetPasswordModelByEmail
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public int TempPassword { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string PasswordConfirm { get; set; }
    }
}