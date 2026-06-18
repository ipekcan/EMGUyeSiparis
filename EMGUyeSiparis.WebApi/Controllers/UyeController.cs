using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMGUyeSiparis.WebApi.Data;
using EMGUyeSiparis.WebApi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EMGUyeSiparis.WebApi.Controllers
{
    public class UyeController : Controller
    {
        private readonly AppDbContext _context;

        public UyeController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Üyeleri Listeleme (Index)
        public async Task<IActionResult> Index()
        {
            var uyeler = await _context.Uyeler.ToListAsync();
            return View(uyeler);
        }

        // 2. Yeni Üye Ekleme (Create - GET)
        public IActionResult Create()
        {
            // Sabitler sınıfındaki Kan Gruplarını Dropdown için ViewBag'e atıyoruz
            ViewBag.KanGruplari = new SelectList(Sabitler.KanGrubu);
            return View();
        }

        // 3. Yeni Üye Ekleme (Create - POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Uye uye)
        {
            if (ModelState.IsValid)
            {
                _context.Add(uye);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.KanGruplari = new SelectList(Sabitler.KanGrubu, uye.KanGrubu);
            return View(uye);
        }

        // 4. Üye Düzenleme (Edit - GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var uye = await _context.Uyeler.FindAsync(id);
            if (uye == null) return NotFound();

            ViewBag.KanGruplari = new SelectList(Sabitler.KanGrubu, uye.KanGrubu);
            return View(uye);
        }

        // 5. Üye Düzenleme (Edit - POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Uye uye)
        {
            if (id != uye.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uye);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Uyeler.Any(e => e.Id == uye.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.KanGruplari = new SelectList(Sabitler.KanGrubu, uye.KanGrubu);
            return View(uye);
        }

        // 6. Üye Silme (Delete - POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var uye = await _context.Uyeler.FindAsync(id);
            if (uye != null)
            {
                _context.Uyeler.Remove(uye);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
