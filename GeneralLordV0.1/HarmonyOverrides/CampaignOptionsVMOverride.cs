using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation.OptionsStage;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Localization;

namespace GeneralLord.HarmonyOverrides
{
    public class CampaignOptionsVMOverride
    {

        [HarmonyPatch(typeof(CampaignOptionsControllerVM))]
        [HarmonyPatch("UpdateDifficultyPreset")]
        public class RefreshValuesOverride
        {
            static void Postfix(CampaignOptionsControllerVM __instance, ref CampaignOptionItemVM ____difficultyPresetOption)
            {
                ____difficultyPresetOption.SelectionSelector.SelectedIndex = 3;
            }
        }

        [HarmonyPatch(typeof(CampaignOptionsControllerVM))]
        [HarmonyPatch("SetAllOptionsFromPreset")]
        public class SetAllOptionsFromPresetOverride
        {
            static void Postfix(CampaignOptionsControllerVM __instance, ref CampaignOptionItemVM ____difficultyPresetOption, ref bool ____isChangingDifficultyPreset)
            {
                ____isChangingDifficultyPreset = true;
                foreach (CampaignOptionItemVM campaignOptionItemVM in __instance.Options)
                {
                    if (campaignOptionItemVM.OptionType == 2 && campaignOptionItemVM != ____difficultyPresetOption)
                    {
                        campaignOptionItemVM.SelectionSelector.SelectedIndex = 2;
                        if (campaignOptionItemVM.Identifier == "PlayerReceivedDamage")
                        {
                            campaignOptionItemVM.SelectionSelector.SelectedIndex = 1;
                        }

                    }

                    if (campaignOptionItemVM.OptionType == 0 && campaignOptionItemVM != ____difficultyPresetOption)
                    {
                        if (campaignOptionItemVM.Identifier == "IronmanMode")
                        {
                            campaignOptionItemVM.ValueAsBoolean = true;
                        }
                    }
                }
                                ____isChangingDifficultyPreset = false;
                ____difficultyPresetOption.SelectionSelector.SelectedIndex = 3;

            }
        }

        [HarmonyPatch(typeof(CharacterCreationOptionsStageVM))]
        [HarmonyPatch("OnNextStage")]
        public class OnNextStageOverride
        {
            static void Prefix(CharacterCreationOptionsStageVM __instance)
            {
                __instance.OptionsController.SetAllOptionsFromPreset();

                __instance.OptionsController.UpdateDifficultyPreset();
            }
        }
    }
}
