namespace Web3_kaypic.Models.Finance.DTOs
{
    public class DashboardRequestDto
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? CategoryId { get; set; }
        public string? MemberId { get; set; }
    }
}