namespace CryptoPortfolioAPI
{
    public static class PercentageCalculator
    {
        public static decimal CalculatePercentageChange(decimal oldValue, decimal newValue)
        {
            if (oldValue == 0)
            {
                if (newValue == 0)
                {
                    return 0; // If both old and new values are 0, the percentage change is 0
                }
                else
                {
                    return decimal.MaxValue; // If the old value is 0 and the new value is not, the percentage change is infinite
                }
            }

            return ((newValue - oldValue) / Math.Abs(oldValue)) * 100;
        }
    }
}
