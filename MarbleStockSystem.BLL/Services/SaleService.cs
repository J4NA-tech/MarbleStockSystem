using MarbleStockSystem.BLL.Interfaces;
using MarbleStockSystem.DAL.Entities;
using MarbleStockSystem.DAL.Repositories;

namespace MarbleStockSystem.BLL.Services
{
    /// <summary>
    /// Satış işlemleri için service implementasyonu
    /// İş kuralları (stok kontrolü, otomatik fiyat hesaplama) burada uygulanır
    /// </summary>
    public class SaleService : ISaleService
    {
        private readonly IRepository<Sale> _saleRepository;
        private readonly IRepository<Marble> _marbleRepository;
        private readonly IRepository<Customer> _customerRepository;

        /// <summary>
        /// Constructor - Repository'leri dependency injection ile alır
        /// </summary>
        public SaleService(
            IRepository<Sale> saleRepository,
            IRepository<Marble> marbleRepository,
            IRepository<Customer> customerRepository)
        {
            _saleRepository = saleRepository;
            _marbleRepository = marbleRepository;
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Tüm satışları getirir
        /// </summary>
        public IEnumerable<Sale> GetAllSales()
        {
            return _saleRepository.GetAll();
        }

        /// <summary>
        /// ID'ye göre satış getirir
        /// </summary>
        public Sale? GetSaleById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Geçersiz satış ID'si", nameof(id));

            return _saleRepository.GetById(id);
        }

        /// <summary>
        /// Yeni satış yapar (stok kontrolü ve otomatik fiyat hesaplama ile)
        /// </summary>
        public Sale CreateSale(int marbleId, int customerId, decimal quantity)
        {
            // Validasyonlar
            if (marbleId <= 0)
                throw new ArgumentException("Geçersiz mermer ID'si", nameof(marbleId));

            if (customerId <= 0)
                throw new ArgumentException("Geçersiz müşteri ID'si", nameof(customerId));

            if (quantity <= 0)
                throw new ArgumentException("Satış miktarı 0'dan büyük olmalıdır", nameof(quantity));

            // Mermer kontrolü
            var marble = _marbleRepository.GetById(marbleId);
            if (marble == null)
                throw new InvalidOperationException("Mermer bulunamadı");

            // Müşteri kontrolü
            var customer = _customerRepository.GetById(customerId);
            if (customer == null)
                throw new InvalidOperationException("Müşteri bulunamadı");

            // Stok kontrolü - İŞ KURALI: Stok yetersizse satış yapılamaz
            if (marble.StockQuantity < quantity)
                throw new InvalidOperationException(
                    $"Yetersiz stok! Mevcut stok: {marble.StockQuantity} m², İstenen: {quantity} m²");

            // Toplam fiyat hesaplama - İŞ KURALI: Otomatik fiyat hesaplama
            decimal totalPrice = marble.PricePerM2 * quantity;

            // Satış kaydı oluştur
            var sale = new Sale
            {
                MarbleId = marbleId,
                CustomerId = customerId,
                Quantity = quantity,
                TotalPrice = totalPrice,
                SaleDate = DateTime.Now
            };

            _saleRepository.Add(sale);

            // Stok güncelleme - İŞ KURALI: Satış yapıldığında stok otomatik azalmalı
            marble.StockQuantity -= quantity;
            _marbleRepository.Update(marble);

            // Değişiklikleri kaydet
            _saleRepository.SaveChanges();

            return sale;
        }

        /// <summary>
        /// Satış bilgilerini günceller
        /// </summary>
        public void UpdateSale(Sale sale)
        {
            if (sale == null)
                throw new ArgumentNullException(nameof(sale), "Satış bilgisi boş olamaz");

            if (sale.SaleId <= 0)
                throw new ArgumentException("Geçersiz satış ID'si", nameof(sale));

            if (sale.Quantity <= 0)
                throw new ArgumentException("Satış miktarı 0'dan büyük olmalıdır", nameof(sale));

            // Mevcut satışı getir
            var existingSale = _saleRepository.GetById(sale.SaleId);
            if (existingSale == null)
                throw new InvalidOperationException("Güncellenecek satış bulunamadı");

            var marble = _marbleRepository.GetById(sale.MarbleId);
            if (marble == null)
                throw new InvalidOperationException("Mermer bulunamadı");

            // Miktar değiştiyse stok kontrolü yap
            if (sale.Quantity != existingSale.Quantity)
            {
                // Eski miktarı geri ekle
                marble.StockQuantity += existingSale.Quantity;

                // Yeni miktar için stok kontrolü
                if (marble.StockQuantity < sale.Quantity)
                    throw new InvalidOperationException(
                        $"Yetersiz stok! Mevcut stok: {marble.StockQuantity} m², İstenen: {sale.Quantity} m²");

                // Yeni miktarı çıkar
                marble.StockQuantity -= sale.Quantity;
                _marbleRepository.Update(marble);
            }

            // Fiyatı yeniden hesapla
            sale.TotalPrice = marble.PricePerM2 * sale.Quantity;

            _saleRepository.Update(sale);
            _saleRepository.SaveChanges();
        }

        /// <summary>
        /// Satışı siler (stok geri eklenir)
        /// </summary>
        public void DeleteSale(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Geçersiz satış ID'si", nameof(id));

            var sale = _saleRepository.GetById(id);
            if (sale == null)
                throw new InvalidOperationException("Silinecek satış bulunamadı");

            // Stok geri ekleme - İŞ KURALI: Satış silindiğinde stok geri eklenir
            var marble = _marbleRepository.GetById(sale.MarbleId);
            if (marble != null)
            {
                marble.StockQuantity += sale.Quantity;
                _marbleRepository.Update(marble);
            }

            _saleRepository.DeleteById(id);
            _saleRepository.SaveChanges();
        }

        /// <summary>
        /// Belirli bir mermere ait satışları getirir
        /// </summary>
        public IEnumerable<Sale> GetSalesByMarbleId(int marbleId)
        {
            if (marbleId <= 0)
                throw new ArgumentException("Geçersiz mermer ID'si", nameof(marbleId));

            return _saleRepository.Find(s => s.MarbleId == marbleId);
        }

        /// <summary>
        /// Belirli bir müşteriye ait satışları getirir
        /// </summary>
        public IEnumerable<Sale> GetSalesByCustomerId(int customerId)
        {
            if (customerId <= 0)
                throw new ArgumentException("Geçersiz müşteri ID'si", nameof(customerId));

            return _saleRepository.Find(s => s.CustomerId == customerId);
        }
    }
}



