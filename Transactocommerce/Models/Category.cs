using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Transactocommerce.Models
{
    public class Category
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [NotNull]
        [Required]
        public string Name { get; set; }
    }
}
