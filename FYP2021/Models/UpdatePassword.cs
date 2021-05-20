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
        [Required(ErrorMessage = "Cannot be empty!")]
        [DataType(DataType.Password)]
        [Remote("VerifyCurrentPassword", "Account", ErrorMessage = "Incorrect password!")]
        public string CurrentPassword { get; set; }


        [Required(ErrorMessage = "Cannot be empty!")]
        [DataType(DataType.Password)]
        [Remote("VerifyNewPassword", "Account", ErrorMessage = "Cannot reuse password!")]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "Cannot be empty!")]
        [DataType(DataType.Password)]
        [Remote("ChangePassword", "Account", ErrorMessage = "Password not confirmed!")]
        public string ConfirmPassword { get; set; }
    }
}
