using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Security.Claims;
using FYP2021.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace FYP2021.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminAccount")]
    public class AdminAccountController : Controller

    {
        //For signing in
        private const string AUTHSCHEME = "AdminAccount";

        //To check sign in credentials
        private const string LOGIN_SQL =
           @"SELECT * FROM Admin
            WHERE admin_email = '{0}' 
              AND admin_password = HASHBYTES('SHA1', '{1}')";

        //For checking last login 
        private const string LASTLOGIN_SQL =
           @"UPDATE SRUser SET LastLogin=GETDATE() 
                        WHERE Email='{0}'";

        private const string NAME_COL = "admin_name";

        //Where to redirect to after sign in
        private const string REDIRECT_CNTR = "Admin";
        private const string REDIRECT_ACTN = "Index";

        private const string LOGIN_VIEW = "Login";

        //AppdbContext
        private AppDbContext _dbContext;

        public AdminAccountController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            TempData["ReturnUrl"] = returnUrl;
            return View(LOGIN_VIEW);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginUser user)
        {
            if (!AuthenticateUser(user.Email, user.Password, out ClaimsPrincipal principal))
            {
                ViewData["Message"] = "Incorrect Email or Password";
                ViewData["MsgType"] = "warning";
                return View(LOGIN_VIEW);
            }

            //Sign in using the assigned AUTH SCH
            else
            {
                HttpContext.SignInAsync(
                   AUTHSCHEME,
                   principal,
               new AuthenticationProperties
               {
                   IsPersistent = false
               });

                // Update the Last Login Timestamp of the User
                DBUtl.ExecSQL(LASTLOGIN_SQL, user.Email);

                if (TempData["returnUrl"] != null)
                {
                    string returnUrl = TempData["returnUrl"].ToString();
                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                }

                return RedirectToAction(REDIRECT_ACTN, REDIRECT_CNTR);
            }
        }

        [Authorize]
        public IActionResult Logoff(string returnUrl = null)
        {
            //For signing out
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

        private bool AuthenticateUser(string Email, string Password, out ClaimsPrincipal principal)
        {
            principal = null;

            DataTable ds = DBUtl.GetTable(LOGIN_SQL, Email, Password);
            if (ds.Rows.Count == 1)
            {
                principal =
                   new ClaimsPrincipal(
                      new ClaimsIdentity(
                         new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier, ds.Rows[0]["admin_email"].ToString()),
                        new Claim(ClaimTypes.Name, ds.Rows[0][NAME_COL].ToString()),
                         }, "Basic"
                      )
                   );
                return true;
            }
            return false;
        }










        //HTTP GET FORGETPASSWORD
        [AllowAnonymous]
        public IActionResult ForgetPassword()
        {
            return View();
        }


        //HTTP POST FORGET PASSWORD
        [AllowAnonymous]
        [HttpPost]
        public IActionResult ForgetPassword(string email)
        {
            IFormCollection form = HttpContext.Request.Form;
            string AdminEmail = form["Email"].ToString().Trim();

            string select = ("SELECT * FROM Admin WHERE admin_email = '{0}'");
            List<Admin> search = DBUtl.GetList<Admin>(select, email);

            
            if(search.Count == 0)
            {
                ViewData["Message"] = "Email does not exist!";
                ViewData["MsgType"] = "warning";
            }
            else
            {
                Admin user = search[0];
                string template = @"Hi {0},
                               <p>Your request for a password change has been acknowledged. Click the link to change your password.</p>
                                    <a href='http://concessioncard-myrp.azurewebsites.net/AdminAccount/ChangePassword?AdminEmail={1}'>Click Here!</a>";

                string body = String.Format(template, user.AdminName, user.AdminEmail);
                string subject = "Forget Password";
                string result;

                if (EmailUtl.SendEmail(AdminEmail,subject, body, out result))
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
            
            return View();
        }


        //HTTPGET FORGET PASSWORD
        [AllowAnonymous]
        public IActionResult ChangePassword()
        {
            return View();
        }


        [AllowAnonymous]
        public JsonResult VerifyCurrentPassword(string CurrentPassword)
        {
            DbSet<LoginUser> dbs = _dbContext.LoginUser;
            var email = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //Convert to ASCII Byte array first
             var pw_bytes = System.Text.Encoding.ASCII.GetBytes(CurrentPassword);

            LoginUser user = dbs.FromSqlInterpolated($"SELECT * FROM Login WHERE Id = {email} AND Password = HASHBYTES('SHA1', {pw_bytes})").FirstOrDefault();

            //IF NOT NULL
             if (user != null)
                return Json(true);
            else
                return Json(false);
        }


        [AllowAnonymous]
        public JsonResult VerifyNewPassword(string NewPassword)
        {

            DbSet<LoginUser> dbs = _dbContext.LoginUser;
            var email = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //Convert to ASCII Byte array first
             var pw_bytes = System.Text.Encoding.ASCII.GetBytes(NewPassword);

            LoginUser user = dbs.FromSqlInterpolated($"SELECT * FROM AppUser WHERE Id = {email} AND Password = HASHBYTES('SHA1', {pw_bytes})").FirstOrDefault();

            //IF NULL
             if (user == null)
                return Json(true);
            else
                return Json(false);


        }



        //ChangePassword HttpPost


       [HttpPost]
         [Authorize]
        public IActionResult ChangePassword(UpdatePassword pu)
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var npw_bytes = System.Text.Encoding.ASCII.GetBytes(pu.NewPassword);
            var cpw_bytes = System.Text.Encoding.ASCII.GetBytes(pu.CurrentPassword);

            int num = _dbContext.Database.ExecuteSqlInterpolated($"UPDATE AppUser SET Password = HASHBYTES('SHA1', {npw_bytes}) WHERE Id = {email} AND Password = HASHBYTES('SHA1', {cpw_bytes})");


            if (num == 1)
            {
                ViewData["Msg"] = "Password successfully updated!";
            }
            else
            {
                ViewData["Msg"] = "Failed to update password!";
            }

            return View();

        }
    }
}