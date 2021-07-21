using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace GeneralLord.HarmonyOverrides
{
    class PlayerEncounterOverride
    {
        [HarmonyPatch(typeof(PlayerEncounter))]
        [HarmonyPatch("DoLootParty")]
        class DoLootPartyOverride
        {
            static bool Prefix(PlayerEncounter __instance, ref bool ____stateHandled)
            {
                ____stateHandled = true;
                return false;
            }
        }

        [HarmonyPatch(typeof(PlayerEncounter))]
        [HarmonyPatch("DoLootInventory")]
        class DoLootInventoryOverride
        {
            static bool Prefix(PlayerEncounter __instance, ref bool ____stateHandled)
            {
                //InformationManager.DisplayMessage(new InformationMessage(____stateHandled.ToString()));
                ____stateHandled = true;    
                return false;
            }
        }
    }
}
