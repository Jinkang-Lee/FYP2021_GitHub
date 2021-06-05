using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FYP2021.Controllers
{
    public class ChartController : Controller
    {
        public IActionResult Pie()
        {
            ViewData["Chart"] = "pie";
            ViewData["Title"] = "Report Summary";
            ViewData["ShowLegend"] = "true";
            return View("Chart");
        }
    }
}
