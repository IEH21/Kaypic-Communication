namespace Web3_kaypic.Models.Finance.DTOs
{
    public class TransactionCreateDto
    {
        public decimal Amount { get; set; }
        public string Type { get; set; } // revenu / depense
        public string Note { get; set; }
        public int? CategoryId { get; set; }
        public DateTime Date { get; set; }
    }
}