using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EMGUyeSiparis.WebApi.Data;
using EMGUyeSiparis.WebApi.Models;

namespace EMGUyeSiparis.WebApi.Controllers
{
    public class SiparisController : Controller
    {
        private readonly AppDbContext _context;

        public SiparisController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Gelişmiş Sipariş Listeleme (Index) - Master/Detail GroupBy Yapısı
        public async Task<IActionResult> Index()
        {
            // Veri tabanındaki tabloları birleştirip ham veriyi çekiyoruz
            var hamSiparisler = await (from ozet in _context.SiparisOzetleri
                                       join uye in _context.Uyeler on ozet.UyeId equals uye.Id
                                       join detay in _context.SiparisDetaylari on ozet.Id equals detay.SiparisOzetId
                                       select new
                                       {
                                           SiparisOzetId = ozet.Id,
                                           SiparisTarih = ozet.Tarih,
                                           UyeIsim = uye.Isim,
                                           UyeUnvan = uye.Unvan,
                                           DetayId = detay.Id,
                                           detay.Urun,
                                           detay.Adet,
                                           detay.Beden,
                                           KanGrubu = !string.IsNullOrEmpty(detay.KanGrubu) ? detay.KanGrubu : uye.KanGrubu ?? "Belirtilmedi"
                                       })
                                       .OrderByDescending(x => x.SiparisTarih)
                                       .ToListAsync();

            // Çekilen veriyi Sipariş Özet ID'sine göre gruplayıp ViewModel'lara eşliyoruz
            var gelismisSiparisListesi = hamSiparisler
                .GroupBy(g => new { g.SiparisOzetId, g.SiparisTarih, g.UyeIsim, g.UyeUnvan })
                .Select(g => new SiparisOzetViewModel
                {
                    SiparisOzetId = g.Key.SiparisOzetId,
                    Tarih = g.Key.SiparisTarih,
                    UyeIsim = g.Key.UyeIsim,
                    Unvan = g.Key.UyeUnvan,
                    Detaylar = g.Select(d => new SiparisDetayViewModel
                    {
                        SiparisDetayId = d.DetayId,
                        Urun = d.Urun,
                        Adet = d.Adet,
                        Beden = d.Beden,
                        KanGrubu = d.KanGrubu,
                        // Sabitler sınıfındaki sözlükten birim fiyatı eşleştiriyoruz
                        BirimFiyat = Sabitler.UrunFiyatlari.TryGetValue(d.Urun, out var fiyat) ? fiyat : 0
                    }).ToList()
                }).ToList();

            return View(gelismisSiparisListesi);
        }

        // GET: Siparis/Create
        public async Task<IActionResult> Create()
        {
            var uyeler = await _context.Uyeler.ToListAsync();
            ViewBag.UyeId = new SelectList(uyeler, "Id", "Isim");

            // string[] dizilerini SelectList nesnesine dönüştürerek gönderiyoruz
            ViewBag.Urunler = new SelectList(Sabitler.Urunler);
            ViewBag.Bedenler = new SelectList(Enum.GetNames(typeof(Sabitler.BedenTablosu)));
            ViewBag.KanGruplari = new SelectList(Sabitler.KanGrubu);

            return View();
        }

        // POST: Siparis/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SiparisKayitViewModel model)
        {
            if (model.Urunler == null || model.Urunler.Count == 0)
            {
                ModelState.AddModelError("", "En az bir ürün eklemelisiniz.");
            }

            if (ModelState.IsValid)
            {
                var siparisOzet = new SiparisOzet
                {
                    UyeId = model.UyeId,
                    Tarih = model.Tarih 
                };
                _context.SiparisOzetleri.Add(siparisOzet);
                await _context.SaveChangesAsync();

                foreach (var urun in model.Urunler)
                {
                    var siparisDetay = new SiparisDetay
                    {
                        SiparisOzetId = siparisOzet.Id,
                        Urun = urun.Urun,
                        Adet = urun.Adet,
                        Beden = urun.Beden,
                        KanGrubu = urun.KanGrubu
                    };
                    _context.SiparisDetaylari.Add(siparisDetay);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Hata durumunda da SelectList olarak geri dolduruyoruz
            var uyeler = await _context.Uyeler.ToListAsync();
            ViewBag.UyeId = new SelectList(uyeler, "Id", "Isim", model.UyeId);
            ViewBag.Urunler = new SelectList(Sabitler.Urunler);
            ViewBag.Bedenler = new SelectList(Enum.GetNames(typeof(Sabitler.BedenTablosu)));
            ViewBag.KanGruplari = new SelectList(Sabitler.KanGrubu);

            return View(model);
        }



        // 4. Sipariş Silme İşlemi (POST) - Cascade Delete ile alt detaylar da silinir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var siparisOzet = await _context.SiparisOzetleri.FindAsync(id);
            if (siparisOzet != null)
            {
                _context.SiparisOzetleri.Remove(siparisOzet);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Siparis/UrunOzet
        public async Task<IActionResult> UrunOzet()
        {
            // 1. Tüm sipariş detaylarını ve üye bilgilerini veri tabanından tek seferde çekiyoruz
            var hamDetaylar = await (from detay in _context.SiparisDetaylari
                                     join ozet in _context.SiparisOzetleri on detay.SiparisOzetId equals ozet.Id
                                     join uye in _context.Uyeler on ozet.UyeId equals uye.Id
                                     select new
                                     {
                                         detay.Urun,
                                         detay.Beden,
                                         detay.Adet,
                                         SiparisNo = ozet.Id,
                                         ozet.Tarih,
                                         UyeIsim = uye.Isim
                                     }).ToListAsync();

            // 2. Çekilen verileri Sabitler.Urunler listesini baz alarak ürün adına göre grupluyoruz
            var urunBazliOzet = hamDetaylar
                .GroupBy(g => g.Urun)
                .Select(g => new UrunOzetViewModel
                {
                    UrunAdi = g.Key,
                    // Sabitler sınıfından ilgili ürünün fiyatını eşleştiriyoruz
                    BirimFiyat = Sabitler.UrunFiyatlari.TryGetValue(g.Key, out var fiyat) ? fiyat : 0,
                    ToplamSatishAdedi = g.Sum(x => x.Adet),
                    SiparisVerenler = g.Select(x => new UrunSiparisVerenUyeModel
                    {
                        SiparisNo = x.SiparisNo,
                        UyeIsim = x.UyeIsim,
                        Beden = x.Beden,
                        Adet = x.Adet,
                        Tarih = x.Tarih
                    })
                    .OrderByDescending(x => x.Tarih)
                    .ToList()
                })
                .OrderByDescending(o => o.ToplamCiro) // En çok kazandıran ürün en üstte görünür
                .ToList();

            return View(urunBazliOzet);
        }

    }
}
