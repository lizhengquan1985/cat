using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDapper;
using SharpDapper.Extensions;
using Newtonsoft.Json;

namespace DataDao
{
    public class BuyInfoDao : BaseDao
    {
        public BuyInfoDao() : base()
        {
        }

        public void CreateBuyInfo(BuyInfo buyInfo)
        {
            Database.Insert(buyInfo);
        }

        /// <summary>
        /// 出售之后，修改下数据。
        /// </summary>
        /// <param name="buyInfo"></param>
        public void UpdateBuyInfoWhenSell(BuyInfo buyInfo)
        {
            var sql = $"update t_buy_info set SellClientOid=@SellClientOid, SellPrice=@SellPrice, SellQuantity=@SellQuantity, SellOrderId=@SellOrderId, SellResult=@SellResult, SellStatus='{OrderStatus.prepare}' where Id=@Id and BuyClientOid=@BuyClientOid";
            Database.Execute(sql, new { buyInfo.SellClientOid, buyInfo.SellPrice, buyInfo.SellQuantity, buyInfo.SellOrderId, buyInfo.SellResult, buyInfo.Id, buyInfo.BuyClientOid });
        }

        /// <summary>
        /// 列出5个最低的购买且未出售的记录，
        /// </summary>
        /// <param name="quote"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public List<BuyInfo> List5LowerBuyForBuy(string quote, string symbol)
        {
            var sql = $"select * from t_buy_info where UserName='qq' and Quote=@Quote and Symbol=@Symbol and (SellStatus='{OrderStatus.cancelled}' or SellStatus='{OrderStatus.open}' or SellStatus='{OrderStatus.prepare}' or SellStatus is null) and BuyStatus!='{OrderStatus.cancelled}' order by BuyPrice asc limit 8";
            return Database.Query<BuyInfo>(sql, new { Quote = quote, Symbol = symbol }).ToList();
        }

        public int GetNotSellCount(string quote, string symbol)
        {
            try
            {
                var sql = $"select count(1) from t_buy_info where UserName='qq' and Quote=@Quote and Symbol=@Symbol and (SellStatus!='filled' or SellStatus is null) and BuyStatus='filled'";
                return Database.ExecuteScalar<int>(sql, new { Quote = quote, Symbol = symbol });
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                return 0;
            }
        }

        public List<BuyInfo> ListNeedSellOrder(string quote, string symbol, int count = 5)
        {
            var sql = $"select * from t_buy_info where UserName='qq' and Quote=@Quote and Symbol=@Symbol and BuyStatus='filled' and (SellStatus is null or SellStatus='{OrderStatus.cancelled}') order by BuyPrice asc limit {count}";
            return Database.Query<BuyInfo>(sql, new { Quote = quote, Symbol = symbol }).ToList();
        }

        #region 订单结果查询以及匹配

        public List<BuyInfo> ListNeedQueryBuyDetail(string quote, string symbol)
        {
            var sql = $"select * from t_buy_info where UserName='qq' and Quote=@Quote and Symbol=@Symbol and (BuyStatus!=@BuyStatus or BuyTradePrice is null)";
            return Database.Query<BuyInfo>(sql, new { Quote = quote, Symbol = symbol, BuyStatus = "filled" }).ToList();
        }

        public void UpdateNotFillBuy(OrderInfo orderInfo)
        {
            var sql = $"update t_buy_info set BuyQuantity=@BuyQuantity, BuyCreateAt=@BuyCreateAt, BuyTradePrice=@BuyTradePrice, " +
                $" BuyFilledNotional=@BuyFilledNotional, BuyStatus=@BuyStatus where BuyClientOid=@BuyClientOid";
            Database.Execute(sql, new
            {
                BuyTradePrice = orderInfo.price,
                BuyQuantity = orderInfo.size,
                BuyCreateAt = orderInfo.created_at,
                BuyFilledNotional = orderInfo.filled_notional,
                BuyStatus = orderInfo.status,
                BuyClientOid = orderInfo.client_oid
            });
        }

        public void UpdateNotFillBuyToCancel(OrderInfo orderInfo)
        {
            if(orderInfo.status == OrderStatus.cancelled)
            {
                if(orderInfo.filled_size > 0)
                {
                    var sqlA = $"update t_buy_info set BuyStatus=@BuyStatus, BuyQuantity=@BuyQuantity, BuyTradePrice=@BuyTradePrice, BuyFilledNotional=@BuyFilledNotional where BuyClientOid=@BuyClientOid";
                    Database.Execute(sqlA, new
                    {
                        BuyStatus = OrderStatus.filled,
                        BuyQuantity = orderInfo.filled_size,
                        BuyTradePrice = orderInfo.price,
                        BuyFilledNotional = orderInfo.filled_notional,
                        BuyClientOid = orderInfo.client_oid
                    });
                    return;
                }
            }

            var sql = $"update t_buy_info set BuyStatus=@BuyStatus where BuyClientOid=@BuyClientOid";
            Database.Execute(sql, new
            {
                BuyStatus = orderInfo.status,
                BuyClientOid = orderInfo.client_oid
            });
        }

        public List<BuyInfo> ListNeedQuerySellDetail(string quote, string symbol)
        {
            var sql = $"select * from t_buy_info where UserName='qq' and Quote=@Quote and Symbol=@Symbol and (SellStatus='{OrderStatus.open}' or SellStatus='{OrderStatus.prepare}' or SellStatus='{OrderStatus.part_filled}' or (SellTradePrice is null and SellStatus='filled'))";
            return Database.Query<BuyInfo>(sql, new { Quote = quote, Symbol = symbol }).ToList();
        }

        public void UpdateNotFillSell(OrderInfo orderInfo)
        {
            var sql = $"update t_buy_info set SellQuantity=@SellQuantity, SellCreateAt=@SellCreateAt, SellOrderId=@SellOrderId, SellTradePrice=@SellTradePrice, " +
                $" SellFilledNotional=@SellFilledNotional, SellStatus=@SellStatus where SellClientOid=@SellClientOid";
            Database.Execute(sql, new
            {
                SellTradePrice = orderInfo.price,
                SellOrderId = orderInfo.order_id,
                SellQuantity = orderInfo.size,
                SellCreateAt = orderInfo.created_at,
                SellFilledNotional = orderInfo.filled_notional,
                SellStatus = orderInfo.status,
                SellClientOid = orderInfo.client_oid
            });
        }

        public void UpdateNotFillSellToCancel(OrderInfo orderInfo)
        {
            var sql = $"update t_buy_info set SellStatus=@SellStatus where SellClientOid=@SellClientOid";
            Database.Execute(sql, new
            {
                SellStatus = orderInfo.status,
                SellClientOid = orderInfo.client_oid
            });
        }

        #endregion

        public BuyInfo GetBuyOrder(string buyClientOid)
        {
            var sql = $"select * from t_buy_info where BuyClientOid=@BuyClientOid";
            return Database.Query<BuyInfo>(sql, new { BuyClientOid = buyClientOid }).FirstOrDefault();
        }

        public BuyInfo GetSellOrder(string sellClientOid)
        {
            var sql = $"select * from t_buy_info where SellClientOid=@SellClientOid";
            return Database.Query<BuyInfo>(sql, new { SellClientOid = sellClientOid }).FirstOrDefault();
        }
    }
}
