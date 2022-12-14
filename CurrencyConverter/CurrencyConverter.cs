using System;
using System.Collections.Generic;
using CurrencyConverter.extenssions;
using CurrencyConverter.models;

namespace CurrencyConverter
{
    public class CurrencyConverter
    {
        public event Action<List<Currency>> CurrencyRatesChanged;

        private readonly ICurrencyService currencyService;
        private readonly List<string> currencyNames;

        private List<Currency> previousCurrencies;

        public CurrencyConverter()
        {
            currencyService = new CurrencyService();
            currencyNames = new List<string> { "usd", "eur" };
        }

        public void UpdateCurrencies(object sender, EventArgs args)
        {
            var newCurrencies = currencyService.GetCurrencies(currencyNames);

            if (previousCurrencies == null || !newCurrencies.IsSame(previousCurrencies))
            {
                previousCurrencies = newCurrencies;
                CurrencyRatesChanged?.Invoke(newCurrencies);
            }
        }
    }
}