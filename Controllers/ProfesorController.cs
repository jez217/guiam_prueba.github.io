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
using static NuGet.Packaging.PackagingConstants;

namespace Pautas.Controllers
{
    [Authorize]
    public class ProfesorController : Controller
    {
        private readonly ILogger<ProfesorController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly FolderAccessService _profesorServices;

        public ProfesorController(
            ILogger<ProfesorController> logger,
            IWebHostEnvironment webHostEnvironment,
            FolderAccessService folderAccessService)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _profesorServices = folderAccessService;
        }
        public IActionResult Index()
        {
            var rootFoldersCurso = _profesorServices.GetRootFoldersCurso();

            // Pasa las carpetas y subcarpetas al ViewBag para que estén disponibles en el layout

            return View(rootFoldersCurso);
        }

        [HttpGet]
        public IActionResult Nivel(int id)
        {
            var folderCurso = _profesorServices.GetFolderCursoById(id); // Método para obtener el curso por Id
            ViewBag.FolderCursoId = folderCurso?.Id; // Asignar el Id del curso al ViewBag

            var folderLevels = _profesorServices.GetFolderByLevel(id); // Método para obtener los niveles de la carpeta
            ViewBag.name = folderCurso?.Name;

           var folderLevel = _profesorServices.GetFolderLevelById(id); // Método para obtener los niveles de la carpeta
            ViewBag.id_level = folderLevel.Id_level_reference;


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
            return RedirectToAction("View", new { id = model.Id_Folders_level, Nivel = model.Id_level_reference });
        }

        [HttpGet]
        public IActionResult CreateSubFolder(int id)
        {
            var parentFolder = _profesorServices.GetSubFolderById(id);
            ViewBag.ParentFolderName = parentFolder?.Name;

            Folder model = new Folder
            {
                ParentFolderId = id,
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
            return RedirectToAction("SubView", new { id = model.Id, Nivel = model.Id_level_reference });
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
        public async Task<IActionResult> UploadSubFile(IFormFile file, int Id, int nivel)
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

                    _profesorServices.UploadSubFile(file.FileName, filePath, Id, nivel);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error al subir el archivo: {ex.Message}");
                    ModelState.AddModelError("", "Ocurrió un error al subir el archivo.");
                }
            }

            return RedirectToAction("SubView", new { id = Id, nivel = nivel });
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
        public IActionResult View(int id, int Nivel)
        {
            ViewBag.id_level = Nivel;
            ViewBag.Id_Folders_level = id;

            var folderLevel = _profesorServices.GetLevelFolderById(id);
            ViewBag.Folder = folderLevel.Id_Folders_level;

            var folder = _profesorServices.GetFolderById(id);
            ViewBag.name = folderLevel?.Name;
            //ViewBag.Id_level_reference = folder?.Id_level_reference;


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
        public IActionResult SubView(int id, int Nivel)
        {
            ViewBag.PreviousId = id; // Guardar el ID para usarlo al regresar

            var folderLevel = _profesorServices.GetFolderById(id);
            ViewBag.Id_Folders_level = folderLevel?.Id_Folders_level;

            var folder = _profesorServices.GetSubFolderById(id);            
            ViewBag.name = folderLevel?.Name;

            ViewBag.Id = id;
            ViewBag.id_level = Nivel;
            ViewBag.Id_level_reference = folder?.Id_level_reference;

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
        public IActionResult RenameFile(int id, string name)
        {
            try
            {
                _profesorServices.RenameFile(id, name);
                var folder = _profesorServices.GetFileId(id); // Obtener la carpeta actualizada
                return Json(new { success = true, message = "Archivo renombrado exitosamente", folderName = folder.Name });
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

        public IActionResult DeleteFile(int id)
        {
            try
            {
                var file = _profesorServices.GetFileId(id); // Obtener el archivo por ID
                if (file == null)
                {
                    return Json(new { success = false, message = "El archivo no se encontró." });
                }

                // Eliminar el archivo del sistema de archivos
                if (!string.IsNullOrEmpty(file.FilePath))
                {
                    if (System.IO.File.Exists(file.FilePath))
                    {
                        System.IO.File.Delete(file.FilePath); // Eliminar archivo del sistema de archivos
                    }
                }

                // Luego eliminar el registro de la base de datos
                _profesorServices.DeleteFile(id); // Asegúrate de que este método existe en tu servicio

                return Json(new { success = true, message = "¡Archivo eliminado correctamente!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al eliminar el archivo: {ex.Message}" });
            }
        }

        public IActionResult DownloadFile(int id)
        {
            var file = _profesorServices.GetFileId(id);
            if (file == null || string.IsNullOrEmpty(file.FilePath))
            {
                return NotFound("El archivo no existe o la ruta es inválida.");
            }

            try
            {
                var fileBytes = System.IO.File.ReadAllBytes(file.FilePath);
                return File(fileBytes, "application/octet-stream", file.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al descargar el archivo: {ex.Message}");
                return NotFound("Hubo un problema al descargar el archivo.");
            }
        }
    }
}
