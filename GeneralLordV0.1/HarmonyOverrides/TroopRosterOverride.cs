using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace GeneralLord.HarmonyOverrides
{
    class TroopRosterOverride
    {
        [HarmonyPatch(typeof(TroopRoster))]
        [HarmonyPatch("RemoveTroop")]
        public class GetGoldForCostUpgradeOverride
        {

            static bool Prefix(TroopRoster __instance, ref CharacterObject troop)
            {
                int index = __instance.FindIndexOfTroop(troop);
                //InformationManager.DisplayMessage(new InformationMessage(((int)explainedNumber.ResultNumber * 10).ToString()));
                //InformationManager.DisplayMessage(new InformationMessage(__result.ToString()));

                if (index == -1) return false;

                return true;
                

            }
        }
    }
}
