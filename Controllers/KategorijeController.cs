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
    public class KategorijeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KategorijeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Kategorije
        public async Task<IActionResult> Index()
        {
            var kategorije = await _context.Kategorije
                .Include(k => k.Knjige)
                .OrderBy(k => k.Ime)
                .ToListAsync();
            return View(kategorije);
        }

        // GET: Kategorije/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategorija = await _context.Kategorije
                .Include(k => k.Knjige)
                    .ThenInclude(k => k.Avtor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (kategorija == null)
            {
                return NotFound();
            }

            return View(kategorija);
        }

        // GET: Kategorije/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kategorije/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ime,Opis")] Kategorija kategorija)
        {
            if (ModelState.IsValid)
            {
                // Preveri, če kategorija z istim imenom že obstaja
                var obstojecaKategorija = await _context.Kategorije
                    .FirstOrDefaultAsync(k => k.Ime.ToLower() == kategorija.Ime.ToLower());

                if (obstojecaKategorija != null)
                {
                    ModelState.AddModelError("Ime", "Kategorija s tem imenom že obstaja.");
                    return View(kategorija);
                }

                _context.Add(kategorija);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Kategorija {kategorija.Ime} je bila uspešno dodana.";
                return RedirectToAction(nameof(Index));
            }
            return View(kategorija);
        }

        // GET: Kategorije/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategorija = await _context.Kategorije.FindAsync(id);
            if (kategorija == null)
            {
                return NotFound();
            }
            return View(kategorija);
        }

        // POST: Kategorije/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ime,Opis")] Kategorija kategorija)
        {
            if (id != kategorija.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Preveri, če kategorija z istim imenom že obstaja (razen trenutne)
                    var obstojecaKategorija = await _context.Kategorije
                        .FirstOrDefaultAsync(k => k.Ime.ToLower() == kategorija.Ime.ToLower() && k.Id != kategorija.Id);

                    if (obstojecaKategorija != null)
                    {
                        ModelState.AddModelError("Ime", "Kategorija s tem imenom že obstaja.");
                        return View(kategorija);
                    }

                    _context.Update(kategorija);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Kategorija {kategorija.Ime} je bila uspešno posodobljena.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KategorijaExists(kategorija.Id))
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
            return View(kategorija);
        }

        // GET: Kategorije/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategorija = await _context.Kategorije
                .Include(k => k.Knjige)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (kategorija == null)
            {
                return NotFound();
            }

            return View(kategorija);
        }

        // POST: Kategorije/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kategorija = await _context.Kategorije
                .Include(k => k.Knjige)
                .FirstOrDefaultAsync(k => k.Id == id);

            if (kategorija != null)
            {
                // Preveri, če kategorija ima knjige
                if (kategorija.Knjige.Any())
                {
                    TempData["ErrorMessage"] = $"Kategorije {kategorija.Ime} ni mogoče izbrisati, ker vsebuje knjige. Najprej izbriši vse knjige te kategorije ali jih premakni v drugo kategorijo.";
                    return RedirectToAction(nameof(Delete), new { id = id });
                }

                _context.Kategorije.Remove(kategorija);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Kategorija {kategorija.Ime} je bila uspešno odstranjena.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool KategorijaExists(int id)
        {
            return _context.Kategorije.Any(e => e.Id == id);
        }
    }
}