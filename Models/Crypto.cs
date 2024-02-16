using Newtonsoft.Json;

namespace CryptoPortfolioAPI.Models
{
    public class Crypto
    {
        public string Symbol { get; set; }
        public string PriceUsd { get; set; }
    }
}
