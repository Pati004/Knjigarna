using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Knjigarna.Models.FormModels;
using System.Text.Json;

namespace Knjigarna.Controllers
{
    public class FormController : Controller
    {
        #region Obrazec za uporabnika
        // GET: Form/ObrazecEdit
        public IActionResult ObrazecEdit()
        {
            // Preveri, če so podatki v TempData za prikaz (PRG vzorec)
            var submittedData = TempData["ObrazecSubmitted"]?.ToString();
            if (!string.IsNullOrEmpty(submittedData))
            {
                ViewBag.Submitted = true;
                var model = JsonSerializer.Deserialize<EditorObrazecModel>(submittedData);
                return View("ObrazecDisplay", model);
            }

            var noviModel = new EditorObrazecModel
            {
                DatumRojstva = DateTime.Today.AddYears(-20),
                Starost = 20
            };

            return View(noviModel);
        }

        // POST: Form/ObrazecSubmit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ObrazecSubmit(EditorObrazecModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ObrazecEdit", model);
            }

            // Shrani podatke v TempData za prikaz v naslednji zahtevi (PRG vzorec)
            TempData["ObrazecSubmitted"] = JsonSerializer.Serialize(model);

            // PRG vzorec - preusmeri na GET akcijo
            return RedirectToAction(nameof(ObrazecEdit));
        }
        #endregion

        #region Obrazec za knjigo
        // GET: Form/KnjigaEdit
        public IActionResult KnjigaEdit()
        {
            // Preveri, če so podatki v TempData za prikaz (PRG vzorec)
            var submittedData = TempData["KnjigaSubmitted"]?.ToString();
            if (!string.IsNullOrEmpty(submittedData))
            {
                ViewBag.Submitted = true;
                var model = JsonSerializer.Deserialize<KnjigaModel>(submittedData);
                return View("KnjigaDisplay", model);
            }

            var noviModel = new KnjigaModel
            {
                DatumIzida = DateTime.Today,
                LetoIzdaje = DateTime.Today.Year,
                JeNaVoljo = true,
                Cena = 19.99,
                Zaloga = 10,
                SteviloStrani = 200,
                Ocena = 4
            };

            return View(noviModel);
        }

        // POST: Form/KnjigaSubmit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult KnjigaSubmit(KnjigaModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("KnjigaEdit", model);
            }

            // Shrani podatke v TempData za prikaz v naslednji zahtevi (PRG vzorec)
            TempData["KnjigaSubmitted"] = JsonSerializer.Serialize(model);

            // PRG vzorec - preusmeri na GET akcijo
            return RedirectToAction(nameof(KnjigaEdit));
        }
        #endregion
    }
}