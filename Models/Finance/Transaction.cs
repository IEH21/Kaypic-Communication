using System;
using System.ComponentModel.DataAnnotations;
using web3_kaypic.Models;
using Web3_kaypic.Models;

namespace Web3_kaypic.Models.Finance
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        public string Titre { get; set; }  // ← Ajouté

        [Required]
        public decimal Montant { get; set; }  // ← Ajouté

        [Required]
        public string Type { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }
    }
}