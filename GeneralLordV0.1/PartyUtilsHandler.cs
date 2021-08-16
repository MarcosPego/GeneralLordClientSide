
using GeneralLordWebApiClient.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace GeneralLord
{
    public class PartyUtilsHandler
    {

        public static TroopRoster GarrisonedTroops = new TroopRoster(PartyBase.MainParty);

        public static WoundedTroopArmy WoundedTroopArmy = new WoundedTroopArmy();


        private static int totalCount = 3500;
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
                //InformationManager.DisplayMessage(new InformationMessage(DateTime.Now.ToString()));

                if (CheckIfFirstTimer()) RecoverTroopGroup();
                mainManagerScreen._viewModel.MainOverview.RefreshValues();
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
            //InformationManager.DisplayMessage(new InformationMessage("Worked"));
            foreach (WoundedTroop wt  in woundedTroopGroupToRecover.woundedTroops)
            {
                PartyBase.MainParty.MemberRoster.WoundTroop(CharacterObject.Find(wt.stringId), -wt.troopCount);
            }

            WoundedTroopArmy.WoundedTroopsGroup.RemoveAt(0);

            JsonBattleConfig.ExecuteSubmitPartyUtils();
            JsonBattleConfig.ExecuteSubmitAc();
        }



        public static void OpenGarrisonRoster()
        {
            PartyScreenManager.OpenScreenAsQuest(GarrisonedTroops, new TextObject("{=}Garrisoned Troops", null),
                0, new PartyPresentationDoneButtonConditionDelegate(PartyScreenDoneCondition),
                new PartyScreenClosedDelegate(SellPrisonersDoneHandler),
                new PartyScreenLogic.IsTroopTransferableDelegate(RightTransferableDelegate), null);
        }

        private static bool TroopTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase LeftOwnerParty)
        {
            return true;
        }

        private static bool PartyScreenDoneClicked(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, FlattenedTroopRoster takenPrisonerRoster, FlattenedTroopRoster releasedPrisonerRoster, bool isForced, List<MobileParty> leftParties, List<MobileParty> rigthParties)
        {
            return true;
        }

        private static Tuple<bool, TextObject> PartyScreenDoneCondition(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, int leftLimitNum, int rightLimitNum)
        {
            return new Tuple<bool, TextObject>(true, new TextObject("", null));
        }

        private static void SellPrisonersDoneHandler(PartyBase leftOwnerParty, TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, PartyBase rightOwnerParty, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster)
        {
            //SellPrisonersAction.ApplyForSelectedPrisoners(MobileParty.MainParty, leftPrisonRoster, Hero.MainHero.CurrentSettlement);

            //GiveGoldAction.ApplyBetweenCharacters(null, PartyBase.MainParty.LeaderHero, 100, false);
            //return true;
        }

        public static bool RightTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase LeftOwnerParty)
        {
            //return side == PartyScreenLogic.PartyRosterSide.Left && type == PartyScreenLogic.TroopType.Member;
            return true;
        }
    }
}
