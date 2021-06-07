using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using FYP2021.Models;

namespace FYP2021.Controllers
{
    public class AnnouncementController : Controller
    {
        public IActionResult Options()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Delete()
        {
            return View();
        }

        public IActionResult Update()
        {
            string sql = "SELECT * FROM Announcement";
            DataTable dt = DBUtl.GetTable(sql);
            return View(dt.Rows);
        }

        [HttpGet]
        public IActionResult AnnouncementEdit(int id)
        {
            string select = "SELECT * FROM Announcement WHERE Id={0}";
            List<Announcement> list = DBUtl.GetList<Announcement>(select, id);
            if (list.Count == 1)
            {
                return View(list[0]);

            } 
            else
            {
                TempData["Message"] = "Announcement not found";
                TempData["MsgType"] = "warning";
                return RedirectToAction("Update");
            }
        }

        [HttpPost]
        public IActionResult AnnouncementEdit(Announcement ant)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("Update");
            }
            else
            {
                string update =
                    @"UPDATE Announcement
                            SET Announcement='{1}', Announcement_Date='{2:yyyy-MM-dd}'
                                WHERE Id={0}";
                int res = DBUtl.ExecSQL(update, ant.Id, ant.ViewAnnouncement, ant.Announcement_Date);

                if (res == 1)
                {
                    TempData["Message"] = "Announcement Updated";
                    TempData["MsgType"] = "success";

                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";

                }
                return RedirectToAction("Update");
            }
        }
    }
}
