using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FYP2021.Models;
using Microsoft.AspNetCore.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Dynamic;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace FYP2021.Controllers
{

    [Authorize(AuthenticationSchemes = "AdminAccount")]
    public class CardstatusController : Controller
    {
        [Authorize]
        public IActionResult ListCard(Student CardStatusDate)
        {
            //List<Student> list = DBUtl.GetList<Student>("SELECT * FROM Student WHERE cardstatus_date = {0}", CardStatusDate);
            //string dateInString = Student.CardStatusDate;

            //DateTime startDate = DateTime.Parse(dateInString);
            //DateTime expiryDate = startDate.AddDays(90);
            //if (DateTime.Now > expiryDate)
            //{
            //    string delete = "DELETE FROM Student";
            //    DBUtl.ExecSQL(delete);
            //}

            //string sql = "SELECT * FROM Student";
            DataTable dt = DBUtl.GetTable("SELECT * FROM Student");
            return View("ListCard",dt.Rows);

            

        }

        [HttpGet]
        [Authorize]
        public IActionResult EditCardstatus(int Id)
        {
            List<Student> list = DBUtl.GetList<Student>("SELECT * FROM Student WHERE student_Id = {0}", Id);
            Student model = null;

            if (list.Count == 1)
            {
                model = list[0];
                return View("EditCardstatus", model);
            }
            else
            {
                TempData["Message"] = "Student Not Found!";
                TempData["MsgType"] = "warning";
                return RedirectToAction("Index");
            }

            //    string sql = "SELECT * FROM Student WHERE student_email = '{0}'";
            //    DataTable list = DBUtl.GetTable(sql);

            //    if (list.Columns.Count == 1)
            //    {
            //        return View(list.TableName[0]);
            //    }
            //    else
            //    {
            //        TempData["Message"] = "Student Not Found!";
            //        TempData["MsgType"] = "warning";
            //        return RedirectToAction("Index");
            //    }
            }

        [HttpPost]
        [Authorize]
        public IActionResult EditCardstatusPost(Student student, IFormCollection form)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Invalid Input!";
                TempData["MsgType"] = "warning";
                return View("ListCard");
            }
            else
            {
                string update = @"UPDATE Student SET card_status='{0}' WHERE student_Id={1}";

                int result = DBUtl.ExecSQL(update, student.CardStatus, student.Id);
                if (result == 1)
                {
                    TempData["Message"] = "Card Status Updated";
                    TempData["MsgType"] = "success";

                    //string custname = form["StudName"].ToString().Trim();
                    //string email = form["StudEmail"].ToString().Trim();
                    //string cardStatus = form["CardStatus"].ToString().Trim();
                    //string subject = "New Card Status";

                    //string template =
                    //        @"Dear {0}, <br/>
                    //            <p>Your new updated card status is {1}. For more information, visit the website.</p>
                    //            Sincerely RP Team";

                    //string msg = String.Format(template, custname, cardStatus);
                    //string res; 
                    //if (EmailUtl.SendEmail(email, subject, msg, out res))
                    //{

                    //    ViewData["Message"] = "Email Successfully Sent!";
                    //    ViewData["MsgType"] = "success";
                    //}

                    //else
                    //{
                    //    ViewData["Message"] = result;
                    //    ViewData["MsgType"] = "warning";
                    //}

                    //return View("Index");
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
                return RedirectToAction("ListCard");
            }

        }

        [Authorize]
        public IActionResult UpdateOptions()
        {
            return View();
        }

        [Authorize]
        public IActionResult ListCardCSV()
        {
            string sql = "SELECT * FROM Student";
            DataTable dt = DBUtl.GetTable(sql);
            return View(dt.Rows);
        }

        public static void Main(Student student)
        {
            string dateInString = student.CardStatusDate;

            DateTime startDate = DateTime.Parse(dateInString);
            DateTime expiryDate = startDate.AddDays(90);
            if (DateTime.Now > expiryDate)
            {
                string delete = "DELETE FROM Student";
                DBUtl.ExecSQL(delete);
            }
        }
    }
}