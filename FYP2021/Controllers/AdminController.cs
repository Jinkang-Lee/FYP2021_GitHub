using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FYP2021.Models;
using Microsoft.AspNetCore.Http;

using System.IO;
using System.Dynamic;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

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




        public IActionResult ListStudent()
        {
            string sql = "SELECT * FROM Student";
            DataTable dt = DBUtl.GetTable(sql);
            return View(dt.Rows);
        }




        //HTTP GET FOR EDITING STUDENT IN THE LIST
        public IActionResult ListEditStudent(string email)
        {
            string select = ("SELECT * FROM Student WHERE student_email = '{0}'");
            List<Student> list = DBUtl.GetList<Student>(select, email);

            if(list.Count == 1)
            {
                return View(list[0]);
            }
            else
            {
                TempData["Message"] = "Student Not Found!";
                TempData["MsgType"] = "warning";
                return RedirectToAction("ListStudent");
            }
        }




        //HTTP POST FOR EDITING STUDENT IN THE LIST
        [HttpPost]
        public IActionResult ListEditStudent(Student student)
        {
            if(!ModelState.IsValid)
            {
                TempData["Message"] = "Invalid Input!";
                TempData["MsgType"] = "warning";
                return View("ListStudent");
            }
            else
            {
                string update = @"UPDATE Student SET student_email='{0}', student_name='{1}', ph_num={2}, card_status='{3}'";

                int res = DBUtl.ExecSQL(update, student.StudEmail, student.StudName, student.StudPhNum, student.CardStatus);

                if (res == 1)
                {
                    TempData["Message"] = "Success!";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
                return RedirectToAction("ListStudent");
            }
        }


    }
}
