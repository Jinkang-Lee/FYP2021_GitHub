using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Collections.Generic;

namespace FYP2021.Models
{
    public partial class Student
    {
        public string StudEmail { get; set; }

        public string StudName { get; set; }

        public int StudPhNum { get; set; }

        public string CardStatus { get; set; }
    }
}
