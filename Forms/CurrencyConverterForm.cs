using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using CurrencyConverter.extenssions;
using CurrencyConverter.models;

namespace CurrencyConverter
{
    public partial class CurrencyForm : Form
    {
        private readonly Timer timer;
        private readonly CurrencyConverter currencyConverter;

        private string from = "usd";
        private string to = "eur";
        private double amount = 1;

        private Currency usd;
        private Currency eur;
        private Currency rub;

        public CurrencyForm()
        {
            InitializeComponent();
            const int requestInterval = 1;
            timer = new Timer { Interval = requestInterval };
            timer.Start();
            currencyConverter = new CurrencyConverter();
            timer.Tick += (o, args) =>
            {
                timer.Stop();
                currencyConverter.UpdateCurrencies(o, args);
            };
            currencyConverter.CurrencyRatesChanged += MakeFormStep;
            
        }

        private void MakeFormStep(List<Currency> currencies)
        {
            AddTopLabels(currencies);
            var amountInput = new TextBox
            {
                Location = new Point(44, 246),
                Size = new Size(100, 20),
                TabIndex = 0,
                Text = "1"
            };
            var resultInput = new Label
            {
                Location = new Point(613, 246),
                Size = new Size(100, 23),
                TabIndex = 1,
                Text = $"={GetResult()}"
            };
            Controls.Add(amountInput);
            Controls.Add(resultInput);
            amountInput.TextChanged += (sender, args) =>
            {
                amount = double.TryParse(amountInput.Text, out var newAmount) ? newAmount : 0;
                resultInput.Text = $"={GetResult()}";
            };
            var fromCombobox = GetComboBox(new object[] { "usd", "eur", "rub" }, 240, "usd");
            var toCombobox = GetComboBox(new object[] { "eur", "usd", "rub" }, 480, "eur");
            Controls.Add(fromCombobox);
            Controls.Add(toCombobox);
            fromCombobox.TextChanged += (sender, args) =>
            {
                from = fromCombobox.Text;
                resultInput.Text = $"={GetResult()}";
            };
            toCombobox.TextChanged += (sender, args) =>
            {
                to = toCombobox.Text;
                resultInput.Text = $"={GetResult()}";
            };
        }
        
        private ComboBox GetComboBox(object[] names, int x, string initValue)
        {
            var comboBox = new ComboBox()
            {
                Location = new Point(x, 246),
                Size = new Size(100, 20),
                Text = initValue
            };
            comboBox.Items.AddRange(names);

            return comboBox;
        }

        private string GetResult() =>
            GetCurrencyByName(from).ConvertTo(GetCurrencyByName(to), amount).ToString(CultureInfo.InvariantCulture);

        private Currency GetCurrencyByName(string name)
        {
            if (name == "rub")
                return rub;

            if (name == "eur")
                return eur;

            return usd;
        }

        private void AddTopLabels(IReadOnlyList<Currency> currencies)
        {
            usd = currencies[0];
            eur = currencies[1];
            rub = new Currency { Name = "rub", Amount = 1 };

            var usdLabel = new Label
            {
                Text = $"1 {usd.Name} = {usd.ConvertTo(rub)} rub",
                Location = new Point(115, 94),
                Size = new Size(140, 140),
            };
            var eurLabel = new Label
            {
                Text = $"1 {eur.Name} = {eur.ConvertTo(rub)} rub",
                Location = new Point(347, 94),
                Size = new Size(140, 140),
            };
            var usdEurLabel = new Label
            {
                Text = $"1 {usd.Name} = {usd.ConvertTo(eur)} {eur.Name}",
                Location = new Point(598, 94),
                Size = new Size(140, 140),
            };
            Controls.Add(usdLabel);
            Controls.Add(eurLabel);
            Controls.Add(usdEurLabel);
        }
    }
}