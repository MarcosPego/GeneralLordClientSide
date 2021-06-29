using HarmonyLib;
using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu;
using TaleWorlds.Localization;

namespace GeneralLord.HarmonyOverrides
{

    public class RecruitmentOverride
    {

        [HarmonyPatch(typeof(RecruitmentVM))]
        [HarmonyPatch("RefreshValues")]
        class RefreshValuesOverride
        {
            static void Prefix(RecruitmentVM __instance)
            {


			}

        }
    }
}
