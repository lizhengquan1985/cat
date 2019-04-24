using SharpDapper;
using SharpDapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDao
{
    public class SellInfoDao : BaseDao
    {
        public SellInfoDao() : base()
        {
        }

        public void CreateSellInfo(SellInfo sellInfo)
        {
            Database.Insert(sellInfo);
        }

        /// <summary>
        /// 当出售的时候，需要把订单号保存下
        /// </summary>
        /// <param name="sellInfo"></param>
        public void UpdateSellInfoWhenBuy(SellInfo sellInfo)
        {
            var sql = $"update t_sell_info set BuyClientOid=@BuyClientOid, BuyPrice=@BuyPrice, BuyQuantity=@BuyQuantity, BuyOrderId=@BuyOrderId, BuyResult=@BuyResult, BuyStatus='prepare' where Id=@Id and SellClientOid=@SellClientOid";
            Database.Execute(sql, new { sellInfo.BuyClientOid, sellInfo.BuyPrice, sellInfo.BuyQuantity, sellInfo.BuyOrderId, sellInfo.BuyResult, sellInfo.Id, sellInfo.SellClientOid });
        }

        /// <summary>
        /// 列出5个最高的出售单子且未购买的记录，
        /// </summary>
        /// <param name="quote"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public List<SellInfo> List5HigherSell(string quote, string symbol)
        {
            var sql = $"select * from t_sell_info where UserName='qq' and Quote=@Quote and Symbol=@Symbol and (BuyStatus!='filled' or BuyStatus is null) order by SellPrice desc limit 8";
            return Database.Query<SellInfo>(sql, new { Quote = quote, Symbol = symbol }).ToList();
        }

        /// <summary>
        /// 为了下一次投注的加额
        /// </summary>
        /// <param name="quote"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public int GetNotBuyCount(string quote, string symbol)
        {
            try
            {
                var sql = $"select count(1) from t_sell_info where UserName='qq' and Quote=@Quote and Symbol=@Symbol and (BuyStatus!='filled' or BuyStatus is null) and SellStatus='filled'";
                return Database.ExecuteScalar<int>(sql, new { Quote = quote, Symbol = symbol });
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                return 0;
            }
        }

        public List<SellInfo> ListNeedBuyOrder(string quote, string symbol, int count = 5)
        {
            var sql = $"select * from t_sell_info where UserName='qq' and Quote=@Quote and Symbol=@Symbol and SellStatus='filled' and (BuyStatus is null or BuyStatus='{OrderStatus.cancelled}') order by SellPrice desc limit {count}";
            return Database.Query<SellInfo>(sql, new { Quote = quote, Symbol = symbol }).ToList();
        }

        #region 订单结果查询以及匹配

        public List<SellInfo> ListNeedQuerySellDetail(string quote, string symbol)
        {
            var sql = $"select * from t_sell_info where UserName='qq' and Quote=@Quote and Symbol=@Symbol and ((SellStatus!='{OrderStatus.filled}' and SellStatus!='{OrderStatus.cancelled}') or SellTradePrice is null)";
            return Database.Query<SellInfo>(sql, new { Quote = quote, Symbol = symbol }).ToList();
        }

        public void UpdateNotFillSell(OrderInfo orderInfo)
        {
            var sql = $"update t_sell_info set SellQuantity=@SellQuantity, SellCreateAt=@SellCreateAt, SellTradePrice=@SellTradePrice, " +
                $" SellFilledNotional=@SellFilledNotional, SellStatus=@SellStatus where SellClientOid=@SellClientOid";
            Database.Execute(sql, new
            {
                SellTradePrice = orderInfo.price,
                SellQuantity = orderInfo.size,
                SellCreateAt = orderInfo.created_at,
                SellFilledNotional = orderInfo.filled_notional,
                SellStatus = orderInfo.status,
                SellClientOid = orderInfo.client_oid
            });
        }

        public void UpdateNotFillSellToCancel(OrderInfo orderInfo)
        {
            var sql = $"update t_sell_info set SellStatus=@SellStatus where SellClientOid=@SellClientOid";
            Database.Execute(sql, new
            {
                SellStatus = orderInfo.status,
                SellClientOid = orderInfo.client_oid
            });
        }

        public List<SellInfo> ListNeedQueryBuyDetail(string quote, string symbol)
        {
            var sql = $"select * from t_sell_info where UserName='qq' and Quote=@Quote and Symbol=@Symbol and (BuyStatus='{OrderStatus.open}' or BuyStatus='prepare' or (BuyTradePrice is null and BuyStatus='filled'))";
            return Database.Query<SellInfo>(sql, new { Quote = quote, Symbol = symbol }).ToList();
        }

        public void UpdateNotFillBuy(OrderInfo orderInfo)
        {
            var sql = $"update t_sell_info set BuyQuantity=@BuyQuantity, BuyCreateAt=@BuyCreateAt, BuyOrderId=@BuyOrderId, BuyTradePrice=@BuyTradePrice, " +
                $" BuyFilledNotional=@BuyFilledNotional, BuyStatus=@BuyStatus where BuyClientOid=@BuyClientOid";
            Database.Execute(sql, new
            {
                BuyTradePrice = orderInfo.price,
                BuyOrderId = orderInfo.order_id,
                BuyQuantity = orderInfo.size,
                BuyCreateAt = orderInfo.created_at,
                BuyFilledNotional = orderInfo.filled_notional,
                BuyStatus = orderInfo.status,
                BuyClientOid = orderInfo.client_oid
            });
        }

        public void UpdateNotFillBuyToCancel(OrderInfo orderInfo)
        {
            var sql = $"update t_sell_info set BuyStatus=@BuyStatus where BuyClientOid=@BuyClientOid";
            Database.Execute(sql, new
            {
                BuyStatus = orderInfo.status,
                BuyClientOid = orderInfo.client_oid
            });
        }

        #endregion

        public SellInfo GetSellOrder(string sellClientOid)
        {
            var sql = $"select * from t_sell_info where SellClientOid=@SellClientOid";
            return Database.Query<SellInfo>(sql, new { SellClientOid = sellClientOid }).FirstOrDefault();
        }

        public SellInfo GetBuyOrder(string buyClientOid)
        {
            var sql = $"select * from t_sell_info where BuyClientOid=@BuyClientOid";
            return Database.Query<SellInfo>(sql, new { BuyClientOid = buyClientOid }).FirstOrDefault();
        }
    }
}
