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

            List<Student> list = DBUtl.GetList<Student>("SELECT * FROM Student");

            //if (list.Count > 0)
            //{

            //    foreach (Student stud in list)
            //    {

            //        string dateInString = stud.CardStatusDate;

            //        DateTime startDate = DateTime.Parse(dateInString);
            //        DateTime expiryDate = startDate.AddDays(90);

            //        if (DateTime.Now > expiryDate)
            //        {
            //            string delete = "DELETE FROM Student";
            //            DBUtl.ExecSQL(delete);

            //            TempData["Message"] = "Student Found!";
            //            TempData["MsgType"] = "success";
            //        }

            //        else
            //        {
            //            TempData["Message"] = "Student Not Found!";
            //            TempData["MsgType"] = "warning";
            //        }


            //    }
            //    //TempData["Message"] = "Student Found!";
            //    //TempData["MsgType"] = "success";
            //}

            //else
            //{
            //    TempData["Message"] = "Student Not Found!";
            //    TempData["MsgType"] = "warning";
            //}

            DataTable dt = DBUtl.GetTable("SELECT * FROM Student");
            return View("ListCard", dt.Rows);



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
                return View("Index");
            }
            else
            {

                //Pending for Transitlink
                if (student.CardStatus == "Pending for TransitLink")
                {
                    string update = @"UPDATE Student SET card_status='{0}', cardstatus_date ='{1}', pending_date ='{1}' WHERE student_Id={2}";



                    int result = DBUtl.ExecSQL(update, student.CardStatus, student.CardStatusDate, student.Id);


                    if (result == 1)
                    {

                        TempData["Message"] = "Card Status Updated";
                        TempData["MsgType"] = "success";

                        string email = form["StudEmail"].ToString().Trim();
                        string cardStatus = form["CardStatus"].ToString().Trim();
                        string subject = "New Card Status";

                        string template =
                                @"Dear student, <br/>
                                    <p>Your new updated card status is {0}. For more information, visit the website.</p>
                                    Sincerely RP Team";

                        string msg = String.Format(template, cardStatus);
                        string res;
                        if (EmailUtl.SendEmail(email, subject, msg, out res))
                        {

                            ViewData["Message"] = "Email Successfully Sent!";
                            ViewData["MsgType"] = "success";
                        }

                        else
                        {
                            ViewData["Message"] = result;
                            ViewData["MsgType"] = "warning";
                        }

                        return View("UpdateOptions");
                    }


                }

                //Ready for Application
                else if (student.CardStatus == "Ready for Application")
                {
                    string update = @"UPDATE Student SET card_status='{0}', cardstatus_date ='{1}', readyapplication_date ='{1}' WHERE student_Id={2}";



                    int result = DBUtl.ExecSQL(update, student.CardStatus, student.CardStatusDate, student.Id);


                    if (result == 1)
                    {

                        TempData["Message"] = "Card Status Updated";
                        TempData["MsgType"] = "success";

                        string email = form["StudEmail"].ToString().Trim();
                        string cardStatus = form["CardStatus"].ToString().Trim();
                        string subject = "New Card Status";

                        string template =
                                @"Dear student, <br/>
                                    <p>Your new updated card status is {0}. For more information, visit the website.</p>
                                    Sincerely RP Team";

                        string msg = String.Format(template, cardStatus);
                        string res;
                        if (EmailUtl.SendEmail(email, subject, msg, out res))
                        {

                            ViewData["Message"] = "Email Successfully Sent!";
                            ViewData["MsgType"] = "success";
                        }

                        else
                        {
                            ViewData["Message"] = result;
                            ViewData["MsgType"] = "warning";
                        }

                        return View("UpdateOptions");
                    }


                }

                //Card Ready
                else if (student.CardStatus == "Card Ready")
                {
                    string update = @"UPDATE Student SET card_status='{0}', cardstatus_date ='{1}', cardready_date ='{1}' WHERE student_Id={2}";



                    int result = DBUtl.ExecSQL(update, student.CardStatus, student.CardStatusDate, student.Id);


                    if (result == 1)
                    {

                        TempData["Message"] = "Card Status Updated";
                        TempData["MsgType"] = "success";

                        string email = form["StudEmail"].ToString().Trim();
                        string cardStatus = form["CardStatus"].ToString().Trim();
                        string subject = "New Card Status";

                        string template =
                                @"Dear student, <br/>
                                    <p>Your new updated card status is {0}. For more information, visit the website.</p>
                                    Sincerely RP Team";

                        string msg = String.Format(template, cardStatus);
                        string res;
                        if (EmailUtl.SendEmail(email, subject, msg, out res))
                        {

                            ViewData["Message"] = "Email Successfully Sent!";
                            ViewData["MsgType"] = "success";
                        }

                        else
                        {
                            ViewData["Message"] = result;
                            ViewData["MsgType"] = "warning";
                        }

                        return View("UpdateOptions");
                    }


                }

                //Card Dispatched
                else if (student.CardStatus == "Card Dispatched")
                {
                    string update = @"UPDATE Student SET card_status='{0}', cardstatus_date ='{1}', carddispatched_date ='{1}' WHERE student_Id={2}";



                    int result = DBUtl.ExecSQL(update, student.CardStatus, student.CardStatusDate, student.Id);


                    if (result == 1)
                    {

                        TempData["Message"] = "Card Status Updated";
                        TempData["MsgType"] = "success";

                        string email = form["StudEmail"].ToString().Trim();
                        string cardStatus = form["CardStatus"].ToString().Trim();
                        string subject = "New Card Status";

                        string template =
                                @"Dear student, <br/>
                                    <p>Your new updated card status is {0}. For more information, visit the website.</p>
                                    Sincerely RP Team";

                        string msg = String.Format(template, cardStatus);
                        string res;
                        if (EmailUtl.SendEmail(email, subject, msg, out res))
                        {

                            ViewData["Message"] = "Email Successfully Sent!";
                            ViewData["MsgType"] = "success";
                        }

                        else
                        {
                            ViewData["Message"] = result;
                            ViewData["MsgType"] = "warning";
                        }

                        return View("UpdateOptions");
                    }


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

        //public IActionResult Delete(Student student)
        //{
        //    List<Student> list = DBUtl.GetList<Student>("SELECT cardstatus_date FROM Student");

        //    if (list.Count == 1)
        //    {
        //        foreach (Student stud in list)
        //        {
        //            DateTime startDate = DateTime.Parse(stud.CardStatusDate);
        //            DateTime expiryDate = startDate.AddDays(90);

        //            if (DateTime.Now > expiryDate)
        //            {
        //                string delete = "DELETE FROM Student";
        //                DBUtl.ExecSQL(delete);
        //            }
        //        }
        //    }

        //    DateTime startDate = DateTime.Parse(student.CardStatusDate);
        //    DateTime expiryDate = startDate.AddDays(90);
        //    DateTime exDate = DateTime.Parse(student.CardStatusDate).AddDays(1);
        //    if (DateTime.Now > expiryDate)
        //    {
        //        string delete = "DELETE FROM Student WHERE cardstatus_date < {0}";
        //        DBUtl.ExecSQL(delete);
        //    }

        //    if (DBUtl.ExecSQL(@"DELETE FROM Student WHERE cardstatus_date < {0}", exDate) == 1)
        //        return View("ListCard");
        //    else
        //        return View("ListCard");

        //    return View("ListCard");

        //}
    }
}