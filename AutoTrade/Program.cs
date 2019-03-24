using System;
using System.Collections.Generic;
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

            }
            // 标记下一次读取时间
        }

        static void PrepareBuy()
        {
            if (dt > DateTime.Now.AddMinutes(-2))
            {
                // 如果2分钟内购买过一单， 则不能再次购买
                return;
            }
        }
    }
}
