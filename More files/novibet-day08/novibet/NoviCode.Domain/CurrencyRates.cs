using System;
using System.Collections.Generic;
using System.Text;

namespace NoviCode
{
    public class CurrencyRates
    {

        public CurrencyRates(string currency, decimal rate, DateTime date)
        {
            Currency = currency;
            Rate = rate;
            Date = date;
        }

        public string Currency { get; }
        public decimal Rate { get; }
        public DateTime Date { get; }

        
    }
}
