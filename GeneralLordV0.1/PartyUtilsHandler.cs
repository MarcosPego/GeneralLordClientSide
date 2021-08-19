
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
        public static TroopRoster WoundedTroops = new TroopRoster(PartyBase.MainParty);


        public static WoundedTroopArmy WoundedTroopArmy = new WoundedTroopArmy();


        private static int totalCount = 3500;
        private static double maxCount = 5000;
        public static float healingRatio = 0.5f;


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
                CharacterObject characterObject = CharacterObject.Find(wt.stringId);

                int id = PartyBase.MainParty.MemberRoster.FindIndexOfTroop(characterObject);
                if(id != -1)
                {
                    int woundedNumber = PartyBase.MainParty.MemberRoster.GetElementWoundedNumber(id);
                    if (woundedNumber >= wt.troopCount)
                    {
                        PartyBase.MainParty.MemberRoster.WoundTroop(characterObject, -wt.troopCount);
                    }
                    else
                    {
                        PartyBase.MainParty.MemberRoster.WoundTroop(characterObject, -woundedNumber);
                    }
                }


            }

            WoundedTroopArmy.WoundedTroopsGroup.RemoveAt(0);

            JsonBattleConfig.ExecuteSubmitPartyUtils();
            JsonBattleConfig.ExecuteSubmitProfileWithAc();
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
        }

        public static bool RightTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase LeftOwnerParty)
        {
            //return side == PartyScreenLogic.PartyRosterSide.Left && type == PartyScreenLogic.TroopType.Member;
            return true;
        }


        public void UpdateWoundedTroopsReforged()
        {
            WoundedTroopGroup woundedTroopGroup = new WoundedTroopGroup();
            woundedTroopGroup.timeUntilRecovery = DateTime.Now.AddMinutes(JsonBattleConfig.recoveryMinuteCooldown);

            foreach (TroopRosterElement tc in JsonBattleConfig.copyOfTroopRosterPreviousToBattle)
            {
                if (tc.Character.StringId != "main_hero")
                {
                    CharacterObject characterObject = CharacterObject.Find(tc.Character.StringId);
                    if (PartyBase.MainParty.MemberRoster.FindIndexOfTroop(characterObject) == -1)
                    {
                        int troopsToRecover = (int)(healingRatio * tc.Number);
                        int downedTroops = tc.Number - troopsToRecover;


                        PartyBase.MainParty.AddMember(characterObject, troopsToRecover);

                        if (downedTroops > 0)
                        {
                            WoundedTroops.AddToCounts(characterObject, downedTroops, false, 0, 0, true, -1);
                            WoundedTroop woundedTroop = new WoundedTroop { stringId = tc.Character.StringId, troopCount = downedTroops };
                            woundedTroopGroup.woundedTroops.Add(woundedTroop);
                            woundedTroopGroup.totalWoundedTroops += downedTroops;
                        }

                        //InformationManager.DisplayMessage(new InformationMessage("Character" + characterObject.Name + " recovered and lost:" + troopsToRecover + "; " + downedTroops));
                    }


                }
            }
        }
    }
}
