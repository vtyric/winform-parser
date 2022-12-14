using System.Collections.Generic;
using System.Windows.Forms;
using CurrencyConverter.models;

namespace CurrencyConverter
{
    public partial class CurrencyForm : Form
    {
        private readonly Timer timer;
        private readonly CurrencyConverter currencyConverter;

        private List<Currency> currencies;

        public CurrencyForm()
        {
            const int requestInterval = 60 * 60;
            InitializeComponent();
            timer = new Timer { Interval = requestInterval };
            timer.Start();
            currencyConverter = new CurrencyConverter();
            timer.Tick += currencyConverter.UpdateCurrencies;

            currencyConverter.CurrencyRatesChanged += currencies => { this.currencies = currencies; };
        }
    }
}