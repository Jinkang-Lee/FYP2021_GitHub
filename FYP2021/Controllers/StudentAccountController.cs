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
         WHERE student_email = '{0}'";
        //NEED TO THINK ABOUT HOW TO DO THE 6 DIGIT PIN THING


        private const string ROLE_COL = "Role";
        private const string NAME_COL = "student_email";

        private const string REDIRECT_CNTR = "Student";
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
            if (!AuthenticateUser(user.Email, user.OTP, out ClaimsPrincipal principal))
            {
                ViewData["Message"] = "Incorrect Email";
                ViewData["MsgType"] = "warning";
                return View(LOGIN_VIEW);
            }

            //Sign in using the assigned AUTH SCHEME
            else
            {
                HttpContext.SignInAsync(
                   AUTHSCHEME,
                   principal,
               new AuthenticationProperties
               {
                   IsPersistent = false
               });

                return RedirectToAction(REDIRECT_ACTN, REDIRECT_CNTR);
            }
        }


        // FOR LOGOUT
        [Authorize]
        public IActionResult Logoff(string returnUrl = null)
        {
            HttpContext.SignOutAsync(AUTHSCHEME);
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction(REDIRECT_ACTN, REDIRECT_CNTR);
        }

        [AllowAnonymous]
        public IActionResult Forbidden()
        {
            return View();
        }


        // FOR AUTHENTICATING USERS
        private bool AuthenticateUser(string uid, string pw, out ClaimsPrincipal principal)
        {
            principal = null;

            DataTable ds = DBUtl.GetTable(LOGIN_SQL, uid, pw);
            if (ds.Rows.Count == 1)
            {
                principal =
                   new ClaimsPrincipal(
                      new ClaimsIdentity(
                         new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier, ds.Rows[0]["Id"].ToString()),
                        new Claim(ClaimTypes.Name, ds.Rows[0][NAME_COL].ToString()),
                        new Claim(ClaimTypes.Role, ds.Rows[0][ROLE_COL].ToString())
                         }, "Basic"
                      )
                   );
                return true;
            }
            return false;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult SendEmailOTP(string email)
        {
            //Random 6 digit pin number
            Random rand1 = new Random();
            int pin_num = rand1.Next(000001, 999999);


            IFormCollection form = HttpContext.Request.Form;
            string StudentEmail = form["Email"].ToString().Trim();

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
                               <p>Hello! Your OTP for login is {0}. Click the link below and enter your OTP.</p>
                                    <a href='https://localhost:44383/StudentAccount/EnterOTP?StudEmail={1}'>Click Here!</a>
                                                <p>Sincerely, </p>
                                                <p>RP Team</p>";

                string body = String.Format(template, pin_num, StudentEmail);
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
            return View("Login");
        }

        

        [AllowAnonymous]
        [HttpGet]
        public IActionResult EnterOTP()
        {
            return View();
        }
    }
}
