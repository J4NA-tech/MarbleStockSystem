using MarbleStockSystem.DAL.Entities;

namespace MarbleStockSystem.BLL.Interfaces
{
    /// <summary>
    /// Mermer işlemleri için service interface'i
    /// </summary>
    public interface IMarbleService
    {
        /// <summary>
        /// Tüm mermerleri getirir
        /// </summary>
        IEnumerable<Marble> GetAllMarbles();

        /// <summary>
        /// ID'ye göre mermer getirir
        /// </summary>
        Marble? GetMarbleById(int id);

        /// <summary>
        /// Yeni mermer ekler
        /// </summary>
        void AddMarble(Marble marble);

        /// <summary>
        /// Mermer bilgilerini günceller
        /// </summary>
        void UpdateMarble(Marble marble);

        /// <summary>
        /// Mermeri siler
        /// </summary>
        void DeleteMarble(int id);

        /// <summary>
        /// Stok miktarını günceller
        /// </summary>
        void UpdateStock(int marbleId, decimal quantity);
    }
}



