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
            instruments.Add(new TradeItem { quote = "eth", symbol = "ltc" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "okb" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "etc" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "eos" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "xrp" });

            instruments.Add(new TradeItem { quote = "eth", symbol = "trx" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "dash" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "neo" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "qtum" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "xlm" });

            instruments.Add(new TradeItem { quote = "eth", symbol = "ada" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "aac" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "abl" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "abt" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "act" });

            instruments.Add(new TradeItem { quote = "eth", symbol = "ae" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "ark" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "auto" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "bec" }); // xxxxxxx
            //instruments.Add(new TradeItem { quote = "eth", symbol = "bkx" });

            instruments.Add(new TradeItem { quote = "eth", symbol = "bnt" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "btm" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "btt" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "cai" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "cic" });

            instruments.Add(new TradeItem { quote = "eth", symbol = "cmt" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "ctxc" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "cvc" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "dadi" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "dcr" });

            //instruments.Add(new TradeItem { quote = "eth", symbol = "dgb" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "dgd" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "dpy" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "edo" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "egt" });

            instruments.Add(new TradeItem { quote = "eth", symbol = "elf" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "fair" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "gas" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "gnt" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "gnx" });

            instruments.Add(new TradeItem { quote = "eth", symbol = "gto" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "hc" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "hmc" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "hpb" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "hyc" });

            instruments.Add(new TradeItem { quote = "eth", symbol = "icx" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "ins" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "insur" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "int" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "iost" });

            instruments.Add(new TradeItem { quote = "eth", symbol = "iota" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "itc" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "kan" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "kcash" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "lba" });

            instruments.Add(new TradeItem { quote = "eth", symbol = "let" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "light" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "link" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "lrc" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "lsk" });

            instruments.Add(new TradeItem { quote = "eth", symbol = "mana" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "mco" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "mdt" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "mith" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "mkr" });

            //instruments.Add(new TradeItem { quote = "eth", symbol = "mof" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "mvp" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "nano" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "nas" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "nuls" });

            //instruments.Add(new TradeItem { quote = "eth", symbol = "of" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "omg" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "ont" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "ors" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "pay" });

            //instruments.Add(new TradeItem { quote = "eth", symbol = "ppt" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "pra" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "pst" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "qun" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "r" });

            //instruments.Add(new TradeItem { quote = "eth", symbol = "rfr" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "rnt" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "sc" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "sda" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "snc" });

            instruments.Add(new TradeItem { quote = "eth", symbol = "snt" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "soc" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "ssc" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "storj" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "swftc" });

            instruments.Add(new TradeItem { quote = "eth", symbol = "theta" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "topc" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "tra" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "trio" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "true" });

            //instruments.Add(new TradeItem { quote = "eth", symbol = "uct" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "ugc" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "vib" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "vite" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "waves" });

            //instruments.Add(new TradeItem { quote = "eth", symbol = "win" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "wtc" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "xem" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "xmr" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "xuc" });

            //instruments.Add(new TradeItem { quote = "eth", symbol = "you" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "zco" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "zec" });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "zen" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "zil" });

            //instruments.Add(new TradeItem { quote = "eth", symbol = "zip" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "zrx" });

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
