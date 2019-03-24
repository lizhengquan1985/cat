using DataDao;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrade
{
    public class OkApi
    {
        static string root = "https://www.okex.com/";

        #region rest调用

        public static T Post<T>(string url, object param)
        {
            var client = new RestClient(url);
            RestRequest req = new RestRequest(Method.POST);
            req.AddHeader("content-type", "application/json");
            req.AddHeader("cache-type", "no-cache");
            req.AddJsonBody(param);
            var response = client.Execute(req);
            var result = JsonConvert.DeserializeObject<T>(response.Content);
            return result;
        }

        public static T Get<T>(string url)
        {
            var client = new RestClient(url);
            RestRequest req = new RestRequest(Method.GET);
            //req.AddHeader("content-type", "application/json");
            //req.AddHeader("cache-type", "no-cache");
            var response = client.Execute(req);
            var result = JsonConvert.DeserializeObject<T>(response.Content);
            return result;
        }

        #endregion


        public static List<CoinInfo> getdataAsync(string instrument)
        {
            var url = $"{root}api/spot/v3/instruments/{instrument}/candles";

            try
            {
                var res = Get<List<List<string>>>(url);
                var coinInfos = new List<CoinInfo>();
                foreach (var item in res)
                {
                    CoinInfo coinInfo = new CoinInfo()
                    {
                        time = DateTime.Parse(item[0]),
                        open = decimal.Parse(item[1]),
                        high = decimal.Parse(item[2]),
                        low = decimal.Parse(item[3]),
                        close = decimal.Parse(item[4]),
                        volume = decimal.Parse(item[5])
                    };
                    coinInfos.Add(coinInfo);
                }

                return coinInfos;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static TradeResult Buy(string client_oid, string instrument_id, decimal price, decimal size)
        {
            var url = $"{root}/api/spot/v3/orders";
            var obj = new
            {
                client_oid = client_oid,
                type = "limit",
                side = "buy",
                instrument_id = "",
                order_type = "0",
                margin_trading = "1",
                price,
                size
            };
            var res = Post<TradeResult>(url, obj);
            return res;
        }

        public static TradeResult Sell(string client_oid, string instrument_id, decimal price, decimal size)
        {
            var url = $"{root}/api/spot/v3/orders";
            var obj = new
            {
                client_oid = client_oid,
                type = "limit",
                side = "sell",
                instrument_id = "",
                order_type = "0",
                margin_trading = "1",
                price,
                size
            };
            var res = Post<TradeResult>(url, obj);
            return res;
        }
    }

    public class CoinInfo
    {
        public DateTime time { get; set; }
        public decimal open { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal close { get; set; }
        public decimal volume { get; set; }
    }

    public class TradeResult
    {
        public string order_id { get; set; }
        public string client_oid { get; set; }
        public bool result { get; set; }
    }
}
