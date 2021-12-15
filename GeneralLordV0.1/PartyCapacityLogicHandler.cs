using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment.Managers;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;

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

            int trainValue = BuyRenownPrice;
            int trainXp = RenownBought;

            if (Input.IsKeyDown(InputKey.LeftShift))
            {
                trainValue *= 10;
                trainXp *= 10;
            }


            if (PartyBase.MainParty.LeaderHero.Gold - trainValue < 0)
            {

                if (trainValue == BuyRenownPrice)
                {
                    InformationManager.DisplayMessage(new InformationMessage("Not Enough Money To Buy Renown 1 time! "));
                }
                else
                {
                    InformationManager.DisplayMessage(new InformationMessage("Not Enough Money To Buy Renown 10 times! "));
                }

                //InformationManager.DisplayMessage(new InformationMessage("Not Enough Money To Buy Renown! "));
                return;
            }
            else
            {
                GiveGoldAction.ApplyBetweenCharacters(null, PartyBase.MainParty.LeaderHero, -trainValue, false);
                GainRenownAction.Apply(PartyBase.MainParty.LeaderHero, trainXp, false);
            }
        }

        public static void HandleTrainSteward()
        {

            int trainValue = TrainStewardship;
            int trainXp = StewardXP;

            if (Input.IsKeyDown(InputKey.LeftShift))
            {
                trainValue *= 10;
                trainXp *= 10;
            }

            if (PartyBase.MainParty.LeaderHero.Gold - trainValue < 0)
            {
                if (trainValue == TrainStewardship){
                    InformationManager.DisplayMessage(new InformationMessage("Not Enough Money To Train Stewardship 1 time! "));
                } else
                {
                    InformationManager.DisplayMessage(new InformationMessage("Not Enough Money To Train Stewardship 10 times! "));
                }
                
                return;
            }
            else
            {
                GiveGoldAction.ApplyBetweenCharacters(null, PartyBase.MainParty.LeaderHero, -trainValue, false);
                PartyBase.MainParty.LeaderHero.HeroDeveloper.AddSkillXp(DefaultSkills.Steward, trainXp, true, true);
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
