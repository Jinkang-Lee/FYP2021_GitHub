using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FYP2021.Controllers
{
    public class ReportController : Controller
    {
        // View to the generate report in Admin folder
        public IActionResult PendingForTransitLink()
        {
            string sql = "SELECT * FROM Student " +
                "WHERE card_status='Pending for TransitLink'";
            DataTable dt = DBUtl.GetTable(sql);
            return View(dt.Rows);
        }
        public IActionResult ReadyForApplication()
        {
            string sql = "SELECT * FROM Student " +
                "WHERE card_status='Ready for Application'";
            DataTable dt = DBUtl.GetTable(sql);
            return View(dt.Rows);
        }
        public IActionResult CardReady()
        {
            string sql = "SELECT * FROM Student " +
                "WHERE card_status='Card Ready'";
            DataTable dt = DBUtl.GetTable(sql);
            return View(dt.Rows);
        }
        public IActionResult CardDispatched()
        {
            string sql = "SELECT * FROM Student " +
                "WHERE card_status='Card Dispatched'";
            DataTable dt = DBUtl.GetTable(sql);
            return View(dt.Rows);
        }
        public IActionResult SummaryReport()
        {
            string sql = "SELECT * FROM Student";
            DataTable dt = DBUtl.GetTable(sql);
            return View(dt.Rows);
            
        }
        public IActionResult ChooseReport()
        {
            return View();
        }
    }
}
