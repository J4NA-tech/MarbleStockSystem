using MarbleStockSystem.DAL.Entities;

namespace MarbleStockSystem.BLL.Interfaces
{
    /// <summary>
    /// Satış işlemleri için service interface'i
    /// </summary>
    public interface ISaleService
    {
        /// <summary>
        /// Tüm satışları getirir
        /// </summary>
        IEnumerable<Sale> GetAllSales();

        /// <summary>
        /// ID'ye göre satış getirir
        /// </summary>
        Sale? GetSaleById(int id);

        /// <summary>
        /// Yeni satış yapar (stok kontrolü ve otomatik fiyat hesaplama ile)
        /// </summary>
        /// <param name="marbleId">Satış yapılacak mermer ID'si</param>
        /// <param name="customerId">Müşteri ID'si</param>
        /// <param name="quantity">Satış miktarı (m²)</param>
        /// <returns>Oluşturulan satış kaydı</returns>
        Sale CreateSale(int marbleId, int customerId, decimal quantity);

        /// <summary>
        /// Satış bilgilerini günceller
        /// </summary>
        void UpdateSale(Sale sale);

        /// <summary>
        /// Satışı siler (stok geri eklenir)
        /// </summary>
        void DeleteSale(int id);

        /// <summary>
        /// Belirli bir mermere ait satışları getirir
        /// </summary>
        IEnumerable<Sale> GetSalesByMarbleId(int marbleId);

        /// <summary>
        /// Belirli bir müşteriye ait satışları getirir
        /// </summary>
        IEnumerable<Sale> GetSalesByCustomerId(int customerId);
    }
}



