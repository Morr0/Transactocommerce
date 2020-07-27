using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Transactocommerce.Models
{
    public class Order
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // Human readable ID
        public long OrderNo { get; set; }

        [ForeignKey("Products")]
        public List<Product> Products { get; set; }

        [Required]
        [NotNull]
        public string Name { get; set; }

        [Required]
        [NotNull]
        public string Email { get; set; }

        [Required]
        [NotNull]
        public string Address { get; set; }

        [Required]
        [NotNull]
        public string TransactionId { get; set; }

        public DateTime OrderConfirmTime { get; set; } = DateTime.UtcNow;
    }
}
