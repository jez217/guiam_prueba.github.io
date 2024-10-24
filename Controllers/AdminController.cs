﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pautas.Models;
using Pautas.Models.Admin;

using Pautas.Models.Login;
using Pautas.Models.Pautas;
using Pautas.Services.Extensions;
using Pautas.Services.Pauta;
using Pautas.Services.ProfesorService;
using Pautas.Services.Users;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pautas.Controllers
{        
[Authorize]
    public class AdminController : Controller
    {
        AdminService _adminservice = new AdminService();
        LoginServices _loginService = new LoginServices();
        private readonly FolderAccessService _profesorServices;

        private readonly ILogger<AdminController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        FolderAccessService folderAccessService;

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
                    new Claim("IdRol", resp.IdRol.ToString()),             
                    new Claim("Curso", resp.IdCurso.ToString())

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
            var userNameClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            string username = userNameClaim.Value;

            ViewBag.Name = username;

            model.ROLDROPDOWNS = _adminservice.RolDropdown();
            model.ListaUserDetail = _adminservice.SP_USER_SELECT();
            model.LEVELDROPDOWNS = _adminservice.LevelDropdown();
            model.CURSODROPDOWNS = _adminservice.CursoDropdown();

            return View(model);
        }


        public IActionResult Calendario()
        {

            return View();
        }

        #region Create User GET
        [HttpGet]
        public IActionResult Create()
        {
            User user = new User();
            user.ROLDROPDOWNS = _adminservice.RolDropdown();
            user.LEVELDROPDOWNS = _adminservice.LevelDropdown();

            return View(user);
        }
        #endregion

        #region Create User POST
        [HttpPost]
        public IActionResult Create(User model)
        {
            GenericResponse resp = new GenericResponse();
            resp = _adminservice.CreateUser(model);

            if (resp.code == "success")
            {
                return Json(new { message = resp.message, code = resp.code });
            }
            else
            {
                return Json(new { message = resp.message, code = resp.code });
            }
        }
        #endregion

        #region Insert Level User POST
        [HttpPost]
        public IActionResult InsertLevelUser(int USERID, int LEVELID)
        {
            Student resp = new Student();
            resp = _adminservice.SP_GM_LEVEL_INSERT(USERID, LEVELID);
            return Json(new { id = resp.id, message = resp.message });
        }
        #endregion

        #region Edit User GET
        [HttpGet]
        public IActionResult Edit(int id, string mensaje)
        {
            //User user = new User();

            User model = _adminservice.SP_USER_SELECT_BY_ONE(id);
            model.ROLDROPDOWNS = _adminservice.RolDropdown();
            model.LEVELDROPDOWNS = _adminservice.LevelDropdown();
            model.CURSODROPDOWNS = _adminservice.CursoDropdown();

            return View(model);
        }

        #endregion

        #region Update User POST
        [HttpPost]
        public IActionResult Edit(User model)
        {
            var resp = _adminservice.UpdateUser(model);

            if (resp.code == "success")
            {
                return Json(new { message = resp.message, code = resp.code });
            }
            else
            {
                return Json(new { message = resp.message, code = resp.code });
            }
        }
        #endregion

        #region Delete User GET
        [HttpGet]
        public IActionResult Delete(int id)
        {
            User user = _adminservice.SP_USER_SELECT_BY_ONE(id);
            return View(user);
        }
        #endregion

        #region Delete User POST
        [HttpPost]
        public IActionResult DeleteUserConfirmed(int userId)
        {
            var resp = _adminservice.DeleteUser(userId);

            if (resp.code == "success")
            {
                return Json(new { message = resp.message, code = resp.code });
            }
            else
            {
                return Json(new { message = resp.message, code = resp.code });
            }
        }
        #endregion

        #region Privacy de Login
        public IActionResult Privacy()
        {
            User userAuth = HttpContext.Session.GetObject<User>("Name");
            User rolAuth = HttpContext.Session.GetObject<User>("IdRol");
            User cursoAuth = HttpContext.Session.GetObject<User>("Curso");


            if (userAuth == null || rolAuth == null)
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
