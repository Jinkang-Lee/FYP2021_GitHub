using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP2021.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }



        public IActionResult StudentLoginpage()
        {
            return View();
        }
    }
}
