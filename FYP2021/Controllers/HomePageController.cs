using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using FYP2021.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace FYP2021.Controllers
{
    public class HomePageController : Controller
    {
        
        public IActionResult Index()
        {
            string sql = "SELECT * FROM Announcement";
            DataTable dt = DBUtl.GetTable(sql);
            return View(dt.Rows);
            
        }


    }
}
