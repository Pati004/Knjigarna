using Microsoft.AspNetCore.Mvc;
using Knjigarna.Models.Registracija;

namespace Knjigarna.Controllers
{
    public class RegistracijaController : Controller
    {
        private readonly RegistracijaStoritev _registracijaStoritev;

        public RegistracijaController(RegistracijaStoritev registracijaStoritev)
        {
            _registracijaStoritev = registracijaStoritev;
        }

        // GET: Registracija/Korak1
        public IActionResult Korak1()
        {
            return View(_registracijaStoritev.GetKorak1Model());
        }

        // POST: Registracija/Korak1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Korak1(Korak1Model model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _registracijaStoritev.ShraniKorak1(model);

            // PRG vzorec - preusmeri na naslednji korak
            return RedirectToAction(nameof(Korak2));
        }

        // GET: Registracija/Korak2
        public IActionResult Korak2()
        {
            // Preveri, če je uporabnik že izpolnil prejšnji korak
            if (!_registracijaStoritev.JeKorakIzpolnjen(2))
            {
                return RedirectToAction(nameof(Korak1));
            }

            return View(_registracijaStoritev.GetKorak2Model());
        }

        // POST: Registracija/Korak2
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Korak2(Korak2Model model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _registracijaStoritev.ShraniKorak2(model);

            // PRG vzorec - preusmeri na naslednji korak
            return RedirectToAction(nameof(Korak3));
        }

        // GET: Registracija/Korak3
        public IActionResult Korak3()
        {
            // Preveri, če je uporabnik že izpolnil prejšnji korak
            if (!_registracijaStoritev.JeKorakIzpolnjen(3))
            {
                return RedirectToAction(nameof(Korak2));
            }

            return View(_registracijaStoritev.GetKorak3Model());
        }

        // POST: Registracija/Korak3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Korak3(Korak3Model model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _registracijaStoritev.ShraniKorak3(model);

            // PRG vzorec - preusmeri na povzetek
            return RedirectToAction(nameof(Povzetek));
        }

        // GET: Registracija/Povzetek
        public IActionResult Povzetek()
        {
            // Preveri, če je uporabnik že izpolnil prejšnji korak
            if (!_registracijaStoritev.JeKorakIzpolnjen(4))
            {
                return RedirectToAction(nameof(Korak3));
            }

            return View(_registracijaStoritev.GetRegistracijskiPodatki());
        }

        // GET: Registracija/Zakljuci
        public IActionResult Zakljuci()
        {
            _registracijaStoritev.PocistiPodatke();
            return RedirectToAction("Index", "Home");
        }
    }
}