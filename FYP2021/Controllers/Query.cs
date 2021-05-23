using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP2021.Controllers
{
    public class Query : Controller
    {
        public IActionResult QueryCardStatus()
        {
            return View();
        }
    }
}
