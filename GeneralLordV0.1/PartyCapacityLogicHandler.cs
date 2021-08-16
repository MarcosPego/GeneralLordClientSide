using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment.Managers;
using TaleWorlds.Core;

namespace GeneralLord
{
    public class PartyCapacityLogicHandler
    {
        public static int BuyRenownPrice = 250;
        public static int RenownBought = 10;

        public static int TrainStewardship = 50;
        public static int StewardXP = 100;

        public static void HandleRenownBuy()
        {
            if (PartyBase.MainParty.LeaderHero.Gold - BuyRenownPrice < 0)
            {
                InformationManager.DisplayMessage(new InformationMessage("Not Enough Money To Buy Renown! "));
                return;
            }
            else
            {
                GiveGoldAction.ApplyBetweenCharacters(null, PartyBase.MainParty.LeaderHero, -BuyRenownPrice, false);
                GainRenownAction.Apply(PartyBase.MainParty.LeaderHero, RenownBought, false);
            }
        }

        public static void HandleTrainSteward()
        {
            if (PartyBase.MainParty.LeaderHero.Gold - TrainStewardship < 0)
            {
                InformationManager.DisplayMessage(new InformationMessage("Not Enough Money To Train Stewardship! "));
                return;
            }
            else
            {
                GiveGoldAction.ApplyBetweenCharacters(null, PartyBase.MainParty.LeaderHero, -TrainStewardship, false);
                PartyBase.MainParty.LeaderHero.HeroDeveloper.AddSkillXp(DefaultSkills.Steward, StewardXP, true, true);
            }


        }

        public static bool ShouldTrainBeAvailable()
        {
            if(PartyBase.MainParty.LeaderHero.Gold - TrainStewardship > 0)
            {
                return true;
            }

            return false;
        }

        public static bool ShouldRenownBuyBeAvailable()
        {
            if (PartyBase.MainParty.LeaderHero.Gold - BuyRenownPrice > 0)
            {
                return true;
            }
            return false;
        }

    }
}
