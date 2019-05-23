using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DataDao;
using log4net.Repository.Hierarchy;
using Newtonsoft.Json;

namespace AutoTrade
{
    public class InstrumentsUtils
    {
        private static List<TradeItem> instruments = new List<TradeItem>();
        private static List<instruments> okInstruments = new List<instruments>();

        public static void InitAllCoins()
        {
            okInstruments = OkApi.Listinstruments();
            Console.WriteLine("okInstruments -- > " + okInstruments.Count);
            //Console.WriteLine(JsonConvert.SerializeObject(okInstruments.FindAll(it=>it.quote_currency.ToLower() == "btc")));

            InitBtc();
            var btcCount = instruments.Count;
            Console.WriteLine("btc count -- > " + btcCount);
            InitOKB();
            var okbCount = instruments.Count - btcCount;
            Console.WriteLine("okb count -- > " + okbCount);
            InitEth();
            var ethCount = instruments.Count - okbCount - btcCount;
            Console.WriteLine("eth count -- > " + ethCount);
            Console.WriteLine($"总共这个多个交易对： {instruments.Count}");

            // 查找没有加入交易的信息
            var findNoTrade = okInstruments.FindAll(it => it.quote_currency.ToLower() != "usdt" && instruments.Find(item => item.quote.ToLower() == it.quote_currency.ToLower() && item.symbol.ToLower() == it.base_currency.ToLower()) == null);
            if (findNoTrade != null && findNoTrade.Count > 0)
            {
                Console.WriteLine("-------------------------");
                Console.WriteLine("-------------------------");
                foreach (var item in findNoTrade)
                {
                    Console.WriteLine($"未加入: {item.quote_currency}- {item.base_currency}");
                }
                Console.WriteLine("-------------------------");
                Console.WriteLine("-------------------------");
            }

            //var maxInputPriceList = new MaxInputPrice().ListMaxInputPriceInfo();
            foreach (var item in instruments)
            {
                //var findItem = maxInputPriceList.Find(it => it.Quote == item.quote && it.Symbol == item.symbol);
                //if (findItem != null)
                //{
                //    item.MaxBuyPrice = findItem.MaxInputPrice;
                //}
            }
        }

