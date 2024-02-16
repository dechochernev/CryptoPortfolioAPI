namespace CryptoPortfolioAPI.Models
{
    public class CryptoItem
    {
        public decimal Amount { get; set; }
        public string? Coin { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal ChangePercentage { get; set; }
    }
}
