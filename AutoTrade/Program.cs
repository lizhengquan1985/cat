using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDao;
using log4net;
using System.Threading;
using Newtonsoft.Json;
using log4net.Config;
using System.IO;

namespace AutoTrade
{


    class Program
    {
        protected static ILog logger = LogManager.GetLogger(typeof(Program));

        static Dictionary<string, DateTime> nextDate = new Dictionary<string, DateTime>();

        static List<TradeItem> instruments = new List<TradeItem>();

        static int runCount = 1;
        static DateTime beginRunDate = DateTime.Now;

        static void Main(string[] args)
        {
            // 注册日志
            XmlConfigurator.Configure(new FileInfo("log4net.config"));

            // 初始化币种
            InstrumentsUtils.InitAllCoins();
            instruments = InstrumentsUtils.GetAll();
            //logger.Error(JsonConvert.SerializeObject(InstrumentsUtils.GetOkInstruments()));

            while (true)
            {
                Console.WriteLine($"-------------> 运行次数:{runCount++} ");
                foreach (var item in instruments)
                {
                    // 查询订单结果
                    QueryOrderDetail(item.quote, item.symbol);

                    // 获取行情，
                    var klineDataList = OkApi.GetKLineDataAsync(item.symbol + "-" + item.quote);
                    if (klineDataList == null || klineDataList.Count < 50)
                    {
                        logger.Error($"获取行情数据有误 {item.symbol}, quote:{item.quote}");
                        continue;
                    }

                    if (runCount < 3)
                    {
                        var totalAmount = klineDataList.Sum(it => it.volume * (it.open + it.close) / 2);
                        if (item.quote == "btc" && totalAmount < (decimal)0.2)
                        {
                            Console.WriteLine($"交易量太少， 不好办啊 {item.quote},{item.symbol}, totalAmount:{totalAmount}");
                        }

                        if (item.quote == "eth" && totalAmount < 5)
                        {
                            Console.WriteLine($"交易量太少， 不好办啊 {item.quote},{item.symbol}, totalAmount:{totalAmount}");
                        }
                    }

                    if (item.MaxBuyPrice <= 0)
                    {
                        Console.WriteLine($"MaxBuyPrice -->没有设置 {item.quote}-{item.symbol} --> {klineDataList.Min(it => it.low)}");
                    }
                    if (item.MaxBuyPrice < klineDataList[0].close * (decimal)1.5)
                    {
                        // 这里只是做粗略记录和控制
                        Console.WriteLine($"MaxBuyPrice -->设置的有点小 {item.quote}-{item.symbol} --> {klineDataList.Min(it => it.low)}");
                    }
                    if (item.MaxBuyPrice > klineDataList[0].close * 3)
                    {
                        // 这里只是做粗略记录和控制
                        Console.WriteLine($"MaxBuyPrice xxxx -->设置的过大会接盘哦 {item.quote}-{item.symbol} --> {klineDataList.Min(it => it.low)}");
                    }

                    // 启动交易
                    RunTrade(klineDataList, item);

                    // 每走一遍, 休眠一下
                    Thread.Sleep(500);
                }
            }

            Console.ReadLine();
        }

        private static DateTime lastBuyDate = DateTime.MinValue;
        private static DateTime lastSellDate = DateTime.MinValue;

        static void RunTrade(List<KLineData> coinInfos, TradeItem tradeItem)
        {
            string quote = tradeItem.quote;
            string symbol = tradeItem.symbol;
            decimal nowPrice = coinInfos[0].close;
            // 读取数据库 看看以前的交易
            var oldData = new BuyInfoDao().List5LowertBuy(quote, symbol);
            if (oldData.Count == 0 || nowPrice * (decimal)1.07 < oldData[0].BuyPrice)
            {
                // coinfInfos的最高价和最低价相差不能太大
                var min = coinInfos.Min(it => it.low);
                var max = coinInfos.Max(it => it.high);
                if (tradeItem.MaxBuyPrice > 0 && nowPrice > tradeItem.MaxBuyPrice)
                {
                    logger.Error($"价格过高，不能购入 quote: {quote}, symbol:{symbol}");
                    return;
                }
                // 是否超过了最大限价
                if (max < 2 * min && InstrumentsUtils.CheckMaxBuyPrice(quote, symbol, coinInfos[0].close))
                {
                    // 购买一单
                    Console.WriteLine($"PrepareBuy --> {quote}, {symbol}");
                    PrepareBuy(quote, symbol, nowPrice);
                    return;
                }
            }

            // 判断是否交易。
            var needSellOldData = new BuyInfoDao().List5NeedSell(quote, symbol);
            foreach (var item in needSellOldData)
            {
                if (item.BuyStatus != "filled")
                {
                    continue;
                }
                // 这里的策略真的很重要
                // 如果超过20%, 则不需要考虑站稳, 只要有一丁点回调就可以
                // 如果超过7%, 站稳则需要等待3个小时
                if (coinInfos[0].close < item.BuyPrice * (decimal)1.09)
                {
                    continue;
                }

                // 找到最大的close
                var maxClose = coinInfos.Max(it => it.close);
                var percent = coinInfos[0].close / item.BuyPrice; // 现在的价格/购买的价格
                var huidiaoPercent = 1 + (percent - 1) / 10;
                if (coinInfos[0].close * (decimal)1.03 < maxClose || coinInfos[0].close * huidiaoPercent < maxClose)
                {
                    Console.WriteLine($"PrepareSell --> {item.Quote}--{item.Symbol}----");
                    // 出售， 适当的回调，可以出售
                    PrepareSell(item, coinInfos[0].close);
                }

                // 如果上涨了过多，则也可以考虑适当的出售 TODO
            }
        }

