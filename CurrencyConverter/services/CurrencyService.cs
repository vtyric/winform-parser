using System.Collections.Generic;
using System.Linq;
using System.Net;
using AngleSharp.Html.Parser;
using CurrencyConverter.models;

namespace CurrencyConverter
{
    public class CurrencyService : ICurrencyService
    {
        private readonly WebClient webClient;
        private readonly HtmlParser htmlParser;

        public CurrencyService()
        {
            webClient = new WebClient();
            htmlParser = new HtmlParser();
        }

        public List<Currency> GetCurrencies(IEnumerable<string> currencyNames) => currencyNames
            .Select(currency => new Currency { Name = currency, Amount = GetAmountByCurrency(currency) })
            .Where(x => x.Amount > 0)
            .ToList();

        private double GetAmountByCurrency(string currency)
        {
            var htmlCodeString = webClient.DownloadString(GetUriByCurrency(currency));
            var document = htmlParser.ParseDocument(htmlCodeString);
            const string currencyClassName = "div.currency-table__large-text";

            return double.TryParse(document.QuerySelectorAll(currencyClassName).FirstOrDefault()?.InnerHtml, out var amount)
                ? amount
                : -1;
        }

        private string GetUriByCurrency(string currency) => $"https://www.banki.ru/products/currency/{currency}/";
    }
}