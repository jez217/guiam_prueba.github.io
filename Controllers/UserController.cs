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
            return View(new User());
        }
        #endregion

        #region Login Post
        [HttpPost]
        public async Task<IActionResult> Login(User model)
        {
            var resp = _loginService.ValidateAccess(model);

            if (resp.code == "True")
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, resp.Name),
            new Claim(ClaimTypes.NameIdentifier, resp.idUser.ToString()),
            new Claim("Curso", resp.IdCurso.ToString()), // Tipo de reclamo personalizado
            new Claim("Level", resp.IdLevel.ToString()), // Tipo de reclamo personalizado
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return Json(new { success = true, role = resp.IdRol });
            }

            return Json(new { success = false, message = resp.message });
        }
        #endregion

        #region Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }
        #endregion
    }
}
