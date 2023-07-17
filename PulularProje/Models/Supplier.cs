using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PulularProje.Models
{
    public class Supplier
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierID { get; set; }

        [StringLength(100)]
        [Required]
        public string? BrandName { get; set; }

        public string? PhotoPath { get; set; }

        public bool Active { get; set; }
    }
}
