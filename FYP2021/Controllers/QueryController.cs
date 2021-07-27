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
        public IActionResult Pending()
        {
            return View();
        }

        public IActionResult ReadyApplciation()
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
