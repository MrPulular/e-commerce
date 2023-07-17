using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PulularProje.Models
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }

        [StringLength(100)]
        [Required]
        public string? ProductName { get; set; }

        public decimal UnitPrice { get; set; }

        [DisplayName("Kategori")]
        public int CategoryID { get; set; }

        [DisplayName("Marka")]
        public int SupplierID { get; set; }
        public int Stock { get; set; }
        public int Discount { get; set; }

        [DisplayName("Statüs")]
        public int StatusID { get; set; }
        public DateTime AddDate { get; set; }
        public string? Keywords { get; set; }        
        private int _Kdv { get; set; }
        public int Kdv
        {
            get { return _Kdv; }
            set
            {
                _Kdv = Math.Abs(value);
            }
        }
        public int HighLighted { get; set; } 
        public int TopSeller { get; set; } 
        public int Related { get; set; } 
        public string? Notes { get; set; }
        public string? PhotoPath { get; set; }
        public bool Active { get; set; }
    }
}
