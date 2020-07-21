using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Transactocommerce.Models
{
    public class Category // Use it for read/write DTOs
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // Made unique in the DataContext
        [NotNull]
        [Required]
        public string Name { get; set; }

        [NotNull]
        public string Description { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
