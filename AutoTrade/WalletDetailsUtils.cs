using DataDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrade
{
    public class WalletDetailsUtils
    {
        public static void ShowDetail()
        {
            Console.WriteLine(" begin ShowDetail");
            var btcPrice = (decimal)0;
            var ethPrice = (decimal)0;
            var okbPrice = (decimal)0;
            List<CoinInfo> allCoinInfos = new List<CoinInfo>();
            try
            {
                allCoinInfos = new CoinInfoDao().ListCoinInfo();
                var btcKlines = OkApi.GetKLineDataAsync("btc-usdt");
                btcPrice = btcKlines[0].close;
                var ethKlines = OkApi.GetKLineDataAsync("eth-usdt");
                ethPrice = ethKlines[0].close;
                var okbKlines = OkApi.GetKLineDataAsync("okb-usdt");
                okbPrice = okbKlines[0].close;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            var wallets = OkApi.ListWallets();
            foreach(var wallet in wallets)
            {
                try
                {
                    var klines = OkApi.GetKLineDataAsync($"{wallet.currency}-usdt");
                    var nowPrice = klines[0].close;
                    if(wallet.balance * nowPrice > 5)
                    {
                        Console.WriteLine($"{wallet.currency} --> {wallet.balance} ");
                    }
                    continue;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                try
                {
                    var klines = OkApi.GetKLineDataAsync($"{wallet.currency}-btc");
                    var nowPrice = klines[0].close;
                    if (wallet.balance * nowPrice * btcPrice > 5)
                    {
                        Console.WriteLine($"btc {wallet.currency} --> {wallet.balance} ");
                    }
                    continue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                try
                {
                    var klines = OkApi.GetKLineDataAsync($"{wallet.currency}-eth");
                    var nowPrice = klines[0].close;
                    if (wallet.balance * nowPrice * ethPrice > 5)
                    {
                        Console.WriteLine($"eth {wallet.currency} --> {wallet.balance} ");
                    }
                    continue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                try
                {
                    var klines = OkApi.GetKLineDataAsync($"{wallet.currency}-okb");
                    var nowPrice = klines[0].close;
                    if (wallet.balance * nowPrice * okbPrice > 5)
                    {
                        Console.WriteLine($"eth {wallet.currency} --> {wallet.balance} ");
                    }
                    continue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine(" end ShowDetail");
        }
    }
}
