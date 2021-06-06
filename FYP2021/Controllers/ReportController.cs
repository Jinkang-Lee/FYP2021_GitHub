using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using FYP2021.Models;

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
        public IActionResult Pie()
        {
            ViewData["Chart"] = "pie";
            ViewData["Title"] = "Report Summary";
            ViewData["ShowLegend"] = "true";
            return View("Chart");
        }

        public IActionResult SummaryChart()
        {
            return View();
        }

        private void PrepareData(int x)
        {
            int[] dataPendingTransitLink = new int[] { 0, 0, 0, 0, 0 };
            int[] dataReadyApplication = new int[] { 0, 0, 0, 0, 0 };
            int[] dataCardReady = new int[] { 0, 0, 0, 0, 0 };
            int[] dataCardDispatched = new int[] { 0, 0, 0, 0, 0 };

            //List<ReportChart> list = DBUtl.GetList(@"");
            string sql = "SELECT card_status FROM Student";
            DataTable dt = DBUtl.GetTable(sql);

            string[] colors = new[] { "cyan", "lightgreen", "yellow", "pink", "lightgrey" };
            string[] grades = new[] { "A", "B", "C", "D", "F" };
            ViewData["Legend"] = "Cadets";
            ViewData["Colors"] = colors;
            ViewData["Labels"] = grades;
        }

    }
}
