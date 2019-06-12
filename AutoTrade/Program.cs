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
using log4net.Repository.Hierarchy;

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
            Console.WriteLine(1);
            // 注册日志
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
            Console.WriteLine(2);

            // 初始化币种
            InstrumentsUtils.InitAllCoins();
            Console.WriteLine(3);
            instruments = InstrumentsUtils.GetAll();

            //DayAvgUtils.CalcAvgPrice();

            // AccountDetailsUtils.ShowDetail();

            //logger.Error(JsonConvert.SerializeObject(InstrumentsUtils.GetOkInstruments()));

            // 判断设置是否合理
            //foreach (var item in instruments)
            //{
            //    // 获取行情，
            //    var klineDataList = OkApi.GetKLineDataAsync(item.symbol + "-" + item.quote, "604800");
            //    if (klineDataList.Count == 0)
            //    {
            //        logger.Error($"获取行情数据有误 {item.quote},{item.symbol}");
            //        continue;
            //    }
            //    var close = klineDataList[0].close;
            //    var max = klineDataList.Max(it => it.open);
            //    var min = klineDataList.Min(it => it.low);
            //    var avg = klineDataList.Sum(it => it.open) / klineDataList.Count;
            //    if (close > item.MaxBuyPrice)
            //    {
            //        logger.Error($"现在价格大于MaxBuyPrice --> close:{close}, {item.quote}-{item.symbol} -- MaxBuyPrice: {item.MaxBuyPrice}");
            //    }
            //    if (item.MaxBuyPrice > avg)
            //    {
            //        Console.WriteLine($"大于加权平均 --> avg:{avg}, {item.quote}-{item.symbol} -- MaxBuyPrice: {item.MaxBuyPrice}");
            //    }
            //    if (item.MaxBuyPrice > max)
            //    {
            //        Console.WriteLine($"超过历史最大值 --> max:{max},min:{min}, {item.quote}-{item.symbol} -- MaxBuyPrice: {item.MaxBuyPrice}");
            //    }
            //    else if (item.MaxBuyPrice > min + (max - min) * (decimal)0.7)
            //    {
            //        Console.WriteLine($"超过0.7 --> max:{max}, {item.quote}-{item.symbol}");
            //    }
            //    Thread.Sleep(200);
            //}

            while (true)
            {
                var now = DateTime.Now;

                var btcPrice = (decimal)0;
                var ethPrice = (decimal)0;
                var okbPrice = (decimal)0;
                List<CoinInfo> allCoinInfos = new List<CoinInfo>();
                try
                {
                    allCoinInfos = new CoinInfoDao().ListCoinInfo();
                    var btcKlines = OkApi.GetKLineDataAsync("btc-usdt");
                    btcPrice = btcKlines[0].close;
                    var ethKlines = OkApi.GetKLineDataAsync("eth-usdt");
                    ethPrice = ethKlines[0].close;
                    var okbKlines = OkApi.GetKLineDataAsync("okb-usdt");
                    okbPrice = okbKlines[0].close;
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                }
                // 获取所有ticks
                var tickers = OkApi.ListTickers();

                foreach (var item in instruments)
                {
                    // 查询订单结果
                    QueryOrderDetail(item.quote, item.symbol, true);

                    // 找到当前的ticker
                    var ticker = tickers.Find(it => it.instrument_id.ToLower() == $"{item.symbol}-{item.quote}".ToLower());
                    var needContinue = true;
                    var oldData = new BuyInfoDao().List5LowerBuyForBuy(item.quote, item.symbol);
                    if (oldData.Count == 0 || oldData[0].BuyPrice > ticker.last * (decimal)1.05)
                    {
                        needContinue = false;
                    }
                    if (needContinue)
                    {
                        var needSellForMoreList = new BuyInfoDao().ListNeedSellOrder(item.quote, item.symbol);
                        if (needSellForMoreList.Count > 0 && needSellForMoreList[0].BuyPrice * (decimal)1.08 < ticker.last)
                        {
                            needContinue = false;
                        }
                    }
                    if (needContinue)
                    {
                        // 读取数据库 看看以前的交易
                        var higherSell = new SellInfoDao().List5HigherSellForEmpty(item.quote, item.symbol);
                        if (higherSell.Count > 0 && higherSell[0].SellPrice * (decimal)1.075 < ticker.last)
                        {
                            needContinue = false;
                        }
                    }
                    if (needContinue)
                    {
                        var needBuyForEmptyList = new SellInfoDao().ListNeedBuyOrder(item.quote, item.symbol);
                        if (needBuyForEmptyList.Count > 0 && needBuyForEmptyList[0].SellPrice > ticker.last * (decimal)1.075)
                        {
                            needContinue = false;
                        }
                    }

                    if (needContinue)
                    {
                        continue;
                    }

                    // 查询订单结果
                    QueryOrderDetail(item.quote, item.symbol, false);

                    // 核实订单
                    CheckOrderUtils.Check(item);

                    // 获取行情，
                    var klineDataList = OkApi.GetKLineDataAsync(item.symbol + "-" + item.quote);
                    if (klineDataList == null || klineDataList.Count < 50)
                    {
                        logger.Error($"获取行情数据有误 {item.symbol}, quote:{item.quote}");
                        continue;
                    }

                    // 记录下价格
                    var findCoinfInfo = allCoinInfos.Find(it => it.Symbol == item.symbol);
                    if (findCoinfInfo != null)
                    {
                        var nowPrice = (decimal)0; ;
                        if (item.quote.ToLower() == "btc")
                        {
                            nowPrice = klineDataList[0].close * btcPrice;
                        }
                        else if (item.quote.ToLower() == "eth")
                        {
                            nowPrice = klineDataList[0].close * ethPrice;
                        }
                        else if (item.quote.ToLower() == "okb")
                        {
                            nowPrice = klineDataList[0].close * okbPrice;
                        }
                        if (nowPrice > 0)
                        {
                            new CoinInfoDao().UpdateCoinInfo(item.symbol, nowPrice);
                        }
                    }

                    if (runCount < 3)
                    {
                        var totalAmount = klineDataList.Sum(it => it.volume * (it.open + it.close) / 2);
                        if (item.quote == "btc" && totalAmount < (decimal)0.01)
                        {
                            Console.WriteLine($"交易量太少， 不好办啊 {item.quote},{item.symbol}, totalAmount:{totalAmount}");
                        }

                        if (item.quote == "eth" && totalAmount < 1)
                        {
                            Console.WriteLine($"交易量太少， 不好办啊 {item.quote},{item.symbol}, totalAmount:{totalAmount}");
                        }
                    }

                    if (item.MaxBuyPrice <= 0)
                    {
                        Console.WriteLine($"MaxBuyPrice -->没有设置 {item.quote}-{item.symbol} --> {klineDataList.Min(it => it.low)}");
                    }
                    if (item.MaxBuyPrice > klineDataList[0].close * 4)
                    {
                        // 这里只是做粗略记录和控制
                        Console.WriteLine($"MaxBuyPrice xxxx -->设置的过大会接盘哦 {item.quote}-{item.symbol} --> {klineDataList.Min(it => it.low)}");
                    }

                    try
                    {
                        // 启动交易
                        RunTrade(klineDataList, item);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message, ex);
                    }

                    // 每走一遍, 休眠一下
                    Thread.Sleep(500);
                }

                Console.WriteLine($"-------------> 运行次数:{runCount++}, 花费时间{(DateTime.Now - now).TotalSeconds} ");
                Thread.Sleep(1000 * 5);
            }

        }

        private static DateTime lastBuyDate = DateTime.MinValue;
        private static DateTime lastSellDate = DateTime.MinValue;

        static void RunTrade(List<KLineData> coinInfos, TradeItem tradeItem)
        {
            // 做多购买
            PrepareBuyForMore(coinInfos, tradeItem);

            // 多单收割
            PrepareSellForMore(coinInfos, tradeItem);

            // 做空出售
            PrepareSellForEmpty(coinInfos, tradeItem);

            // 做空购买
            PrepareBuyForEmpty(coinInfos, tradeItem);
        }

        #region 多

        static void PrepareBuyForMore(List<KLineData> coinInfos, TradeItem tradeItem)
        {
            string quote = tradeItem.quote;
            string symbol = tradeItem.symbol;
            decimal nowPrice = coinInfos[0].close;
            // 读取数据库 看看以前的交易
            var oldData = new BuyInfoDao().List5LowerBuyForBuy(quote, symbol);

            if (tradeItem.symbol.ToLower() == "egt" && tradeItem.quote.ToLower() == "okb")
            {
                logger.Error($"egt 数量： {oldData.Count}, {new DateTime(2019, 6, 12)}");
                oldData.RemoveAll(it => DateUtils.GetDate(it.BuyCreateAt) < new DateTime(2019, 6, 12));
                logger.Error($"egt 剩余数量： {oldData.Count}");
            }
            // 判断是否阶梯
            var smallThenBuyPrice = false;
            if (oldData.Count > 0)
            {
                var rateDecimal = (decimal)1.078;
                if (tradeItem.quote.ToLower() == "okb")
                {
                    rateDecimal = (decimal)1.05;
                }

                if (tradeItem.BuyLadderRatio > (decimal)1.066)
                {
                    rateDecimal = tradeItem.BuyLadderRatio;
                }

                smallThenBuyPrice = nowPrice * rateDecimal < oldData[0].BuyPrice;
                if (oldData[0].BuyTradePrice > 0 && oldData[0].BuyTradePrice <= oldData[0].BuyPrice)
                {
                    smallThenBuyPrice = nowPrice * rateDecimal < oldData[0].BuyTradePrice;
                }
            }
            if (oldData.Count == 0 || smallThenBuyPrice)
            {
                // coinfInfos的最高价和最低价相差不能太大
                var min = coinInfos.Min(it => it.low);
                var max = coinInfos.Max(it => it.high);
                if (tradeItem.MaxBuyPrice > 0 && nowPrice > tradeItem.MaxBuyPrice)
                {
                    logger.Error($"价格过高，不能购入 quote: {quote}, symbol:{symbol}");
                }
                else
                {
                    // 是否超过了最大限价
                    if (max < 2 * min && InstrumentsUtils.CheckMaxBuyPrice(quote, symbol, coinInfos[0].close))
                    {
                        // 购买一单
                        Console.WriteLine($"PrepareBuy --> {quote}, {symbol}");
                        if (oldData.Count > 0)
                        {
                            logger.Error($"相差间隔 lastPrice: {oldData[0].Id} -- {oldData[0].BuyTradePrice}, nowPrice:{nowPrice}, rate: {(oldData[0].BuyTradePrice == 0 ? 0 : (nowPrice / oldData[0].BuyTradePrice))} --  { oldData[0].BuyTradePrice / nowPrice}");
                        }
                        DoBuyForMore(quote, symbol, nowPrice);
                        return;
                    }
                }
            }
        }

        static void DoBuyForMore(string quote, string symbol, decimal nowPrice)
        {
            if (lastBuyDate > DateTime.Now.AddMinutes(-1))
            {
                // 如果1分钟内购买过一单， 则不能再次购买
                return;
            }

            var buyAmount = (decimal)0.00;
            if (quote.ToLower() == "eth")
            {
                // 获取没有出售的数量
                var count = new BuyInfoDao().GetNotSellCount(quote, symbol);
                if (count > 60)
                {
                    count = 60;
                }

                buyAmount = (decimal)0.01;
                buyAmount = buyAmount * ((decimal)1 + count / (decimal)40);
                Console.WriteLine($"已购买数量：{symbol} -> {count}, {buyAmount}");
            }
            else if (quote.ToLower() == "btc")
            {
                var count = new BuyInfoDao().GetNotSellCount(quote, symbol);
                if (count > 60)
                {
                    count = 60;
                }
                buyAmount = (decimal)0.00028;
                buyAmount = buyAmount * ((decimal)1 + count / (decimal)40);
                Console.WriteLine($"已购买数量：{symbol} -> {count}, {buyAmount}");
            }
            else if (quote.ToLower() == "okb")
            {
                var count = new BuyInfoDao().GetNotSellCount(quote, symbol);
                if (count > 60)
                {
                    count = 60;
                }
                buyAmount = (decimal)1.8;
                buyAmount = buyAmount * ((decimal)1 + count / (decimal)20);
                Console.WriteLine($"已购买数量：{symbol} -> {count}, {buyAmount}");
            }
            else if (quote.ToLower() == "usdt")
            {
                buyAmount = (decimal)2;
            }
            else
            {
                logger.Error("000000000000000");
                return;
            }

            var buySize = buyAmount / nowPrice;
            var buyPrice = nowPrice * (decimal)1.01;
            var okInstrument = InstrumentsUtils.GetOkInstruments(quote, symbol);
            if (okInstrument == null)
            {
                logger.Error($"不存在的交易对 {quote},{symbol}");
                return;
            }

            buyPrice = decimal.Round(buyPrice, okInstrument.GetTickSizeNumber());
            buySize = decimal.Round(buySize, okInstrument.GetSizeIncrementNumber());

            if (buySize < okInstrument.min_size)
            {
                logger.Error($"购买最小额度有问题 ----> buySize: {buySize}, min_size:{okInstrument.min_size}");
                if (buySize * (decimal)1.4 > okInstrument.min_size)
                {
                    buySize = buySize * (decimal)1.4;
                }
            }

            if (quote.ToLower() == "btc" && symbol.ToLower() == "bch")
            {
                buySize = (decimal)0.0108;
            }

            var client_oid = "buy" + DateTime.Now.Ticks;

            try
            {
                logger.Error($"");
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
                logger.Error($"");
                lastBuyDate = DateTime.Now;
            }
            catch (Exception e)
            {
                logger.Error("购买异常 严重 --> " + e.Message, e);

                Thread.Sleep(1000 * 60 * 60);
            }
        }

        static void PrepareSellForMore(List<KLineData> coinInfos, TradeItem tradeItem)
        {
            string quote = tradeItem.quote;
            string symbol = tradeItem.symbol;
            // 判断是否交易。
            var needSellOldData = new BuyInfoDao().ListNeedSellOrder(quote, symbol);
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
                    DoSellForMore(item, coinInfos[0].close);
                }
            }
        }

        static void DoSellForMore(BuyInfo buyInfo, decimal nowPrice)
        {
            if (lastSellDate > DateTime.Now.AddMinutes(-1))
            {
                // 如果1分钟内购出售一单， 则不能再次出售
                return;
            }

            if (!string.IsNullOrEmpty(buyInfo.SellClientOid) && buyInfo.SellStatus != OrderStatus.cancelled)
            {
                return;
            }

            var percent = 1 + ((nowPrice / buyInfo.BuyPrice) - 1) / 3;
            var sellSize = buyInfo.BuyQuantity / percent;
            var sellPrice = nowPrice / (decimal)1.01; // 更低的价格出售， 是为了能够出售

            var okInstrument = InstrumentsUtils.GetOkInstruments(buyInfo.Quote, buyInfo.Symbol);
            if (okInstrument == null)
            {
                logger.Error($"出售时候发现 不存在的交易对 {buyInfo.Quote},{buyInfo.Symbol}");
                return;
            }

            sellPrice = decimal.Round(sellPrice, okInstrument.GetTickSizeNumber());
            sellSize = decimal.Round(sellSize, okInstrument.GetSizeIncrementNumber());

            if (buyInfo.Quote.ToLower() == "btc" && buyInfo.Symbol.ToLower() == "bch")
            {
                if (sellSize < (decimal)0.01)
                {
                    sellSize = (decimal)0.01;
                }
            }

            var client_oid = "sell" + DateTime.Now.Ticks;
            try
            {
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
                buyInfo.SellOrderId = sellResult.order_id;
                new BuyInfoDao().UpdateBuyInfoWhenSell(buyInfo);

                logger.Error($"3: 添加记录完成");
                logger.Error($"");

                lastSellDate = DateTime.Now;
            }
            catch (Exception e)
            {
                logger.Error("出售异常 严重 --> " + e.Message, e);

                Thread.Sleep(1000 * 60 * 60);
            }
        }

        #endregion

        #region 空

        private static DateTime lastBuyForEmptyDate = DateTime.MinValue;
        private static DateTime lastSellForEmptyDate = DateTime.MinValue;

        static void PrepareSellForEmpty(List<KLineData> coinInfos, TradeItem tradeItem)
        {
            if (tradeItem.SmallSellPrice <= 0 || tradeItem.EmptySize <= 0)
            {
                return;
            }

            string quote = tradeItem.quote;
            string symbol = tradeItem.symbol;
            decimal nowPrice = coinInfos[0].close;
            // 读取数据库 看看以前的交易
            var oldData = new SellInfoDao().List5HigherSellForEmpty(quote, symbol);
            // 判断是否阶梯
            var bigTheSellPrice = false;
            if (oldData.Count > 0)
            {
                var rateDecimal = (decimal)1.088;
                bigTheSellPrice = nowPrice > oldData[0].SellPrice * rateDecimal;
                if (oldData[0].SellTradePrice > 0 && oldData[0].SellTradePrice >= oldData[0].SellPrice)
                {
                    bigTheSellPrice = nowPrice > oldData[0].SellTradePrice * rateDecimal;
                }
            }
            if (oldData.Count == 0 || bigTheSellPrice)
            {
                // coinfInfos的最高价和最低价相差不能太大
                var min = coinInfos.Min(it => it.low);
                var max = coinInfos.Max(it => it.high);
                if (tradeItem.SmallSellPrice > 0 && nowPrice < tradeItem.SmallSellPrice)
                {
                    // logger.Error($"价格过低，不能售出 quote: {quote}, symbol:{symbol}");
                    return;
                }
                // 是否超过了最小售价
                if (InstrumentsUtils.CheckSmallSellPrice(quote, symbol, coinInfos[0].close))
                {
                    // 出售一单
                    Console.WriteLine($"PrepareSellForEmpty --> {quote}, {symbol}");
                    if (oldData.Count > 0)
                    {
                        logger.Error($"相差间隔 lastPrice: {oldData[0].Id} -- {oldData[0].SellTradePrice}, nowPrice:{nowPrice}, rate: {(oldData[0].SellTradePrice == 0 ? 0 : (nowPrice / oldData[0].SellTradePrice))} --  { oldData[0].SellTradePrice / nowPrice}");
                    }
                    DoSellForEmpty(tradeItem, nowPrice);
                    return;
                }
            }
        }

        static void DoSellForEmpty(TradeItem tradeItem, decimal nowPrice)
        {
            string quote = tradeItem.quote;
            string symbol = tradeItem.symbol;
            if (lastSellForEmptyDate > DateTime.Now.AddSeconds(-20))
            {
                // 如果20秒内做空过一单， 则不能再次做空
                return;
            }

            var sellSize = tradeItem.EmptySize;
            if (sellSize <= 0)
            {
                return;
            }

            var count = new SellInfoDao().GetNotBuyCount(quote, symbol);
            count = Math.Min(count, 60);

            sellSize = sellSize * (1 + count / (decimal)30);
            var sellPrice = nowPrice / (decimal)1.01;

            var okInstrument = InstrumentsUtils.GetOkInstruments(quote, symbol);
            sellPrice = decimal.Round(sellPrice, okInstrument.GetTickSizeNumber());
            sellSize = decimal.Round(sellSize, okInstrument.GetSizeIncrementNumber());

            var client_oid = "sell" + DateTime.Now.Ticks;

            try
            {
                logger.Error($"");
                logger.Error($"1: 准备出售(空) {quote}-{symbol}, client_oid:{client_oid},  nowPrice:{nowPrice}, sellPrice:{sellPrice.ToString()}, sellSize:{sellSize.ToString()}");
                var tradeResult = OkApi.Sell(client_oid, symbol + "-" + quote, sellPrice.ToString(), sellSize.ToString());
                logger.Error($"2: 下单完成 {JsonConvert.SerializeObject(tradeResult)}");

                new SellInfoDao().CreateSellInfo(new SellInfo
                {
                    SellClientOid = client_oid,
                    SellPrice = sellPrice,
                    SellQuantity = sellSize,
                    SellCreateAt = DateTime.Now.ToString("yyyy-MM-dd"),
                    SellFilledNotional = (decimal)0,
                    SellStatus = "prepare",
                    SellOrderId = tradeResult.order_id,
                    SellResult = tradeResult.result,

                    Quote = quote,
                    Symbol = symbol,
                    UserName = "qq"
                });
                logger.Error($"3: 添加记录完成");
                logger.Error($"");

                lastSellForEmptyDate = DateTime.Now;
            }
            catch (Exception e)
            {
                logger.Error("做空出售异常 严重 --> " + e.Message, e);
                Thread.Sleep(1000 * 60 * 10);
            }
        }

        static void PrepareBuyForEmpty(List<KLineData> coinInfos, TradeItem tradeItem)
        {
            string quote = tradeItem.quote;
            string symbol = tradeItem.symbol;
            // 判断是否交易。
            var needBuyOldData = new SellInfoDao().ListNeedBuyOrder(quote, symbol);
            if (needBuyOldData.Count > 0)
            {
                Console.WriteLine($"----------> {quote} {symbol} 空： {needBuyOldData.Count}");
            }
            foreach (var item in needBuyOldData)
            {
                if (item.SellStatus != "filled")
                {
                    continue;
                }
                // 这里的策略真的很重要
                // 如果超过20%, 则不需要考虑站稳, 只要有一丁点回调就可以
                // 如果超过7%, 站稳则需要等待3个小时
                if (coinInfos[0].close * (decimal)1.09 > item.SellPrice)
                {
                    continue;
                }

                // 找到最大的close
                var minClose = coinInfos.Min(it => it.close);
                var percent = item.SellPrice / coinInfos[0].close; // 现在的价格/出售的价格
                var huidiaoPercent = 1 + (percent - 1) / 10;
                if (coinInfos[0].close > minClose * (decimal)1.03 || coinInfos[0].close > minClose * huidiaoPercent)
                {
                    Console.WriteLine($"PrepareBuyForEmpty --> {item.Quote}--{item.Symbol}----");
                    // 出售， 适当的回调，可以购入
                    DoBuyForEmpty(item, coinInfos[0].close);
                }
            }
        }

        static void DoBuyForEmpty(SellInfo sellInfo, decimal nowPrice)
        {
            if (lastBuyForEmptyDate > DateTime.Now.AddSeconds(-20))
            {
                // 如果20秒内购购买一单， 则不能再次购买
                return;
            }

            if (!string.IsNullOrEmpty(sellInfo.BuyClientOid))
            {
                return;
            }

            var percent = 1 + ((sellInfo.SellPrice / nowPrice) - 1) / 3;
            var buySize = sellInfo.SellQuantity * percent;
            var buyPrice = nowPrice * (decimal)1.01; // 更高的价格购入， 是为了能够购入

            var okInstrument = InstrumentsUtils.GetOkInstruments(sellInfo.Quote, sellInfo.Symbol);
            if (okInstrument == null)
            {
                logger.Error($"出售时候发现 不存在的交易对 {sellInfo.Quote},{sellInfo.Symbol}");
                return;
            }

            buyPrice = decimal.Round(buyPrice, okInstrument.GetTickSizeNumber());
            buySize = decimal.Round(buySize, okInstrument.GetSizeIncrementNumber());

            var client_oid = "buy" + DateTime.Now.Ticks;
            try
            {
                logger.Error($"");
                logger.Error($"{JsonConvert.SerializeObject(sellInfo)}");
                logger.Error($"");
                logger.Error($"1: 准备购买(空) {sellInfo.Quote}-{sellInfo.Symbol}, client_oid:{client_oid},  nowPrice:{nowPrice.ToString()}, buyPrice:{buyPrice.ToString()}, buySize:{buySize}");
                var sellResult = OkApi.Buy(client_oid, sellInfo.Symbol + "-" + sellInfo.Quote, buyPrice.ToString(), buySize.ToString());
                logger.Error($"2: 下单完成 {JsonConvert.SerializeObject(sellResult)}");

                sellInfo.BuyClientOid = client_oid;
                sellInfo.BuyPrice = buyPrice;
                sellInfo.BuyQuantity = buySize;
                sellInfo.BuyResult = sellResult.result;
                sellInfo.BuyOrderId = sellResult.order_id;
                new SellInfoDao().UpdateSellInfoWhenBuy(sellInfo);

                logger.Error($"3: 添加记录完成");
                logger.Error($"");

                lastBuyForEmptyDate = DateTime.Now;
            }
            catch (Exception e)
            {
                logger.Error("购买异常（空） 严重 --> " + e.Message, e);

                Thread.Sleep(1000 * 60 * 10);
            }
        }

        #endregion

        #region 查询订单结果

        static void QueryOrderDetail(string quote, string symbol, bool queryNear = false)
        {
            try
            {
                // 查询购买结果
                QueryBuyDetailForMore(quote, symbol, queryNear);
            }
            catch (Exception ex)
            {
                logger.Error("查询购买结果 --> " + ex.Message, ex);
            }

            try
            {
                // 查询出售结果
                QuerySellDetailForMore(quote, symbol, queryNear);
            }
            catch (Exception ex)
            {
                logger.Error("查询出售结果 --> " + ex.Message, ex);
            }

            try
            {
                // 查询出售结果
                QueryBuyDetailForEmpty(quote, symbol, queryNear);
            }
            catch (Exception ex)
            {
                logger.Error("查询出售结果 --> " + ex.Message, ex);
            }

            try
            {
                // 查询出售结果
                QuerySellDetailForEmpty(quote, symbol, queryNear);
            }
            catch (Exception ex)
            {
                logger.Error("查询出售结果 --> " + ex.Message, ex);
            }
        }

        static void QueryBuyDetailForMore(string quote, string symbol, bool queryNear = false)
        {
            var notFillBuyList = new BuyInfoDao().ListNeedQueryBuyDetail(quote, symbol);
            if (notFillBuyList.Count == 0)
            {
                return;
            }

            foreach (var item in notFillBuyList)
            {
                try
                {
                    if (queryNear && DateUtils.GetDate(item.BuyCreateAt) < DateTime.Now.AddMinutes(-5))
                    {
                        continue;
                    }

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
                    else
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

        static void QuerySellDetailForMore(string quote, string symbol, bool queryNear = false)
        {
            var notFillSellList = new BuyInfoDao().ListNeedQuerySellDetail(quote, symbol);
            if (notFillSellList == null || notFillSellList.Count == 0)
            {
                return;
            }

            foreach (var item in notFillSellList)
            {
                try
                {
                    if (queryNear && DateUtils.GetDate(item.SellCreateAt) < DateTime.Now.AddMinutes(-5))
                    {
                        continue;
                    }

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
                    else
                    {
                        if (orderInfo.filled_notional > item.BuyFilledNotional)
                        {
                            Console.WriteLine($"{item.Symbol}-{item.Quote} 虽然没有完成，但是可以取消");
                        }
                        new BuyInfoDao().UpdateNotFillSellToCancel(orderInfo);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                }
            }
        }

        static void QueryBuyDetailForEmpty(string quote, string symbol, bool queryNear = false)
        {
            var notFillBuyList = new SellInfoDao().ListNeedQueryBuyDetail(quote, symbol);
            if (notFillBuyList.Count == 0)
            {
                return;
            }

            foreach (var item in notFillBuyList)
            {
                try
                {
                    if (queryNear && DateUtils.GetDate(item.BuyCreateAt) < DateTime.Now.AddMinutes(-5))
                    {
                        continue;
                    }

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
                        new SellInfoDao().UpdateNotFillBuy(orderInfo);
                    }
                    else
                    {
                        new SellInfoDao().UpdateNotFillBuyToCancel(orderInfo);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                }
            }
        }

        static void QuerySellDetailForEmpty(string quote, string symbol, bool queryNear = false)
        {
            var notFillSellList = new SellInfoDao().ListNeedQuerySellDetail(quote, symbol);
            if (notFillSellList == null || notFillSellList.Count == 0)
            {
                return;
            }

            foreach (var item in notFillSellList)
            {
                try
                {
                    if (queryNear && DateUtils.GetDate(item.SellCreateAt) < DateTime.Now.AddMinutes(-5))
                    {
                        continue;
                    }

                    // 查询我的购买结果
                    var orderInfo = OkApi.QueryOrderDetail(item.SellClientOid, $"{item.Symbol}-{item.Quote}".ToUpper());
                    if (orderInfo == null)
                    {
                        continue;
                    }

                    Console.WriteLine($"QuerySellDetailForEmpty --> {quote}, {symbol} --> {JsonConvert.SerializeObject(orderInfo)}");

                    // 如果成交了。
                    if (orderInfo.status == "filled")
                    {
                        new SellInfoDao().UpdateNotFillSell(orderInfo);
                    }
                    else
                    {
                        new SellInfoDao().UpdateNotFillSellToCancel(orderInfo);
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