        public static void InitOKB()
        {
            instruments.Add(new TradeItem { quote = "okb", symbol = "ltc", MaxBuyPrice = (decimal)108.88, SmallSellPrice = (decimal)81.88, EmptySize = (decimal)0.01 });
            instruments.Add(new TradeItem { quote = "okb", symbol = "etc", MaxBuyPrice = (decimal)12.88, SmallSellPrice = (decimal)8.18, EmptySize = (decimal)0.1 });
            instruments.Add(new TradeItem { quote = "okb", symbol = "eos", MaxBuyPrice = (decimal)9.88, SmallSellPrice = (decimal)6.88, EmptySize = (decimal)0.1 });
            instruments.Add(new TradeItem { quote = "okb", symbol = "xrp", MaxBuyPrice = (decimal)0.52, SmallSellPrice = (decimal)0.36, EmptySize = 4 });
            instruments.Add(new TradeItem { quote = "okb", symbol = "trx", MaxBuyPrice = (decimal)0.0388, SmallSellPrice = (decimal)0.0288, EmptySize = 50 });

            instruments.Add(new TradeItem { quote = "okb", symbol = "dash", MaxBuyPrice = (decimal)288, SmallSellPrice = (decimal)188, EmptySize = (decimal)0.01 });
            instruments.Add(new TradeItem { quote = "okb", symbol = "neo", MaxBuyPrice = (decimal)18.8, SmallSellPrice = (decimal)12.8, EmptySize = (decimal)0.045 }); // --
            instruments.Add(new TradeItem { quote = "okb", symbol = "qtum", MaxBuyPrice = (decimal)6.18, SmallSellPrice = (decimal)3.88, EmptySize = (decimal)0.22 });
            instruments.Add(new TradeItem { quote = "okb", symbol = "ada", MaxBuyPrice = (decimal)0.188, SmallSellPrice = (decimal)0.066, EmptySize = (decimal)9 });
            instruments.Add(new TradeItem { quote = "okb", symbol = "ae", MaxBuyPrice = (decimal)1.18, SmallSellPrice = (decimal)0.68, EmptySize = (decimal)0.9 });

            instruments.Add(new TradeItem { quote = "okb", symbol = "bloc", MaxBuyPrice = (decimal)0.038, SmallSellPrice = (decimal)0.0266, EmptySize = (decimal)18 });
            instruments.Add(new TradeItem { quote = "okb", symbol = "cai", MaxBuyPrice = (decimal)0.000288, SmallSellPrice = (decimal)0.000126, EmptySize = (decimal)4000 });
            instruments.Add(new TradeItem { quote = "okb", symbol = "dcr", MaxBuyPrice = (decimal)36.8, SmallSellPrice = (decimal)28.8, EmptySize = (decimal)0.02 });
            instruments.Add(new TradeItem { quote = "okb", symbol = "egt", MaxBuyPrice = (decimal)0.00288, SmallSellPrice = (decimal)0.00226, EmptySize = 200 });
            instruments.Add(new TradeItem { quote = "okb", symbol = "hpb", MaxBuyPrice = (decimal)0.88, SmallSellPrice = (decimal)0.48, EmptySize = (decimal)0.8 });

            instruments.Add(new TradeItem { quote = "okb", symbol = "iota", MaxBuyPrice = (decimal)0.88, SmallSellPrice = (decimal)0.521, EmptySize = (decimal)1 });
            instruments.Add(new TradeItem { quote = "okb", symbol = "kan", MaxBuyPrice = (decimal)0.00688, SmallSellPrice = (decimal)0.00388, EmptySize = 100 });
            instruments.Add(new TradeItem { quote = "okb", symbol = "lba", MaxBuyPrice = (decimal)0.0666, SmallSellPrice = (decimal)0.0388, EmptySize = 10 });
            instruments.Add(new TradeItem { quote = "okb", symbol = "let", MaxBuyPrice = (decimal)0.0088, SmallSellPrice = (decimal)0.00588, EmptySize = 100 });
            instruments.Add(new TradeItem { quote = "okb", symbol = "nas", MaxBuyPrice = (decimal)1.88, SmallSellPrice = (decimal)0.96, EmptySize = (decimal)0.4 });

            instruments.Add(new TradeItem { quote = "okb", symbol = "ors", MaxBuyPrice = (decimal)0.0099, SmallSellPrice = (decimal)0.0068, EmptySize = (decimal)80 });
            instruments.Add(new TradeItem { quote = "okb", symbol = "sc", MaxBuyPrice = (decimal)0.00588, SmallSellPrice = (decimal)0.00366, EmptySize = (decimal)100 });
            instruments.Add(new TradeItem { quote = "okb", symbol = "sda", MaxBuyPrice = (decimal)0.0088, SmallSellPrice = (decimal)0.0066, EmptySize = (decimal)80 });
            instruments.Add(new TradeItem { quote = "okb", symbol = "vite", MaxBuyPrice = (decimal)0.038, SmallSellPrice = (decimal)0.028, EmptySize = (decimal)18 });
            instruments.Add(new TradeItem { quote = "okb", symbol = "waves", MaxBuyPrice = (decimal)5.88, SmallSellPrice = (decimal)3.66, EmptySize = (decimal)0.13 });

            instruments.Add(new TradeItem { quote = "okb", symbol = "win", MaxBuyPrice = (decimal)0.0009, SmallSellPrice = (decimal)0.00078, EmptySize = 400 });
            instruments.Add(new TradeItem { quote = "okb", symbol = "xas", MaxBuyPrice = (decimal)0.138, SmallSellPrice = (decimal)0.108, EmptySize = 3 });
            instruments.Add(new TradeItem { quote = "okb", symbol = "you", MaxBuyPrice = (decimal)0.0198, SmallSellPrice = (decimal)0.0158, EmptySize = 20 });
            instruments.Add(new TradeItem { quote = "okb", symbol = "zco", MaxBuyPrice = (decimal)0.0099, SmallSellPrice = (decimal)0.00688, EmptySize = 35 });
            instruments.Add(new TradeItem { quote = "okb", symbol = "zec", MaxBuyPrice = (decimal)188, SmallSellPrice = (decimal)108, EmptySize = (decimal)0.003 });
        }

