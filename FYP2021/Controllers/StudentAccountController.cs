using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
// FOR ACCOUNT PURPOSES
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

// ADD using MODEL
using FYP2021.Models;
using Microsoft.AspNetCore.Http;

namespace FYP2021.Controllers

{
    [Authorize(AuthenticationSchemes = "StudentAccount")]
    public class StudentAccountController : Controller
    {
        private const string AUTHSCHEME = "StudentAccount";

        private const string LOGIN_SQL =
        @"SELECT * FROM Student
         WHERE student_email = '{0}' AND passcode = {1}";


        private const string NAME_COL = "student_name";

        //Where to redirect to after sign in
        private const string REDIRECT_CNTR = "StudentAccount";
        private const string REDIRECT_ACTN = "Index";

        private const string LOGIN_VIEW = "Login";

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            TempData["ReturnUrl"] = returnUrl;
            return View(LOGIN_VIEW);
        }





        // FOR LOGIN
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginUser user)
        {

            //Collect student Email from the form
            IFormCollection form = HttpContext.Request.Form;
            string studentEmail = form["Email"].ToString().Trim();



            List<Student> list = DBUtl.GetList<Student>("SELECT * FROM Student WHERE student_email = '{0}' AND attempts > 9", studentEmail);


            //Need to set value to user.Attempts
            //!!ISSUE: Right now user.Attempts always starts with 0!!
            if (user.Email == studentEmail && list.Count > 0)
            {
                //If login attempt is more than 9
                ViewData["Message"] = "You have exceeded the maximum amount of tries! Your account is now locked.";
                ViewData["MsgType"] = "danger";
                return View(LOGIN_VIEW);
            }
            else
            {
                
                //If Sign in attempt fails
                if (!AuthenticateUser(user.Email, user.OTP, out ClaimsPrincipal principal))
                {
                    //+1 to the number of attempts
                    //user.Attempts += 1;

                    string update = @"UPDATE Student SET attempts = attempts +1 WHERE student_email = '{0}'";
                    int res = DBUtl.ExecSQL(update, studentEmail);

                    ViewData["Message"] = "Incorrect Email or OTP";
                    ViewData["MsgType"] = "warning";


                    return View(LOGIN_VIEW);
                }


                //If Sign in attempt pass
                else
                {
                    HttpContext.SignInAsync(
                       AUTHSCHEME,
                       principal,
                   new AuthenticationProperties
                   {
                       IsPersistent = false
                   });

                    //List<Student> list1 = DBUtl.GetList<Student>("SELECT card_status FROM Student WHERE student_email= '{0}'", studentEmail);

                    

                    DataTable list1 = DBUtl.GetTable("SELECT card_status FROM Student WHERE student_email = '{0}'", studentEmail);
                    
                    foreach (DataRow r in list1.Rows)
                    {
                        string studcardstatus = r["card_status"].ToString();
                        if (studcardstatus == "Pending for TransitLink")
                        {
                            return View("Pending");
                        }

                        else if (studcardstatus == "Ready for Application")
                        {
                            return View("ReadyApplication");
                        }

                        else if (studcardstatus == "Card Ready")
                        {
                            return View("CardReady");
                        }

                        else if (studcardstatus == "Card Dispatched")
                        {
                            return View("CardDispatched");
                        }

                    }


                    return RedirectToAction("Index");
                    //return RedirectToAction(REDIRECT_ACTN, REDIRECT_CNTR);
                }
            }
        }


        // FOR LOGOUT
        [Authorize]
        public IActionResult Logoff(string returnUrl = null)
        {
            HttpContext.SignOutAsync(AUTHSCHEME);
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "HomePage");
        }

        [AllowAnonymous]
        public IActionResult Forbidden()
        {
            return View();
        }


        // FOR AUTHENTICATING USERS
        private bool AuthenticateUser(string Email, string OTP, out ClaimsPrincipal principal)
        {
            principal = null;

            DataTable ds = DBUtl.GetTable(LOGIN_SQL, Email, OTP);
            if (ds.Rows.Count == 1)
            {
                principal =
                   new ClaimsPrincipal(
                      new ClaimsIdentity(
                         new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier, ds.Rows[0]["student_email"].ToString()),
                        new Claim(ClaimTypes.Name, ds.Rows[0][NAME_COL].ToString())
                         }, "Basic"
                      )
                   );
                return true;
            }
            return false;
        }







        //Create a Random Variable
        Random rand1 = new Random();

        [AllowAnonymous]
        [HttpPost]
        public IActionResult SendEmailOTP(string email)
        {
            //Random 6 digit pin number
            int pin_num = rand1.Next(000001, 999999);


            IFormCollection form = HttpContext.Request.Form;
            string StudentEmail = form["Email"].ToString().Trim();

            //Insert 6 digit passcode to Student table
            string insert = @"UPDATE Student SET passcode={0} WHERE student_email = '{1}'";
            if (DBUtl.ExecSQL(insert, pin_num, StudentEmail) == 1)
            {
                ViewData["Message"] = "Passcode added";
                ViewData["MsgType"] = "success";
            }

            string select = ("SELECT * FROM Student WHERE student_email = '{0}'");
            List<Student> search = DBUtl.GetList<Student>(select, email);

            if (search.Count == 0)
            {
                ViewData["Message"] = "This Email does not exist! Please Try Again.";
                ViewData["MsgType"] = "warning";
            }
            else
            {
                Student user = search[0];
                string template = @"Login Request has been received,
                               <p>Hello! Your OTP for login is {0}. Click the link below to proceed to login page.</p>
                                    <a href='https://localhost:44383/StudentAccount/Login'>Click Here!</a>
                                                <p>Sincerely, </p>
                                                <p>RP Team</p>";

                string body = String.Format(template, pin_num);
                string subject = "Student Login Request";
                string result;

                if (EmailUtl.SendEmail(StudentEmail, subject, body, out result))
                {
                    ViewData["Message"] = "Email Successfully Sent";
                    ViewData["MsgType"] = "success";
                }
                else
                {
                    ViewData["Message"] = result;
                    ViewData["MsgType"] = "warning";
                }

                

            }
            return View("GetOTP");
        }



        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetOTP()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
