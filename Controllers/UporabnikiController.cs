using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Knjigarna.Data;
using Knjigarna.Models;

namespace Knjigarna.Controllers
{
    public class UporabnikiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UporabnikiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Uporabniki
        public async Task<IActionResult> Index()
        {
            var uporabniki = await _context.Uporabniki
                .OrderBy(u => u.Priimek)
                .ThenBy(u => u.Ime)
                .ToListAsync();
            return View(uporabniki);
        }

        // GET: Uporabniki/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uporabnik = await _context.Uporabniki
                .FirstOrDefaultAsync(m => m.Id == id);

            if (uporabnik == null)
            {
                return NotFound();
            }

            return View(uporabnik);
        }

        // GET: Uporabniki/Create
        public IActionResult Create()
        {
            var noviUporabnik = new Uporabnik
            {
                DatumRojstva = DateTime.Today.AddYears(-25),
                Starost = 25,
                JeAktiven = true
            };
            return View(noviUporabnik);
        }

        // POST: Uporabniki/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ime,Priimek,DatumRojstva,KrajRojstva,EMSO,Email,Starost,JeAktiven")] Uporabnik uporabnik)
        {
            if (ModelState.IsValid)
            {
                // Preveri, če uporabnik z istim EMŠO že obstaja
                var obstojUporabnik = await _context.Uporabniki
                    .FirstOrDefaultAsync(u => u.EMSO == uporabnik.EMSO);

                if (obstojUporabnik != null)
                {
                    ModelState.AddModelError("EMSO", "Uporabnik s tem EMŠO že obstaja.");
                    return View(uporabnik);
                }

                // Preveri, če uporabnik z istim e-naslovom že obstaja
                var obstojEmail = await _context.Uporabniki
                    .FirstOrDefaultAsync(u => u.Email == uporabnik.Email);

                if (obstojEmail != null)
                {
                    ModelState.AddModelError("Email", "Uporabnik s tem e-naslovom že obstaja.");
                    return View(uporabnik);
                }

                uporabnik.DatumRegistracije = DateTime.Now;
                _context.Add(uporabnik);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Uporabnik {uporabnik.PolnoIme} je bil uspešno dodan.";
                return RedirectToAction(nameof(Index));
            }
            return View(uporabnik);
        }

        // GET: Uporabniki/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uporabnik = await _context.Uporabniki.FindAsync(id);
            if (uporabnik == null)
            {
                return NotFound();
            }
            return View(uporabnik);
        }

        // POST: Uporabniki/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ime,Priimek,DatumRojstva,KrajRojstva,EMSO,Email,Starost,JeAktiven,DatumRegistracije")] Uporabnik uporabnik)
        {
            if (id != uporabnik.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Preveri, če uporabnik z istim EMŠO že obstaja (razen trenutnega)
                    var obstojUporabnik = await _context.Uporabniki
                        .FirstOrDefaultAsync(u => u.EMSO == uporabnik.EMSO && u.Id != uporabnik.Id);

                    if (obstojUporabnik != null)
                    {
                        ModelState.AddModelError("EMSO", "Uporabnik s tem EMŠO že obstaja.");
                        return View(uporabnik);
                    }

                    // Preveri, če uporabnik z istim e-naslovom že obstaja (razen trenutnega)
                    var obstojEmail = await _context.Uporabniki
                        .FirstOrDefaultAsync(u => u.Email == uporabnik.Email && u.Id != uporabnik.Id);

                    if (obstojEmail != null)
                    {
                        ModelState.AddModelError("Email", "Uporabnik s tem e-naslovom že obstaja.");
                        return View(uporabnik);
                    }

                    _context.Update(uporabnik);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Podatki uporabnika {uporabnik.PolnoIme} so bili uspešno posodobljeni.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UporabnikExists(uporabnik.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(uporabnik);
        }

        // GET: Uporabniki/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uporabnik = await _context.Uporabniki
                .FirstOrDefaultAsync(m => m.Id == id);

            if (uporabnik == null)
            {
                return NotFound();
            }

            return View(uporabnik);
        }

        // POST: Uporabniki/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var uporabnik = await _context.Uporabniki.FindAsync(id);
            if (uporabnik != null)
            {
                _context.Uporabniki.Remove(uporabnik);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Uporabnik {uporabnik.PolnoIme} je bil uspešno odstranjen.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UporabnikExists(int id)
        {
            return _context.Uporabniki.Any(e => e.Id == id);
        }
    }
}