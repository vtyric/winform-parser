using System.Collections.Generic;
using CurrencyConverter.models;

namespace CurrencyConverter
{
    public interface ICurrencyService
    {
        List<Currency> GetCurrencies(IEnumerable<string> currencyNames);
    }
}