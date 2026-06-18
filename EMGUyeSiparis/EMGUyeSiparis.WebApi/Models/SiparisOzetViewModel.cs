using System;
using System.Collections.Generic;
using System.Linq;

namespace EMGUyeSiparis.WebApi.Models
{
    public class SiparisOzetViewModel
    {
        public int SiparisOzetId { get; set; }
        public string UyeIsim { get; set; } = string.Empty;
        public string Unvan { get; set; } = string.Empty;
        public DateOnly Tarih { get; set; }

        // Bu siparişe ait alt ürün detaylarının listesi
        public List<SiparisDetayViewModel> Detaylar { get; set; } = new List<SiparisDetayViewModel>();

        // Siparişteki tüm ürünlerin genel toplam tutarı
        public decimal GenelToplamTutar => Detaylar.Sum(d => d.ToplamTutar);
        
        // Siparişteki toplam ürün adedi
        public int ToplamUrunAdeti => Detaylar.Sum(d => d.Adet);
    }
}
