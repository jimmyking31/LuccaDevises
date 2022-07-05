using LuccaDevises.Models;
using LuccaDevises.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuccaDevises.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private List<ExchangeRate> ExchangeRates = new List<ExchangeRate>();
        private TreeNode TreeNode = new TreeNode();

        public void Add(ExchangeRate exchangeRate)
        {
            ExchangeRates.Add(exchangeRate);
            TreeNode.Add(exchangeRate);
        }

        public decimal Convert(string source, string target, decimal amount)
        {
            var path = TreeNode.ShortestPath(source, target);

            decimal result = amount;

            foreach (var rate in path.Select(connection => connection.Rate))
            {
                result = Math.Round(result * rate, 4, MidpointRounding.AwayFromZero);
            }

            return Math.Round(result, 0, MidpointRounding.AwayFromZero);
        }

    }
}
