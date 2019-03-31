﻿using System;
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

        public void UpdateBuyInfo(BuyInfo buyInfo)
        {
            var sql = $"update t_buy_info set SellClientOid=@SellClientOid, SellPrice=@SellPrice, SellQuantity=@SellQuantity, SellResult=@SellResult, SellStatus='prepare' where Id=@Id and BuyClientOid=@BuyClientOid";
            Database.Execute(sql, new { buyInfo.SellClientOid, buyInfo.SellPrice, buyInfo.SellQuantity, buyInfo.SellResult, buyInfo.Id, buyInfo.BuyClientOid });
        }

        public List<BuyInfo> List5LowertBuy(string quote, string symbol)
        {
            var sql = $"select * from t_buy_info where UserName='qq' and Quote=@Quote and Symbol=@Symbol and (SellStatus!='filled' or SellStatus is null) order by BuyPrice asc limit 8";
            return Database.Query<BuyInfo>(sql, new { Quote = quote, Symbol = symbol }).ToList();
        }

        public int GetNotSellCount(string quote, string symbol)
        {
            try
            {
                var sql = $"select * from t_buy_info where UserName='qq' and Quote=@Quote and Symbol=@Symbol and (SellStatus!='filled' or SellStatus is null) and BuyStatus='filled'";
                return Database.ExecuteScalar<int>(sql, new { Quote = quote, Symbol = symbol });
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                return 0;
            }
        }

        public List<BuyInfo> List5NeedSell(string quote, string symbol)
        {
            var sql = $"select * from t_buy_info where UserName='qq' and Quote=@Quote and Symbol=@Symbol and BuyStatus='filled' and (SellStatus is null or SellStatus='nosell') order by BuyPrice asc limit 8";
            return Database.Query<BuyInfo>(sql, new { Quote = quote, Symbol = symbol }).ToList();
        }

        #region 订单结果查询以及匹配

        public List<BuyInfo> ListNotFillBuy(string quote, string symbol)
        {
            var sql = $"select * from t_buy_info where UserName='qq' and Quote=@Quote and Symbol=@Symbol and BuyStatus!=@BuyStatus";
            return Database.Query<BuyInfo>(sql, new { Quote = quote, Symbol = symbol, BuyStatus = "filled" }).ToList();
        }

        public void UpdateNotFillBuy(OrderInfo orderInfo)
        {
            var sql = $"update t_buy_info set BuyPrice=@BuyPrice, BuyQuantity=@BuyQuantity, BuyCreateAt=@BuyCreateAt, " +
                $" BuyFilledNotional=@BuyFilledNotional, BuyStatus=@BuyStatus where BuyClientOid=@BuyClientOid";
            Database.Execute(sql, new
            {
                BuyPrice = orderInfo.price,
                BuyQuantity = orderInfo.size,
                BuyCreateAt = orderInfo.created_at,
                BuyFilledNotional = orderInfo.filled_notional,
                BuyStatus = orderInfo.status,
                BuyClientOid = orderInfo.client_oid
            });
        }

        public List<BuyInfo> ListNotFillSell(string quote, string symbol)
        {
            var sql = $"select * from t_buy_info where UserName='qq' and Quote=@Quote and Symbol=@Symbol and SellStatus!=@SellStatus and SellStatus!='nosell'";
            return Database.Query<BuyInfo>(sql, new { Quote = quote, Symbol = symbol, SellStatus = "filled" }).ToList();
        }

        public void UpdateNotFillSell(OrderInfo orderInfo)
        {
            var sql = $"update t_buy_info set SellPrice=@SellPrice, SellQuantity=@SellQuantity, SellCreateAt=@SellCreateAt, " +
                $" SellFilledNotional=@SellFilledNotional, SellStatus=@SellStatus where SellClientOid=@SellClientOid";
            Database.Execute(sql, new
            {
                SellPrice = orderInfo.price,
                SellQuantity = orderInfo.size,
                SellCreateAt = orderInfo.created_at,
                SellFilledNotional = orderInfo.filled_notional,
                SellStatus = orderInfo.status,
                SellClientOid = orderInfo.client_oid
            });
        }

        #endregion
    }
}
