using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pautas.Models.Profesor;
using Pautas.Models.Pautas;
using Pautas.Services.Profesor;
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
ProfesorServices _profesorServices = new ProfesorServices();

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
            return View(rootFolders);
        }

        public IActionResult ViewFolder(int id)
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
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, int folderId)
        {
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                _profesorServices.UploadFile(file.FileName, filePath, folderId);
            }

            return RedirectToAction("ViewFolder", new { id = folderId });
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
