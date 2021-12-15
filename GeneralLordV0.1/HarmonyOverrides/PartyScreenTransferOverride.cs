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
using TaleWorlds.Localization;

namespace GeneralLord.HarmonyOverrides
{
    //[HarmonyPatch(typeof(PartyCharacterVM))]
    //[HarmonyPatch("ExecuteTransferSingle")]
    public class PartyScreenTransferOverride
    {
        private static int GoldValue = 100;

        private static List<string> NobleTroops = new List<string>
        {
            "imperial_vigla_recruit",
            "sturgian_warrior_son",
            "vlandian_squire",
            "aserai_youth",
            "battanian_highborn_youth",
            "khuzait_noble_son",
            "karakhuzaits_tier_1",
            "beni_zilal_tier_1",
            "ghilman_tier_1",  
            "skolderbrotva_tier_1"
        };


        [HarmonyPatch(typeof(PartyCharacterVM))]
        [HarmonyPatch("ExecuteTransferSingle")]
        class TransferSingleOverride
        {
            static bool Prefix(PartyCharacterVM __instance, ref PartyVM ____partyVm)
            {
                if (__instance != null && PartyScreenState.currentState == PartyScreenStateEnum.RecruitmentScreen)
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

                    if (troopAmount > PartyBase.MainParty.PartySizeLimit - PartyBase.MainParty.MemberRoster.TotalManCount)
                    {
                        InformationManager.DisplayMessage(new InformationMessage("Can't go over party capacity! Consider putting some troops in the garrison first!"));
                        return false;
                    }

                    if (PartyBase.MainParty.LeaderHero.Gold + (____partyVm.PartyScreenLogic.CurrentData.PartyGoldChangeAmount - CalculateGoldValue(__instance) * troopAmount) < 0)
                    {
                        int possibleValue = (PartyBase.MainParty.LeaderHero.Gold + PartyScreenState.goldToChange) / CalculateGoldValue(__instance);

                        InformationManager.DisplayMessage(new InformationMessage("Not Enough Money to Recruit " + troopAmount.ToString() + " Troops; Max Possible: " + possibleValue.ToString()));
                        return false;
                    }

                    //PartyScreenState.goldToChange -= CalculateGoldValue(__instance) * troopAmount;
                    ____partyVm.PartyScreenLogic.CurrentData.PartyGoldChangeAmount -= CalculateGoldValue(__instance) * troopAmount;

                    MBTextManager.SetTextVariable("PAY_OR_GET", (____partyVm.PartyScreenLogic.CurrentData.PartyGoldChangeAmount > 0) ? 1 : 0);
                    MBTextManager.SetTextVariable("TRADE_AMOUNT", Math.Abs(____partyVm.PartyScreenLogic.CurrentData.PartyGoldChangeAmount));
                    ____partyVm.GoldChangeText = ((____partyVm.PartyScreenLogic.CurrentData.PartyGoldChangeAmount == 0) ? "" : GameTexts.FindText("str_inventory_trade_label", null).ToString());
                    //int absoluteValue = Math.Abs(PartyScreenState.goldToChange);
                    //InformationManager.DisplayMessage(new InformationMessage("Current price to pay: " + absoluteValue.ToString()));
                    return true;

                }
                else if (__instance != null && PartyScreenState.currentState == PartyScreenStateEnum.GarrisonScreen)
                {
                    //InformationManager.DisplayMessage(new InformationMessage("Right Side"));


                    int availableToTransfer = __instance.Troop.Number - __instance.Troop.WoundedNumber;

                    int troopAmount = 1;

                    if (availableToTransfer == 0)
                    {
                        troopAmount = 0;

                    }

                    if (Input.IsKeyDown(InputKey.LeftShift))
                    {
                        troopAmount = Math.Min(5, availableToTransfer);
                    }

                    if (Input.IsKeyDown(InputKey.LeftControl))
                    {
                        troopAmount = availableToTransfer;
                    }

                    if (__instance.Side == PartyScreenLogic.PartyRosterSide.Left && troopAmount > PartyBase.MainParty.PartySizeLimit - PartyBase.MainParty.MemberRoster.TotalManCount)
                    {
                        InformationManager.DisplayMessage(new InformationMessage("Can't go over party capacity! Consider putting some troops in the garrison first!"));
                        return false;
                    }

                    __instance.OnTransfer(__instance, -1, troopAmount, __instance.Side);
                    __instance.ThrowOnPropertyChanged();
                    //__instance.ApplyTransfer(transferAmount, __instance.Side);
                    if (__instance.Side == PartyScreenLogic.PartyRosterSide.Left && !__instance.IsPrisoner)
                    {
                        PartyCharacterVM partyCharacterVM = ____partyVm.MainPartyTroops.FirstOrDefault((PartyCharacterVM x) => x.Character == __instance.Character);
                        if (partyCharacterVM != null)
                        {
                            partyCharacterVM.InitializeUpgrades();
                        }
                    }
                    __instance.InitializeUpgrades();
                    ____partyVm.ExecuteRemoveZeroCounts();

                    return false;
                }
                else
                {
                    //InformationManager.DisplayMessage(new InformationMessage("Also Wrong"));
                    return true;
                }
            }
        }

