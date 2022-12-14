using System;
using System.Collections.Generic;
using System.Linq;
using CurrencyConverter.models;

namespace CurrencyConverter.extenssions
{
    public static class CurrenciesListExtensions
    {
        public static bool IsSame(this IEnumerable<Currency> oldCurrencies, IEnumerable<Currency> newCurrencies) =>
            oldCurrencies.All(a => newCurrencies.Any(b => b.Name.SequenceEqual(a.Name) && Math.Abs(a.Amount - b.Amount) < 10e4));

        public static double ConvertTo(this Currency from, Currency to, double count = 1) =>
            Math.Round(from.Amount * count / to.Amount, 3);
    }
}