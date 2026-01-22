namespace Web3_kaypic.Models.Finance.DTOs
{
    public class ReportRequestDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Category { get; set; }
    }
}