        public static void InitBtc()
        {
            instruments.Add(new TradeItem { quote = "btc", symbol = "ltc", MaxBuyPrice = (decimal)0.022 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "eth", MaxBuyPrice = (decimal)0.08 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "okb", MaxBuyPrice = (decimal)0.0005 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "etc", MaxBuyPrice = (decimal)0.003, SmallSellPrice = (decimal)0.0015, EmptySize = (decimal)0.5 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "bch", MaxBuyPrice = (decimal)0.08 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "eos", MaxBuyPrice = (decimal)0.0022 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "xrp", MaxBuyPrice = (decimal)0.00016, SmallSellPrice = (decimal)0.0001, EmptySize = 10 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "trx", MaxBuyPrice = (decimal)0.00001, SmallSellPrice = (decimal)0.00000588, EmptySize = 100 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "bsv", MaxBuyPrice = (decimal)0.016 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "dash", MaxBuyPrice = (decimal)0.05 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "neo", MaxBuyPrice = (decimal)0.004 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "qtum", MaxBuyPrice = (decimal)0.0014 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "xlm", MaxBuyPrice = (decimal)0.000052 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "ada", MaxBuyPrice = (decimal)0.000022 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "aac", MaxBuyPrice = (decimal)0.0000019, SmallSellPrice = (decimal)0.0000016, EmptySize = 100 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "atom", MaxBuyPrice = (decimal)0.0009 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "abl", MaxBuyPrice = (decimal)0.00000557 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "abt", MaxBuyPrice = (decimal)0.0000960, SmallSellPrice = (decimal)0.00009, EmptySize = (decimal)1.2 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "ace", MaxBuyPrice = (decimal)0.00001888, SmallSellPrice = (decimal)0.00000888, EmptySize = 20 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "act", MaxBuyPrice = (decimal)0.0000066 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "ae", MaxBuyPrice = (decimal)0.000266 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "ardr", MaxBuyPrice = (decimal)0.000026 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "ark", MaxBuyPrice = (decimal)0.000150 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "bcd", MaxBuyPrice = (decimal)0.000354 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "bec", MaxBuyPrice = (decimal)0.00002 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "bkx", MaxBuyPrice = (decimal)0.00000833 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "bloc", MaxBuyPrice = (decimal)0.00001 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "bnt", MaxBuyPrice = (decimal)0.000338 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "btg", MaxBuyPrice = (decimal)0.00481 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "btm", MaxBuyPrice = (decimal)0.00006 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "btt", MaxBuyPrice = (decimal)0.00000024 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "cai", MaxBuyPrice = (decimal)0.00000005 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "chat", MaxBuyPrice = (decimal)0.000002 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "cic", MaxBuyPrice = (decimal)0.00000026, SmallSellPrice = (decimal)0.00000023, EmptySize = 600 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "cmt", MaxBuyPrice = (decimal)0.000019 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "ctxc", MaxBuyPrice = (decimal)0.00009 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "cvc", MaxBuyPrice = (decimal)0.0000266 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "cvt", MaxBuyPrice = (decimal)0.00000335 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "dadi", MaxBuyPrice = (decimal)0.00001899 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "dcr", MaxBuyPrice = (decimal)0.01101 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "dgb", MaxBuyPrice = (decimal)0.00000497 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "dgd", MaxBuyPrice = (decimal)0.0088, SmallSellPrice = (decimal)0.0066, EmptySize = (decimal)0.02 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "dpy", MaxBuyPrice = (decimal)0.00002230 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "edo", MaxBuyPrice = (decimal)0.000349 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "egt", MaxBuyPrice = (decimal)0.000000688, SmallSellPrice = (decimal)0.000000458, EmptySize = 300 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "elf", MaxBuyPrice = (decimal)0.0001 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "gas", MaxBuyPrice = (decimal)0.0018 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "gnt", MaxBuyPrice = (decimal)0.000042 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "gnx", MaxBuyPrice = (decimal)0.00000767 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "gto", MaxBuyPrice = (decimal)0.00000987 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "gusd", MaxBuyPrice = (decimal)0 });  稳定币
            instruments.Add(new TradeItem { quote = "btc", symbol = "hc", MaxBuyPrice = (decimal)0.00066 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "hmc", MaxBuyPrice = (decimal)0.00000180 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "hpb", MaxBuyPrice = (decimal)0.0001021 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "hyc", MaxBuyPrice = (decimal)0.00000269 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "icx", MaxBuyPrice = (decimal)0.00019 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "ins", MaxBuyPrice = (decimal)0.000119 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "insur", MaxBuyPrice = (decimal)0.0000000911 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "int", MaxBuyPrice = (decimal)0.000012 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "iost", MaxBuyPrice = (decimal)0.000007 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "iota", MaxBuyPrice = (decimal)0.000126 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "itc", MaxBuyPrice = (decimal)0.0000888 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "kan", MaxBuyPrice = (decimal)0.00000188, SmallSellPrice = (decimal)0.0000011, EmptySize = 80 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "kcash", MaxBuyPrice = (decimal)0.0000088, SmallSellPrice = (decimal)0.0000066, EmptySize = 20 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "knc", MaxBuyPrice = (decimal)0.0000873 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "lba", MaxBuyPrice = (decimal)0.0000188, SmallSellPrice = (decimal)0.0000126, EmptySize = 8 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "let", MaxBuyPrice = (decimal)0.0000018, SmallSellPrice = (decimal)0.0000016, EmptySize = 80 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "light", MaxBuyPrice = (decimal)0.0000000171 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "link", MaxBuyPrice = (decimal)0.000188, SmallSellPrice = (decimal)0.000158, EmptySize = (decimal)1.1 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "lrc", MaxBuyPrice = (decimal)0.00003668 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "lsk", MaxBuyPrice = (decimal)0.001 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "mana", MaxBuyPrice = (decimal)0.000018 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "mco", MaxBuyPrice = (decimal)0.0014 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "mdt", MaxBuyPrice = (decimal)0.00000527 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "mith", MaxBuyPrice = (decimal)0.000022 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "mkr", MaxBuyPrice = (decimal)0.183 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "mof", MaxBuyPrice = (decimal)0.00003 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "nano", MaxBuyPrice = (decimal)0.0005 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "nas", MaxBuyPrice = (decimal)0.0005 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "nuls", MaxBuyPrice = (decimal)0.000268 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "nxt", MaxBuyPrice = (decimal)0.000016 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "of", MaxBuyPrice = (decimal)0.0000001208 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "omg", MaxBuyPrice = (decimal)0.001 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "ont", MaxBuyPrice = (decimal)0.00066 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "ors", MaxBuyPrice = (decimal)0.0000023, SmallSellPrice = (decimal)0.0000021, EmptySize = 100 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "pax", MaxBuyPrice = (decimal)0 });  稳定币
            instruments.Add(new TradeItem { quote = "btc", symbol = "pay", MaxBuyPrice = (decimal)0.00008 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "pst", MaxBuyPrice = (decimal)0.00002975 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "qun", MaxBuyPrice = (decimal)0.0000025, SmallSellPrice = (decimal)0.0000022, EmptySize = 50 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "r", MaxBuyPrice = (decimal)0.0000509 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "rfr", MaxBuyPrice = (decimal)0.000000884 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "sbtc", MaxBuyPrice = (decimal)0.000624 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "sc", MaxBuyPrice = (decimal)0.000000889 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "snc", MaxBuyPrice = (decimal)0.0000111 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "snt", MaxBuyPrice = (decimal)0.0000126 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "soc", MaxBuyPrice = (decimal)0.00000266 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "ssc", MaxBuyPrice = (decimal)0.0000019, SmallSellPrice = (decimal)0.0000015, EmptySize = 100 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "stc", MaxBuyPrice = (decimal)0.0000012 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "swftc", MaxBuyPrice = (decimal)0.0000012, SmallSellPrice = (decimal)0.000001, EmptySize = 100 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "tct", MaxBuyPrice = (decimal)0.0000107 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "theta", MaxBuyPrice = (decimal)0.000045 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "trio", MaxBuyPrice = (decimal)0.00000138, SmallSellPrice = (decimal)0.00000108, EmptySize = 150 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "true", MaxBuyPrice = (decimal)0.000166 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "tusd", MaxBuyPrice = (decimal)0 });  稳定币
            instruments.Add(new TradeItem { quote = "btc", symbol = "ugc", MaxBuyPrice = (decimal)0.000001710 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "usdc", MaxBuyPrice = (decimal)0 });  稳定币
            instruments.Add(new TradeItem { quote = "btc", symbol = "vib", MaxBuyPrice = (decimal)0.00001835 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "vite", MaxBuyPrice = (decimal)0.0000124, SmallSellPrice = (decimal)0.00001, EmptySize = (decimal)10 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "waves", MaxBuyPrice = (decimal)0.0012, SmallSellPrice = (decimal)0.0008, EmptySize = (decimal)0.13 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "win", MaxBuyPrice = (decimal)0.00000022, SmallSellPrice = (decimal)0.00000020, EmptySize = 500 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "wtc", MaxBuyPrice = (decimal)0.0008, SmallSellPrice = (decimal)0.000488, EmptySize = (decimal)0.25 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "xas", MaxBuyPrice = (decimal)0.000042, SmallSellPrice = (decimal)0.0000288, EmptySize = 4 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "xem", MaxBuyPrice = (decimal)0.00003, SmallSellPrice = (decimal)0.00002, EmptySize = (decimal)5 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "xmr", MaxBuyPrice = (decimal)0.0288, SmallSellPrice = (decimal)0.0188, EmptySize = (decimal)0.007 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "you", MaxBuyPrice = (decimal)0.00000488, SmallSellPrice = (decimal)0.00000388, EmptySize = 30 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "zco", MaxBuyPrice = (decimal)0.00000365, SmallSellPrice = (decimal)0.00000188, EmptySize = (decimal)80 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "zec", MaxBuyPrice = (decimal)0.03, SmallSellPrice = (decimal)0.015, EmptySize = (decimal)0.008 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "zen", MaxBuyPrice = (decimal)0.00251, SmallSellPrice = (decimal)0.00186, EmptySize = (decimal)0.09 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "zil", MaxBuyPrice = (decimal)0.00000612, SmallSellPrice = (decimal)0.00000488, EmptySize = (decimal)25 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "zip", MaxBuyPrice = (decimal)0.000000288, SmallSellPrice = (decimal)0.000000168, EmptySize = (decimal)8000 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "zrx", MaxBuyPrice = (decimal)0.000118, SmallSellPrice = (decimal)0.00011, EmptySize = (decimal)1 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "bcx", MaxBuyPrice = (decimal)0.00000016, SmallSellPrice = (decimal)0.00000022, EmptySize = (decimal)500 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "xuc", MaxBuyPrice = (decimal)0.0008, SmallSellPrice = (decimal)0.000588, EmptySize = (decimal)0.2 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "yoyo", MaxBuyPrice = (decimal)0.00000488, SmallSellPrice = (decimal)0.00000466, EmptySize = 23 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "fun", MaxBuyPrice = (decimal)0.00000138, SmallSellPrice = (decimal)0.00000126, EmptySize = 100 });

        }

