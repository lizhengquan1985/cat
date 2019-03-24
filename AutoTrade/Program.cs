using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDao;

namespace AutoTrade
{
    class Program
    {
        static Dictionary<string, DateTime> nextDate = new Dictionary<string, DateTime>();

        static Dictionary<string, string> instruments = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            // 初始化币种
            instruments.Add("eth", "xrp");

            foreach (var key in instruments.Keys)
            {
                // 获取行情，
                var coinInfos = OkApi.getdataAsync(instruments[key] + "-" + key);
            }


            Console.ReadLine();
        }

        private static DateTime dt = DateTime.Now;

        static void RunTrade(List<CoinInfo> coinInfos, string quote, string symbol)
        {
            // 读取数据库 看看以前的交易
            var oldData = new BuyInfoDao().List5LowertBuy(quote, symbol);
            if (oldData.Count == 0 )
            {
                // 购买一单
                PrepareBuy();
                return;
            }

            if (coinInfos[0].close * (decimal)1.07 < oldData[0].BuyPrice)
            {
                // 购买一单
                return;
            }

            // 判断是否交易。
            foreach (var item in oldData)
            {
                // 没有超过8%
                if (coinInfos[0].close < oldData[0].BuyPrice * (decimal)1.09)
                {
                    continue;
                }

                // 找到最大的close
                var maxClose = coinInfos.Max(it => it.close);
                var percent = coinInfos[0].close / oldData[0].BuyPrice;
                if (coinInfos[0].close * (decimal)1.03 < maxClose || coinInfos[0].close * (decimal)percent < maxClose)
                {
                    // 出售， 适当的回调，可以出售
                    PrepareSell();
                }

                // 如果上涨了过多，则也可以考虑适当的出售

            }
            // 标记下一次读取时间
        }

        static void PrepareBuy(string quote, string symbol, decimal nowPrice)
        {
            if (dt > DateTime.Now.AddMinutes(-1))
            {
                // 如果1分钟内购买过一单， 则不能再次购买
                return;
            }

            var buyAmount = (decimal) 0.07;
            if (quote == "eth")
            {
                buyAmount = (decimal)0.009;
            }
            else if (quote == "btc")
            {
                buyAmount = (decimal)0.0007;
            } else  if (quote == "usdt")
            {
                buyAmount = (decimal)2;
            }

            var buySize = buyAmount / nowPrice;
            var buyPrice = nowPrice * (decimal) 1.02;
            var client_oid = "buy" + DateTime.Now.Ticks;
            OkApi.Buy(client_oid, symbol + "-" + quote, buyPrice, buySize);
        }

        static void PrepareSell(BuyInfo buyInfo, decimal nowPrice)
        {
            var percent = 1 + (1 - nowPrice / buyInfo.BuyPrice) / 3;
            var sellSize = buyInfo.BuyQuantity / percent;
            var sellPrice = nowPrice / (decimal)1.02; // 更低的价格出售， 是为了能够出售
            var client_oid = "sell" + DateTime.Now.Ticks;
            OkApi.Sell(client_oid, buyInfo.Symbol + "-" + buyInfo.Quote, sellPrice, sellSize);
            // 出售完了以后要标记一下。
        }
    }
}
