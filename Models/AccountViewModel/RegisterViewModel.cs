using System.ComponentModel.DataAnnotations;

namespace ClubMates.Web.Models.AccountViewModel

{

    public class RegisterViewModel

    {

        [Required(ErrorMessage = "Full Name is Mandatory")]

        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email Id is Mandatory")]

        [EmailAddress(ErrorMessage = "Email is not in valid email")]

        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is require")]

        [DataType(DataType.Password)]

        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password should be match")]

        [DataType(DataType.Password)]

        public string ConfirmPassword { get; set; } = string.Empty;

    }

}

