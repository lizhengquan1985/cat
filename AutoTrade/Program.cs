﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDao;
using log4net;
using System.Threading;
using Newtonsoft.Json;

namespace AutoTrade
{
    class Program
    {
        protected static ILog logger = LogManager.GetLogger(typeof(Program));

        static Dictionary<string, DateTime> nextDate = new Dictionary<string, DateTime>();

        static Dictionary<string, string> instruments = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            // 初始化币种
            instruments.Add("eth", "xrp");

            foreach (var key in instruments.Keys)
            {
                // 查询订单结果
                QueryOrderDetail(key, instruments[key]);

                // 获取行情，
                var coinInfos = OkApi.getdataAsync(instruments[key] + "-" + key);
                RunTrade(coinInfos, key, instruments[key]);

                // 每走一遍, 休眠一下
                Thread.Sleep(1000 * 1);
            }

            Console.ReadLine();
        }

        private static DateTime lastBuyDate = DateTime.Now;
        private static DateTime lastSellDate = DateTime.Now;

        static void RunTrade(List<CoinInfo> coinInfos, string quote, string symbol)
        {
            // 读取数据库 看看以前的交易
            var oldData = new BuyInfoDao().List5LowertBuy(quote, symbol);
            if (oldData.Count == 0 || coinInfos[0].close * (decimal)1.07 < oldData[0].BuyPrice)
            {
                // 购买一单
                PrepareBuy(quote, symbol, coinInfos[0].close);
                return;
            }

            // 判断是否交易。
            foreach (var item in oldData)
            {
                // 没有超过9% 不考虑出售
                if (coinInfos[0].close < oldData[0].BuyPrice * (decimal)1.09)
                {
                    continue;
                }

                // 找到最大的close
                var maxClose = coinInfos.Max(it => it.close);
                var percent = coinInfos[0].close / oldData[0].BuyPrice; // 现在的价格/购买的价格
                var huidiaoPercent = 1 + (percent - 1) / 10;
                if (coinInfos[0].close * (decimal)1.03 < maxClose || coinInfos[0].close * huidiaoPercent < maxClose)
                {
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
            if (quote == "eth")
            {
                buyAmount = (decimal)0.009;
            }
            else if (quote == "btc")
            {
                buyAmount = (decimal)0.0007;
            }
            else if (quote == "usdt")
            {
                buyAmount = (decimal)2;
            }

            var buySize = buyAmount / nowPrice;
            var buyPrice = nowPrice * (decimal)1.02;
            var client_oid = "buy" + DateTime.Now.Ticks;

            try
            {
                logger.Error($"1: 准备购买 {quote}-{symbol}, client_oid:{client_oid},  nowPrice:{nowPrice}, buyPrice:{buyPrice}, buySize:{buySize}");
                var tradeResult = OkApi.Buy(client_oid, symbol + "-" + quote, buyPrice, buySize);
                logger.Error($"2: 下单完成 {JsonConvert.SerializeObject(tradeResult)}");

                new BuyInfoDao().CreateBuyInfo(new BuyInfo
                {
                    BuyClientOid = client_oid,
                    BuyPrice = buyPrice,
                    BuyQuantity = buySize,
                    BuyCreateAt = DateTime.Now.ToString("yyyy-MM-dd"),
                    BuyFilledNotional = (decimal)0,
                    BuyStatus = "",

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
                logger.Error(e.Message, e);

                Thread.Sleep(1000 * 60 * 60);
            }
        }

        static void PrepareSell(BuyInfo buyInfo, decimal nowPrice)
        {
            if (lastSellDate > DateTime.Now.AddMinutes(-1))
            {
                // 如果1分钟内购买过一单， 则不能再次购买
                return;
            }

            var percent = 1 + (1 - nowPrice / buyInfo.BuyPrice) / 3;
            var sellSize = buyInfo.BuyQuantity / percent;
            var sellPrice = nowPrice / (decimal)1.02; // 更低的价格出售， 是为了能够出售
            var client_oid = "sell" + DateTime.Now.Ticks;
            try
            {
                logger.Error($"1: 准备出售 {buyInfo.Quote}-{buyInfo.Symbol}, client_oid:{client_oid},  nowPrice:{nowPrice}, sellPrice:{sellPrice}, sellSize:{sellSize}");
                var sellResult = OkApi.Sell(client_oid, buyInfo.Symbol + "-" + buyInfo.Quote, sellPrice, sellSize);
                logger.Error($"2: 下单完成 {JsonConvert.SerializeObject(sellResult)}");

                buyInfo.SellClientOid = client_oid;
                buyInfo.SellPrice = sellPrice;
                buyInfo.SellQuantity = sellSize;
                new BuyInfoDao().UpdateBuyInfo(buyInfo);

                logger.Error($"3: 添加记录完成");

                lastSellDate = DateTime.Now;
            }
            catch (Exception e)
            {
                logger.Error("出售 --> " + e.Message, e);

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

                    if (orderInfo.status == "filled")
                    {
                        // 如果成交了。
                        new BuyInfoDao().UpdateNotFillBuy(orderInfo);
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

                    if (orderInfo.status == "filled")
                    {
                        // 如果成交了。
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
