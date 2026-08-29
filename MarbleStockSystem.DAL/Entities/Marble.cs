using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarbleStockSystem.DAL.Entities
{
    /// <summary>
    /// Mermer entity'si - Stokta bulunan mermer çeşitlerini temsil eder
    /// </summary>
    [Table("Marbles")]
    public class Marble
    {
        /// <summary>
        /// Mermer benzersiz kimlik numarası (Primary Key)
        /// </summary>
        [Key]
        public int MarbleId { get; set; }

        /// <summary>
        /// Mermer adı
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Mermer tipi (örn: Traverten, Granit, Mermer vb.)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Mermer rengi
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Mermer kalınlığı (cm cinsinden)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Thickness { get; set; }

        /// <summary>
        /// Metrekare başına fiyat
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerM2 { get; set; }

        /// <summary>
        /// Stok miktarı (metrekare cinsinden)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal StockQuantity { get; set; }

        /// <summary>
        /// Navigation property - Bu mermerle yapılan satışlar
        /// </summary>
        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}



