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
            var rootFolders = _profesorServices.GetRootFolders();

            // Pasa las carpetas y subcarpetas al ViewBag para que estén disponibles en el layout
            ViewBag.Folders = rootFolders;

            return View(rootFolders);
        }


        public IActionResult View(int id)
        {

            var folder = _profesorServices.GetFolderById(id);

            if (folder.Id == 0)
            {
                return NotFound();
            }

            return View(folder);
        }

        [HttpGet]
        public IActionResult CreateFolder()
        {
            Folder model = new Folder();
            string user = User.Identity.Name;


            model.CreatedBy = user;


            return View(model);
        }

        [HttpPost]
        public IActionResult CreateFolder(Folder model)
        {
            string user = User.Identity.Name;

            model.CreatedBy = user;
            _profesorServices.CreateFolder(model);

            // Actualizar la lista de carpetas y subcarpetas después de crear la carpeta
            var rootFolders = _profesorServices.GetRootFolders(); // Asegúrate de obtener las carpetas correctamente
            ViewBag.Folders = rootFolders;  // Pasa la lista de carpetas y subcarpetas al ViewBag

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, int folderId)
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

                    _profesorServices.UploadFile(file.FileName, filePath, folderId);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error al subir el archivo: {ex.Message}");
                    ModelState.AddModelError("", "Ocurrió un error al subir el archivo.");
                }
            }

            return RedirectToAction("View", new { id = folderId });
        }

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
