using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarbleStockSystem.DAL.Entities
{
    /// <summary>
    /// Müşteri entity'si - Sisteme kayıtlı müşterileri temsil eder
    /// </summary>
    [Table("Customers")]
    public class Customer
    {
        /// <summary>
        /// Müşteri benzersiz kimlik numarası (Primary Key)
        /// </summary>
        [Key]
        public int CustomerId { get; set; }

        /// <summary>
        /// Müşteri tam adı
        /// </summary>
        [Required]
        [StringLength(200)]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Müşteri telefon numarası
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Müşteri adresi
        /// </summary>
        [StringLength(500)]
        public string? Address { get; set; }

        /// <summary>
        /// Navigation property - Bu müşterinin yaptığı satışlar
        /// </summary>
        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}