        static void PrepareBuy(string quote, string symbol, decimal nowPrice)
        {
            if (lastBuyDate > DateTime.Now.AddMinutes(-1))
            {
                // 如果1分钟内购买过一单， 则不能再次购买
                return;
            }

            var buyAmount = (decimal)0.07;
            if (quote.ToLower() == "eth")
            {
                // 获取没有出售的数量
                var count = new BuyInfoDao().GetNotSellCount(quote, symbol);
                if(count > 50)
                {
                    count = 50;
                }

                buyAmount = (decimal)0.012;
                buyAmount = buyAmount * (1 + count / 50);
            }
            else if (quote.ToLower() == "btc")
            {
                var count = new BuyInfoDao().GetNotSellCount(quote, symbol);
                if (count > 50)
                {
                    count = 50;
                }
                buyAmount = (decimal)0.0005;
                buyAmount = buyAmount * (1 + count / 50);
            }
            else if (quote == "usdt")
            {
                buyAmount = (decimal)2;
            }
            else
            {
                logger.Error("000000000000000");
                return;
            }

            var buySize = buyAmount / nowPrice;
            var buyPrice = nowPrice * (decimal)1.02;
            var okInstrument = InstrumentsUtils.GetOkInstruments(quote, symbol);
            if (okInstrument == null)
            {
                logger.Error($"不存在的交易对 {quote},{symbol}");
                return;
            }

            buyPrice = decimal.Round(buyPrice, okInstrument.GetTickSizeNumber());
            buySize = decimal.Round(buySize, okInstrument.GetSizeIncrementNumber());

            var client_oid = "buy" + DateTime.Now.Ticks;

            try
            {
                logger.Error($"1: 准备购买 {quote}-{symbol}, client_oid:{client_oid},  nowPrice:{nowPrice}, buyPrice:{buyPrice.ToString()}, buySize:{buySize.ToString()}");
                var tradeResult = OkApi.Buy(client_oid, symbol + "-" + quote, buyPrice.ToString(), buySize.ToString());
                logger.Error($"2: 下单完成 {JsonConvert.SerializeObject(tradeResult)}");

                new BuyInfoDao().CreateBuyInfo(new BuyInfo
                {
                    BuyClientOid = client_oid,
                    BuyPrice = buyPrice,
                    BuyQuantity = buySize,
                    BuyCreateAt = DateTime.Now.ToString("yyyy-MM-dd"),
                    BuyFilledNotional = (decimal)0,
                    BuyStatus = "prepare",

                    Quote = quote,
                    Symbol = symbol,
                    UserName = "qq",

                    BuyOrderId = tradeResult.order_id,
                    BuyResult = tradeResult.result
                });
                logger.Error($"3: 添加记录完成");
                lastBuyDate = DateTime.Now;
            }
            catch (Exception e)
            {
                logger.Error("购买异常 严重 --> " + e.Message, e);

                Thread.Sleep(1000 * 60 * 60);
            }
        }

