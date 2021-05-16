using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FYP2021.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage = "Email cannot be empty!")]
        [DataType(DataType.EmailAddress,
                  ErrorMessage = "Use your registred email to login!")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Empty password not allowed!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public int Id { get; set; }
        public string UserName { get; set; }
    }
}
