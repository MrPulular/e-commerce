using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PulularProje.Models
{
    public class Status
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusID { get; set; }

        [StringLength(100)]
        [Required]
        public string? StatusName { get; set; }
        public bool Active { get; set; }
    }
}
