using MarbleStockSystem.BLL.Interfaces;
using MarbleStockSystem.DAL.Entities;
using MarbleStockSystem.DAL.Repositories;

namespace MarbleStockSystem.BLL.Services
{
    /// <summary>
    /// Mermer işlemleri için service implementasyonu
    /// İş kuralları ve validasyonlar burada uygulanır
    /// </summary>
    public class MarbleService : IMarbleService
    {
        private readonly IRepository<Marble> _marbleRepository;

        /// <summary>
        /// Constructor - Repository'yi dependency injection ile alır
        /// </summary>
        public MarbleService(IRepository<Marble> marbleRepository)
        {
            _marbleRepository = marbleRepository;
        }

        /// <summary>
        /// Tüm mermerleri getirir
        /// </summary>
        public IEnumerable<Marble> GetAllMarbles()
        {
            return _marbleRepository.GetAll();
        }

        /// <summary>
        /// ID'ye göre mermer getirir
        /// </summary>
        public Marble? GetMarbleById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Geçersiz mermer ID'si", nameof(id));

            return _marbleRepository.GetById(id);
        }

        /// <summary>
        /// Yeni mermer ekler
        /// </summary>
        public void AddMarble(Marble marble)
        {
            if (marble == null)
                throw new ArgumentNullException(nameof(marble), "Mermer bilgisi boş olamaz");

            // Validasyonlar
            if (string.IsNullOrWhiteSpace(marble.Name))
                throw new ArgumentException("Mermer adı boş olamaz", nameof(marble));

            if (string.IsNullOrWhiteSpace(marble.Type))
                throw new ArgumentException("Mermer tipi boş olamaz", nameof(marble));

            if (string.IsNullOrWhiteSpace(marble.Color))
                throw new ArgumentException("Mermer rengi boş olamaz", nameof(marble));

            if (marble.Thickness <= 0)
                throw new ArgumentException("Mermer kalınlığı 0'dan büyük olmalıdır", nameof(marble));

            if (marble.PricePerM2 <= 0)
                throw new ArgumentException("Metrekare fiyatı 0'dan büyük olmalıdır", nameof(marble));

            if (marble.StockQuantity < 0)
                throw new ArgumentException("Stok miktarı negatif olamaz", nameof(marble));

            _marbleRepository.Add(marble);
            _marbleRepository.SaveChanges();
        }

        /// <summary>
        /// Mermer bilgilerini günceller
        /// </summary>
        public void UpdateMarble(Marble marble)
        {
            if (marble == null)
                throw new ArgumentNullException(nameof(marble), "Mermer bilgisi boş olamaz");

            if (marble.MarbleId <= 0)
                throw new ArgumentException("Geçersiz mermer ID'si", nameof(marble));

            // Mevcut entity'yi DB'den al (TRACKED)
            var existingMarble = _marbleRepository.GetById(marble.MarbleId);

            if (existingMarble == null)
                throw new InvalidOperationException("Güncellenecek mermer bulunamadı");

            // Validasyonlar
            if (string.IsNullOrWhiteSpace(marble.Name))
                throw new ArgumentException("Mermer adı boş olamaz", nameof(marble));

            if (string.IsNullOrWhiteSpace(marble.Type))
                throw new ArgumentException("Mermer tipi boş olamaz", nameof(marble));

            if (string.IsNullOrWhiteSpace(marble.Color))
                throw new ArgumentException("Mermer rengi boş olamaz", nameof(marble));

            if (marble.Thickness <= 0)
                throw new ArgumentException("Mermer kalınlığı 0'dan büyük olmalıdır", nameof(marble));

            if (marble.PricePerM2 <= 0)
                throw new ArgumentException("Metrekare fiyatı 0'dan büyük olmalıdır", nameof(marble));

            if (marble.StockQuantity < 0)
                throw new ArgumentException("Stok miktarı negatif olamaz", nameof(marble));

            // Alanları güncelle 
            existingMarble.Name = marble.Name;
            existingMarble.Type = marble.Type;
            existingMarble.Color = marble.Color;
            existingMarble.Thickness = marble.Thickness;
            existingMarble.PricePerM2 = marble.PricePerM2;
            existingMarble.StockQuantity = marble.StockQuantity;

            _marbleRepository.SaveChanges();
        }


        /// <summary>
        /// Mermeri siler
        /// </summary>
        public void DeleteMarble(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Geçersiz mermer ID'si", nameof(id));

            var marble = _marbleRepository.GetById(id);
            if (marble == null)
                throw new InvalidOperationException("Silinecek mermer bulunamadı");

            // İlişkili satışlar varsa silme işlemi engellenir (Foreign Key constraint)
            _marbleRepository.DeleteById(id);
            _marbleRepository.SaveChanges();
        }

        /// <summary>
        /// Stok miktarını günceller
        /// </summary>
        public void UpdateStock(int marbleId, decimal quantity)
        {
            if (marbleId <= 0)
                throw new ArgumentException("Geçersiz mermer ID'si", nameof(marbleId));

            var marble = _marbleRepository.GetById(marbleId);
            if (marble == null)
                throw new InvalidOperationException("Mermer bulunamadı");

            marble.StockQuantity = quantity;
            _marbleRepository.Update(marble);
            _marbleRepository.SaveChanges();
        }
    }
}



