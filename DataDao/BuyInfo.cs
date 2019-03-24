using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDao
{
    public class BuyInfo
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Quote { get; set; }
        public string Symbol { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal BuyQuantity { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsFinished { get; set; }

        public decimal SellQuantity { get; set; }
        public decimal SellPrice { get; set; }
        public string BuyOrderId { get; set; }
        public string SellOrderId { get; set; }

    }
}
