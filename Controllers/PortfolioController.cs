using CryptoPortfolioAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CryptoPortfolioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly IHttpClientFactory ClientFactory;
        private readonly ILogger<PortfolioController> Logger;

        public PortfolioController(IHttpClientFactory clientFactory, ILogger<PortfolioController> logger)
        {
            ClientFactory = clientFactory;
            Logger = logger;
        }

        [HttpPost("portfolio-calculated-data")]
        public async Task<IActionResult> UploadAndCalculatePortfolio([Bind] IFormFile file)
        {
            try
            {
                // Read file content
                using (var streamReader = new StreamReader(file.OpenReadStream()))
                {
                    var fileContent = await streamReader.ReadToEndAsync();

                    // Parse file content and calculate portfolio
                    var portfolio = ParseFileContent(fileContent);
                    var portfolioData = await CalculatePortfolioValue(portfolio);

                    return Ok(portfolioData);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        private List<CryptoItem> ParseFileContent(string fileContent)
        {
            var lines = fileContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var portfolio = new List<CryptoItem>();

            foreach (var line in lines)
            {
                var parts = line.Trim().Split('|');
                if (parts.Length == 3)
                {
                    var amount = Convert.ToDecimal(parts[0]);
                    var coin = parts[1];
                    var buyPrice = Convert.ToDecimal(parts[2]);
                    portfolio.Add(new CryptoItem { Amount = amount, Coin = coin, BuyPrice = buyPrice });
                }
            }

            return portfolio;
        }

        private async Task<PortfolioData> CalculatePortfolioValue(List<CryptoItem> portfolio)
        {
            var client = ClientFactory.CreateClient();
            var response = await client.GetAsync("https://api.coinlore.com/api/tickers/");
            var deserializeOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseUpper
            };
            var cryptoData = await response.Content.ReadFromJsonAsync<CryptoData>(deserializeOptions);

            decimal currentTotalPortfolioValue = 0;
            decimal initialTotalPortfolioValue = 0;

            foreach (var item in portfolio)
            {
                var crypto = cryptoData?.Data.FirstOrDefault(c => c.Symbol == item.Coin);
                if (crypto != null)
                {
                    var currentPrice = decimal.Parse(crypto.PriceUsd);
                    var currentValue = item.Amount * currentPrice;
                    var initialValue = item.Amount * item.BuyPrice;
                    initialTotalPortfolioValue += initialValue;
                    currentTotalPortfolioValue += currentValue;
                    item.CurrentValue = currentValue;
                    item.ChangePercentage = PercentageCalculator.CalculatePercentageChange(initialValue, currentValue);
                }
            }

            decimal changePercentageTotalPortfolio = PercentageCalculator.CalculatePercentageChange(initialTotalPortfolioValue, currentTotalPortfolioValue);

            //Convert portfolio
            portfolio.ForEach(p => {
                p.Amount = Math.Round(p.Amount, 4, MidpointRounding.ToZero);
                p.BuyPrice = Math.Round(p.BuyPrice, 4, MidpointRounding.ToZero);
                p.CurrentValue = Math.Round(p.CurrentValue, 4, MidpointRounding.ToZero);
                p.ChangePercentage = Math.Round(p.ChangePercentage, 2, MidpointRounding.ToZero);
            });

            var portfolioData = new PortfolioData()
            {
                ChangePercentageTotalPortfolio = Math.Round(changePercentageTotalPortfolio, 2, MidpointRounding.ToZero),
                CurrentTotalPortfolioValue = Math.Round(currentTotalPortfolioValue, 4, MidpointRounding.ToZero),
                InitialTotalPortfolioValue = Math.Round(initialTotalPortfolioValue, 4, MidpointRounding.ToZero),
                Portfolio = portfolio
            };

            LogResultOfCalculationOperations(portfolio, portfolioData);

            return portfolioData;
        }

        private void LogResultOfCalculationOperations(List<CryptoItem> portfolio, PortfolioData portfolioData)
        {
            var logContent = $"{DateTime.Now.ToString()} - Portfolio: {JsonSerializer.Serialize(portfolio)}," +
                $" Total Portfolio Value: {portfolioData.CurrentTotalPortfolioValue}" +
                $" Initial Total Portfolio Value: {portfolioData.InitialTotalPortfolioValue}" +
                $" Change Persentage Total Portfolio Value: {portfolioData.ChangePercentageTotalPortfolio}%\n";
            Logger.LogInformation(logContent);
            var path = "log.txt";
            System.IO.File.AppendAllText(path, logContent);
        }
    }
}
