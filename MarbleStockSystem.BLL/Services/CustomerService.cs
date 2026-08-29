using MarbleStockSystem.BLL.Interfaces;
using MarbleStockSystem.DAL.Entities;
using MarbleStockSystem.DAL.Repositories;

namespace MarbleStockSystem.BLL.Services
{
    /// <summary>
    /// Müşteri işlemleri için service implementasyonu
    /// İş kuralları ve validasyonlar burada uygulanır
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;

        /// <summary>
        /// Constructor - Repository'yi dependency injection ile alır
        /// </summary>
        public CustomerService(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Tüm müşterileri getirir
        /// </summary>
        public IEnumerable<Customer> GetAllCustomers()
        {
            return _customerRepository.GetAll();
        }

        /// <summary>
        /// ID'ye göre müşteri getirir
        /// </summary>
        public Customer? GetCustomerById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Geçersiz müşteri ID'si", nameof(id));

            return _customerRepository.GetById(id);
        }

        /// <summary>
        /// Yeni müşteri ekler
        /// </summary>
        public void AddCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer), "Müşteri bilgisi boş olamaz");

            // Validasyonlar
            if (string.IsNullOrWhiteSpace(customer.FullName))
                throw new ArgumentException("Müşteri adı boş olamaz", nameof(customer));

            if (string.IsNullOrWhiteSpace(customer.Phone))
                throw new ArgumentException("Telefon numarası boş olamaz", nameof(customer));

            _customerRepository.Add(customer);
            _customerRepository.SaveChanges();
        }

        /// <summary>
        /// Müşteri bilgilerini günceller
        /// </summary>
        public void UpdateCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer), "Müşteri bilgisi boş olamaz");

            if (customer.CustomerId <= 0)
                throw new ArgumentException("Geçersiz müşteri ID'si", nameof(customer));

            // Mevcut müşteriyi DB'den al (TRACKED)
            var existingCustomer = _customerRepository.GetById(customer.CustomerId);

            if (existingCustomer == null)
                throw new InvalidOperationException("Güncellenecek müşteri bulunamadı");

            // Validasyonlar
            if (string.IsNullOrWhiteSpace(customer.FullName))
                throw new ArgumentException("Müşteri adı boş olamaz", nameof(customer));

            if (string.IsNullOrWhiteSpace(customer.Phone))
                throw new ArgumentException("Telefon numarası boş olamaz", nameof(customer));

            existingCustomer.FullName = customer.FullName;
            existingCustomer.Phone = customer.Phone;
            existingCustomer.Address = customer.Address;

            _customerRepository.SaveChanges();
        }

        /// <summary>
        /// Müşteriyi siler
        /// </summary>
        public void DeleteCustomer(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Geçersiz müşteri ID'si", nameof(id));

            var customer = _customerRepository.GetById(id);
            if (customer == null)
                throw new InvalidOperationException("Silinecek müşteri bulunamadı");

            // İlişkili satışlar varsa silme işlemi engellenir (Foreign Key constraint)
            _customerRepository.DeleteById(id);
            _customerRepository.SaveChanges();
        }
    }
}



