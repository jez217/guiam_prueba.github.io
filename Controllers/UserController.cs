using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pautas.Models.Login;
using Pautas.Services.Extensions;
using Pautas.Services.Pauta;
using Pautas.Services.Users;
using System.Security.Claims;

namespace Pautas.Controllers
{
    public class UserController : Controller
    {
        LoginServices _loginService = new LoginServices();

        #region Login Get
        [HttpGet]
        public IActionResult Login()
        {
            User model = new User();

            return View(model);

        }
        #endregion

        #region Login Post
        [HttpPost]
        public IActionResult Login(User model)
        {
            var resp = _loginService.ValidateAccess(model);

            if (resp.code == "success")
            {
                HttpContext.Session.SetObject("Name", resp);

            }

            return Json(new { resp });
        }
        #endregion

        #region Logout
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }
        #endregion

       
    }
}
