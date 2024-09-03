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
using Pautas.Services.ProfesorService;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Pautas.Models.Profesor;

namespace Pautas.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly FolderAccessService _adminService;
        private readonly LoginServices _loginService;
        private readonly FolderAccessService _folderAccessService;
        private readonly ILogger<StudentController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StudentController(ILogger<StudentController> logger, IWebHostEnvironment webHostEnvironment, FolderAccessService folderAccessService, FolderAccessService profesorServices)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _folderAccessService = folderAccessService;
            _adminService =  profesorServices; // Aquí inicializa tu servicio ProfesorServices correctamente
            _loginService = new LoginServices();
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
        [AllowAnonymous]
        public async Task<IActionResult> Login(User model)
        {
            var resp = _loginService.ValidateAccess(model);

            if (resp.code == "success")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, resp.Name),
                    //new Claim(ClaimTypes.Role, resp.Role) // Asumiendo que `Role` es el nivel del usuario
                    // Agrega otros claims según sea necesario
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index");
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
        public IActionResult Index()
        {
            // Obtener el ID y nivel del usuario desde los claims

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var userLevelClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            var userCursoClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            if (userIdClaim == null || userCursoClaim == null)
            {
                return Unauthorized();
            }

            string userId = userIdClaim.Value;
            string userCurso = userCursoClaim.Value;

            // Obtener todos los folders para el nivel del estudiante
            var allFolders = _adminService.GetFoldersByLevel(userCurso);

        


            return View(allFolders);
        }

        public IActionResult View(int id)
        {
            Models.Profesor.Folder folder = _adminService.GetFolderById(id);
            if (folder == null)
            {
                return NotFound();
            }

            folder.Listamages = _adminService.GetFilesByFolderId(id);

            return View(folder);
        }

        [HttpGet]
        public IActionResult Nivel(int id)
        {
            var folderCurso = _adminService.GetFolderCursoById(id); // Método para obtener el curso por Id
            ViewBag.FolderCursoId = folderCurso?.Id; // Asignar el Id del curso al ViewBag

            var folderLevels = _adminService.GetFolderByLevel(id); // Método para obtener los niveles de la carpeta
            ViewBag.name = folderCurso?.Name;

            return View(folderLevels);
        }

        //[Authorize]
        //public async Task<IActionResult> AccessFolder(int folderId)
        //{
        //    // Obtener el ID del usuario desde los claims
        //    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        //    if (userIdClaim == null)
        //    {
        //        return Unauthorized();
        //    }

        //    int userId = int.Parse(userIdClaim.Value);

        //    // Verificar el acceso a la carpeta usando el servicio
        //    bool hasAccess = await _folderAccessService.HasAccessToFolder(userId, folderId);

        //    if (hasAccess)
        //    {
        //        // Lógica para mostrar el contenido de la carpeta
        //        return View("FolderView", folderId); // Suponiendo que tienes una vista FolderView
        //    }
        //    else
        //    {
        //        return Forbid();
        //    }
        //}

        #region Privacy de Login
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
