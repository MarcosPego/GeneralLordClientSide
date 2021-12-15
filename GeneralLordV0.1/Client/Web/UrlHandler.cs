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

        public static string urlBase = "http://localhost:*****/values/";

        public static string GetMatchHistory = "getMatchHistoryRecord";
        public static string GetPartyUtils = "getPartyUtils";
        public static string GetRankingProfiles = "getRankingProfiles";
        public static string PostBattleProcess = "postBattleProcess";
        public static string MultipleFromProfile = "multipleFromProfile";
        public static string SingleId = "singleId";
        public static string SingleFirst = "singleFirst";
        public static string SaveProfile = "save";
        public static string SubmitPartyUtils = "submitPartyUtils";
        public static string CalculateElo = "calculateElo";
        public static string IsCurrentVersion = "isCorrectVersion";

        //public static string PostBattleProcess = "postBattleProcess";

        public static void ReleaseVersion(bool isRelease)
        {
            if (isRelease)
            {
                urlBase = "http://192.***.***.**/values";
            }
            else
            {
                urlBase = "http://localhost:*****/values/";
            }
        }

        public static string GetUrlFromString(string target)
        {
            return Path.Combine(urlBase, target);
        }
    }
}
