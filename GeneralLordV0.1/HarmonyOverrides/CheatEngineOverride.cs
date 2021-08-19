using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;

namespace GeneralLord.HarmonyOverrides
{
    public class CheatEngineOverride
    {
        static bool _firstLoad = true;
       
        [HarmonyPatch(typeof(MBGameManager))]
        [HarmonyPatch("CheatMode", MethodType.Getter)]
        public class OnConfigChangedOverride
        {

            static void Postfix(MBGameManager __instance, ref bool __result)
            {
                if (NativeConfig.CheatMode && _firstLoad) { 
                    InformationManager.DisplayMessage(new InformationMessage("Warning Game Cheats Detected! Game Cheats have been Deactivated!"));
                    _firstLoad = false;
                }
                __result = false;
            }
        }
    }
}
