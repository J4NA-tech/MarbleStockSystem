using System.Linq.Expressions;

namespace MarbleStockSystem.DAL.Repositories
{
    /// <summary>
    /// Generic Repository Interface
    /// Tüm entity'ler için ortak CRUD işlemlerini tanımlar
    /// </summary>
    /// <typeparam name="T">Entity tipi</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Tüm kayıtları getirir
        /// </summary>
        /// <returns>Entity koleksiyonu</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Belirli bir koşula göre kayıtları getirir
        /// </summary>
        /// <param name="predicate">Filtreleme koşulu</param>
        /// <returns>Filtrelenmiş entity koleksiyonu</returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// ID'ye göre tek bir kayıt getirir
        /// </summary>
        /// <param name="id">Entity ID'si</param>
        /// <returns>Bulunan entity veya null</returns>
        T? GetById(int id);

        /// <summary>
        /// Yeni bir kayıt ekler
        /// </summary>
        /// <param name="entity">Eklenecek entity</param>
        void Add(T entity);

        /// <summary>
        /// Mevcut bir kaydı günceller
        /// </summary>
        /// <param name="entity">Güncellenecek entity</param>
        void Update(T entity);

        /// <summary>
        /// Bir kaydı siler
        /// </summary>
        /// <param name="entity">Silinecek entity</param>
        void Delete(T entity);

        /// <summary>
        /// ID'ye göre bir kaydı siler
        /// </summary>
        /// <param name="id">Silinecek entity ID'si</param>
        void DeleteById(int id);

        /// <summary>
        /// Değişiklikleri veritabanına kaydeder
        /// </summary>
        /// <returns>Etkilenen satır sayısı</returns>
        int SaveChanges();
    }
}



