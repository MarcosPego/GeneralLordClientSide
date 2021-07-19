using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;

namespace GeneralLord.HarmonyOverrides
{
    //[HarmonyPatch(typeof(PartyCharacterVM))]
    //[HarmonyPatch("ExecuteTransferSingle")]
    public class PartyScreenTransferOverride
    {
        private static int GoldValue = 100;


        [HarmonyPatch(typeof(PartyCharacterVM))]
        [HarmonyPatch("ExecuteTransferSingle")]
        class TransferSingleOverride
        {
            static bool Prefix(PartyCharacterVM __instance)
            {
                if (__instance != null && PartyScreenState.currentState == PartyScreenStateEnum.RecruitmentScreen)
                {
                    //GiveGoldAction.ApplyBetweenCharacters(null, PartyBase.MainParty.LeaderHero, 100, false);
                    /*if (PartyBase.MainParty.LeaderHero.Gold +  (PartyScreenState.goldToChange - 100) < 0)
                    {
                        InformationManager.DisplayMessage(new InformationMessage("Not Enough Money to recruit more"));
                        return false;
                    }*/

                    return ValidateCommand(__instance);

                }
                else
                {
                    return true;
                }
            }
        }

        [HarmonyPatch(typeof(PartyCharacterVM))]
        [HarmonyPatch("RefreshValues")]
        class RefreshValuesOverride
        {
            static void Postfix(PartyCharacterVM __instance)
            {
                if (__instance != null && PartyScreenState.currentState == PartyScreenStateEnum.RecruitmentScreen)
                {
                    if(__instance.Side == PartyScreenLogic.PartyRosterSide.Left) __instance.Name = __instance.Name + "   Cost: " + (__instance.Character.Tier* GoldValue).ToString();
                }
            }
        }


        public static bool ValidateCommand(PartyCharacterVM __instance)
        {
            int troopAmount = 1;

            if (Input.IsKeyDown(InputKey.LeftShift))
            {
                troopAmount = Math.Min(5, __instance.Troop.Number);
            }

            if (Input.IsKeyDown(InputKey.LeftControl))
            {
                troopAmount = __instance.Troop.Number;
            }

            if (PartyBase.MainParty.LeaderHero.Gold + (PartyScreenState.goldToChange - CalculateGoldValue(__instance) * troopAmount) < 0)
            {
                int possibleValue = (PartyBase.MainParty.LeaderHero.Gold + PartyScreenState.goldToChange) / CalculateGoldValue(__instance);

                InformationManager.DisplayMessage(new InformationMessage("Not Enough Money to Recruit " + troopAmount.ToString() + " Troops; Max Possible: " + possibleValue.ToString()));
                return false;
            }

            PartyScreenState.goldToChange -= CalculateGoldValue(__instance) * troopAmount;

            int absoluteValue = Math.Abs(PartyScreenState.goldToChange);
            InformationManager.DisplayMessage(new InformationMessage("Current price to pay: " + absoluteValue.ToString()));
            return true;
        }

        public static int CalculateGoldValue(PartyCharacterVM __instance)
        {



            return __instance.Character.Tier * GoldValue;
        }

    }


        /*public static bool Prefix(ref TaleWorlds.CampaignSystem.ViewModelCollection.PartyCharacterVM __instance)
        {
            GiveGoldAction.ApplyBetweenCharacters(null, PartyBase.MainParty.LeaderHero, 100, false);
            InformationManager.DisplayMessage(new InformationMessage("Transfer"));
            return true;
        }*/

}
