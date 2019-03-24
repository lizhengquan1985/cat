using log4net;
using MySql.Data.MySqlClient;
using SharpDapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDao
{
    public class BaseDao
    {
        protected ILog logger = LogManager.GetLogger(typeof(BaseDao));

        public string LikeStr(string str)
        {
            StringBuilder sb = new StringBuilder(str);
            sb.Replace("'", "''");
            sb.Insert(0, "%", 1);
            sb.Append("%");
            sb.Replace(@"\", @"\\");

            return sb.ToString();
        }

        protected IDapperConnection Database { get; private set; }
        public BaseDao()
        {
            string connectionString = "server=localhost;port=3306;user id=root; password=lyx123456; database=okdata; pooling=true; charset=utf8mb4";
            var connection = new MySqlConnection(connectionString);
            Database = new DapperConnection(connection);
        }

        public string GetStateStringIn(List<string> stateList)
        {
            // List<string> stateList = new List<string>() { StateConst.PartialCanceled, StateConst.Filled };
            var states = "";
            stateList.ForEach(it =>
            {
                if (states != "")
                {
                    states += ",";
                }
                states += $"'{it}'";
            });
            return states;
        }
    }
}
