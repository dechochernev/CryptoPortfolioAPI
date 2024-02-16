namespace CryptoPortfolioAPI.Models
{
    public class PortfolioData
    {
        public List<CryptoItem> Portfolio {  get; set; }
        public decimal CurrentTotalPortfolioValue { get; set; }
        public decimal InitialTotalPortfolioValue { get; set; }
        public decimal ChangePercentageTotalPortfolio { get; set; }
    }
}
