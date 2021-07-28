using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;

namespace GeneralLord
{
    public class OpponentPartyHandler
    {
        public static MobileParty CurrentOpponentParty = null;
        public static TroopRoster PreBattleTroopRoster = null;
        public static int GoldToAdd = 0;

        public static int minBaseGold = 20;
        public static int maxBaseGold = 30;
        public static void RemoveOpponentParty()
        {
            if (CurrentOpponentParty != null)
            {
                if(CurrentOpponentParty.MemberRoster.TotalManCount > 0)
                CurrentOpponentParty.RemoveParty();
            }
            CurrentOpponentParty = null;
            PreBattleTroopRoster = null;
        }

        public static int VerifyGoldPerKilled()
        {

            int sum = 0;

            foreach (TroopRosterElement troop in PreBattleTroopRoster.GetTroopRoster())
            {
                var rand = new Random();
                if (CurrentOpponentParty.MemberRoster.Contains(troop.Character))
                {
                    int index = CurrentOpponentParty.MemberRoster.FindIndexOfTroop(troop.Character);

                    /*InformationManager.DisplayMessage(new InformationMessage("Total Before: " + troop.Number.ToString() + "  Total After: "+ 
                        CurrentOpponentParty.MemberRoster.GetElementNumber(index).ToString()
                        + "Wounded: " + CurrentOpponentParty.MemberRoster.GetElementWoundedNumber(index).ToString()));*/

                    int healthyLeft = CurrentOpponentParty.MemberRoster.GetElementNumber(index) - CurrentOpponentParty.MemberRoster.GetElementWoundedNumber(index);

                    sum += (troop.Number - healthyLeft) * troop.Character.Tier * rand.Next(minBaseGold, maxBaseGold);
                }
                else
                {
                    sum += troop.Number * troop.Character.Tier * rand.Next(minBaseGold, maxBaseGold);
                }
            }

            return sum;
        }

        public static void AddGoldToParty()
        {
            GiveGoldAction.ApplyBetweenCharacters(null, PartyBase.MainParty.LeaderHero, GoldToAdd, false);
            GoldToAdd = 0;
        }
    }


}
