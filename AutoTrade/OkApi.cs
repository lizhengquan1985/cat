using DataDao;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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


        public static T PostSign<T>(string url, object param, string pathAndQuery)
        {
            var client = new RestClient(url);
            RestRequest req = new RestRequest(Method.POST);
            req.AddHeader("content-type", "application/json");
            req.AddJsonBody(param);

            var _bodyStr = JsonConvert.SerializeObject(param);

            var method = "POST";
            var now = DateTime.Now;
            var timeStamp = TimeZoneInfo.ConvertTimeToUtc(now).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var requestUrl = pathAndQuery; 
            string sign = "";
            if (!String.IsNullOrEmpty(_bodyStr))
            {
                sign = Encryptor.HmacSHA256($"{timeStamp}{method}{requestUrl}{_bodyStr}", Config.secretKey);
            }
            else
            {
                sign = Encryptor.HmacSHA256($"{timeStamp}{method}{requestUrl}", Config.secretKey);
            }

            req.AddHeader("OK-ACCESS-KEY", Config.apiKey);
            req.AddHeader("OK-ACCESS-SIGN", sign);
            req.AddHeader("OK-ACCESS-TIMESTAMP", timeStamp.ToString());
            req.AddHeader("OK-ACCESS-PASSPHRASE", Config._passPhrase);



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
            var url = $"{root}api/spot/v3/orders";
            var obj = new
            {
                client_oid = client_oid,
                type = "limit",
                side = "buy",
                instrument_id = instrument_id,
                order_type = "0",
                margin_trading = "1",
                price,
                size
            };
            var res = PostSign<TradeResult>(url, obj, "/api/spot/v3/orders");
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

    static class Encryptor
    {
        public static string HmacSHA256(string infoStr, string secret)
        {
            byte[] sha256Data = Encoding.UTF8.GetBytes(infoStr);
            byte[] secretData = Encoding.UTF8.GetBytes(secret);
            using (var hmacsha256 = new HMACSHA256(secretData))
            {
                byte[] buffer = hmacsha256.ComputeHash(sha256Data);
                return Convert.ToBase64String(buffer);
            }
        }

        public static string MakeSign(string apiKey, string secret, string phrase)
        {
            var timeStamp = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            var sign = Encryptor.HmacSHA256($"{timeStamp}GET/users/self/verify", secret);
            var info = new
            {
                op = "login",
                args = new List<string>()
                {
                    apiKey,phrase,timeStamp.ToString(),sign
                }
            };
            return JsonConvert.SerializeObject(info);
        }
    }
}
