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
using static NuGet.Packaging.PackagingConstants;

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
            var userLevelClaim = User.Claims.FirstOrDefault(c => c.Type == "Level");  // Reclamo personalizado para el nivel
            var userCursoClaim = User.Claims.FirstOrDefault(c => c.Type == "Curso");  // Reclamo personalizado para el curso
            var userPorcentajeClaim = User.Claims.FirstOrDefault(c => c.Type == "Porcentaje");  
            var userPagarClaim = User.Claims.FirstOrDefault(c => c.Type == "Pagar");  

            if (userIdClaim == null || userLevelClaim == null || userCursoClaim == null || userPorcentajeClaim == null || userPagarClaim == null)
            {
                return Unauthorized();
            }

            string userId = userIdClaim.Value;
            string userLevel = userLevelClaim.Value;
            string userCurso = userCursoClaim.Value;
            string userPorcentaje = userPorcentajeClaim.Value;
            string userPagar = userPagarClaim.Value;

            ViewBag.userPorcentaje = userPorcentaje;
            ViewBag.userPagar = userPagar;


            // Obtener todos los folders para el nivel del estudiante
            var allFolders = _adminService.GetFoldersByCurso(userCurso);

            return View(allFolders);
        }

        [HttpGet]
        public IActionResult Nivel(int id)
        {
            var userLevelClaim = User.Claims.FirstOrDefault(c => c.Type == "Level");  // Reclamo personalizado para el nivel

            string userLevel = userLevelClaim.Value;

            var folders = _folderAccessService.GetFolderByLevelStudent(id, userLevel);

            var filteredFolders = folders.Where(f => f.Id_level_reference <= Convert.ToUInt16(userLevel)).ToList();

            var folderCurso = _adminService.GetFolderCursoById(id); 
            ViewBag.FolderCursoId = folderCurso?.Id; 

            var folderLevels = _adminService.GetFolderByLevel(id); 
            ViewBag.name = folderCurso?.Name;
            return View(filteredFolders);
        }



        [HttpGet]
        public IActionResult View(int id, int Nivel)
        {
            ViewBag.id_level = Nivel;
            ViewBag.Id_Folders_level = id;

            var folderLevel = _adminService.GetLevelFolderById(id);
            ViewBag.Folder = folderLevel.Id_Folders_level;

            var folder = _adminService.GetFolderById(id);
            ViewBag.name = folderLevel?.Name;
            //ViewBag.Id_level_reference = folder?.Id_level_reference;


            //var folderCurso = _profesorServices.GetFolderCursoById(id); // Método para obtener el curso por Id
            //ViewBag.FolderCursoId = folderCurso?.Id; // Asignar el Id del curso al ViewBag

            //var folderLevels = _profesorServices.GetFolderByLevel(id); // Método para obtener los niveles de la carpeta
            //ViewBag.name = folderCurso?.Name;



            //var folder = _profesorServices.GetFolderById(id);
            //ViewBag.Folder = folder?.Id_Folders_level;
            var file = _adminService.GetFilesByFolderId(id);

            //if (folder.Id == 0)
            //{
            //    return NotFound();
            //}

            return View(folder);


        }

        [HttpGet]
        public IActionResult SubView(int id, int Nivel)
        {


            var folder = _adminService.GetSubFolderById(id);
            ViewBag.Id = id;
            ViewBag.id_level = Nivel;
            ViewBag.Id_level_reference = folder?.Id_level_reference;

            var file = _adminService.GetFilesByFolderId(id);

            //if (folder.Id == 0)
            //{
            //    return NotFound();
            //}

            return View(folder);


        }

        [HttpGet]
        public IActionResult UploadFile(int id)
        {
            var folderLevel = _adminService.GetLevelFolderById(id);
            ViewBag.Folder = folderLevel.Id_Folders_level;

            //Folder model = new Folder
            //{
            //    ParentFolderId = parentId,
            //    CreatedBy = User.Identity.Name
            //};

            return View();
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
