namespace Web3_kaypic.Models.Finance.DTOs
{
    public class StripeCheckoutDto
    {
        public decimal Amount { get; set; }
        public string SuccessUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}