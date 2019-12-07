using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LatinMedia.WEB.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content("Hello Asp.Net Core 2.2");
        }
    }
}