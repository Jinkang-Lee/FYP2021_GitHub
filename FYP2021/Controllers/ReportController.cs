using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using FYP2021.Models;
using Rotativa.AspNetCore;

namespace FYP2021.Controllers
{
    public class ReportController : Controller
    {

        public IActionResult ReportViewAsPDF()
        {
            List<Student> list = DBUtl.GetList<Student>("SELECT * FROM Student");

            //DataTable dt = DBUtl.GetTable("SELECT * FROM Student");

            if (list != null)
                return new ViewAsPdf("ReportViewAsPDF", list)
                {
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape

                };
            else
            {
                TempData["Msg"] = "FAILED";
                return RedirectToAction("SummaryReport");
            }
        }

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

        public IActionResult SummaryChart()
        {
            PrepareData(0);
            ViewData["Chart"] = "pie";
            ViewData["Title"] = "Report Summary";
            ViewData["ShowLegend"] = "true";
            return View("SummaryChart");
        }


        private void PrepareData(int x)
        {
            int[] dataPendingTransitLink = new int[] { 0, 5, 7, 6 };
            int[] dataReadyApplication = new int[] { 0, 0, 0, 0 };
            int[] dataCardReady = new int[] { 0, 0, 0, 0 };
            int[] dataCardDispatched = new int[] { 0, 0, 0, 0 };


            List<ReportChart> list = DBUtl.GetList<ReportChart>("SELECT card_status FROM Student");

            foreach (ReportChart rpt in list)
            { 
                dataPendingTransitLink[rpt.PendingForTransitLink]++;

                dataReadyApplication[rpt.ReadyForApplication]++;

                dataCardReady[rpt.CardReady]++;

                dataCardDispatched[rpt.CardDispatched]++;

        }
    
            string[] colors = new[] { "cyan", "lightgreen", "yellow", "pink" };
            string[] cardstatus = new[] { "Pending for TransitLink", "Ready for Application", "Card Ready", "Card Dispatched" };

            ViewData["Legend"] = "Report";
            ViewData["Colors"] = colors;
            ViewData["Labels"] = cardstatus;

            if (x == 0)
                ViewData["Data"] = dataPendingTransitLink;
            else if (x == 1)
                ViewData["Data"] = dataReadyApplication;
            else if (x == 2)
                ViewData["Data"] = dataCardReady;
            else
                ViewData["Data"] = dataCardDispatched;
        }
    }
}
