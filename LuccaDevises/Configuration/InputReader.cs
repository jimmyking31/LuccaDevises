using LuccaDevises.Models;
using LuccaDevises.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LuccaDevises.Configuration
{
    public class InputReader
    {
        public string? SourceCurrency { get; internal set; }
        public string? TargetCurrency { get; internal set; }
        public decimal Amount { get; internal set; }


        private readonly IExchangeRateService _exchangeRateService;

        public InputReader(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }


        private string GetCurrency(string currency)
        {
            if (currency.Length != 3)
                throw new ArgumentException(string.Format("Currency {0} is invalid, its contains {1} characters, expecting 3.", currency, currency.Length));
            return currency;
        }

        private decimal GetAmount(string amount)
        {
            try
            {
                int result = int.Parse(amount, NumberStyles.Integer);

                if (result < 0)
                    throw new ArgumentException(string.Format("Specified amount {0} is invalid. It should be positive.", amount));
                return Convert.ToDecimal(result);
            }
            catch (FormatException ex)
            {
                throw new ArgumentException(string.Format("Specified amount {0} is invalid. It should be a number (integer greater than 0).", amount), ex);
            }
        }

        private decimal GetExchangeRate(string amount)
        {
            if (!decimal.TryParse(amount, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal result))
                throw new ArgumentException(string.Format("Format of exchange rate {0} is invalid.", amount));
            return result;
        }


        public void ReadFirstLine(string line)
        {
            string[] lineArray = line.Split(';');

            if (lineArray.Length != 3)
                throw new ArgumentException(string.Format(
                    "The first line is invalid. It should contain source currency, amount and destination currency separated by ';' character.", line));

            SourceCurrency = GetCurrency(lineArray[0]);
            Amount = GetAmount(lineArray[1]);
            TargetCurrency = GetCurrency(lineArray[2]);
        }

        public void ReadExchangesRate(string[] lines)
        {
            if (!int.TryParse(lines[0], out int nbExchangeRates))
                throw new ArgumentException(string.Format("Format of number of currency change rates {0} is invalid.", lines[0]));

            if (lines.Length <= nbExchangeRates)
                throw new ArgumentException(string.Format("Expected {0} currency exchanges rates lines but got {1}.", lines[0], lines.Length - 1));

            for (int index = 0; index < nbExchangeRates - 1; index++)
            {
                string[] row = lines[1 + index].Split(';');

                _exchangeRateService.Add(new ExchangeRate()
                {
                    Source = GetCurrency(row[0]),
                    Target = GetCurrency(row[1]),
                    Rate = GetExchangeRate(row[2])
                });
            }
        }

        public void ReadFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new ArgumentException(String.Format("Cannot find the file: {0}", filePath));

            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length < 3)
                throw new ArgumentException("The input file require at least 3 lines.");

            ReadFirstLine(lines[0]);
            ReadExchangesRate(lines.Skip(1).ToArray());
        }

    }
}
