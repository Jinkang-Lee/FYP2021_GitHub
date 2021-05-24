using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Collections.Generic;

namespace FYP2021.Models
{
    public class Student
    {
        [Required(ErrorMessage = "Email cannot be empty!")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Enter the correct Email format!")]
        public string StudEmail { get; set; }


        [Required(ErrorMessage = "Name cannot be empty!")]
        [RegularExpression(@"\D", ErrorMessage = "Name should not contain numbers!")]
        public string StudName { get; set; }

        [Required(ErrorMessage = "Phone Number cannot be empty!")]
        [RegularExpression(@"\d{9}", ErrorMessage = "Length of phone number must be 9!")]
        public int StudPhNum { get; set; }


        [Required(ErrorMessage = "Card Status cannot be empty!")]
        [RegularExpression(@"\D", ErrorMessage = "Card Status should not contain numbers!")]
        public string CardStatus { get; set; }
    }
}
