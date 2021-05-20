using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FYP2021.Controllers
{
    public class ReportController : Controller
    {
        // View to the generate report in Admin folder
        public IActionResult PendingForTransitLink()
        {
            return View();
        }
        public IActionResult ReadyForApplication()
        {
            return View();
        }
        public IActionResult CardReady()
        {
            return View();
        }
        public IActionResult CardDispatched()
        {
            return View();
        }
    }
}