        public static void InitEth()
        {
            instruments.Add(new TradeItem { quote = "eth", symbol = "ltc", MaxBuyPrice = (decimal)0.388 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "okb", MaxBuyPrice = (decimal)0.00766 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "etc", MaxBuyPrice = (decimal)0.0588, SmallSellPrice = (decimal)0.0366, EmptySize = (decimal)0.5 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "eos", MaxBuyPrice = (decimal)0.0288 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "xrp", MaxBuyPrice = (decimal)0.003, SmallSellPrice = (decimal)0.0038, EmptySize = 10 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "trx", MaxBuyPrice = (decimal)0.00017, SmallSellPrice = (decimal)0.00025, EmptySize = 100 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "dash", MaxBuyPrice = (decimal)1.11 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "neo", MaxBuyPrice = (decimal)0.088 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "qtum", MaxBuyPrice = (decimal)0.0266 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "xlm", MaxBuyPrice = (decimal)0.00126 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "ada", MaxBuyPrice = (decimal)0.001 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "aac", MaxBuyPrice = (decimal)0.000068, SmallSellPrice = (decimal)0.000048, EmptySize = 100 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "abl", MaxBuyPrice = (decimal)0.00022374 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "abt", MaxBuyPrice = (decimal)0.003, SmallSellPrice = (decimal)0.0026, EmptySize = (decimal)1.2 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "act", MaxBuyPrice = (decimal)0.0002200 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "atom", MaxBuyPrice = (decimal)0.05 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "ae", MaxBuyPrice = (decimal)0.0066 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "ark", MaxBuyPrice = (decimal)0.01 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "auto", MaxBuyPrice = (decimal)0.00002219 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "bec", MaxBuyPrice = (decimal)0.0006 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "bkx", MaxBuyPrice = (decimal)0.000355 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "bnt", MaxBuyPrice = (decimal)0.01 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "btm", MaxBuyPrice = (decimal)0.002 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "btt", MaxBuyPrice = (decimal)0.000012 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "cai", MaxBuyPrice = (decimal)0.000001701 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "cic", MaxBuyPrice = (decimal)0.0000099, SmallSellPrice = (decimal)0.000008, EmptySize = 400 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "cmt", MaxBuyPrice = (decimal)0.00051 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "ctxc", MaxBuyPrice = (decimal)0.0024 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "cvc", MaxBuyPrice = (decimal)0.0012 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "dadi", MaxBuyPrice = (decimal)0.000576 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "dcr", MaxBuyPrice = (decimal)0.3 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "dgb", MaxBuyPrice = (decimal)0.0001908 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "dgd", MaxBuyPrice = (decimal)0.259, SmallSellPrice = (decimal)0.222, EmptySize = (decimal)0.01 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "dpy", MaxBuyPrice = (decimal)0.001259 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "edo", MaxBuyPrice = (decimal)0.01 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "egt", MaxBuyPrice = (decimal)0.0000180, SmallSellPrice = (decimal)0.000014, EmptySize = 200 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "elf", MaxBuyPrice = (decimal)0.003 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "fair", MaxBuyPrice = (decimal)0.00005, SmallSellPrice = (decimal)0.00003, EmptySize = 100 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "gas", MaxBuyPrice = (decimal)0.05 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "gnt", MaxBuyPrice = (decimal)0.0013 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "gnx", MaxBuyPrice = (decimal)0.00031 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "gto", MaxBuyPrice = (decimal)0.000411 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "hc", MaxBuyPrice = (decimal)0.02 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "hmc", MaxBuyPrice = (decimal)0.0000551 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "hpb", MaxBuyPrice = (decimal)0.00712 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "hyc", MaxBuyPrice = (decimal)0.0000983 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "icx", MaxBuyPrice = (decimal)0.0041 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "ins", MaxBuyPrice = (decimal)0.00518 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "insur", MaxBuyPrice = (decimal)0.00000320 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "int", MaxBuyPrice = (decimal)0.000362 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "iost", MaxBuyPrice = (decimal)0.00014 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "iota", MaxBuyPrice = (decimal)0.005 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "itc", MaxBuyPrice = (decimal)0.0012 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "kan", MaxBuyPrice = (decimal)0.000042, SmallSellPrice = (decimal)0.000032, EmptySize = 80 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "kcash", MaxBuyPrice = (decimal)0.00021, SmallSellPrice = (decimal)0.00029, EmptySize = 10 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "lba", MaxBuyPrice = (decimal)0.0005, SmallSellPrice = (decimal)0.0004, EmptySize = 10 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "let", MaxBuyPrice = (decimal)0.000066, SmallSellPrice = (decimal)0.000045, EmptySize = 100 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "light", MaxBuyPrice = (decimal)0.000000510 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "link", MaxBuyPrice = (decimal)0.0062, SmallSellPrice = (decimal)0.0052, EmptySize = (decimal)1.1 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "lrc", MaxBuyPrice = (decimal)0.000962 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "lsk", MaxBuyPrice = (decimal)0.03 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "mana", MaxBuyPrice = (decimal)0.00051 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "mco", MaxBuyPrice = (decimal)0.06 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "mdt", MaxBuyPrice = (decimal)0.0001371 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "mith", MaxBuyPrice = (decimal)0.0005 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "mkr", MaxBuyPrice = (decimal)9.69 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "mof", MaxBuyPrice = (decimal)0.0008 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "mvp", MaxBuyPrice = (decimal)0.000006 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "nano", MaxBuyPrice = (decimal)0.019 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "nas", MaxBuyPrice = (decimal)0.015 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "nuls", MaxBuyPrice = (decimal)0.011 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "of", MaxBuyPrice = (decimal)0.00000364 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "omg", MaxBuyPrice = (decimal)0.03 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "ont", MaxBuyPrice = (decimal)0.021 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "ors", MaxBuyPrice = (decimal)0.00012, SmallSellPrice = (decimal)0.00008, EmptySize = 100 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "pay", MaxBuyPrice = (decimal)0.0041 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "ppt", MaxBuyPrice = (decimal)0.02 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "pra", MaxBuyPrice = (decimal)0.000365 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "pst", MaxBuyPrice = (decimal)0.001963 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "qun", MaxBuyPrice = (decimal)0.0000887, SmallSellPrice = (decimal)0.00006, EmptySize = 50 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "r", MaxBuyPrice = (decimal)0.00189 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "ref", MaxBuyPrice = (decimal)0.0051 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "rct", MaxBuyPrice = (decimal)0.000012 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "yoyo", MaxBuyPrice = (decimal)0.0003 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "rfr", MaxBuyPrice = (decimal)0.000031488 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "rnt", MaxBuyPrice = (decimal)0.000764 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "sc", MaxBuyPrice = (decimal)0.0000399 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "sda", MaxBuyPrice = (decimal)0.0000913 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "snc", MaxBuyPrice = (decimal)0.000366 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "snt", MaxBuyPrice = (decimal)0.00031 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "soc", MaxBuyPrice = (decimal)0.0000931 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "ssc", MaxBuyPrice = (decimal)0.00007, SmallSellPrice = (decimal)0.00005, EmptySize = 100 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "storj", MaxBuyPrice = (decimal)0.0041 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "swftc", MaxBuyPrice = (decimal)0.000023, SmallSellPrice = (decimal)0.000020, EmptySize = 100 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "theta", MaxBuyPrice = (decimal)0.0015 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "topc", MaxBuyPrice = (decimal)0.00009195 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "tra", MaxBuyPrice = (decimal)0.000000593 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "trio", MaxBuyPrice = (decimal)0.000038, SmallSellPrice = (decimal)0.000032, EmptySize = 150 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "true", MaxBuyPrice = (decimal)0.00746 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "uct", MaxBuyPrice = (decimal)0.0001109 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "ugc", MaxBuyPrice = (decimal)0.00005091 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "vib", MaxBuyPrice = (decimal)0.000549 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "vite", MaxBuyPrice = (decimal)0.000368 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "waves", MaxBuyPrice = (decimal)0.04 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "win", MaxBuyPrice = (decimal)0.000008, SmallSellPrice = (decimal)0.000007, EmptySize = 300 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "wtc", MaxBuyPrice = (decimal)0.03251 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "xem", MaxBuyPrice = (decimal)0.00071 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "xmr", MaxBuyPrice = (decimal)0.9 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "xuc", MaxBuyPrice = (decimal)0.04 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "you", MaxBuyPrice = (decimal)0.00019, SmallSellPrice = (decimal)0.00014, EmptySize = 10 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "zco", MaxBuyPrice = (decimal)0.00009 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "zec", MaxBuyPrice = (decimal)0.9 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "zen", MaxBuyPrice = (decimal)0.0973 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "zil", MaxBuyPrice = (decimal)0.00031 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "zip", MaxBuyPrice = (decimal)0.00000666 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "zrx", MaxBuyPrice = (decimal)0.005 });

        }

