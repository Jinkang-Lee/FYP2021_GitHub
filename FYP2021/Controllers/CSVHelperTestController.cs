using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using FYP2021.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using CsvHelper;
using System.Globalization;

namespace FYP2021.Controllers
{
    public class CSVHelperTestController : Controller
    {
        [HttpGet]
        public IActionResult IndexTest(List<Student> students = null)
        {
            students = students == null ? new List<Student>() : students;

            return View(students);
        }

        [HttpPost]
        public IActionResult IndexTest(IFormFile file, [FromServices] IHostingEnvironment hostingEnvironment)
        {
            #region Upload CSV
            string fileName = $"{hostingEnvironment.WebRootPath}\\files\\{file.FileName}";
            using (FileStream fileStream = System.IO.File.Create(fileName))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }
            #endregion

            var students = this.GetStudentList(file.FileName);
            return IndexTest(students);

        }

        public List<Student> GetStudentList(string fileName)
        {
            List<Student> students = new List<Student>();

            TempData["Msg"] = "Hi!";

            

            
            #region Read CSV
            var path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fileName;
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {

                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {

                    var student = csv.GetRecord<Student>();
                    students.Add(student);

                    string cardstatus = student.CardStatus;
                    string studentemail = student.StudEmail;
                    //string date = csv.GetRecord(student.StudEmail);
                    //DateTime cardstatusdate = DateTime.Parse(csv.GetField(5));

                    //DataTable table = new DataTable();
                    //table.Columns.Add(cardstatus);
                    //table.Columns.Add(studentemail);
                    //table.Columns.Add(date); 

                    //foreach(Student st in students)
                    //{
                    List<Student> list = DBUtl.GetList<Student>("SELECT * FROM Student WHERE student_email = '{0}'", studentemail);

                    if (list.Count > 0)
                    {

                        string update = @"UPDATE Student SET card_status='{0}' WHERE student_email ='{1}'";

                        int result = DBUtl.ExecSQL(update, cardstatus, student.StudEmail);
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
                    }
                    else
                    {
                        TempData["Message"] = "Student Not Found!";
                        TempData["MsgType"] = "warning";

                    }
                }
            }
            #endregion





           

            return students;


        }

    }
}