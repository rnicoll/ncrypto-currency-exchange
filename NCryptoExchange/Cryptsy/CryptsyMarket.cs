﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lostics.NCryptoExchange.Model;
using Newtonsoft.Json.Linq;

namespace Lostics.NCryptoExchange.Cryptsy
{
    public class CryptsyMarket : Market
    {
        public CryptsyMarket(CryptsyMarketId setMarketId, string baseCurrencyCode, string baseCurrencyName,
            string quoteCurrencyCode, string quoteCurrencyName, string label,
            MarketStatistics statistics, DateTime created)
            : base(setMarketId, baseCurrencyCode, quoteCurrencyCode, label, statistics)
        {
            this.Created = created;
            this.BaseCurrencyName = baseCurrencyName;
            this.QuoteCurrencyName = quoteCurrencyName;
        }

        public static CryptsyMarket Parse(JObject marketObj, TimeZoneInfo timeZone) {
            DateTime created = DateTime.Parse(marketObj.Value<string>("created"));

            TimeZoneInfo.ConvertTimeToUtc(created, timeZone);
            
            MarketStatistics statistics = new MarketStatistics()
            {
                Volume24HBase = marketObj.Value<decimal>("current_volume"),
                LastTrade = marketObj.Value<decimal>("last_trade"),
                HighTrade = marketObj.Value<decimal>("high_trade"),
                LowTrade = marketObj.Value<decimal>("low_trade")
            };

            return new CryptsyMarket(new CryptsyMarketId(marketObj.Value<string>("marketid")),
                marketObj.Value<string>("primary_currency_code"), marketObj.Value<string>("primary_currency_name"),
                marketObj.Value<string>("secondary_currency_code"), marketObj.Value<string>("secondary_currency_name"),
                marketObj.Value<string>("label"),
                statistics, created
            );
        }

        public DateTime Created { get; private set; }
        public string BaseCurrencyName { get; private set; }
        public string QuoteCurrencyName { get; private set; }
    }
}
