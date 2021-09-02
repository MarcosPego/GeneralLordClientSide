using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace GeneralLord.HarmonyOverrides
{
    class BehaviorDefendOverride
    {
        [HarmonyPatch(typeof(BehaviorDefend))]
        [HarmonyPatch("TickOccasionally")]
        class BehaviorDefendFormationOverride
        {

            static bool Prefix(BehaviorDefend __instance)
            {

                //InformationManager.DisplayMessage(new InformationMessage("Tick"));
                return false;
            }

        }


        [HarmonyPatch(typeof(BehaviorDefend))]
        [HarmonyPatch("OnBehaviorActivatedAux")]
        class OnBehaviorActivatedAuxOverride
        {

            static bool Prefix(BehaviorDefend __instance)
            {
                //__instance.CalculateCurrentOrder();
                __instance.Formation.SetMovementOrder(__instance.CurrentOrder);
                //__instance.Formation.FacingOrder = __instance.CurrentFacingOrder;
                //base.Formation.ArrangementOrder = ArrangementOrder.ArrangementOrderLoose;
                __instance.Formation.FiringOrder = FiringOrder.FiringOrderFireAtWill;
                __instance.Formation.FormOrder = FormOrder.FormOrderWide;
                __instance.Formation.WeaponUsageOrder = WeaponUsageOrder.WeaponUsageOrderUseAny;
                return false;
            }

        }
    }
}
