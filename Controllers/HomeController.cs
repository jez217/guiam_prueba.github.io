using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pautas.Models;
using Pautas.Models.Login;
using Pautas.Models.Pautas;
using Pautas.Services.Extensions;
using Pautas.Services.Pauta;
using System.Diagnostics;

namespace Pautas.Controllers
{
    public class HomeController : Controller
    {
        PautaServices _pautaservice = new PautaServices();
        StoreServices _storeservice = new StoreServices();

        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        #region <<<Index Pauta>>>
        [Authorize]
        public IActionResult Index()
        {


            var (total, iniciado, proceso, terminado) = _pautaservice.SP_COUNT_PAUTAS();

            ViewData["Total"] = total;
            ViewData["Iniciado"] = iniciado;
            ViewData["Proceso"] = proceso;
            ViewData["Terminado"] = terminado;

            ViewBag.TotalCount = total;
            ViewBag.IniciadoCount = iniciado;
            ViewBag.ProcesoCount = proceso;
            ViewBag.TerminadoCount = terminado;

            return View();
        }
        #endregion

        #region <<<Edit Pautas>>>
        [HttpGet]
        public IActionResult Edit(int id, string mensaje)
        {
            PautaModel model = new PautaModel();

            User user = new User();

            model = _pautaservice.SP_PAUTAS_SELECTBYONE(id);

            model.MONTHDROPDOWNS = _pautaservice.DropdownMeses();
            model.STOREDROPDOWNS = _storeservice.StoreDropdown();
            model.PCDROPDOWNS = _pautaservice.ProfitCenterDropdown();
            model.ListaProducDetail = _pautaservice.SP_PRODUCT_SELECT_DETAILS(id);
            model.ListaPautaImages = _pautaservice.SP_IMAGEN_SELECT(id);

            model.USER_CREATE = user.Name;

            ViewBag.mensaje = mensaje;
            ViewBag.TotalVenta = model.ListaProducDetail.Sum(x=>x.SALE);
            ViewBag.TotalPiezas = model.ListaProducDetail.Sum(x => x.QTY);
            ViewBag.TotalUtilidad = model.ListaProducDetail.Sum(x => x.UTILITY);

            return View(model);
        }


        [HttpPost]
        public IActionResult Edit(PautaModel model)
        {
            User userAuth = HttpContext.Session.GetObject<User>("name");

            model.USER_UPDATE = userAuth.Name;

            GenericResponse resp = new GenericResponse();
            resp = _pautaservice.SP_PAUTAS_EDIT(model);


            if (resp.Resp)
            {
                return Json(new { resp.id, resp.message });
            }
            else
            {
                return RedirectToAction("Details");
            }

        }
        #endregion

        #region <<<Cancelar Pautas>>>
        public IActionResult Details()
        {

            List<PautaModel> olista = new List<PautaModel>();
            olista = _pautaservice.SP_PAUTAS_SELECT();

            return View(olista);
        }
        #endregion


