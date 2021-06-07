using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//INSERT MODEL HERE
using Microsoft.AspNetCore.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Dynamic;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FYP2021.Controllers
{

    //This controller is using "Student" AUTH SCHEME
    [Authorize(AuthenticationSchemes = "StudentAccount")]
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

        public IActionResult StudentOTPpage()
        {
            return View();
        }

    }
}
