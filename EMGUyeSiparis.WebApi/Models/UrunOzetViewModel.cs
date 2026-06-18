using System.Collections.Generic;

namespace EMGUyeSiparis.WebApi.Models
{
    public class UrunOzetViewModel
    {
        public string UrunAdi { get; set; } = string.Empty;
        public decimal BirimFiyat { get; set; }
        public int ToplamSatishAdedi { get; set; }
        public decimal ToplamCiro => BirimFiyat * ToplamSatishAdedi;
        
        // Bu ürünü sipariş eden üyelerin listesi
        public List<UrunSiparisVerenUyeModel> SiparisVerenler { get; set; } = new();
    }

    public class UrunSiparisVerenUyeModel
    {
        public int SiparisNo { get; set; }
        public string UyeIsim { get; set; } = string.Empty;
        public string Beden { get; set; } = string.Empty;
        public int Adet { get; set; }
        public DateOnly Tarih { get; set; }
    }
}
