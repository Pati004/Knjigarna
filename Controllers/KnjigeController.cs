using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Knjigarna.Data;
using Knjigarna.Models;

namespace Knjigarna.Controllers
{
    public class KnjigeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KnjigeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Knjige
        public async Task<IActionResult> Index(string iskanje, int? kategorijaId, int? avtorId, bool? jeNaVoljo)
        {
            var knjige = _context.Knjige
                .Include(k => k.Avtor)
                .Include(k => k.Kategorija)
                .AsQueryable();

            // Filtri za iskanje
            if (!string.IsNullOrEmpty(iskanje))
            {
                knjige = knjige.Where(k => k.Naslov.Contains(iskanje) || k.Avtor.Ime.Contains(iskanje) || k.Avtor.Priimek.Contains(iskanje));
            }

            if (kategorijaId.HasValue)
            {
                knjige = knjige.Where(k => k.KategorijaId == kategorijaId);
            }

            if (avtorId.HasValue)
            {
                knjige = knjige.Where(k => k.AvtorId == avtorId);
            }

            if (jeNaVoljo.HasValue)
            {
                knjige = knjige.Where(k => k.JeNaVoljo == jeNaVoljo);
            }

            // Pripravi podatke za dropdown liste
            ViewBag.Kategorije = new SelectList(await _context.Kategorije.OrderBy(k => k.Ime).ToListAsync(), "Id", "Ime");
            ViewBag.Avtorji = new SelectList(await _context.Avtorji.OrderBy(a => a.Priimek).ThenBy(a => a.Ime).ToListAsync(), "Id", "PolnoIme");

            // Ohrani vrednosti filtrov
            ViewBag.Iskanje = iskanje;
            ViewBag.KategorijaId = kategorijaId;
            ViewBag.AvtorId = avtorId;
            ViewBag.JeNaVoljo = jeNaVoljo;

            return View(await knjige.OrderBy(k => k.Naslov).ToListAsync());
        }

        // GET: Knjige/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var knjiga = await _context.Knjige
                .Include(k => k.Avtor)
                .Include(k => k.Kategorija)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (knjiga == null)
            {
                return NotFound();
            }

            return View(knjiga);
        }

        // GET: Knjige/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.AvtorId = new SelectList(await _context.Avtorji.OrderBy(a => a.Priimek).ThenBy(a => a.Ime).ToListAsync(), "Id", "PolnoIme");
            ViewBag.KategorijaId = new SelectList(await _context.Kategorije.OrderBy(k => k.Ime).ToListAsync(), "Id", "Ime");

            var novaKnjiga = new Knjiga
            {
                JeNaVoljo = true,
                Zaloga = 0,
                LetoIzdaje = DateTime.Now.Year
            };

            return View(novaKnjiga);
        }

        // POST: Knjige/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ISBN,Naslov,Opis,LetoIzdaje,Cena,SteviloStrani,AvtorId,KategorijaId,SlikaNaslovnice,Zaloga,JeNaVoljo")] Knjiga knjiga)
        {
            if (ModelState.IsValid)
            {
                // Preveri, če knjiga z istim ISBN že obstaja
                var obstojecaKnjiga = await _context.Knjige
                    .FirstOrDefaultAsync(k => k.ISBN == knjiga.ISBN);

                if (obstojecaKnjiga != null)
                {
                    ModelState.AddModelError("ISBN", "Knjiga s tem ISBN že obstaja.");
                    await PrepareViewBagsForCreate();
                    return View(knjiga);
                }

                knjiga.DatumDodajanja = DateTime.Now;
                _context.Add(knjiga);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Knjiga '{knjiga.Naslov}' je bila uspešno dodana.";
                return RedirectToAction(nameof(Index));
            }

            await PrepareViewBagsForCreate();
            return View(knjiga);
        }

        // GET: Knjige/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var knjiga = await _context.Knjige.FindAsync(id);
            if (knjiga == null)
            {
                return NotFound();
            }

            ViewBag.AvtorId = new SelectList(await _context.Avtorji.OrderBy(a => a.Priimek).ThenBy(a => a.Ime).ToListAsync(), "Id", "PolnoIme", knjiga.AvtorId);
            ViewBag.KategorijaId = new SelectList(await _context.Kategorije.OrderBy(k => k.Ime).ToListAsync(), "Id", "Ime", knjiga.KategorijaId);

            return View(knjiga);
        }

        // POST: Knjige/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ISBN,Naslov,Opis,LetoIzdaje,Cena,SteviloStrani,AvtorId,KategorijaId,SlikaNaslovnice,Zaloga,JeNaVoljo,DatumDodajanja")] Knjiga knjiga)
        {
            if (id != knjiga.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Preveri, če knjiga z istim ISBN že obstaja (razen trenutne)
                    var obstojecaKnjiga = await _context.Knjige
                        .FirstOrDefaultAsync(k => k.ISBN == knjiga.ISBN && k.Id != knjiga.Id);

                    if (obstojecaKnjiga != null)
                    {
                        ModelState.AddModelError("ISBN", "Knjiga s tem ISBN že obstaja.");
                        await PrepareViewBagsForEdit(knjiga);
                        return View(knjiga);
                    }

                    _context.Update(knjiga);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"Podatki knjige '{knjiga.Naslov}' so bili uspešno posodobljeni.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KnjigaExists(knjiga.Id))
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

            await PrepareViewBagsForEdit(knjiga);
            return View(knjiga);
        }

        // GET: Knjige/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var knjiga = await _context.Knjige
                .Include(k => k.Avtor)
                .Include(k => k.Kategorija)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (knjiga == null)
            {
                return NotFound();
            }

            return View(knjiga);
        }

        // POST: Knjige/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var knjiga = await _context.Knjige.FindAsync(id);
            if (knjiga != null)
            {
                _context.Knjige.Remove(knjiga);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Knjiga '{knjiga.Naslov}' je bila uspešno odstranjena.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool KnjigaExists(int id)
        {
            return _context.Knjige.Any(e => e.Id == id);
        }

        private async Task PrepareViewBagsForCreate()
        {
            ViewBag.AvtorId = new SelectList(await _context.Avtorji.OrderBy(a => a.Priimek).ThenBy(a => a.Ime).ToListAsync(), "Id", "PolnoIme");
            ViewBag.KategorijaId = new SelectList(await _context.Kategorije.OrderBy(k => k.Ime).ToListAsync(), "Id", "Ime");
        }

        private async Task PrepareViewBagsForEdit(Knjiga knjiga)
        {
            ViewBag.AvtorId = new SelectList(await _context.Avtorji.OrderBy(a => a.Priimek).ThenBy(a => a.Ime).ToListAsync(), "Id", "PolnoIme", knjiga.AvtorId);
            ViewBag.KategorijaId = new SelectList(await _context.Kategorije.OrderBy(k => k.Ime).ToListAsync(), "Id", "Ime", knjiga.KategorijaId);
        }
    }
}