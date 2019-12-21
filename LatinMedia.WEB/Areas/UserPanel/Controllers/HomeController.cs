using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LatinMedia.WEB.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class HomeController : Controller
    {
        IUserService _userService;
        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View(_userService.UserInformation(User.Identity.GetEmail()));
        }

        [HttpGet]
        [Route("UserPanel/EditProfile")]
        public IActionResult EditProfile()
        {
            var user = _userService.GetUserInformationForEdit(User.Identity.GetEmail());
            return View(user);
        }

        [HttpPost]
        [Route("UserPanel/EditProfile")]
        public IActionResult EditProfile(EditUserProfileViewModel userProfile)
        {
            if (!ModelState.IsValid)
            {
                return View(userProfile);
            }
            _userService.EditProfile(User.Identity.GetEmail(), userProfile);

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login?EditProfile=true");
        }

        [HttpGet]
        [Route("UserPanel/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Route("UserPanel/ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordViewModel newPass)
        {
            if (!ModelState.IsValid)
            {
                return View(newPass);
            }
            _userService.ChangePassword(User.Identity.GetEmail(), newPass);
            return Redirect("/Login");
          }
    }
}