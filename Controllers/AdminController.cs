using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pautas.Models;
using Pautas.Models.Admin;
using Pautas.Models.Login;
using Pautas.Models.Pautas;
using Pautas.Services.Extensions;
using Pautas.Services.Pauta;
using Pautas.Services.Users;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

namespace Pautas.Controllers
{
    public class AdminController : Controller
    {
        AdminServices _adminservice = new AdminServices();
        LoginServices _loginService = new LoginServices();

        private readonly ILogger<AdminController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(ILogger<AdminController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }


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

            if (resp.code == "success")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, resp.Name),
                    // Agrega otros claims según sea necesario
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Admin", "User");
            }

            return View(model); // O redirigir a la pantalla de inicio de sesión nuevamente
        }
        #endregion

        #region Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }
        #endregion

        [Authorize]
        public IActionResult Admin()
        {
            User model = new User();

            model.ROLDROPDOWNS = _adminservice.RolDropdown();
            model.ListaUserDetail = _adminservice.SP_USER_SELECT();

            return View(model);
        }

        #region <<<Crear Pauta>>>
        [HttpGet]
        public IActionResult Create()
        {
            User user = new User();


            //model.MONTHDROPDOWNS = _adminservice.DropdownMeses();
            //model.STOREDROPDOWNS = _storeservice.StoreDropdown();
            //model.PCDROPDOWNS = _adminservice.ProfitCenterDropdown();
            //model.ANNO = DateTime.Now.Year;
            user.ROLDROPDOWNS = _adminservice.RolDropdown();

            return View(user);
        }
        #endregion

        #region <<<Crear Usuario>>>        
        [HttpPost]
        public IActionResult Create(User model)
        {

            GenericResponse resp = new GenericResponse();
            resp = _adminservice.CreateUser(model);

            if (resp.Resp)
            {
                return Json(new { resp.id, resp.message });
            }
            else
            {
                return RedirectToAction("Create");
            }


        }
        #endregion

        #region <<<Crear Usuario>>>        
        [HttpPost]
        public IActionResult InsertLevelUser(int USERID, int LEVELID)
        {
            Student resp = new Student();

            resp = _adminservice.SP_GM_LEVEL_INSERT( USERID, LEVELID);

            return Json(new { id = resp.id, message = resp.message });

        }
        #endregion

        #region <<<Privacy de Login>>>
        public IActionResult Privacy()
        {
            User userAuth = HttpContext.Session.GetObject<User>("Name");

            if (userAuth == null)
            {
                return RedirectToAction("Login", "User");
            }

            return View();
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

