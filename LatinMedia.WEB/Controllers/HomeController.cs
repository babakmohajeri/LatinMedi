using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LatinMedia.WEB.Controllers
{
    public class HomeController : Controller
    {
        IUserService _UserService;
        public HomeController(IUserService UserService)
        {
            _UserService = UserService;
        }


        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [Route("MyTest")]
        public IActionResult Test()
        {
            return View();
        }
    }
}