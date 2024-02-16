# Crypto Portfolio API

This is a .NET Core API that allows users to manage their cryptocurrency portfolios.

## PortfolioController

The `PortfolioController` provides endpoints for uploading a file containing cryptocurrency portfolio data, calculating the portfolio value, and logging the results.

### Endpoints

- **POST /api/portfolio/portfolio-calculated-data**: Uploads a file containing cryptocurrency portfolio data and calculates the portfolio value.
  - Request body should contain a file with portfolio data in the specified format.

## Technologies Used

- .NET Core
- C#
- ASP.NET Core Web API

## Getting Started

To get started with using the Crypto Portfolio API, follow the instructions below:

1. Clone the repository: `git clone https://github.com/your-username/your-repo.git`
2. Open the solution in Visual Studio or your preferred IDE.
3. Build the solution.
4. Run the API project.
5. Use the provided endpoint (`POST /api/portfolio/portfolio-calculated-data`) to upload a file containing your cryptocurrency portfolio data and calculate its value.

## Usage

1. **POST /api/portfolio/portfolio-calculated-data**: Upload a file containing cryptocurrency portfolio data and calculate the portfolio value.
   - Example request:
     ```http
     POST https://localhost:5001/api/portfolio/portfolio-calculated-data
     Content-Type: multipart/form-data

     [File with portfolio data]
     ```
   - Example response:
     ```json
     {
       "portfolio": [
         {
           "amount": 10.0000,
           "coin": "ETH",
           "buyPrice": 123.1400,
           "currentValue": 2568.3500,
           "changePercentage": 108.44
         },
         {
           "amount": 12.1245,
           "coin": "BTC",
           "buyPrice": 24012.4300,
           "currentValue": 290926.6225,
           "changePercentage": 111.49
         },
         ...
       ],
       "currentTotalPortfolioValue": 3182649.5731,
       "initialTotalPortfolioValue": 3002512.0780,
       "changePercentageTotalPortfolio": 6.00
     }
     ```
