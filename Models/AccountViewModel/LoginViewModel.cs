using System.ComponentModel.DataAnnotations;

namespace ClubMates.Web.Models.AccountViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please provide a username")]
        public string? UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please provide a password")]
        public string? Password { get; set; } = string.Empty;
    }
}