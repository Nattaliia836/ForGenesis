using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class BtcRate
    {
        //public decimal BTC { get; set; }
        public decimal USD { get; set; }
        public decimal ConvertToUSD(decimal priceRUB) => priceRUB / USD;
        public decimal UAH { get; set; }
        public decimal ConvertToUAH(decimal priceBTC) => priceBTC / UAH;

    }
}
