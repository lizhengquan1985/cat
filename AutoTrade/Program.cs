using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrade
{
    class Program
    {
        static Dictionary<string, DateTime> nextDate = new Dictionary<string, DateTime>();

        static Dictionary<string, string> instrument = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            // 初始化币种
            instrument.Add("eth", "xrp");

            // 获取行情，
            // 读取数据库 看看以前的交易
            // 判断是否交易。
            // 标记下一次读取时间
        }
    }
}
