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
    public class CardstatusController : Controller
    {
        public IActionResult ListCard()
        {
            string sql = "SELECT * FROM Student";
            DataTable dt = DBUtl.GetTable(sql);
            return View(dt.Rows);
        }
    }
}