        #region <<<Crear Pauta>>>
        [HttpGet]
        public IActionResult Create()
        {
            User user = new User();

            PautaModel model = new PautaModel();

            model.MONTHDROPDOWNS = _pautaservice.DropdownMeses();
            model.STOREDROPDOWNS = _storeservice.StoreDropdown();
            model.PCDROPDOWNS = _pautaservice.ProfitCenterDropdown();
            model.ANNO = DateTime.Now.Year;
            model.USER_CREATE = user.Name;

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(PautaModel model)
        {
            User userAuth = HttpContext.Session.GetObject<User>("name");
            if (userAuth == null)
            {
                return RedirectToAction("Login", "User");
            }
            model.USER_CREATE = userAuth.Name;

            GenericResponse resp = new GenericResponse();
            resp = _pautaservice.SP_PAUTAS_CREATE(model);

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

        #region <<<Cancelar Pautas>>>
        public IActionResult Canceled(int PAUTA_ID)
        {
            GenericResponse resp = new GenericResponse();
            resp = _pautaservice.SP_PAUTAS_CANCELED_STATUS(PAUTA_ID, 4);

            return Json(new { message = resp.message });

        }
        #endregion

        #region <<<Finalizar Pautas>>>
        public IActionResult PautaFinish(int PAUTA_ID)
        {
            GenericResponse resp = new GenericResponse();
            resp = _pautaservice.SP_PAUTAS_UPDATE_STATUS(PAUTA_ID, 3);
            return Json(new { id = resp.id, message = resp.message });
        }
        #endregion

        #region <<<Insertar Producto>>>
        public IActionResult InsertProduct(int PAUTA_ID, int PRODUCT_CODE)
        {
            ProductModel resp = new ProductModel();
            resp = _pautaservice.SP_PAUTAS_PRODUCT_INSERT(PAUTA_ID, PRODUCT_CODE);
            return Json(new { id = resp.id, message = resp.message });
        }
        #endregion

        #region <<<Insertar Imagen>>>
        public IActionResult InsertImagen(PautaModel model)
        {
            PautaModel resp = new PautaModel();
            resp = _pautaservice.SP_IMAGEN_INSERT(model);
            return Json(new { id = resp.id, message = resp.message });
        }
        #endregion

        #region <<<Buscar Producto>>>
        public IActionResult SearchProduct(int PRODUCT_CODE)
     {
            ProductModel oProduct = new ProductModel();

            oProduct = _pautaservice.SP_PRODUCT_SELECT_BYONE(PRODUCT_CODE);

            return Json(new { oProduct.PROD_DESC, oProduct.SUPPLIER_CODE, oProduct.PRICE_RETAIL, oProduct.PHOTO_PATH });
        }
        #endregion

        #region <<<Eliminar Producto>>>
        public IActionResult DeleteProduct(int PRODUCT_CODE, int PAUTA_ID)
        {
            GenericResponse resp = new GenericResponse();
            resp = _pautaservice.SP_PAUTAS_DELETE_PRODUCT(PRODUCT_CODE, PAUTA_ID);

            return Json(new { message = resp.message });

        }
        #endregion

        #region <<<Delete Image>>>
        public IActionResult DeleteImage(int ID)
        {
            GenericResponse resp = new GenericResponse();
            resp = _pautaservice.p_PT_Pautas_Imagen_Delete_ByOne(ID);

            return Json(new { message = resp.message });

        }
        #endregion

        #region <<<Upload Image>>>
        [HttpPost]
        public IActionResult UploadImage(int PAUTA_ID, IFormFile file)
        {
            try
            {
                // file = Request.Form.Files["imageInput"]; // Assuming the image is sent as form data

                if (file != null && file.Length > 0)
                {

                    // Get the file name
                    string fileName = PAUTA_ID.ToString() + "_" + Path.GetFileName(file.FileName);
                    // Specify the directory where you want to save the image
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "ImgPauta");
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    // Create the directory if it doesn't exist
                    Directory.CreateDirectory(uploadsFolder);

                    // Save the image to the specified directory
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    PautaModel resp = new PautaModel
                    {
                        ID = 0, // Assuming ID is auto-generated in the database
                        PAUTA_ID = PAUTA_ID, // Assuming PAUTA_ID is provided in the request
                        IMAGES_ID = 0, // Assuming IMAGES_ID is auto-generated in the database
                        URL = filePath, // Save the file path as the URL in the database
                        IMAGES_NAME = fileName // Save the file name in the database
                    };

                    resp = _pautaservice.SP_IMAGEN_INSERT(resp);

                    return Json(new { PAUTA_ID, id = resp.id, message = resp.message });

                }
                else
                {
                    return RedirectToAction("Edit", new { PAUTA_ID, mensaje = "No file selected." });

                }
            }
            catch (Exception ex)
            {
                // Log the error for debugging purposes
                // logger.LogError(ex, "Error occurred during image upload.");
                return RedirectToAction("Edit", new { PAUTA_ID, mensaje = "Error: " + ex.Message });
            }
        }
        #endregion

        #region <<<PrintPautaPdf>>>
        [HttpGet]
        public IActionResult PrintPautaPdf(int id)
        {
            PautaModel model = new PautaModel();

            User user = new User();

            model = _pautaservice.SP_PAUTAS_SELECTBYONE(id);

            model.MONTHDROPDOWNS = _pautaservice.DropdownMeses();
            model.STOREDROPDOWNS = _storeservice.StoreDropdown();
            model.PCDROPDOWNS = _pautaservice.ProfitCenterDropdown();
            model.ListaProducDetail = _pautaservice.SP_PRODUCT_SELECT_DETAILS(id);
            model.ListaPautaImages = _pautaservice.SP_IMAGEN_SELECT(id);

            model.USER_CREATE = user.Name;

            ViewBag.TotalVenta = model.ListaProducDetail.Sum(x => x.SALE);
            ViewBag.TotalPiezas = model.ListaProducDetail.Sum(x => x.QTY);
            ViewBag.TotalUtilidad = model.ListaProducDetail.Sum(x => x.UTILITY);

            return View(model);
        }
        #endregion

        #region <<<Privacy de Login>>>
        public IActionResult Privacy()
        {
            User userAuth = HttpContext.Session.GetObject<User>("name");

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