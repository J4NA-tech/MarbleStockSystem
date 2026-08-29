using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarbleStockSystem.DAL.Entities
{
    /// <summary>
    /// Satış entity'si - Yapılan satış işlemlerini temsil eder
    /// </summary>
    [Table("Sales")]
    public class Sale
    {
        /// <summary>
        /// Satış benzersiz kimlik numarası (Primary Key)
        /// </summary>
        [Key]
        public int SaleId { get; set; }

        /// <summary>
        /// Satış yapılan mermer ID'si (Foreign Key)
        /// </summary>
        [Required]
        public int MarbleId { get; set; }

        /// <summary>
        /// Satış yapılan müşteri ID'si (Foreign Key)
        /// </summary>
        [Required]
        public int CustomerId { get; set; }

        /// <summary>
        /// Satış miktarı (metrekare cinsinden)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Toplam satış fiyatı
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Satış tarihi
        /// </summary>
        [Required]
        public DateTime SaleDate { get; set; }

        /// <summary>
        /// Navigation property - Satış yapılan mermer
        /// </summary>
        [ForeignKey("MarbleId")]
        public virtual Marble? Marble { get; set; }

        /// <summary>
        /// Navigation property - Satış yapılan müşteri
        /// </summary>
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }
    }
}



