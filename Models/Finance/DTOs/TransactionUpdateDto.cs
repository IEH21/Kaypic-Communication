namespace Web3_kaypic.Models.Finance.DTOs
{
    public class TransactionUpdateDto
    {
        public decimal Amount { get; set; }
        public string Note { get; set; }
        public int? CategoryId { get; set; }
        public DateTime Date { get; set; }
    }
}