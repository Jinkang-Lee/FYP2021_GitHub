using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using FYP2021.Models;
using Rotativa.AspNetCore;
using System.Text;
using System.Linq;

namespace FYP2021.Controllers
{
    public class ReportController : Controller
    {

        public IActionResult ReportViewAsPDF()
        {
            //Take entry from database
            List<Student> list = DBUtl.GetList<Student>("SELECT student_id,student_email,student_name,card_status,cardstatus_date FROM Students");

            //Manual Entry
            List<Student> list2 = new List<Student>
            {
                new Student{Id=1, StudEmail="19030130@myrp.edu.sg", StudName="Syakir", CardStatus="Pending for TransitLink", CardStatusDate="9/7/2201"},
                new Student{Id=2, StudEmail="19030131@myrp.edu.sg", StudName="YiYang", CardStatus="Ready for Application", CardStatusDate="15/7/2201"},
                new Student{Id=3, StudEmail="19030132@myrp.edu.sg", StudName="Jinkang", CardStatus="Card Ready", CardStatusDate="20/7/2201"},
                new Student{Id=4, StudEmail="19030133@myrp.edu.sg", StudName="Zhenni", CardStatus="Card Dispatched", CardStatusDate="29/7/2201"}

            };

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

        //Manual Entry for CSV file
        private List<Student> list2 = new List<Student>

        {
            new Student{Id=1,StudEmail="19030130@myrp.edu.sg",StudName="Syakir",CardStatus="Pending for TransitLink",CardStatusDate="9/7/2021"},
            new Student{Id=2, StudEmail="19030131@myrp.edu.sg",StudName="YiYang",CardStatus="Ready for Application",CardStatusDate="15/7/2201"},
            new Student{Id=3, StudEmail="19030132@myrp.edu.sg",StudName="Jinkang",CardStatus="Card Ready",CardStatusDate="20/7/2201"},
            new Student{Id=4, StudEmail="19030133@myrp.edu.sg",StudName="Zhenni",CardStatus="Card Dispatched",CardStatusDate="29/7/2201"}
        };

        public IActionResult ReportToCSV()
        {
            //Entry from database
            List<Student> list = DBUtl.GetList<Student>("SELECT student_id,student_email,student_name,card_status,cardstatus_date FROM Students");

            var builder = new StringBuilder();
            builder.AppendLine("Id,StudentEmail,StudentName,CardStatus,CardStatusDate");
            foreach (var student in list)
            {
                builder.AppendLine($"{student.Id},{student.StudEmail},{student.StudName},{student.CardStatus},{student.CardStatusDate}");

            }
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "students.csv");
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
            PrepareData();
            ViewData["Chart"] = "pie";
            ViewData["Title"] = "Report Summary";
            ViewData["ShowLegend"] = "true";
            return View("SummaryChart");
        }

        private int Count(int count)
        {
            List<ReportChart> list = DBUtl.GetList<ReportChart>("SELECT card_status FROM Student");

            if (list.Equals("Pending for TransitLink"))
                return count += 1;

            else if (list.Equals("Ready for Application"))
                return count += 1;

            else if (list.Equals("Card Ready"))
                return count += 1;

            else if (list.Equals("Card Dispatched"))
                return count += 1;

            else return count;
        }

        private void PrepareData()
        {
            int[] dataStatus = new int[] { 3, 3, 5, 6 };

            List<ReportChart> list = DBUtl.GetList<ReportChart>("SELECT card_status FROM Student");

            foreach (ReportChart rpt in list)
            {
                if (rpt.Equals("Pending for TransitLink"))
                {
                    dataStatus[Count(rpt.PendingForTransitLink)]++;
                }
                else if (rpt.Equals("Ready for Application"))
                {
                    dataStatus[Count(rpt.ReadyForApplication)]++;
                }
                else if (rpt.Equals("Card Ready"))
                {
                    dataStatus[Count(rpt.CardReady)]++;
                }
                else if (rpt.Equals("Card Dispatched"))
                {
                    dataStatus[Count(rpt.CardDispatched)]++;
                }
            }

            string[] colors = new[] { "cyan", "lightgreen", "yellow", "pink" };
            string[] cardstatus = new[] { "Pending for TransitLink", "Ready for Application", "Card Ready", "Card Dispatched" };

            ViewData["Legend"] = "Report";
            ViewData["Colors"] = colors;
            ViewData["Labels"] = cardstatus;
            ViewData["Data"] = dataStatus;
        }
    }
}
