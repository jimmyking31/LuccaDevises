using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuccaDevises.Models
{
    public class ExchangeRate
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public decimal Rate { get; set; }
    }
}
