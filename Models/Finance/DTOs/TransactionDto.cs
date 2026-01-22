namespace Web3_kaypic.Models.Finance.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Montant { get; set; }  
        public string Category { get; set; }
        public string Titre { get; set; }     
        public string Note { get; set; }    
        public DateTime Date { get; set; }
    }

}