namespace Web3_kaypic.Models.Finance.DTOs
{
    public class DashboardResponseDto
    {
        public decimal CurrentBalance { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }

        public Dictionary<string, decimal> MonthlyTotals { get; set; } = new();
        public Dictionary<string, decimal> CategoryTotals { get; set; } = new();

        public List<TransactionDto> RecentTransactions { get; set; } = new();
    }
}