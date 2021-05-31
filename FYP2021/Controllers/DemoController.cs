using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace FYP2021.Controllers
{
    public class DemoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult SendEmail()
        //{

        //    string sql = "SELECT * FROM Student";
        //    DataTable dt = DBUtl.GetTable(sql);

        //    if(dt.)
        //    {

        //    }
        //    return View(dt.Rows);

        //    string template =
        //    @"Dear {0}, <br/>
        //    <p>Your new card status is {1}.</p>
        //    Sincerely RP Team";

        //    string email = "19044856@myrp.edu.sg";
        //    string cust = "Valerie Teo";
        //    string prod = "Card ready";
        //    string msg = String.Format(template, cust, prod);
        //    string title = "Thank You for Registering";
        //    string result;
        //    EmailUtl.SendEmail(email, title, msg, out result);

            

        //    return View("");
        //}

    }
}
