using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//INSERT MODEL HERE
using FYP2021.Models;
using Microsoft.AspNetCore.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Dynamic;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FYP2021.Controllers
{
    //This controller is using "AdminAccount" AUTH SCHEME
    [Authorize(AuthenticationSchemes = "AdminAccount")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }




        // View to the generate report in Admin folder
        public IActionResult Report()
        {
            return View();
        }


        public IActionResult EditStudent()
        {
            return View();
        }


        //HTTP GET
        public IActionResult CreateStudent()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateStudent(Student student)
        {
            string userid = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (ModelState.IsValid)
            {
                string sql = @"INSERT INTO Student
                                            (student_email, student_name, ph_num, card_status)
                                            VALUES ('{0}', '{1}', {2}, '{3}')";

                if (DBUtl.ExecSQL(sql, student.StudEmail, student.StudName, student.StudPhNum, student.CardStatus) == 1)
                    TempData["Msg"] = "New student Added!";
                return RedirectToAction("EditStudent");

            }
            else
            {
                TempData["Msg"] = "Invalid information entered!";
                return RedirectToAction("Index");
            }
        }
    }
}
