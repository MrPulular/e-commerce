using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PulularProje.Models
{
    public class Setting
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int settingID { get; set; }
        public string? telephone { get; set; }

        [EmailAddress]
        public string? email { get; set; }

        public string? address { get; set; }
        public int mainpageCount { get; set; }
        public int subpageCount { get; set; }
    }
}
