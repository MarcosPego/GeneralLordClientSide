using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using TaleWorlds.Core;

namespace GeneralLord.HarmonyOverrides
{
    public class RecruitmentCostOverride
    {
        [HarmonyPatch(typeof(DefaultPartyTroopUpgradeModel))]
        [HarmonyPatch("GetGoldCostForUpgrade")]
        public class GetGoldForCostUpgradeOverride
        {

            static void Postfix(DefaultPartyTroopUpgradeModel __instance, ref int __result/*, ref ExplainedNumber explainedNumber, ref int __result*/)
            {
                //InformationManager.DisplayMessage(new InformationMessage(((int)explainedNumber.ResultNumber * 10).ToString()));
                //InformationManager.DisplayMessage(new InformationMessage(__result.ToString()));

                int balancePrice = __result * 10;
                __result = balancePrice;

            }
        }
    }
}
