using Microsoft.AspNetCore.Mvc;
using Pautas.Models.Pautas;
using Pautas.Services.Pauta;

namespace Pautas.Controllers
{
    public class StatusController : Controller
    {
        PautaServices _pautaservice = new PautaServices();

        // GET: PautaController
        public IActionResult Begin()
        {
            List<PautaModel> olista = new List<PautaModel>();

            olista = _pautaservice.SP_STATUS_BEGIN();

            return View(olista);
        }

        // GET: PautaController/Finish/
        public IActionResult Finish()
        {
            List<PautaModel> olista = new List<PautaModel>();
            olista = _pautaservice.SP_STATUS_FINISH();

            return View(olista);
        }

        // GET: PautaController/In_Process
        public IActionResult In_Process()
        {
            List<PautaModel> olista = new List<PautaModel>();
            olista = _pautaservice.SP_STATUS_IN_PROCESS();

            return View(olista);
        }

        // POST: PautaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PautaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PautaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PautaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PautaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
