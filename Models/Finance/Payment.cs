using Web3_kaypic.Models;
using System;
using System.ComponentModel.DataAnnotations;
using web3_kaypic.Models;

namespace Web3_kaypic.Models.Finance
{
    public class Payment
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaidAt { get; set; }

        public string PaymentMethod { get; set; } // Stripe, cash, autre
        public string Status { get; set; } // Paid, Pending, Failed

        public string ReferenceId { get; set; } // ID Stripe
    }
}