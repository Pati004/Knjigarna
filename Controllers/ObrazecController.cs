using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Knjigarna.Models;
using System.Text.Json;

namespace Knjigarna.Controllers
{
    public class ObrazecController : Controller
    {
        private const string SessionKeyObrazec = "_ObrazecPodatki";

        // GET: Obrazec
        public IActionResult Index()
        {
            // Preveri, če so že shranjeni podatki v TempData (po uspešni oddaji obrazca)
            var submittedData = TempData["SubmittedForm"]?.ToString();
            if (!string.IsNullOrEmpty(submittedData))
            {
                ViewBag.Submitted = true;
                var model = JsonSerializer.Deserialize<TabObrazecModel>(submittedData);
                return View("Povzetek", model);
            }

            // Preveri, če so shranjeni podatki v seji
            var sessionModel = GetFromSession();
            if (sessionModel != null)
            {
                return View(sessionModel);
            }

            // Ustvari nov model
            var noviModel = new TabObrazecModel
            {
                DatumRojstva = DateTime.Today.AddYears(-20) // Privzeta vrednost
            };

            return View(noviModel);
        }

        // POST: Obrazec/Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Submit(TabObrazecModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            // Shrani podatke v TempData za prikaz v naslednji zahtevi (PRG vzorec)
            TempData["SubmittedForm"] = JsonSerializer.Serialize(model);

            // Počisti podatke iz seje
            HttpContext.Session.Remove(SessionKeyObrazec);

            // PRG vzorec - preusmeri na isto akcijo, ki bo prikazala povzetek
            return RedirectToAction(nameof(Index));
        }

        // POST: Obrazec/SavePartial (AJAX za delno shranjevanje)
        [HttpPost]
        public IActionResult SavePartial(TabObrazecModel model)
        {
            // Shrani delne podatke v sejo (tudi če niso veljavni)
            SaveToSession(model);

            return Json(new { success = true });
        }

        // Pomožna metoda za shranjevanje modela v sejo
        private void SaveToSession(TabObrazecModel model)
        {
            var jsonData = JsonSerializer.Serialize(model);
            HttpContext.Session.SetString(SessionKeyObrazec, jsonData);
        }

        // Pomožna metoda za pridobivanje modela iz seje
        private TabObrazecModel? GetFromSession()
        {
            var jsonData = HttpContext.Session.GetString(SessionKeyObrazec);

            if (string.IsNullOrEmpty(jsonData))
                return null;

            try
            {
                return JsonSerializer.Deserialize<TabObrazecModel>(jsonData);
            }
            catch
            {
                return null;
            }
        }
    }
}