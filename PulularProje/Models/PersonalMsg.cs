using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PulularProje.Models
{
    public class PersonalMsg
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        [Required]
        public string MsgUserName { get; set; }
        [EmailAddress]
        public string MsgEmail { get; set; }
        [Required]
        public string MsgUserMsg { get; set; }
    }
}
