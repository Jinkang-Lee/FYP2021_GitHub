using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP2021.Models;
using System.Data;

namespace FYP2021.Controllers
{
    public class QueryController : Controller
    {
        public IActionResult QueryCardStatus()
        {
            string sql = "SELECT * FROM Student WHERE student_email = '{0}'";
            DataTable dt = DBUtl.GetTable(sql);
            return View();
        }

        public IActionResult Test()
        {
            return View();
        }
    }
}
