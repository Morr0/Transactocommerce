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

        public List<string> ProductsIds { get; set; } = new List<string>();

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

        // A complete order implies that it was handled by the PaymentSystem webhook method
        public bool Complete { get; set; } = false;

        // An order can be completed but failed to happen because of maybe no payment was allowed
        // default -> true -> in case nothing happens
        public bool Failed { get; set; } = true;

        public DateTime OrderStartTime { get; set; } = DateTime.UtcNow;

        public DateTime OrderConfirmTime { get; set; }
    }
}
