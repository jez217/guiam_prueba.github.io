using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pautas.Models.Profesor;
using Pautas.Models.Pautas;
using Pautas.Services.ProfesorService;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Pautas.Models.Login;
using Pautas.Services.Extensions;
using Pautas.Services.Users;

namespace Pautas.Controllers
{
    [Authorize]
    public class ProfesorController : Controller
    {        
FolderAccessService _profesorServices = new FolderAccessService();

        private readonly ILogger<ProfesorController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfesorController(ILogger<ProfesorController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var rootFoldersCurso = _profesorServices.GetRootFoldersCurso();

            // Pasa las carpetas y subcarpetas al ViewBag para que estén disponibles en el layout

            return View(rootFoldersCurso);
        }

        [HttpPost]
        public IActionResult DeleteFile(int id)
        {
            try
            {
                _profesorServices.DeleteFile(id); // Asegúrate de que este método existe en tu servicio
                return Json(new { success = true, message = "¡Archivo eliminado correctamente!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al eliminar el archivo: {ex.Message}" });
            }
        }

        [HttpGet]
        public IActionResult Nivel(int id)
        {
            var folderCurso = _profesorServices.GetFolderCursoById(id); // Método para obtener el curso por Id
            ViewBag.FolderCursoId = folderCurso?.Id; // Asignar el Id del curso al ViewBag

            var folderLevels = _profesorServices.GetFolderByLevel(id); // Método para obtener los niveles de la carpeta
            ViewBag.name = folderCurso?.Name;

            return View(folderLevels);
        }

        [HttpGet]
        public IActionResult CreateFolder()
        {
            string user = User.Identity.Name;
            FoldersLevel modellevel = new FoldersLevel();
            FoldersCurso modelcurso = new FoldersCurso();

            Folder model = new Folder
            {
                Id_Folders_level = modellevel.Id_Folders_level,  // Asume que has agregado estas propiedades en el modelo Folder
                Id_Folder_curso = modellevel.Id_subfolders_curso
            };

            model.CreatedBy = user;


            return View(model);
        }

        [HttpPost]
        public IActionResult CreateFolder(Folder model)
        {
            string user = User.Identity.Name;
            model.CreatedBy = user;

            _profesorServices.CreateFolder(model);

            // Redirigir a la acción de creación de subcarpetas
            return RedirectToAction("View", new { parentId = model.Id_Folders_level });
        }

