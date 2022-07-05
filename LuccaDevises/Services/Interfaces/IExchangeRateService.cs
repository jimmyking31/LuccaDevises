using LuccaDevises.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuccaDevises.Services.Interfaces
{
    public interface IExchangeRateService
    {
        public void Add(ExchangeRate exchangeRate);
        public decimal Convert(string source, string target, decimal amount);
    }
}
