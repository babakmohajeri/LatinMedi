using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LatinMedia.Core.Convertor;
using LatinMedia.Core.Generators;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LatinMedia.WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _UserService;
        public AccountController(IUserService UserService)
        {
            _UserService = UserService;
        }

        #region Register

        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            if (_UserService.IsExistEmail(FixedText.FixedEmail(register.Email)))
            {
                ModelState.TryAddModelError("Email", "ایمیل وارد شده تکراری می باشد");
                return View(register);
            }

            if (await _UserService.IsExistMobileAsync(register.Mobile))
            {
                ModelState.TryAddModelError("Mobile", "موبایل وارد شده تکراری می باشد");
                return View(register);
            }

            User user = new User()
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                Mobile = register.Mobile,
                Email = FixedText.FixedEmail(register.Email),
                IdDelete = false,
                AcrivationCode = GeneratorName.GenerateGUID(),
                IsActive = false,
                Password = PasswordHelper.EncodePasswordMd5(register.Password),
                UserAvatar = "default.png",
                CreateDate = DateTime.Now
            };
            await _UserService.SaveAsync();

            return View();
        }
    }

    #endregion

}
