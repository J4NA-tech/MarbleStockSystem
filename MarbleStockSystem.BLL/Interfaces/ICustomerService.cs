using MarbleStockSystem.DAL.Entities;

namespace MarbleStockSystem.BLL.Interfaces
{
    /// <summary>
    /// Müşteri işlemleri için service interface'i
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// Tüm müşterileri getirir
        /// </summary>
        IEnumerable<Customer> GetAllCustomers();

        /// <summary>
        /// ID'ye göre müşteri getirir
        /// </summary>
        Customer? GetCustomerById(int id);

        /// <summary>
        /// Yeni müşteri ekler
        /// </summary>
        void AddCustomer(Customer customer);

        /// <summary>
        /// Müşteri bilgilerini günceller
        /// </summary>
        void UpdateCustomer(Customer customer);

        /// <summary>
        /// Müşteriyi siler
        /// </summary>
        void DeleteCustomer(int id);
    }
}



