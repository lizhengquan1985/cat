using SharpDapper;
using SharpDapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDao
{
    public class MaxInputPrice : BaseDao
    {
        public void CreateMaxInputPriceInfo(MaxInputPriceInfo item)
        {
            Database.Insert(item);
        }

        public List<MaxInputPriceInfo> ListMaxInputPriceInfo()
        {
            var sql = $"select * from t_max_input_price";
            return Database.Query<MaxInputPriceInfo>(sql).ToList();
        }
    }

    [Table("t_max_input_price")]
    public class MaxInputPriceInfo
    {
        public string Quote { get; set; }
        public string Symbol { get; set; }
        public decimal MaxInputPrice { get; set; }
        public bool IsChecked { get; set; }
    }
}
