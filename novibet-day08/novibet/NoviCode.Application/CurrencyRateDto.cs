using System;
using System.Collections.Generic;
using System.Text;

namespace NoviCode
{
    public class CurrencyRateDto
    {
        public CurrencyRateDto(string currency, decimal rate, DateTime date)
        {
            Currency = currency;
            Rate = rate;
            Date = date;
        }

        public string Currency { get; set; }
        public decimal Rate{ get; set; }
        public DateTime Date{ get; set;  }

       
    }
}
