using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDapper;

namespace DataDao
{
    public class BuyInfoDao : BaseDao
    {
        public List<BuyInfo> List5LowertBuy(string quote, string symbolName)
        {
            var sql = $"select * from t_buy_info where UserName='qq' and Quote=@Quote and SymbolName=@SymbolName order by BuyPrice desc limit 8";
            return Database.Query<BuyInfo>(sql, new { Quote = quote, SymbolName = symbolName }).ToList();
        }
    }
}
