using Microsoft.EntityFrameworkCore;
using EMGUyeSiparis.WebApi.Models; // Modellerinizin bulunduğu klasörün namespace adını buraya yazın

namespace EMGUyeSiparis.WebApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Veri tabanı tabloları
        public DbSet<Uye> Uyeler { get; set; }
        public DbSet<SiparisOzet> SiparisOzetleri { get; set; }
        public DbSet<SiparisDetay> SiparisDetaylari { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Uye Tablosu Yapılandırması
            modelBuilder.Entity<Uye>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Isim).IsRequired().HasMaxLength(150);
                entity.Property(u => u.Unvan).HasMaxLength(100);
                entity.Property(u => u.KanGrubu).HasMaxLength(10);
            });

            // 2. SiparisOzet Tablosu ve Uye İlişkisi
            modelBuilder.Entity<SiparisOzet>(entity =>
            {
                entity.HasKey(s => s.Id);
                
                // Uye ile SiparisOzet arasındaki Bire-Çok ilişki (Bir üyenin birden fazla siparişi olabilir)
                entity.HasOne<Uye>()
                      .WithMany()
                      .HasForeignKey(s => s.UyeId)
                      .OnDelete(DeleteBehavior.Restrict); // Üye silindiğinde siparişlerin kazara silinmesini önler
            });

            // 3. SiparisDetay Tablosu ve SiparisOzet İlişkisi
            modelBuilder.Entity<SiparisDetay>(entity =>
            {
                entity.HasKey(sd => sd.Id);
                entity.Property(sd => sd.Urun).IsRequired().HasMaxLength(100);
                entity.Property(sd => sd.Beden).IsRequired().HasMaxLength(15);
                entity.Property(sd => sd.KanGrubu).HasMaxLength(10);

                // SiparisOzet ile SiparisDetay arasındaki Bire-Çok ilişki (Bir sipariş özetinin birden çok ürün detayı olur)
                entity.HasOne<SiparisOzet>()
                      .WithMany()
                      .HasForeignKey(sd => sd.SiparisOzetId)
                      .OnDelete(DeleteBehavior.Cascade); // Sipariş özeti silindiğinde alt detayları da otomatik silinir
            });
        }
    }
}
