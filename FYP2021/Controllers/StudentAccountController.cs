﻿//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//// FOR ACCOUNT PURPOSES
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using System.Security.Claims;

//// ADD using MODEL
//using FYP2021.Models;

//namespace FYP2021.Controllers

//{
//    [Authorize(AuthenticationSchemes = "StudentAccount")]
//    public class StudentAccountController : Controller
//    {
//        private const string AUTHSCHEME = "StudentAccount";

//        private const string LOGIN_SQL =
//        @"SELECT * FROM Student
//         WHERE student_email = '{0}'";
//        //NEED TO THINK ABOUT HOW TO DO THE 6 DIGIT PIN THING


//        private const string ROLE_COL = "Role";
//        private const string NAME_COL = "student_email";

//        private const string REDIRECT_CNTR = "Student";
//        private const string REDIRECT_ACTN = "Index";

//        private const string LOGIN_VIEW = "Login";

//        [AllowAnonymous]
//        public IActionResult Login(string returnUrl = null)
//        {
//            TempData["ReturnUrl"] = returnUrl;
//            return View(LOGIN_VIEW);
//        }



//        // FOR LOGIN
//        [AllowAnonymous]
//        [HttpPost]
//        public IActionResult Login(LoginUser user)
//        {
//            if (!AuthenticateUser(user.Email, user.Password, out ClaimsPrincipal principal))
//            {
//                ViewData["Message"] = "Incorrect Email or Password";
//                ViewData["MsgType"] = "warning";
//                return View(LOGIN_VIEW);
//            }

//            //Sign in using the assigned AUTH SCHEME
//            else
//            {
//                HttpContext.SignInAsync(
//                   AUTHSCHEME,
//                   principal,
//               new AuthenticationProperties
//               {
//                   IsPersistent = false
//               });

//                return RedirectToAction(REDIRECT_ACTN, REDIRECT_CNTR);
//            }
//        }


//        // FOR LOGOUT
//        [Authorize]
//        public IActionResult Logoff(string returnUrl = null)
//        {
//            HttpContext.SignOutAsync(AUTHSCHEME);
//            if (Url.IsLocalUrl(returnUrl))
//                return Redirect(returnUrl);
//            return RedirectToAction(REDIRECT_ACTN, REDIRECT_CNTR);
//        }

//        [AllowAnonymous]
//        public IActionResult Forbidden()
//        {
//            return View();
//        }


//        // FOR AUTHENTICATING USERS
//        private bool AuthenticateUser(string uid, string pw, out ClaimsPrincipal principal)
//        {
//            principal = null;

//            DataTable ds = DBUtl.GetTable(LOGIN_SQL, uid, pw);
//            if (ds.Rows.Count == 1)
//            {
//                principal =
//                   new ClaimsPrincipal(
//                      new ClaimsIdentity(
//                         new Claim[] {
//                        new Claim(ClaimTypes.NameIdentifier, ds.Rows[0]["Id"].ToString()),
//                        new Claim(ClaimTypes.Name, ds.Rows[0][NAME_COL].ToString()),
//                        new Claim(ClaimTypes.Role, ds.Rows[0][ROLE_COL].ToString())
//                         }, "Basic"
//                      )
//                   );
//                return true;
//            }
//            return false;
//        }
//    }
//}
