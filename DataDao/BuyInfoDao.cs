using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDapper;
using SharpDapper.Extensions;

namespace DataDao
{
    public class BuyInfoDao : BaseDao
    {
        public void CreateBuyInfo(BuyInfo buyInfo)
        {
             Database.InsertAsync(buyInfo);
        }

        public void UpdateBuyInfo(BuyInfo buyInfo)
        {
            var sql = $"update t_buy_info set SellClientOid=@SellClientOid, SellPrice=@SellPrice, SellQuantity=@SellQuantity where Id=@Id and BuyClientOid=@BuyClientOid";
            Database.Execute(sql, new { buyInfo.SellClientOid, buyInfo.SellPrice, buyInfo.SellQuantity, buyInfo.Id, buyInfo.BuyClientOid });
        }

        public List<BuyInfo> List5LowertBuy(string quote, string symbolName)
        {
            var sql = $"select * from t_buy_info where UserName='qq' and Quote=@Quote and SymbolName=@SymbolName order by BuyPrice desc limit 8";
            return Database.Query<BuyInfo>(sql, new { Quote = quote, SymbolName = symbolName }).ToList();
        }
    }
}