        [HttpGet]
        public IActionResult CreateSubFolder(int parentId)
        {
            var parentFolder = _profesorServices.GetSubFolderById(parentId);
            ViewBag.ParentFolderName = parentFolder?.Name;

            Folder model = new Folder
            {
                ParentFolderId = parentId,
                CreatedBy = User.Identity.Name
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult CreateSubFolder(Folder model)
        {
            model.CreatedBy = User.Identity.Name;
            _profesorServices.CreateSubFolder(model);

            // Redirigir a la vista de la carpeta padre para ver la estructura completa
            return RedirectToAction("SubView", new { id = model.Id });
        }



        [HttpGet]
        public IActionResult CreateFolderCurso()
        {
            Folder model = new Folder();
            string user = User.Identity.Name;


            model.CreatedBy = user;


            return View(model);
        }

        [HttpPost]
        public IActionResult CreateFolderCurso(FoldersCurso model)
        {
            string user = User.Identity.Name;

            model.CreatedBy = user;
            _profesorServices.CreateFolderCurso(model);

            // Actualizar la lista de carpetas y subcarpetas después de crear la carpeta
            //var rootFolders = _profesorServices.GetRootFoldersCurso(); // Asegúrate de obtener las carpetas correctamente
            //ViewBag.Folders = rootFolders;  // Pasa la lista de carpetas y subcarpetas al ViewBag

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CreateFolderLevel()
        {
            FoldersLevel model = new FoldersLevel();
            string user = User.Identity.Name;


            model.CreatedBy = user;


            return View(model);
        }

        [HttpPost]
        public IActionResult CreateFolderLevel(FoldersLevel model)
        {
            string user = User.Identity.Name;
            model.CreatedBy = user;

            if (model.Id_subfolders_curso == null)
            {
                // Puedes agregar lógica aquí si necesitas capturar el Id_subfolders_curso de otra forma
            }

            _profesorServices.CreateFolderLevel(model);

            return RedirectToAction("Nivel", new { id = model.Id_Folders_level });
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, int id)
        {
            if (file != null && file.Length > 0)
            {
                try
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                    // Verificar si el directorio existe, si no existe, crearlo
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var filePath = Path.Combine(uploadsFolder, file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    _profesorServices.UploadFile(file.FileName, filePath, id);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error al subir el archivo: {ex.Message}");
                    ModelState.AddModelError("", "Ocurrió un error al subir el archivo.");
                }
            }

            return RedirectToAction("View", new { id = id });
        }

        [HttpPost]
        public async Task<IActionResult> UploadSubFile(IFormFile file, int Id)
        {
            if (file != null && file.Length > 0)
            {
                try
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                    // Verificar si el directorio existe, si no existe, crearlo
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var filePath = Path.Combine(uploadsFolder, file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    _profesorServices.UploadSubFile(file.FileName, filePath, Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error al subir el archivo: {ex.Message}");
                    ModelState.AddModelError("", "Ocurrió un error al subir el archivo.");
                }
            }

            return RedirectToAction("SubView", new { id = Id });
        }

        //[HttpGet]
        //public IActionResult View(int id)
        //{
        //    var folderCurso = _profesorServices.GetFolderCursoById(id); // Método para obtener el curso por Id
        //    ViewBag.FolderCursoId = folderCurso?.Id; // Asignar el Id del curso al ViewBag

        //    var folderLevels = _profesorServices.GetFolderByLevel(id); // Método para obtener los niveles de la carpeta

        //    ViewBag.name = folderCurso?.Name;



        //    var model = new Models.Profesor.ImageModel
        //    {
        //        Listamages = _profesorServices.GetFilesByFolderId(id)
        //    };

        //    ViewBag.Files = _profesorServices.GetFilesByFolderId(id);
        //    return View(model);
        //}


        //[HttpGet]
        //public IActionResult View(int id)
        //{

        //    var folder = _profesorServices.GetFolderById(id);
        //    var file = _profesorServices.GetFilesByFolderId(id);

        //    //if (folder.Id == 0)
        //    //{
        //    //    return NotFound();
        //    //}

        //    return View(folder);
        //}


        [HttpGet]
        public IActionResult UploadFile(int id)
        {
            var folderLevel = _profesorServices.GetLevelFolderById(id);
            ViewBag.Folder = folderLevel.Id_Folders_level;

            //Folder model = new Folder
            //{
            //    ParentFolderId = parentId,
            //    CreatedBy = User.Identity.Name
            //};

            return View();
        }

        [HttpGet]
        public IActionResult View(int id)
        {

            var folderLevel = _profesorServices.GetLevelFolderById(id);
            ViewBag.Folder = folderLevel.Id_Folders_level;

            var folder = _profesorServices.GetFolderById(id);
            ViewBag.name = folderLevel?.Name;


            //var folderCurso = _profesorServices.GetFolderCursoById(id); // Método para obtener el curso por Id
            //ViewBag.FolderCursoId = folderCurso?.Id; // Asignar el Id del curso al ViewBag

            //var folderLevels = _profesorServices.GetFolderByLevel(id); // Método para obtener los niveles de la carpeta
            //ViewBag.name = folderCurso?.Name;



            //var folder = _profesorServices.GetFolderById(id);
            //ViewBag.Folder = folder?.Id_Folders_level;
            var file = _profesorServices.GetFilesByFolderId(id);

            //if (folder.Id == 0)
            //{
            //    return NotFound();
            //}

            return View(folder);


        }

        [HttpGet]
        public IActionResult SubView(int id)
        {


            var folder = _profesorServices.GetSubFolderById(id);
            ViewBag.Id = id;


            var file = _profesorServices.GetFilesByFolderId(id);

            //if (folder.Id == 0)
            //{
            //    return NotFound();
            //}

            return View(folder);


        }

        //[HttpGet]
        //public IActionResult View(int id)
        //{
        //    var folder = _profesorServices.GetFolderById(id);
        //    // var subFolders = _profesorServices.GetSubFoldersByParentId(id); // Método para obtener subcarpetas
        //    var files = _profesorServices.GetFilesByFolderId(id);

        //    //var model = new FolderViewModel
        //    //{
        //    //    Folder = folder,
        //    //    SubFolders = subFolders,
        //    //    Files = files
        //    //};

        //    return View(model);
        //}

        [HttpPost]
        public IActionResult RenameFolder(int id, string name)
        {
            try
            {
                _profesorServices.RenameFolder(id, name);
                var folder = _profesorServices.GetFolderById(id); // Obtener la carpeta actualizada
                return Json(new { success = true, message = "Carpeta renombrada exitosamente", folderName = folder.Name });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al renombrar la carpeta: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult DeleteFolder(int id)
        {
            try
            {
                _profesorServices.DeleteFolder(id);
                return Json(new { success = true, message = "¡Carpeta eliminada correctamente!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al eliminar la carpeta: {ex.Message}" });
            }
        }

        //public IActionResult DownloadFile(int id)
        //{
        //    var file = _profesorServices.GetFileById(id);
        //    if (file == null)
        //    {
        //        return NotFound();
        //    }

        //    var fileBytes = System.IO.File.ReadAllBytes(file.FilePath);
        //    return File(fileBytes, "application/octet-stream", file.Name);
        //}
    }
}
