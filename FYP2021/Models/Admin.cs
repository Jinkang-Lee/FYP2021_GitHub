using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Collections.Generic;

namespace FYP2021.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Email cannot be empty!")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Enter the correct Email format!")]
        public string AdminEmail { get; set; }

        public string AdminName { get; set; }

        public string AdminPhNum { get; set; }

        [Required(ErrorMessage = "Cannot be empty!")]
        [DataType(DataType.Password)]
        public string AdminPassword { get; set; }


        public string UserRole { get; set; }

    }
}
