﻿using Lostics.NCryptoExchange.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lostics.NCryptoExchange
{
    public abstract class AbstractExchange : IDisposable, IMarketDataSource
    {
        public static string GenerateSHA512Signature(FormUrlEncodedContent request, byte[] privateKey)
        {
            HMAC digester = new HMACSHA512(privateKey);
            StringBuilder hex = new StringBuilder();
            byte[] requestBytes = System.Text.Encoding.ASCII.GetBytes(request.ReadAsStringAsync().Result);

            return BitConverter.ToString(digester.ComputeHash(requestBytes)).Replace("-", "").ToLower();
        }

        public static Dictionary<string, string> GetConfiguration(FileInfo configurationFile)
        {
            Dictionary<string, string> properties = new Dictionary<string,string>();

            using (StreamReader reader = new StreamReader(new FileStream(configurationFile.FullName, FileMode.Open)))
            {
                string line = reader.ReadLine();

                while (null != line)
                {
                    line = line.Trim();

                    // Ignore comment lines
                    if (!line.StartsWith("#"))
                    {
                        string[] parts = line.Split(new[] { '=' });
                        if (parts.Length >= 2)
                        {
                            string name = parts[0].Trim().ToLower();

                            properties.Add(name, parts[1].Trim());
                        }
                    }

                    line = reader.ReadLine();
                }
            }

            return properties;
        }

        public abstract Task CancelOrder(OrderId orderId);

        public abstract Task CancelMarketOrders(MarketId marketId);

        public abstract Task<OrderId> CreateOrder(MarketId marketId, OrderType orderType, decimal quantity, decimal price);

        public abstract void Dispose();

        public abstract Task<AccountInfo> GetAccountInfo();

        public abstract Task<List<MyTrade>> GetMyTrades(MarketId marketId, int? limit);

        public abstract Task<List<MyTrade>> GetAllMyTrades(int? limit);

        public abstract Task<List<Market>> GetMarkets();

        public abstract Task<Book> GetMarketOrders(MarketId marketId);

        public abstract Task<List<MarketTrade>> GetMarketTrades(MarketId marketId);

        public abstract Task<Book> GetMarketDepth(MarketId marketId);

        public abstract Task<List<MyOrder>> GetMyActiveOrders(MarketId marketId, int? limit);

        public abstract string GetNextNonce();

        public abstract string Label { get; }
    }
}
