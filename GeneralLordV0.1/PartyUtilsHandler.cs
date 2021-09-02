
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

                if (CheckIfFirstTimer()) RecoverTroopGroupReforged();
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

        public static void UpdateWoundedTroops()
        {
            WoundedTroopGroup woundedTroopGroup = new WoundedTroopGroup();
            woundedTroopGroup.timeUntilRecovery = DateTime.Now.AddMinutes(JsonBattleConfig.recoveryMinuteCooldown);

            foreach (TroopRosterElement tc in JsonBattleConfig.copyOfTroopRosterPreviousToBattle)
            {
                if (tc.Character.StringId != "main_hero")
                {
                    CharacterObject characterObject = CharacterObject.Find(tc.Character.StringId);

                    int healthyNumber = tc.Number - tc.WoundedNumber;

                    //InformationManager.DisplayMessage(new InformationMessage("Total Previous : " + tc.Number +" Healthy Previous : "+ healthyNumber + " Wounded Previous: " + tc.WoundedNumber));

                    if (PartyBase.MainParty.MemberRoster.FindIndexOfTroop(characterObject) == -1)
                    {
                        int troopsToRecover = (int)(healingRatio * healthyNumber);
                        int downedTroops = healthyNumber - troopsToRecover;


                        PartyBase.MainParty.AddMember(characterObject, troopsToRecover);
                        PartyBase.MainParty.AddMember(characterObject, downedTroops, downedTroops);

                        if (downedTroops > 0)
                        {
                            WoundedTroop woundedTroop = new WoundedTroop { stringId = tc.Character.StringId, troopCount = downedTroops };
                            woundedTroopGroup.woundedTroops.Add(woundedTroop);
                            woundedTroopGroup.totalWoundedTroops += downedTroops;
                        }

                        //InformationManager.DisplayMessage(new InformationMessage("Character" + characterObject.Name + " recovered and lost:" + troopsToRecover + "; " + downedTroops));
                    }
                    else
                    {
                        int index1 = PartyBase.MainParty.MemberRoster.FindIndexOfTroop(characterObject);

                        int numberThatWentToBattle = tc.Number - tc.WoundedNumber;

                        //int survingHealthySoldiers = PartyBase.MainParty.MemberRoster.GetElementNumber(characterObject) - PartyBase.MainParty.MemberRoster.GetElementWoundedNumber(index1);
                        int deadSoldiers = numberThatWentToBattle - PartyBase.MainParty.MemberRoster.GetElementNumber(characterObject);
                        int woundedSoldiers = PartyBase.MainParty.MemberRoster.GetElementWoundedNumber(index1) - tc.WoundedNumber;

                        //int dead = healthyNumber - (PartyBase.MainParty.MemberRoster.GetElementNumber(characterObject) - PartyBase.MainParty.MemberRoster.GetElementWoundedNumber(index1));

                        //InformationManager.DisplayMessage(new InformationMessage("Previous Wounded :" +  tc.WoundedNumber + " Wounded :" + woundedSoldiers + " Dead :" + deadSoldiers));

                        //InformationManager.DisplayMessage(new InformationMessage("Current Wounded :" + PartyBase.MainParty.MemberRoster.GetElementWoundedNumber(index1) + " Current Alive :" + PartyBase.MainParty.MemberRoster.GetElementNumber(characterObject)));
                        if ((deadSoldiers + woundedSoldiers) > 0)
                        {
                            int troopsToRecover = (int)(healingRatio * (deadSoldiers + woundedSoldiers));
                            int downedTroops = (deadSoldiers + woundedSoldiers) - troopsToRecover;

                            //PartyBase.MainParty.MemberRoster.WoundTroop(characterObject, -woundedSoldiers);
                            if (woundedSoldiers > 0) PartyBase.MainParty.MemberRoster.AddToCounts(characterObject, -woundedSoldiers, false, -woundedSoldiers, 0, true, -1);

                            //InformationManager.DisplayMessage(new InformationMessage("Post change --- Current Wounded :" + PartyBase.MainParty.MemberRoster.GetElementWoundedNumber(index1) + " Current Alive :" + PartyBase.MainParty.MemberRoster.GetElementNumber(characterObject)));

                            PartyBase.MainParty.AddMember(characterObject, troopsToRecover);
                            PartyBase.MainParty.AddMember(characterObject, downedTroops, downedTroops);

                            if (tc.Number != PartyBase.MainParty.MemberRoster.GetElementNumber(index1))
                            {
                                PartyBase.MainParty.AddMember(characterObject, tc.Number - PartyBase.MainParty.MemberRoster.GetElementNumber(index1));
                            }

                            if (downedTroops > 0)
                            {
                                WoundedTroop woundedTroop = new WoundedTroop { stringId = tc.Character.StringId, troopCount = downedTroops };
                                woundedTroopGroup.woundedTroops.Add(woundedTroop);
                                woundedTroopGroup.totalWoundedTroops += downedTroops;
                            }
                        }


                        //InformationManager.DisplayMessage(new InformationMessage("Character" + characterObject.Name + " recovered and lost:" + troopsToRecover+ "; " + downedTroops));
                    }
                }
            }

            if (woundedTroopGroup.totalWoundedTroops > 0) PartyUtilsHandler.WoundedTroopArmy.WoundedTroopsGroup.Add(woundedTroopGroup);
        }

        public static void OpenGarrisonRoster()
        {
            PartyScreenManager.OpenScreenAsQuest(GarrisonedTroops, new TextObject("{=}Garrisoned Troops", null),
                0, new PartyPresentationDoneButtonConditionDelegate(PartyScreenDoneCondition),
                new PartyScreenClosedDelegate(SellPrisonersDoneHandler),
                new PartyScreenLogic.IsTroopTransferableDelegate(RightTransferableDelegate), null);
        }

        public static void OpenWoundedRoster()
        {
            PartyScreenManager.OpenScreenAsQuest(WoundedTroops, new TextObject("{=}Wounded Troops", null),
                0, new PartyPresentationDoneButtonConditionDelegate(PartyScreenDoneCondition),
                new PartyScreenClosedDelegate(SellPrisonersDoneHandler),
                new PartyScreenLogic.IsTroopTransferableDelegate(NotTroopTransferableDelegate), null);
        }

        private static bool TroopTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase LeftOwnerParty)
        {
            return true;
        }

        private static bool NotTroopTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase LeftOwnerParty)
        {
            return false;
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


        public static void UpdateWoundedTroopsReforged()
        {
            WoundedTroopGroup woundedTroopGroup = new WoundedTroopGroup();
            woundedTroopGroup.timeUntilRecovery = DateTime.Now.AddMinutes(JsonBattleConfig.recoveryMinuteCooldown);

            foreach (TroopRosterElement tc in JsonBattleConfig.copyOfTroopRosterPreviousToBattle)
            {
                if (tc.Character.StringId != "main_hero")
                {
                    CharacterObject characterObject = CharacterObject.Find(tc.Character.StringId);
                    int indexInMemberRoster = PartyBase.MainParty.MemberRoster.FindIndexOfTroop(characterObject);
                    if (indexInMemberRoster == -1)
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
                    else
                    {
                        int deadSoldiers = tc.Number - PartyBase.MainParty.MemberRoster.GetElementNumber(characterObject);
                        int woundedSoldiers = PartyBase.MainParty.MemberRoster.GetElementWoundedNumber(indexInMemberRoster);

                        if ((deadSoldiers + woundedSoldiers) > 0)
                        {
                            int troopsToRecover = (int)(healingRatio * (deadSoldiers + woundedSoldiers));
                            int downedTroops = (deadSoldiers + woundedSoldiers) - troopsToRecover;

                            if (woundedSoldiers > 0) {
                                PartyBase.MainParty.MemberRoster.WoundTroop(characterObject, -woundedSoldiers);
                                PartyBase.MainParty.MemberRoster.RemoveTroop(characterObject, woundedSoldiers);
                            };
                            //if (woundedSoldiers > 0) PartyBase.MainParty.MemberRoster.AddToCounts(characterObject, -woundedSoldiers, false, -woundedSoldiers, 0, true, -1);

                            PartyBase.MainParty.AddMember(characterObject, troopsToRecover);


                            if (downedTroops > 0)
                            {
                                WoundedTroops.AddToCounts(characterObject, downedTroops, false, 0, 0, true, -1);
                                WoundedTroop woundedTroop = new WoundedTroop { stringId = tc.Character.StringId, troopCount = downedTroops };
                                woundedTroopGroup.woundedTroops.Add(woundedTroop);
                                woundedTroopGroup.totalWoundedTroops += downedTroops;
                            }
                        }
                    }

                   
                }

            }

            if (woundedTroopGroup.totalWoundedTroops > 0) PartyUtilsHandler.WoundedTroopArmy.WoundedTroopsGroup.Add(woundedTroopGroup);
        }


        public static void RecoverTroopGroupReforged()
        {
            WoundedTroopGroup woundedTroopGroupToRecover = WoundedTroopArmy.WoundedTroopsGroup.First();
            //InformationManager.DisplayMessage(new InformationMessage("Worked"));
            foreach (WoundedTroop wt in woundedTroopGroupToRecover.woundedTroops)
            {
                CharacterObject characterObject = CharacterObject.Find(wt.stringId);
                //Recover into main party
                //PartyBase.MainParty.AddMember(characterObject, wt.troopCount);
                //Recover into garrison
                GarrisonedTroops.AddToCounts(characterObject, wt.troopCount);

                WoundedTroops.RemoveTroop(characterObject, wt.troopCount);
                /*int id = PartyBase.MainParty.MemberRoster.FindIndexOfTroop(characterObject);
                if (id != -1)
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
                }*/


            }

            WoundedTroopArmy.WoundedTroopsGroup.RemoveAt(0);

            JsonBattleConfig.ExecuteSubmitPartyUtils();
            JsonBattleConfig.ExecuteSubmitProfileWithAc();
        }

        public static void TransferWoundedTroopArmyToRoster()
        {         
            foreach(WoundedTroopGroup woundedTroopGroupToRecover in WoundedTroopArmy.WoundedTroopsGroup)
            {
                foreach (WoundedTroop wt in woundedTroopGroupToRecover.woundedTroops)
                {
                    CharacterObject characterObject = CharacterObject.Find(wt.stringId);
                    WoundedTroops.AddToCounts(characterObject, wt.troopCount);
                    //WoundedTroops.AddMember(characterObject, wt.troopCount);
                }
            }

        }
    }
}
