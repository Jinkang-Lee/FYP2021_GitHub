using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace FYP2021.Models
{
    public class UpdatePassword
    {
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }


        [Required(ErrorMessage = "Email cannot be empty!")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Enter the correct Email format!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Cannot be empty!")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Cannot be empty!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
