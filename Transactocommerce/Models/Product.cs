using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Transactocommerce.Models
{
    public class Product
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [NotNull]
        [Required]
        public string Name { get; set; }

        [NotNull]
        public string Description { get; set; }

        [NotNull]
        [Required]
        [Column(TypeName = "money")] // Postgresql data type
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        [NotNull]
        [Required]
        public string Manufacturer { get; set; }

        [ForeignKey("Category")]
        [NotNull]
        public string CategoryId { get; set; }

        public int Stock { get; set; } = 0;
    }

    public class ProductWriteDTO
    {
        [NotNull]
        [Required]
        public string Name { get; set; }

        [NotNull]
        public string Description { get; set; }

        [NotNull]
        [Required]
        [Column(TypeName = "money")] // Postgresql data type
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        [NotNull]
        [Required]
        public string Manufacturer { get; set; }

        [ForeignKey("Category")]
        [NotNull]
        public string CategoryId { get; set; }

        public int Stock { get; set; } = 0;
    }
}
