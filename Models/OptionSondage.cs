using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web3_kaypic.Models
{


    [Table("OptionsSondages")]
    public class OptionSondage
    {
        [Key] public int Id { get; set; }
        public string Texte { get; set; }
        public int SondageId { get; set; }
    }
}