        public static List<TradeItem> GetAll()
        {
            return instruments;
        }

        public static List<instruments> GetOkInstruments()
        {
            return okInstruments;
        }

        public static instruments GetOkInstruments(string quote, string symbol)
        {
            return okInstruments.Find(it => it.quote_currency.ToLower() == quote && it.base_currency.ToLower() == symbol);
        }

        public static void AddItem(string quote, string symbol)
        {

        }

        public static bool CheckMaxBuyPrice(string quote, string symbol, decimal nowPrice)
        {
            var item = instruments.Find(it => it.quote == quote && it.symbol == symbol);
            if (item != null && item.MaxBuyPrice > 0 && item.MaxBuyPrice < nowPrice)
            {
                Console.WriteLine("价格太高，不能购买");
                return false;
            }

            return true;
        }

        public static bool CheckSmallSellPrice(string quote, string symbol, decimal nowPrice)
        {
            var item = instruments.Find(it => it.quote == quote && it.symbol == symbol);
            if (item == null || item.SmallSellPrice <= 0 || item.SmallSellPrice > nowPrice)
            {
                Console.WriteLine($"没配置，或者smallSellPrice<=0，或者价格太低， 无法做空");
                return false;
            }

            return true;
        }
    }


    public class TradeItem
    {
        public string quote { get; set; }
        public string symbol { get; set; }
        /// <summary>
        /// 最大的购入价格
        /// </summary>
        public decimal MaxBuyPrice { get; set; }
        public decimal BuyLadderRatio { get; set; }
        /// <summary>
        /// 做空价格
        /// </summary>
        public decimal SmallSellPrice { get; set; }
        public decimal SellLadderRatio { get; set; }
        public decimal EmptySize { get; set; }
    }

