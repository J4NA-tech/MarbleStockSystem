using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MarbleStockSystem.DAL.Data;
using MarbleStockSystem.DAL.Repositories;
using MarbleStockSystem.DAL.Entities;
using MarbleStockSystem.BLL.Interfaces;
using MarbleStockSystem.BLL.Services;
using MarbleStockSystem.PL.Forms;

namespace MarbleStockSystem.PL
{
    /// <summary>
    /// Uygulama giriş noktası
    /// Dependency Injection yapılandırması burada yapılır
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Dependency Injection container'ı oluştur
            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;

            // Veritabanını oluştur (Code First yaklaşımı)
            using (var scope = ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MarbleStockDbContext>();
                dbContext.Database.EnsureCreated();
            }

            // Ana formu başlat
            var mainForm = ServiceProvider.GetRequiredService<MainForm>();
            Application.Run(mainForm);
        }

        /// <summary>
        /// Dependency Injection için ServiceProvider
        /// </summary>
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        /// <summary>
        /// Host builder oluşturur ve servisleri yapılandırır
        /// </summary>
        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // DbContext yapılandırması
                    // Connection string'i buradan değiştirebilirsiniz
                    services.AddDbContext<MarbleStockDbContext>(options =>
                        options.UseSqlServer(
                            "Server=(localdb)\\mssqllocaldb;Database=MarbleStockSystemDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"));

                    // Generic Repository'leri kaydet
                    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

                    // Service'leri kaydet
                    services.AddScoped<IMarbleService, MarbleService>();
                    services.AddScoped<ICustomerService, CustomerService>();
                    services.AddScoped<ISaleService, SaleService>();

                    // Form'ları kaydet
                    services.AddScoped<MainForm>();
                    services.AddScoped<MarbleManagementForm>();
                    services.AddScoped<CustomerManagementForm>();
                    services.AddScoped<SaleForm>();
                });
        }
    }
}

