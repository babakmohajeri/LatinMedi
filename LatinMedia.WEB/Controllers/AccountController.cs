using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LatinMedia.Core.Convertor;
using LatinMedia.Core.Convertors;
using LatinMedia.Core.Generators;
using LatinMedia.Core.Security;
using LatinMedia.Core.Senders;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace LatinMedia.WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _UserService;
        private readonly IViewRenderService _renderViewService;
        public AccountController(IUserService UserService, IViewRenderService renderViewService)
        {
            _UserService = UserService;
            _renderViewService = renderViewService;
        }

        #region Register Actions

        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(AccountViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            if (await _UserService.IsExistMobileAsync(register.Mobile))
            {
                ModelState.TryAddModelError("Mobile", "موبایل وارد شده تکراری می باشد");
                return View(register);
            }

            if (_UserService.IsExistEmail(FixedText.FixedEmail(register.Email)))
            {
                ModelState.TryAddModelError("Email", "ایمیل وارد شده تکراری می باشد");
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
            await _UserService.AddUserAsync(user);


            #region Sending Activation Email

            string body = _renderViewService.RenderToStringAsync("_ActiveEmail", user);
            SendEmail.Send(user.Email, "فعال سازی حساب کاربری در بافر شاپ", body);

            #endregion

            return View("SuccessRegister", model: user);
        }

        #endregion

        #region Login

        [Route("Login")]
        [HttpGet]
        public IActionResult Login( bool EditProfile = false)
        {
            ViewBag.EditProfile = EditProfile;
            return View();
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            if (await _UserService.LoginUser(login.Email, login.Password) == null)
            {
                ModelState.AddModelError("Email", "ایمیل یا کلمه عبور اشتباه است");
                return View(login);
            }
            else
            {
                var user = await _UserService.LoginUser(login.Email, login.Password);
                if (!user.IsActive)
                {
                    ModelState.AddModelError("Email", "حساب کاربری شما فعال نشده است");
                    return View(login);
                }
                else
                {
                    var claim = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                        new Claim(ClaimTypes.Name, (user.FirstName + " " + user.LastName)),
                        new Claim(ClaimTypes.Email, user.Email)
                    };
                    var identity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties()
                    {
                        IsPersistent = login.RememberMe
                    };
                    await HttpContext.SignInAsync(principal, properties);

                    ViewBag.IsSuccess = true;
                    return View();
                }
            }

        }
        #endregion

        #region Logout
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
        #endregion

        #region ActivateAccount
        public async Task<IActionResult> ActiveAccount(string id)
        {
            ViewBag.IsActivated = await _UserService.ActivateAccount(id);
            return View();
        }
        #endregion

        #region Forgot Password

        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [Route("ForgotPassword")]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPassViewModel forgotModel)
        {
            if (!ModelState.IsValid)
            {
                return View(forgotModel);
            }

            var user = new User();
            string userEmail = FixedText.FixedEmail(forgotModel.Email);
            if ((user = await _UserService.ForgotPassword(userEmail)) == null)
            {
                ModelState.AddModelError("Email", "ایمیل وارد شده یافت نشد");
                return View(forgotModel);
            }
            else
            {
                string bodyEmail = _renderViewService.RenderToStringAsync("_ForgotPassword", user);
                SendEmail.Send(userEmail, "ایمیل تایید فراموشی رمز عبور", bodyEmail);
                ViewBag.IsSuccess = true;
                return View();
                // return View("ChangePassEmailConfirmation", model: user);
            }
        }

        public IActionResult ResetPassword(string id)
        {
            return View(new ResetPasswordVewModel() {
                ActiveCode=id
            });
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordVewModel reset)
        {
            if (!ModelState.IsValid)
            {
                return View(reset);
            }

            User user = _UserService.GetUserByActiveCode(reset.ActiveCode);
            if (user == null)
            {
                ModelState.AddModelError("Password", "کاربر پیدا نشد");
                return View(reset);
            }

            string HashNewPassword = PasswordHelper.EncodePasswordMd5(reset.Password);
            user.Password = HashNewPassword;
            _UserService.UpdateUser(user);

            return RedirectToAction("Login");
        }

        #endregion
    }
}