
using System.ComponentModel.DataAnnotations;

namespace FYP2021.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage = "Email cannot be empty!")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Use your registred email to login!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Empty password not allowed!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


    }
}
