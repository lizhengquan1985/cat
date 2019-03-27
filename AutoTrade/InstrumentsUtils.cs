using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrade
{
    public class InstrumentsUtils
    {
        static List<TradeItem> instruments = new List<TradeItem>();
        public static List<TradeItem> GetAll()
        {

            instruments.Add(new TradeItem { quote = "eth", symbol = "xrp" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "eos" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "trx" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "ada" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "qtum" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "xlm" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "xmr" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "ont" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "dash" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "ltc" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "omg" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "zil" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "etc" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "okb" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "ae" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "cvc" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "nano" });

            return instruments;
        }

        public static void AddItem(string quote, string symbol)
        {

        }
    }


    public class TradeItem
    {
        public string quote { get; set; }
        public string symbol { get; set; }
    }
}
