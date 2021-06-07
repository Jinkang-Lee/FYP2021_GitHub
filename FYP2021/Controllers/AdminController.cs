using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FYP2021.Models;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Microsoft.AspNetCore.Http;

namespace FYP2021.Controllers
{
    //This controller is using "AdminAccount" AUTH SCHEME
    [Authorize(AuthenticationSchemes = "AdminAccount")]
    public class AdminController : Controller
    {
        //private AppDbContext _dbContext;

        //public AdminController(AppDbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}



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
            //DbSet<Student> dbs = _dbContext.Student;
            //List<Student> model = dbs.ToList();
            //var email = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //if(User.IsInRole("Admin"))
            //    model = model.Where(s => s.StudEmail == email).ToList();


            DataTable dt = DBUtl.GetTable("SELECT * FROM Student");
            return View("ListStudent", dt.Rows);
        }




        //HTTP GET FOR EDITING STUDENT IN THE LIST
        [Authorize]
        public IActionResult ListEditStudent(int id)
        {

            List<Student> list = DBUtl.GetList<Student>("SELECT * FROM Student WHERE student_id = {0}", id);
            Student model = null;
            if (list.Count == 1)
            {
                model = list[0];
                return View("ListEditStudent", model);
            }
            else
            {
                TempData["Message"] = "Student Not Found!";
                TempData["MsgType"] = "warning";
                return RedirectToAction("Index");
            }


        }




        //HTTP POST FOR EDITING STUDENT IN THE LIST
        [HttpPost]
        [Authorize]
        public IActionResult ListEditStudentPost(Student student, IFormCollection form)
        {

            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Invalid Input!";
                TempData["MsgType"] = "warning";
                return View("ListStudent");
            }
            else
            {
                string update = @"UPDATE Student SET student_email='{0}', student_name='{1}', ph_num={2}, card_status='{3}' WHERE student_id = {4}";

                int res = DBUtl.ExecSQL(update, student.StudEmail, student.StudName, student.StudPhNum, student.CardStatus, student.Id);

                if (res == 1)
                {
                    TempData["Message"] = "Success!";
                    TempData["MsgType"] = "success";

                    string custname = form["StudName"].ToString().Trim();
                    string email = form["StudEmail"].ToString().Trim();
                    string cardStatus = form["CardStatus"].ToString().Trim();
                    string subject = "New Card Status";

                    string template =
                            @"Dear {0}, <br/>
                                <p>Your new updated card status is {1}. For more information, visit the website.</p>
                                Sincerely RP Team";

                    string msg = String.Format(template, custname, cardStatus);
                    string result;
                    if (EmailUtl.SendEmail(email, subject, msg, out result))
                    {

                        ViewData["Message"] = "Email Successfully Sent!";
                        ViewData["MsgType"] = "success";
                    }

                    else
                    {
                        ViewData["Message"] = result;
                        ViewData["MsgType"] = "warning";
                    }

                    return View("Index");
                }

                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }

                return RedirectToAction("ListStudent");
            }
        }

        public IActionResult ListCard()
        {
            string sql = "SELECT * FROM Student";
            DataTable dt = DBUtl.GetTable(sql);
            return View(dt.Rows);
        }
    }
}
