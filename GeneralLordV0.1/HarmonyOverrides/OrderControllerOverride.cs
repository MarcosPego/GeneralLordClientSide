using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using CunningLords.Patches;
using CunningLords.Interaction;
using System.IO;
using Path = System.IO.Path;
using Newtonsoft.Json;
using System.Reflection;
using CunningLords.PlanDefinition;
using TaleWorlds.MountAndBlade.ViewModelCollection;

namespace CunningLords.Patches
{
    public class OrderControllerOverride
    {
        [HarmonyPatch(typeof(OrderController))]
        [HarmonyPatch("SetOrder")]
        class SetOrderOverride
        {
            static bool Prefix(OrderType orderType, OrderController __instance)
            {
                List<Formation>.Enumerator enumerator = __instance.SelectedFormations.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    Formation formation = enumerator.Current;
                    formation.IsAIControlled = false;
                }

                return true;
            }
        }
        
        [HarmonyPatch(typeof(OrderController))]
        [HarmonyPatch("SetOrderWithFormation")]
        class SetOrderWithFormationOverride
        {
            static bool Prefix(OrderType orderType, Formation orderFormation, OrderController __instance)
            {
                List<Formation>.Enumerator enumerator = __instance.SelectedFormations.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    Formation formation = enumerator.Current;
                    formation.IsAIControlled = false;
                }

                return true;
            }
        }
        
        [HarmonyPatch(typeof(OrderController))]
        [HarmonyPatch("SetOrderWithPosition")]
        class SetOrderWithPositionOverride
        {
            static bool Prefix(OrderType orderType, WorldPosition orderPosition, OrderController __instance)
            {
                List<Formation>.Enumerator enumerator = __instance.SelectedFormations.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    Formation formation = enumerator.Current;
                    formation.IsAIControlled = false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(OrderController))]
        [HarmonyPatch("SetOrderWithTwoPositions")]
        class SetOrderWithTwoPositionsOverride
        {
            static bool Prefix(OrderType orderType, WorldPosition position1, WorldPosition position2, OrderController __instance)
            {
                List<Formation>.Enumerator enumerator = __instance.SelectedFormations.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    Formation formation = enumerator.Current;
                    formation.IsAIControlled = false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(OrderController))]
        [HarmonyPatch("SetOrderWithAgent")]
        class SetOrderWithAgentOverride
        {
            static bool Prefix(OrderType orderType, Agent agent, OrderController __instance)
            {
                List<Formation>.Enumerator enumerator = __instance.SelectedFormations.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    Formation formation = enumerator.Current;
                    formation.IsAIControlled = false;
                }
                return true;
            }
        }
    }
}
