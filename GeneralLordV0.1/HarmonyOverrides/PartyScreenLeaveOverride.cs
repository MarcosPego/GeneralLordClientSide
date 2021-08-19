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

namespace GeneralLord.HarmonyOverrides
{
    public class PartyScreenLeaveOverride
    {


        [HarmonyPatch(typeof(PartyVM))]
        [HarmonyPatch("ExecuteCancel")]
        class CloseOverride
        {
            static void Postfix(PartyVM __instance)
            {

                //sInformationManager.DisplayMessage(new InformationMessage("Close"));

                if (__instance != null && PartyScreenState.currentState == PartyScreenStateEnum.RecruitmentScreen)
                {

                    PartyScreenState.goldToChange = 0;
                    PartyScreenState.currentState = PartyScreenStateEnum.NormalScreen;
                } else if (__instance != null && PartyScreenState.currentState == PartyScreenStateEnum.GarrisonScreen)
                {
                    PartyScreenState.currentState = PartyScreenStateEnum.NormalScreen;

                }
                else
                {
                    return;
                }
            }

            [HarmonyPatch(typeof(PartyVM))]
            [HarmonyPatch("ExecuteDone")]
            class DoneOverride
            {
                static void Postfix(PartyVM __instance)
                {

                    //InformationManager.DisplayMessage(new InformationMessage("Done"));

                    if (__instance != null && PartyScreenState.currentState == PartyScreenStateEnum.RecruitmentScreen)
                    {
                        JsonBattleConfig.ExecuteSubmitProfileWithAc();

                        GiveGoldAction.ApplyBetweenCharacters(null, PartyBase.MainParty.LeaderHero, PartyScreenState.goldToChange, false);
                        PartyScreenState.goldToChange = 0;
                        PartyScreenState.currentState = PartyScreenStateEnum.NormalScreen;
                    }
                    else if (__instance != null && PartyScreenState.currentState == PartyScreenStateEnum.GarrisonScreen)
                    {
                        PartyScreenState.currentState = PartyScreenStateEnum.NormalScreen;

                        JsonBattleConfig.ExecuteSubmitPartyUtils();
                        JsonBattleConfig.ExecuteSubmitProfileWithAc();
                    }
                    else
                    {
                        return;
                    }
                }
            }


            [HarmonyPatch(typeof(PartyVM))]
            [HarmonyPatch("ExecuteReset")]
            class ResetOverride
            {
                static void Postfix(PartyVM __instance)
                {

                    

                    if (__instance != null && PartyScreenState.currentState == PartyScreenStateEnum.RecruitmentScreen)
                    {

                        PartyScreenState.goldToChange = 0;
                        InformationManager.DisplayMessage(new InformationMessage("Reset; Current price to pay: " + PartyScreenState.goldToChange.ToString()));
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
    }
}
