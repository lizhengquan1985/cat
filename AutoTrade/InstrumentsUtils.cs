﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
            Console.WriteLine("btc count -- > " + instruments.Count);
            InitEth();

            Console.WriteLine($"总共这个多个交易对： {instruments.Count}");
        }

        public static void InitBtc()
        {
            instruments.Add(new TradeItem { quote = "btc", symbol = "ltc", MaxBuyPrice = (decimal)0.03 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "eth", MaxBuyPrice = (decimal)0.08 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "okb", MaxBuyPrice = (decimal)0 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "etc", MaxBuyPrice = (decimal)0.003 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "bch", MaxBuyPrice = (decimal)0.1 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "eos", MaxBuyPrice = (decimal)0.0028 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "xrp", MaxBuyPrice = (decimal)0.0002 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "trx", MaxBuyPrice = (decimal)0.00001 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "bsv", MaxBuyPrice = (decimal)0.03 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "dash", MaxBuyPrice = (decimal)0.05 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "neo", MaxBuyPrice = (decimal)0.004 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "qtum", MaxBuyPrice = (decimal)0.0014 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "xlm", MaxBuyPrice = (decimal)0.00006 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "ada", MaxBuyPrice = (decimal)0.0000361 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "aac", MaxBuyPrice = (decimal)0.000001820 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "abl", MaxBuyPrice = (decimal)0.00001 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "abt", MaxBuyPrice = (decimal)0.0000950 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "ace", MaxBuyPrice = (decimal)0.00000813 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "act", MaxBuyPrice = (decimal)0.0000081 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "ae", MaxBuyPrice = (decimal)0.0003 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "ardr", MaxBuyPrice = (decimal)0.00005 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "ark", MaxBuyPrice = (decimal)0 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "bcd", MaxBuyPrice = (decimal)0 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "bec", MaxBuyPrice = (decimal)0 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "bkx", MaxBuyPrice = (decimal)0 });

            //instruments.Add(new TradeItem { quote = "btc", symbol = "bnt", MaxBuyPrice = (decimal)0 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "btg", MaxBuyPrice = (decimal)0 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "btm", MaxBuyPrice = (decimal)0.00006 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "btt", MaxBuyPrice = (decimal)0.0000004 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "cai", MaxBuyPrice = (decimal)0.00000005 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "chat", MaxBuyPrice = (decimal)0.000002 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "cic", MaxBuyPrice = (decimal)0 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "cmt", MaxBuyPrice = (decimal)0.000019 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "ctxc", MaxBuyPrice = (decimal)0.00009 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "cvc", MaxBuyPrice = (decimal)0.00005 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "cvt", MaxBuyPrice = (decimal)0.00000865 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "dadi", MaxBuyPrice = (decimal)0 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "dcr", MaxBuyPrice = (decimal)0 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "dgb", MaxBuyPrice = (decimal)0 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "dgd", MaxBuyPrice = (decimal)0 });

            //instruments.Add(new TradeItem { quote = "btc", symbol = "dpy", MaxBuyPrice = (decimal)0 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "edo", MaxBuyPrice = (decimal)0.000349 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "egt", MaxBuyPrice = (decimal)0 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "elf", MaxBuyPrice = (decimal)0.0001 }); // ok
            instruments.Add(new TradeItem { quote = "btc", symbol = "gas", MaxBuyPrice = (decimal)0.0018 }); // ok

            instruments.Add(new TradeItem { quote = "btc", symbol = "gnt", MaxBuyPrice = (decimal)0.000042 }); // ok
            //instruments.Add(new TradeItem { quote = "btc", symbol = "gnx", MaxBuyPrice = (decimal)0 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "gto", MaxBuyPrice = (decimal)0 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "gusd", MaxBuyPrice = (decimal)0 });  稳定币
            instruments.Add(new TradeItem { quote = "btc", symbol = "hc", MaxBuyPrice = (decimal)0.0008 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "hmc", MaxBuyPrice = (decimal)0.00000180 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "hpb", MaxBuyPrice = (decimal)0 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "hyc", MaxBuyPrice = (decimal)0 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "icx", MaxBuyPrice = (decimal)0.00019 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "ins", MaxBuyPrice = (decimal)0.00019 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "insur", MaxBuyPrice = (decimal)0.0000000911 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "int", MaxBuyPrice = (decimal)0.000012 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "iost", MaxBuyPrice = (decimal)0.000007 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "iota", MaxBuyPrice = (decimal)0.0002 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "itc", MaxBuyPrice = (decimal)0.0001 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "kan", MaxBuyPrice = (decimal)0.0000022 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "kcash", MaxBuyPrice = (decimal)0.000006 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "knc", MaxBuyPrice = (decimal)0 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "lba", MaxBuyPrice = (decimal)0.00001897 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "let", MaxBuyPrice = (decimal)0.0000026 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "light", MaxBuyPrice = (decimal)0.0000000171 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "link", MaxBuyPrice = (decimal)0.0003 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "lrc", MaxBuyPrice = (decimal)0.00003668 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "lsk", MaxBuyPrice = (decimal)0.001 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "mana", MaxBuyPrice = (decimal)0.000029 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "mco", MaxBuyPrice = (decimal)0.0019 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "mdt", MaxBuyPrice = (decimal)0 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "mith", MaxBuyPrice = (decimal)0.000022 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "mkr", MaxBuyPrice = (decimal)0.3 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "mof", MaxBuyPrice = (decimal)0.00003 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "nano", MaxBuyPrice = (decimal)0.0005 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "nas", MaxBuyPrice = (decimal)0.0005 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "nuls", MaxBuyPrice = (decimal)0.0005 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "nxt", MaxBuyPrice = (decimal)0.00001763 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "of", MaxBuyPrice = (decimal)0.0000001208 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "omg", MaxBuyPrice = (decimal)0.001 }); // ok
            instruments.Add(new TradeItem { quote = "btc", symbol = "ont", MaxBuyPrice = (decimal)0.0008 }); // ok
            //instruments.Add(new TradeItem { quote = "btc", symbol = "ors", MaxBuyPrice = (decimal)0 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "pax", MaxBuyPrice = (decimal)0 });  稳定币
            instruments.Add(new TradeItem { quote = "btc", symbol = "pay", MaxBuyPrice = (decimal)0.0001738 });

            //instruments.Add(new TradeItem { quote = "btc", symbol = "pst", MaxBuyPrice = (decimal)0 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "qun", MaxBuyPrice = (decimal)0 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "r", MaxBuyPrice = (decimal)0 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "rfr", MaxBuyPrice = (decimal)0 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "sbtc", MaxBuyPrice = (decimal)0 });

            //instruments.Add(new TradeItem { quote = "btc", symbol = "sc", MaxBuyPrice = (decimal)0 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "snc", MaxBuyPrice = (decimal)0 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "snt", MaxBuyPrice = (decimal)0.00001606 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "soc", MaxBuyPrice = (decimal)0.00000363 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "ssc", MaxBuyPrice = (decimal)0 });

            //instruments.Add(new TradeItem { quote = "btc", symbol = "stc", MaxBuyPrice = (decimal)0 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "swftc", MaxBuyPrice = (decimal)0.000000827 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "tct", MaxBuyPrice = (decimal)0 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "theta", MaxBuyPrice = (decimal)0.00006 }); // ok
            //instruments.Add(new TradeItem { quote = "btc", symbol = "trio", MaxBuyPrice = (decimal)0 });

            //instruments.Add(new TradeItem { quote = "btc", symbol = "true", MaxBuyPrice = (decimal)0 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "tusd", MaxBuyPrice = (decimal)0 });  稳定币
            instruments.Add(new TradeItem { quote = "btc", symbol = "ugc", MaxBuyPrice = (decimal)0.000001710 });
            //instruments.Add(new TradeItem { quote = "btc", symbol = "usdc", MaxBuyPrice = (decimal)0 });  稳定币
            instruments.Add(new TradeItem { quote = "btc", symbol = "vib", MaxBuyPrice = (decimal)0.00001835 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "vite", MaxBuyPrice = (decimal)0.00001240 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "waves", MaxBuyPrice = (decimal)0.0018 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "win", MaxBuyPrice = (decimal)0.0000002171 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "wtc", MaxBuyPrice = (decimal)0.000900 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "xas", MaxBuyPrice = (decimal)0.00007097 });

            instruments.Add(new TradeItem { quote = "btc", symbol = "xem", MaxBuyPrice = (decimal)0.00003 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "xmr", MaxBuyPrice = (decimal)0.03 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "you", MaxBuyPrice = (decimal)0.00000723 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "zco", MaxBuyPrice = (decimal)0.00000365 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "zec", MaxBuyPrice = (decimal)0.03 });

            //instruments.Add(new TradeItem { quote = "btc", symbol = "zen", MaxBuyPrice = (decimal)0 }); // 交易量很少 2万人民币
            instruments.Add(new TradeItem { quote = "btc", symbol = "zil", MaxBuyPrice = (decimal)0.000012 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "zip", MaxBuyPrice = (decimal)0.000000362 });
            instruments.Add(new TradeItem { quote = "btc", symbol = "zrx", MaxBuyPrice = (decimal)0.00017776 });

        }

        public static void InitEth()
        {
            instruments.Add(new TradeItem { quote = "eth", symbol = "ltc", MaxBuyPrice = (decimal)0.9 });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "okb", MaxBuyPrice = (decimal)0.9 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "etc", MaxBuyPrice = (decimal)0.1 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "eos", MaxBuyPrice = (decimal)0.062 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "xrp", MaxBuyPrice = (decimal)0.005 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "trx", MaxBuyPrice = (decimal)0.0004 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "dash", MaxBuyPrice = (decimal)2 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "neo", MaxBuyPrice = (decimal)0.2 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "qtum", MaxBuyPrice = (decimal)0.05 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "xlm", MaxBuyPrice = (decimal)0.002 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "ada", MaxBuyPrice = (decimal)0.001 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "aac", MaxBuyPrice = (decimal)0.00005240 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "abl", MaxBuyPrice = (decimal)0.00022374 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "abt", MaxBuyPrice = (decimal)0.003 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "act", MaxBuyPrice = (decimal)0.0002200 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "ae", MaxBuyPrice = (decimal)0.01 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "ark", MaxBuyPrice = (decimal)0.01 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "auto", MaxBuyPrice = (decimal)0.00004919 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "bec", MaxBuyPrice = (decimal)0.001190 });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "bkx", MaxBuyPrice = (decimal)0 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "bnt", MaxBuyPrice = (decimal)0.01 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "btm", MaxBuyPrice = (decimal)0.002 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "btt", MaxBuyPrice = (decimal)0.000012 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "cai", MaxBuyPrice = (decimal)0.000001701 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "cic", MaxBuyPrice = (decimal)0.00000993 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "cmt", MaxBuyPrice = (decimal)0.00051 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "ctxc", MaxBuyPrice = (decimal)0.0024 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "cvc", MaxBuyPrice = (decimal)0.0012 });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "dadi", MaxBuyPrice = (decimal)0 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "dcr", MaxBuyPrice = (decimal)0.3 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "dgb", MaxBuyPrice = (decimal)0 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "dgd", MaxBuyPrice = (decimal)0 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "dpy", MaxBuyPrice = (decimal)0.001259 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "edo", MaxBuyPrice = (decimal)0.01 });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "egt", MaxBuyPrice = (decimal)0 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "elf", MaxBuyPrice = (decimal)0.003 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "fair", MaxBuyPrice = (decimal)0.00005 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "gas", MaxBuyPrice = (decimal)0.05 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "gnt", MaxBuyPrice = (decimal)0.0013 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "gnx", MaxBuyPrice = (decimal)0.00031 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "gto", MaxBuyPrice = (decimal)0.000411 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "hc", MaxBuyPrice = (decimal)0.02 });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "hmc", MaxBuyPrice = (decimal)0 });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "hpb", MaxBuyPrice = (decimal)0 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "hyc", MaxBuyPrice = (decimal)0.0000983 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "icx", MaxBuyPrice = (decimal)0.0041 });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "ins", MaxBuyPrice = (decimal)0 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "insur", MaxBuyPrice = (decimal)0.00000320 });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "int", MaxBuyPrice = (decimal)0 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "iost", MaxBuyPrice = (decimal)0.00014 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "iota", MaxBuyPrice = (decimal)0.005 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "itc", MaxBuyPrice = (decimal)0.003 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "kan", MaxBuyPrice = (decimal)0.00007 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "kcash", MaxBuyPrice = (decimal)0.00018 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "lba", MaxBuyPrice = (decimal)0.0005 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "let", MaxBuyPrice = (decimal)0.000081 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "light", MaxBuyPrice = (decimal)0.000000510 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "link", MaxBuyPrice = (decimal)0.0061 });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "lrc", MaxBuyPrice = (decimal)0 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "lsk", MaxBuyPrice = (decimal)0.03 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "mana", MaxBuyPrice = (decimal)0.00071 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "mco", MaxBuyPrice = (decimal)0.06 });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "mdt", MaxBuyPrice = (decimal)0 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "mith", MaxBuyPrice = (decimal)0.000837 });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "mkr", MaxBuyPrice = (decimal)0 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "mof", MaxBuyPrice = (decimal)0.0008 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "mvp", MaxBuyPrice = (decimal)0.000008 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "nano", MaxBuyPrice = (decimal)0.019 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "nas", MaxBuyPrice = (decimal)0.015 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "nuls", MaxBuyPrice = (decimal)0.011 });

            //instruments.Add(new TradeItem { quote = "eth", symbol = "of", MaxBuyPrice = (decimal)0 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "omg", MaxBuyPrice = (decimal)0.03 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "ont", MaxBuyPrice = (decimal)0.021 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "ors", MaxBuyPrice = (decimal)0.0001203 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "pay", MaxBuyPrice = (decimal)0.0041 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "ppt", MaxBuyPrice = (decimal)0.02 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "pra", MaxBuyPrice = (decimal)0.000365 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "pst", MaxBuyPrice = (decimal)0.001963 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "qun", MaxBuyPrice = (decimal)0.0000887 });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "r", MaxBuyPrice = (decimal)0 });

            //instruments.Add(new TradeItem { quote = "eth", symbol = "rfr", MaxBuyPrice = (decimal)0 });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "rnt", MaxBuyPrice = (decimal)0 });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "sc", MaxBuyPrice = (decimal)0 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "sda", MaxBuyPrice = (decimal)0.0000913 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "snc", MaxBuyPrice = (decimal)0.000412 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "snt", MaxBuyPrice = (decimal)0.00031 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "soc", MaxBuyPrice = (decimal)0.0000931 });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "ssc" });
            instruments.Add(new TradeItem { quote = "eth", symbol = "storj", MaxBuyPrice = (decimal)0.0041 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "swftc", MaxBuyPrice = (decimal)0.000021 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "theta", MaxBuyPrice = (decimal)0.0015 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "topc", MaxBuyPrice = (decimal)0 });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "tra", MaxBuyPrice = (decimal)0 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "trio", MaxBuyPrice = (decimal)0.0000373 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "true", MaxBuyPrice = (decimal)0.00746 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "uct", MaxBuyPrice = (decimal)0.0001109 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "ugc", MaxBuyPrice = (decimal)0.00005091 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "vib", MaxBuyPrice = (decimal)0.000549 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "vite", MaxBuyPrice = (decimal)0.000368 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "waves", MaxBuyPrice = (decimal)0.04 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "win", MaxBuyPrice = (decimal)0.000008 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "wtc", MaxBuyPrice = (decimal)0.03251 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "xem", MaxBuyPrice = (decimal)0.00071 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "xmr", MaxBuyPrice = (decimal)1 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "xuc", MaxBuyPrice = (decimal)0.04 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "you", MaxBuyPrice = (decimal)0.0001958 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "zco", MaxBuyPrice = (decimal)0.00009 });
            instruments.Add(new TradeItem { quote = "eth", symbol = "zec", MaxBuyPrice = (decimal)0.9 });
            //instruments.Add(new TradeItem { quote = "eth", symbol = "zen", MaxBuyPrice = (decimal)0 });  成交太少
            instruments.Add(new TradeItem { quote = "eth", symbol = "zil", MaxBuyPrice = (decimal)0.00031 });

            instruments.Add(new TradeItem { quote = "eth", symbol = "zip", MaxBuyPrice = (decimal)0.000009 });
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
    }


    public class TradeItem
    {
        public string quote { get; set; }
        public string symbol { get; set; }
        /// <summary>
        /// 最大的购入价格
        /// </summary>
        public decimal MaxBuyPrice { get; set; }
    }

    public class instruments
    {
        public string instrument_id { get; set; }
        public string base_currency { get; set; }
        public string quote_currency { get; set; }
        /// <summary>
        /// 	最小交易数量
        /// </summary>
        public string min_size { get; set; }
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
