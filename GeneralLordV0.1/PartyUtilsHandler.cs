
using GeneralLordWebApiClient.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace GeneralLord
{
    public class PartyUtilsHandler
    {

        public static TroopRoster GarrisonedTroops;

        public static WoundedTroopArmy WoundedTroopArmy = new WoundedTroopArmy();


        private static int totalCount = 0;
        private static double maxCount = 5000;
        private static TimeSpan timeLimit = TimeSpan.FromSeconds(60);
        private static DateTime lastIncrementTime;

        private Timer timer1;


        public static void TickForRecovery(MainManagerScreen mainManagerScreen)
        {
            totalCount++;
            if (totalCount > maxCount)
            {
                totalCount = 0;
                InformationManager.DisplayMessage(new InformationMessage(DateTime.Now.ToString()));
                mainManagerScreen._viewModel.MainOverview.RefreshValues();
                if (CheckIfFirstTimer()) RecoverTroopGroup();
            }
        }

        public static bool CheckIfFirstTimer()
        {
            if (WoundedTroopArmy.WoundedTroopsGroup.Any())
            {
                return WoundedTroopArmy.WoundedTroopsGroup.First().timeUntilRecovery <= DateTime.Now;
            }
            return false;
        }


        public static void RecoverTroopGroup()
        {
            WoundedTroopGroup woundedTroopGroupToRecover = WoundedTroopArmy.WoundedTroopsGroup.First();

            foreach(WoundedTroop wt  in woundedTroopGroupToRecover.woundedTroops)
            {
                PartyBase.MainParty.MemberRoster.WoundTroop(CharacterObject.Find(wt.stringId), -wt.troopCount);
            }

            WoundedTroopArmy.WoundedTroopsGroup.RemoveAt(0);

            JsonBattleConfig.ExecuteSubmitPartyUtils();
        }
    }
}
