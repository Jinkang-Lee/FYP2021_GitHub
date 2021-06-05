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
    public class CardstatusController : Controller
    {
        public IActionResult ListCard()
        {
            string sql = "SELECT * FROM Student";
            DataTable dt = DBUtl.GetTable(sql);
            return View(dt.Rows);

            //List<Student> student = DBUtl.GetList<Student>(
            //   @"SELECT * FROM Student");
            //return View(student);
        }

        [HttpGet]
        public IActionResult EditCardstatus(string StudEmail)
        {
            String studentSql = @"SELECT card_status FROM Student WHERE student_email = '{0}'";
            List<Student> list = DBUtl.GetList<Student>(studentSql, StudEmail);

            if (list.Count == 1)
            {
                return View(list[0]);
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
        public IActionResult EditCardstatus(Student student)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Invalid Input!";
                TempData["MsgType"] = "warning";
                return View("ListCard");
            }
            else
            {
                string update = @"UPDATE Student card_status='{3}' WHERE student_email={0}";

                int result = DBUtl.ExecSQL(update, student.CardStatus);
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
                return RedirectToAction("ListStudent");
            }

        }

        public IActionResult UpdateOptions()
        {
            return View();
        }

        public IActionResult ListCardCSV()
        {
            string sql = "SELECT * FROM Student";
            DataTable dt = DBUtl.GetTable(sql);
            return View(dt.Rows);
        }
    }
}