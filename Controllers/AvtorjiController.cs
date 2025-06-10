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
    public class AvtorjiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AvtorjiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Avtorji
        public async Task<IActionResult> Index()
        {
            var avtorji = await _context.Avtorji
                .Include(a => a.Knjige)
                .OrderBy(a => a.Priimek)
                .ThenBy(a => a.Ime)
                .ToListAsync();
            return View(avtorji);
        }

        // GET: Avtorji/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avtor = await _context.Avtorji
                .Include(a => a.Knjige)
                    .ThenInclude(k => k.Kategorija)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (avtor == null)
            {
                return NotFound();
            }

            return View(avtor);
        }

        // GET: Avtorji/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Avtorji/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ime,Priimek,DatumRojstva,Drzava,Biografija")] Avtor avtor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(avtor);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Avtor {avtor.PolnoIme} je bil uspešno dodan.";
                return RedirectToAction(nameof(Index));
            }
            return View(avtor);
        }

        // GET: Avtorji/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avtor = await _context.Avtorji.FindAsync(id);
            if (avtor == null)
            {
                return NotFound();
            }
            return View(avtor);
        }

        // POST: Avtorji/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ime,Priimek,DatumRojstva,Drzava,Biografija")] Avtor avtor)
        {
            if (id != avtor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(avtor);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Podatki avtorja {avtor.PolnoIme} so bili uspešno posodobljeni.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AvtorExists(avtor.Id))
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
            return View(avtor);
        }

        // GET: Avtorji/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avtor = await _context.Avtorji
                .Include(a => a.Knjige)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (avtor == null)
            {
                return NotFound();
            }

            return View(avtor);
        }

        // POST: Avtorji/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var avtor = await _context.Avtorji
                .Include(a => a.Knjige)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (avtor != null)
            {
                // Preveri, če avtor ima knjige
                if (avtor.Knjige.Any())
                {
                    TempData["ErrorMessage"] = $"Avtorja {avtor.PolnoIme} ni mogoče izbrisati, ker ima objavljene knjige. Najprej izbriši vse njegove knjige.";
                    return RedirectToAction(nameof(Delete), new { id = id });
                }

                _context.Avtorji.Remove(avtor);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Avtor {avtor.PolnoIme} je bil uspešno odstranjen.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AvtorExists(int id)
        {
            return _context.Avtorji.Any(e => e.Id == id);
        }
    }
}