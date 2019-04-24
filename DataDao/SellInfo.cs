using SharpDapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDao
{
    [Table("t_sell_info")]
    public class SellInfo
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Quote { get; set; }
        public string Symbol { get; set; }

        public decimal BuyPrice { get; set; }
        public decimal BuyTradePrice { get; set; }
        public decimal BuyQuantity { get; set; }
        public string BuyOrderId { get; set; }
        public string BuyClientOid { get; set; }
        public bool BuyResult { get; set; }
        public string BuyCreateAt { get; set; }
        public decimal BuyFilledNotional { get; set; }
        public string BuyStatus { get; set; }

        public decimal SellPrice { get; set; }
        public decimal SellTradePrice { get; set; }
        public decimal SellQuantity { get; set; }
        public string SellOrderId { get; set; }
        public string SellClientOid { get; set; }
        public bool SellResult { get; set; }
        public string SellCreateAt { get; set; }
        public decimal SellFilledNotional { get; set; }
        public string SellStatus { get; set; }
    }
}
