using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;

namespace WebAPI.Models
{
    public class CurrencyService : BackgroundService
    {
        private readonly IMemoryCache memoryCache;

        public CurrencyService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("btc-BTC"); 

                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                    XDocument xml = XDocument.Load("https://api.privatbank.ua/p24api/pubinfo?exchange&coursid=5");

                    BtcRate btcRate = new BtcRate();


                   
                    btcRate.USD = Convert.ToDecimal(xml.Elements("exchangerates").FirstOrDefault(x => x.Attribute("base_ccy").Value == "USD").Elements("exchangerates").Attributes("buy").FirstOrDefault().Value);

                    btcRate.UAH = Convert.ToDecimal(xml.Elements("exchangerates").FirstOrDefault(x => x.Attribute("base_ccy").Value == "UAH").Elements("exchangerates").Attributes("buy").FirstOrDefault().Value);

                    memoryCache.Set("key_currency", btcRate, TimeSpan.FromMinutes(60));
                }
                catch (Exception e)
                {
                    logger.LogError(e.InnerException.Message);
                }

                await Task.Delay(3600000, stoppingToken);
            }
        }
    }
}