    public class instruments
    {
        public string instrument_id { get; set; }
        public string base_currency { get; set; }
        public string quote_currency { get; set; }
        /// <summary>
        /// 最小交易数量
        /// </summary>
        public decimal min_size { get; set; }
        /// <summary>
        /// 交易货币数量精度
        /// </summary>
        public string size_increment { get; set; }
        /// <summary>
        /// 	交易价格精度
        /// </summary>
        public string tick_size { get; set; }

        public int GetTickSizeNumber()
        {
            if (quote_currency.ToLower() == "btc" && base_currency.ToLower() == "btt")
            {
                return 8;
            }

            if (tick_size == "1")
            {
                return 0;
            }
            if (tick_size == "0.1")
            {
                return 1;
            }
            if (tick_size == "0.01")
            {
                return 2;
            }
            if (tick_size == "0.001")
            {
                return 3;
            }
            if (tick_size == "0.0001")
            {
                return 4;
            }
            if (tick_size == "0.00001")
            {
                return 5;
            }
            if (tick_size == "0.000001")
            {
                return 6;
            }
            if (tick_size == "0.0000001")
            {
                return 7;
            }
            if (tick_size == "0.00000001")
            {
                return 8;
            }
            if (tick_size == "0.000000001")
            {
                return 9;
            }
            Console.WriteLine($"错误的精度精度 {tick_size}, {quote_currency},{base_currency}");
            Console.WriteLine($"错误的精度精度 {tick_size}, {quote_currency},{base_currency}");
            Console.WriteLine($"错误的精度精度 {tick_size}, {quote_currency},{base_currency}");
            Console.WriteLine($"错误的精度精度 {tick_size}, {quote_currency},{base_currency}");
            Console.WriteLine($"错误的精度精度 {tick_size}, {quote_currency},{base_currency}");
            return 10;
        }

