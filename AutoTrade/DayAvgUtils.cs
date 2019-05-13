using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrade
{
    public class DayAvgUtils
    {
        protected static ILog logger = LogManager.GetLogger(typeof(DayAvgUtils));

        public static void CalcAvgPrice()
        {
            var instruments = InstrumentsUtils.GetAll();
            var allAvgPrice = new Dictionary<string, decimal>();
            foreach (var item in instruments)
            {
                var klineDataList = OkApi.GetKLineDataAsync(item.symbol + "-" + item.quote, (60 * 60 * 24).ToString());
                if (klineDataList == null)
                {
                    continue;
                }
                var count = 0;
                var totalPrice = (decimal)0;
                foreach (var data in klineDataList)
                {
                    var dataItemPrice = (data.open + data.close) / 2;

                    totalPrice += dataItemPrice;
                    count++;
                }

                // 
                var avgPrice = totalPrice / count;
                avgPrice = decimal.Round(avgPrice, 8);

                allAvgPrice.Add(item.quote + "-" + item.symbol, avgPrice);

                if (item.MaxBuyPrice > avgPrice)
                {
                    logger.Error($"投入价格大于加权平均 {item.quote}-{item.symbol}, MaxBuyPrice:{item.MaxBuyPrice}, avgPrice: {avgPrice}");
                }
            }

            var fs = new FileStream("", FileMode.OpenOrCreate);
            var sw = new StreamWriter(fs);
            sw.Write(JsonConvert.SerializeObject(allAvgPrice));
        }
    }
}