        static void PrepareSell(BuyInfo buyInfo, decimal nowPrice)
        {
            if (lastSellDate > DateTime.Now.AddMinutes(-1))
            {
                // 如果1分钟内购出售一单， 则不能再次出售
                return;
            }

            if (!string.IsNullOrEmpty(buyInfo.SellClientOid))
            {
                return;
            }

            var percent = 1 + ((nowPrice / buyInfo.BuyPrice) - 1) / 3;
            var sellSize = buyInfo.BuyQuantity / percent;
            var sellPrice = nowPrice / (decimal)1.02; // 更低的价格出售， 是为了能够出售

            var okInstrument = InstrumentsUtils.GetOkInstruments(buyInfo.Quote, buyInfo.Symbol);
            if (okInstrument == null)
            {
                logger.Error($"出售时候发现 不存在的交易对 {buyInfo.Quote},{buyInfo.Symbol}");
                return;
            }

            sellPrice = decimal.Round(sellPrice, okInstrument.GetTickSizeNumber());
            sellSize = decimal.Round(sellSize, okInstrument.GetSizeIncrementNumber());

            var client_oid = "sell" + DateTime.Now.Ticks;
            try
            {
                logger.Error($"");
                logger.Error($"");
                logger.Error($"{JsonConvert.SerializeObject(buyInfo)}");
                logger.Error($"");
                logger.Error($"1: 准备出售 {buyInfo.Quote}-{buyInfo.Symbol}, client_oid:{client_oid},  nowPrice:{nowPrice.ToString()}, sellPrice:{sellPrice.ToString()}, sellSize:{sellSize}");
                var sellResult = OkApi.Sell(client_oid, buyInfo.Symbol + "-" + buyInfo.Quote, sellPrice.ToString(), sellSize.ToString());
                logger.Error($"2: 下单完成 {JsonConvert.SerializeObject(sellResult)}");

                buyInfo.SellClientOid = client_oid;
                buyInfo.SellPrice = sellPrice;
                buyInfo.SellQuantity = sellSize;
                buyInfo.SellResult = sellResult.result;
                new BuyInfoDao().UpdateBuyInfo(buyInfo);

                logger.Error($"3: 添加记录完成");

                lastSellDate = DateTime.Now;
            }
            catch (Exception e)
            {
                logger.Error("出售异常 严重 --> " + e.Message, e);

                Thread.Sleep(1000 * 60 * 60);
            }
        }

        #region 查询订单结果

        static void QueryOrderDetail(string quote, string symbol)
        {
            try
            {
                // 查询购买结果
                QueryBuyDetail(quote, symbol);
            }
            catch (Exception ex)
            {
                logger.Error("查询购买结果 --> " + ex.Message, ex);
            }
            try
            {
                // 查询出售结果
                QuerySellDetail(quote, symbol);
            }
            catch (Exception ex)
            {
                logger.Error("查询出售结果 --> " + ex.Message, ex);
            }
        }

        static void QueryBuyDetail(string quote, string symbol)
        {
            var notFillBuyList = new BuyInfoDao().ListNotFillBuy(quote, symbol);
            if (notFillBuyList.Count == 0)
            {
                return;
            }

            foreach (var item in notFillBuyList)
            {
                try
                {
                    // 查询我的购买结果
                    var orderInfo = OkApi.QueryOrderDetail(item.BuyClientOid, $"{item.Symbol}-{item.Quote}".ToUpper());
                    if (orderInfo == null)
                    {
                        continue;
                    }

                    Console.WriteLine($"QueryBuyDetail--> {quote}, {symbol} --> {JsonConvert.SerializeObject(orderInfo)}");

                    // 如果成交了。
                    if (orderInfo.status == "filled")
                    {
                        new BuyInfoDao().UpdateNotFillBuy(orderInfo);
                    }
                    if (orderInfo.status == "cancelled")
                    {
                        new BuyInfoDao().UpdateNotFillBuyToCancel(orderInfo);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                }
            }
        }

        static void QuerySellDetail(string quote, string symbol)
        {
            var notFillSellList = new BuyInfoDao().ListNotFillSell(quote, symbol);
            if (notFillSellList == null || notFillSellList.Count == 0)
            {
                return;
            }

            foreach (var item in notFillSellList)
            {
                try
                {
                    // 查询我的购买结果
                    var orderInfo = OkApi.QueryOrderDetail(item.SellClientOid, $"{item.Symbol}-{item.Quote}".ToUpper());
                    if (orderInfo == null)
                    {
                        continue;
                    }

                    Console.WriteLine($"QuerySellDetail --> {quote}, {symbol} --> {JsonConvert.SerializeObject(orderInfo)}");

                    // 如果成交了。
                    if (orderInfo.status == "filled")
                    {
                        new BuyInfoDao().UpdateNotFillSell(orderInfo);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                }
            }
        }

        #endregion
    }
}
