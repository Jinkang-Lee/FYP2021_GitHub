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
        public IActionResult ListCard()
        {
            //string sql = "SELECT * FROM Student";
            DataTable dt = DBUtl.GetTable("SELECT * FROM Student");
            return View("ListCard",dt.Rows);

            //List<Student> student = DBUtl.GetList<Student>(
            //   @"SELECT * FROM Student");
            //return View(student);
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
        public IActionResult EditCardstatusPost(Student student)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Invalid Input!";
                TempData["MsgType"] = "warning";
                return View("UpdateOptions");
            }
            else
            {
                string update = @"UPDATE Student SET card_status='{0}' WHERE student_Id={1}";

                int result = DBUtl.ExecSQL(update, student.CardStatus, student.Id);
                if (result == 1)
                {
                    TempData["Message"] = "Card Status Updated";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
                return RedirectToAction("UpdateOptions");
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
    }
}