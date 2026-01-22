using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web3_kaypic.Models
{
    [Table("Sondages")]
    public class Sondage
    {
        [Key] public int Id { get; set; }
        public string Question { get; set; }
        public DateTime Expiration { get; set; }
        public List<OptionSondage> Options { get; set; }
    }
}
