using DataDao;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrade
{
    public class CheckOrderUtils
    {
        protected static ILog logger = LogManager.GetLogger(typeof(CheckOrderUtils));

        private static DateTime lastCheckDate = DateTime.Now;

        private static Dictionary<string, DateTime> lastCheckDateDic = new Dictionary<string, DateTime>();

        public static void Check(TradeItem tradeItem)
        {
            try
            {
                DoCheck(tradeItem);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }

        public static void DoCheck(TradeItem tradeItem)
        {
            var instrument = $"{tradeItem.symbol}-{tradeItem.quote}".ToUpper();

            // 核实下 最近 10个小时是否统计过，
            if (lastCheckDate > DateTime.Now.AddMinutes(-1))
            {
                // 最近1分钟有核实过， 则不再核实
                return;
            }
            if (lastCheckDateDic.ContainsKey(instrument) && lastCheckDateDic[instrument] > DateTime.Now.AddHours(-12))
            {
                // 这个交易对， 12个小时内核实过，则不再核实
                return;
            }
            else
            {
                if (lastCheckDateDic.ContainsKey(instrument))
                {
                    lastCheckDateDic[instrument] = DateTime.Now;
                }
                else
                {
                    lastCheckDateDic.Add(instrument, DateTime.Now);
                }
            }

            lastCheckDate = DateTime.Now;


            var orderList = OkApi.ListFilledOrder(instrument);
            Console.WriteLine($"需要核实的订单数据：{orderList.Count}");
            foreach (var order in orderList)
            {
                if (order.side == "buy")
                {
                    // 列出数据库里面的交易记录， 对比一下。
                    var orderInDb = new BuyInfoDao().GetBuyOrder(order.client_oid);
                    var orderInDb2 = new SellInfoDao().GetBuyOrder(order.client_oid);
                    if ((
                            orderInDb == null
                            || orderInDb.BuyClientOid != order.client_oid
                            || orderInDb.BuyCreateAt != order.created_at
                            || orderInDb.BuyStatus != order.status
                            || orderInDb.BuyOrderId != order.order_id
                        )
                        &&
                        (
                            orderInDb2 == null
                            || orderInDb2.BuyClientOid != order.client_oid
                            || orderInDb2.BuyCreateAt != order.created_at
                            || orderInDb2.BuyStatus != order.status
                            || orderInDb2.BuyOrderId != order.order_id
                        ))
                    {
                        logger.Error($"有个订单不合理，快速查看一下 begin");
                        logger.Error($"{JsonConvert.SerializeObject(order)}");
                        logger.Error($"{JsonConvert.SerializeObject(orderInDb)}");
                        logger.Error($"{JsonConvert.SerializeObject(orderInDb2)}");
                        logger.Error($"有个订单不合理，快速查看一下 end ");
                    }
                }
                else if (order.side == "sell")
                {
                    // 列出数据库里面的交易记录， 对比一下。
                    var orderInDb = new BuyInfoDao().GetSellOrder(order.client_oid);
                    var orderInDb2 = new SellInfoDao().GetSellOrder(order.client_oid);
                    if ((
                            orderInDb == null
                            || orderInDb.SellClientOid != order.client_oid
                            || orderInDb.SellCreateAt != order.created_at
                            || orderInDb.SellStatus != order.status
                            || orderInDb.SellOrderId != order.order_id
                        )
                        &&
                        (
                            orderInDb2 == null
                            || orderInDb2.SellClientOid != order.client_oid
                            || orderInDb2.SellCreateAt != order.created_at
                            || orderInDb2.SellStatus != order.status
                            || orderInDb2.SellOrderId != order.order_id
                        ))
                    {
                        logger.Error($"有个订单不合理，快速查看一下 begin");
                        logger.Error($"{JsonConvert.SerializeObject(order)}");
                        logger.Error($"{JsonConvert.SerializeObject(orderInDb)}");
                        logger.Error($"{JsonConvert.SerializeObject(orderInDb2)}");
                        logger.Error($"有个订单不合理，快速查看一下 end ");
                    }
                }
                else
                {
                    logger.Error($"数据统计不正确, 不存在的side");
                    Console.WriteLine($"{JsonConvert.SerializeObject(order)}");
                }
            }


        }
    }
}
