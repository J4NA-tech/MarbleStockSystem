using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MarbleStockSystem.DAL.Data;

namespace MarbleStockSystem.DAL.Repositories
{
    /// <summary>
    /// Generic Repository Implementation
    /// Tüm entity'ler için ortak CRUD işlemlerini gerçekleştirir
    /// </summary>
    /// <typeparam name="T">Entity tipi</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly MarbleStockDbContext _context;
        protected readonly DbSet<T> _dbSet;

        /// <summary>
        /// Constructor - DbContext'i dependency injection ile alır
        /// </summary>
        public Repository(MarbleStockDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <summary>
        /// Tüm kayıtları getirir
        /// </summary>
        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        /// <summary>
        /// Belirli bir koşula göre kayıtları getirir
        /// </summary>
        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        /// <summary>
        /// ID'ye göre tek bir kayıt getirir
        /// </summary>
        public virtual T? GetById(int id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        /// Yeni bir kayıt ekler
        /// </summary>
        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        /// <summary>
        /// Mevcut bir kaydı günceller
        /// </summary>
        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        /// <summary>
        /// Bir kaydı siler
        /// </summary>
        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// ID'ye göre bir kaydı siler
        /// </summary>
        public virtual void DeleteById(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        /// <summary>
        /// Değişiklikleri veritabanına kaydeder
        /// </summary>
        public virtual int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}