        [HarmonyPatch(typeof(PartyVM))]
        [HarmonyPatch("ExecuteTransferAllOtherTroops")]
        class TransferAllOverride
        {
            static bool Prefix(PartyVM __instance)
            {
                if (__instance != null && PartyScreenState.currentState == PartyScreenStateEnum.RecruitmentScreen)
                {
                    return false;

                }
                else if (__instance != null && PartyScreenState.currentState == PartyScreenStateEnum.GarrisonScreen)
                {


                    InformationManager.DisplayMessage(new InformationMessage("Button disabled! Please select the troops you wish to add to your party"));
                    return false;

                }
                else
                {
                    //InformationManager.DisplayMessage(new InformationMessage("Also Wrong"));
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
                    if(__instance.Side == PartyScreenLogic.PartyRosterSide.Left) __instance.Name = __instance.Name + "   Cost: " + CalculateGoldValue(__instance).ToString();
                }
            }
        }


        [HarmonyPatch(typeof(PartyTradeVM))]
        [HarmonyPatch("ExecuteApplyTransaction")]
        class ExecuteApplyTransactionOverride
        {
            static bool Prefix(PartyCharacterVM __instance)
            {
                if (__instance != null && (PartyScreenState.currentState == PartyScreenStateEnum.RecruitmentScreen || PartyScreenState.currentState == PartyScreenStateEnum.GarrisonScreen))
                {
                    return false;
                } else
                {
                    return true;
                }
            }
        }

        [HarmonyPatch(typeof(PartyTradeVM))]
        [HarmonyPatch("ExecuteIncreaseOtherStock")]
        class ExecuteIncreaseOtherStockOverride
        {
            static bool Prefix(PartyCharacterVM __instance)
            {
                if (__instance != null && (PartyScreenState.currentState == PartyScreenStateEnum.RecruitmentScreen || PartyScreenState.currentState == PartyScreenStateEnum.GarrisonScreen))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        [HarmonyPatch(typeof(PartyTradeVM))]
        [HarmonyPatch("ExecuteIncreasePlayerStock")]
        class ExecuteIncreasePlayerStockOverride
        {
            static bool Prefix(PartyCharacterVM __instance)
            {
                if (__instance != null && (PartyScreenState.currentState == PartyScreenStateEnum.RecruitmentScreen || PartyScreenState.currentState == PartyScreenStateEnum.GarrisonScreen))
                {
                    return false;
                }
                else
                {
                    return true;
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

            if(troopAmount > PartyBase.MainParty.PartySizeLimit - PartyBase.MainParty.MemberRoster.TotalManCount)
            {
                InformationManager.DisplayMessage(new InformationMessage("Can't go over party capacity! Consider putting some troops in the garrison first!"));
                return false;
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
            int goldMultiplier = GoldValue;
            if (NobleTroops.Contains(__instance.Character.StringId)) goldMultiplier *= 3;

            return __instance.Character.Tier * goldMultiplier;
        }

    }


        /*public static bool Prefix(ref TaleWorlds.CampaignSystem.ViewModelCollection.PartyCharacterVM __instance)
        {
            GiveGoldAction.ApplyBetweenCharacters(null, PartyBase.MainParty.LeaderHero, 100, false);
            InformationManager.DisplayMessage(new InformationMessage("Transfer"));
            return true;
        }*/

}
