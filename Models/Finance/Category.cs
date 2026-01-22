using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web3_kaypic.Models.Finance
{
    public class Category
    {
        public int Id { get; set; }
        
        public string Nom { get; set; }

        [Required]
        public string Type { get; set; } // "Revenu" / "Dépense"

        public List<Transaction> Transactions { get; set; }
    }
}