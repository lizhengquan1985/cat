using DataDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoTrade
{
    public class AccountDetailsUtils
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

            var accounts = OkApi.ListAccounts();
            foreach(var account in accounts)
            {
                try
                {
                    var klines = OkApi.GetKLineDataAsync($"{account.currency}-usdt");
                    var nowPrice = klines[0].close;
                    if(account.balance * nowPrice > 50)
                    {
                        Console.WriteLine($"usdt {account.currency} --> {account.balance} --> usd{account.balance * nowPrice}  ");
                    }
                    continue;
                }
                catch(Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                }

                try
                {
                    var klines = OkApi.GetKLineDataAsync($"{account.currency}-btc");
                    var nowPrice = klines[0].close;
                    if (account.balance * nowPrice * btcPrice > 50)
                    {
                        Console.WriteLine($"btc {account.currency} --> {account.balance} --> usd{account.balance * nowPrice * btcPrice} ");
                    }
                    continue;
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                }

                try
                {
                    var klines = OkApi.GetKLineDataAsync($"{account.currency}-eth");
                    var nowPrice = klines[0].close;
                    if (account.balance * nowPrice * ethPrice > 50)
                    {
                        Console.WriteLine($"eth {account.currency} --> {account.balance} --> usd{account.balance * nowPrice * ethPrice}  ");
                    }
                    continue;
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                }

                try
                {
                    var klines = OkApi.GetKLineDataAsync($"{account.currency}-okb");
                    var nowPrice = klines[0].close;
                    if (account.balance * nowPrice * okbPrice > 50)
                    {
                        Console.WriteLine($"eth {account.currency} --> {account.balance} --> usd{account.balance * nowPrice * okbPrice}  ");
                    }
                    continue;
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                }

                Thread.Sleep(300);
            }

            Console.WriteLine(" end ShowDetail");
        }
    }
}
