using Web3_kaypic.Models;
using System;
using web3_kaypic.Models;

namespace Web3_kaypic.Models.Finance
{
    public class StripePayment
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string SessionId { get; set; }
        public string PaymentIntentId { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public string Status { get; set; } // pending, succeeded, failed
    }
}