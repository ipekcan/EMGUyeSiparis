using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EMGUyeSiparis.WebApi.Data
{
    // EF Core tasarım aracının (Migration komutlarının) doğrudan kullanacağı fabrika sınıfı
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            
            // Buraya appsettings.json içindeki gerçek bağlantı dizinizi (Connection String) yazın
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=EMGUyeSiparisDb;Trusted_Connection=True;MultipleActiveResultSets=true";
            
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
