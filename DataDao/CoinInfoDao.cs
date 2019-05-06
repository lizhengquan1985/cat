using SharpDapper;
using SharpDapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDao
{
    [Table("t_coin_info")]
    public class CoinInfo
    {
        public string Symbol { get; set; }
        public decimal UsdPrice { get; set; }
        public DateTime UsdPriceDate { get; set; }
    }

    public class CoinInfoDao : BaseDao
    {
        public List<CoinInfo> ListCoinInfo()
        {
            var sql = $"select * from t_coin_info where UsdPriceDate<@Date ";
            return Database.Query<CoinInfo>(sql, new { Date = DateTime.Now.AddHours(-3) }).ToList();
        }

        public void UpdateCoinInfo(string symbol, decimal usdPrice)
        {
            var sql = $"update t_coin_info set UsdPrice=@UsdPrice,UsdPriceDate=@UsdPriceDate where Symbol=@Symbol";
            Database.Execute(sql, new { UsdPrice = usdPrice, Symbol = symbol, UsdPriceDate = DateTime.Now });
        }
    }
}