        public int GetSizeIncrementNumber()
        {
            if (quote_currency.ToLower() == "btc" && base_currency.ToLower() == "btt")
            {
                return 0;
            }
            if (quote_currency.ToLower() == "btc" && base_currency.ToLower() == "cai")
            {
                return 0;
            }

            if (size_increment == "1")
            {
                return 0;
            }
            if (size_increment == "0.1")
            {
                return 1;
            }
            if (size_increment == "0.01")
            {
                return 2;
            }
            if (size_increment == "0.001")
            {
                return 3;
            }
            if (size_increment == "0.0001")
            {
                return 4;
            }
            if (size_increment == "0.00001")
            {
                return 5;
            }
            if (size_increment == "0.000001")
            {
                return 6;
            }
            if (size_increment == "0.0000001")
            {
                return 7;
            }
            if (size_increment == "0.00000001")
            {
                return 8;
            }
            if (size_increment == "0.000000001")
            {
                return 9;
            }
            Console.WriteLine($"错误的精度精度size_increment: {size_increment}, {quote_currency},{base_currency}");
            Console.WriteLine($"错误的精度精度size_increment: {size_increment}, {quote_currency},{base_currency}");
            Console.WriteLine($"错误的精度精度size_increment: {size_increment}, {quote_currency},{base_currency}");
            Console.WriteLine($"错误的精度精度size_increment: {size_increment}, {quote_currency},{base_currency}");
            Console.WriteLine($"错误的精度精度size_increment: {size_increment}, {quote_currency},{base_currency}");
            return 10;
        }
    }

}
