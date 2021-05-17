using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// FOR ACCOUNT PURPOSES
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

// ADD using MODEL
using FYP2021.Models;

namespace FYP2021.Controllers

{
    [Authorize(AuthenticationSchemes = "AdminAccount")]
    public class AdminAccountController : Controller
    {
        private const string AUTHSCHEME = "AdminAccount";

        private const string LOGIN_SQL =
        @"SELECT * FROM Admin
         WHERE admin_email = '{0}' 
        AND admin_password = HASHBYTES('SHA1', '{1}')";


        private const string NAME_COL = "Name";

        private const string REDIRECT_CNTR = "Admin";
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
            if (!AuthenticateUser(user.Email, user.Password, out ClaimsPrincipal principal))
            {
                ViewData["Message"] = "Incorrect Email or Password";
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
                        new Claim(ClaimTypes.NameIdentifier, ds.Rows[0]["Email"].ToString()),
                        new Claim(ClaimTypes.Name, ds.Rows[0][NAME_COL].ToString()),
                         }, "Basic"
                      )
                   );
                return true;
            }
            return false;
        }
    }
}