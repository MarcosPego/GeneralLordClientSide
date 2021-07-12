using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralLordWebApiClient
{
    public class UrlHandler
    {

        public static string urlBase = "http://localhost:40519/values/";

        public static string PostBattleProcess = "postBattleProcess";
        public static string MultipleFromProfile = "multipleFromProfile";
        public static string SingleId = "singleId";
        public static string SingleFirst = "singleFirst";
        public static string SaveProfile = "save";
        public static string CalculateElo = "calculateElo";

        //public static string PostBattleProcess = "postBattleProcess";

        public static string GetUrlFromString(string target)
        {
            return Path.Combine(urlBase, target);
        }
    }
}
