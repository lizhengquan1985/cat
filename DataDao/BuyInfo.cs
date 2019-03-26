using SharpDapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDao
{
    [Table("t_buy_info")]
    public class BuyInfo
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Quote { get; set; }
        public string Symbol { get; set; }

        public decimal BuyPrice { get; set; }
        public decimal BuyQuantity { get; set; }
        public string BuyClientOid { get; set; }
        public string BuyOrderId { get; set; }
        public bool BuyResult { get; set; }
        public string BuyCreateAt { get; set; }
        public decimal BuyFilledNotional { get; set; }
        public string BuyStatus { get; set; }


        public decimal SellPrice { get; set; }
        public decimal SellQuantity { get; set; }
        public string SellOrderId { get; set; }
        public string SellClientOid { get; set; }
        public bool SellResult { get; set; }
        public string SellCreateAt { get; set; }
        public decimal SellFilledNotional { get; set; }
        public string SellStatus { get; set; }



    }
    // {"client_oid":"buy636891564440671263","created_at":"2019-03-25T16:20:57.000Z","filled_notional":"0.00900384","filled_size":"3.984",
    // "funds":"","instrument_id":"XRP-ETH","notional":"","order_id":"2542459732239360","order_type":"0","price":"0.002304",
    // "product_id":"XRP-ETH","side":"buy","size":"3.984","status":"filled","timestamp":"2019-03-25T16:20:57.000Z","type":"limit"}

    //public string client_oid { get; set; }
    //public string created_at { get; set; }
    //public decimal filled_notional { get; set; }
    //public decimal filled_size { get; set; }
    //public string instrument_id { get; set; }
    //public string order_id { get; set; }
    //public string order_type { get; set; }
    //public decimal price { get; set; }
    //public string product_id { get; set; }
    //public string side { get; set; }
    //public decimal size { get; set; }
    //public string status { get; set; }
    //public string timestamp { get; set; }
    //public string type { get; set; }
}
