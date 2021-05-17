using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FYP2021.Controllers
{
    public class CardstatusController : Controller
    {
        public IActionResult ListCard()
        {
            return View();
        }
    }
}
