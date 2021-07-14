using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
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

        private List<Student> GetStudentList(string fileName)
        {
            List<Student> students = new List<Student>();

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



                    string sql = @"INSERT INTO Student
                                            (student_id, student_email, student_name, ph_num, card_status, cardstatus_date, attempts )
                                            VALUES ('{0}', '{1}', {2}, '{3}', '{4}', '{5}', '{6}')";

                    TempData["Msg"] = "New student!";

                    if (DBUtl.ExecSQL(sql, student.Id, student.StudEmail, student.StudName, student.StudPhNum, student.CardStatus, student.CardStatusDate) == 1)
                    {
                        TempData["Msg"] = "New student Added!";
                    }

                    else
                    {
                        TempData["Msg"] = "Invalid information entered!";
                        TempData["MsgType"] = "warning";
                    }

                    TempData["Msg"] = "New!";
                    //foreach(var student in Model)
                    //{
                    //    string sql = @"INSERT INTO Student
                    //                        (student_id, student_email, student_name, ph_num, card_status, cardstatus_date, attempts )
                    //                        VALUES ('{0}', '{1}', {2}, '{3}', '{4}', '{5}', '{6}')";

                    //    if (DBUtl.ExecSQL(sql, student.Id, student.StudEmail, student.StudName, student.StudPhNum, student.CardStatus, student.CardStatusDate) == 1)
                    //    {
                    //        TempData["Msg"] = "New student Added!";
                    //    }

                    //    else
                    //    {
                    //        TempData["Msg"] = "Invalid information entered!";

                    //    }
                    //}




                    //List<Student> list = DBUtl.GetList<Student>("SELECT * FROM Student WHERE student_email = {0}", student.StudEmail);

                    //if (list.Count > 0)
                    //{

                    //    string update = @"UPDATE Student SET card_status='{0}', cardstatus_date ='{1}' WHERE student_email ='{2}'";

                    //    int result = DBUtl.ExecSQL(update, student.CardStatus, student.CardStatusDate, student.StudEmail);
                    //    if (result == 1)
                    //    {
                    //        TempData["Message"] = "Card Status Updated";
                    //        TempData["MsgType"] = "success";
                    //    }

                    //    else
                    //    {
                    //        TempData["Message"] = DBUtl.DB_Message;
                    //        TempData["MsgType"] = "danger";
                    //    }
                    //}
                    //else
                    //{
                    //    TempData["Message"] = "Student Not Found!";
                    //    TempData["MsgType"] = "warning";

                    //}
                }
            }
            #endregion

            //#region Create CSV
            //path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\FilesTo"}";
            //using (var write = new StreamWriter(path + "\\NewFile.csv"))
            //using (var csv = new CsvWriter(write, CultureInfo.InvariantCulture))
            //{
            //    csv.WriteRecords(students);
            //}
            //#endregion

            return students;
        }

    }
}