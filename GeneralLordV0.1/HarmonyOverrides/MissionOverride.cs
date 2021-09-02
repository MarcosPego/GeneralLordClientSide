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
using GeneralLord.FormationBattleTest;
using GeneralLord.FormationPlanHandler;

namespace CunningLords.Patches
{
    public class MissionOverride
    {
        public static BattleSideEnum PlayerBattleSide { get; set; } = BattleSideEnum.None;

        public static int FrameCounter = 0;

        public static bool IsPlanActive = false;

        private static PlanGenerator AttackerGenerator = null;
        private static PlanGenerator DefenderGenerator = null;

        private static int PlanCounter = 0;

        [HarmonyPatch(typeof(Mission))]
        [HarmonyPatch("OnTick")]
        class OnTickOverride
        {
            static void Postfix(Mission __instance)
            {
                if (__instance != null && __instance.MainAgent != null)
                {
                    MissionOverride.PlayerBattleSide = __instance.MainAgent.Team.Side;
                }
                else
                {
                    return;
                }

                if (MissionOverride.FrameCounter == 0)
                {
                    MissionOverride.IsPlanActive = false;
                    MissionOverride.AttackerGenerator = new PlanGenerator();

                    Utils.OnStartOrders(__instance);

                    if (BattleTestHandler.BattleTestEnabled == BattleTestHandler.BattleTestEnabledState.BattleTest)
                    {
                        Mission m = Mission.Current;

                        Team playerTeam = Mission.Current.MainAgent.Team;

                        foreach (Formation f in playerTeam.Formations)
                        {
                            f.FiringOrder = FiringOrder.FiringOrderHoldYourFire;
                        }
                    } else if (EnemyFormationHandler.EnemyUseDefensiveSettings == 1)
                    {
                        MissionOverride.DefenderGenerator = new PlanGenerator(false);
                    }

                }


                MissionOverride.FrameCounter++;
                Utils.ManageInputKeys(__instance);

                if (MissionOverride.IsPlanActive && __instance.MainAgent != null)
                {
                    if (MissionOverride.PlanCounter == 0)
                    {
                        foreach (Formation f in __instance.MainAgent.Team.Formations)
                        {
                            f.IsAIControlled = true;
                        }

                        MissionOverride.PlanCounter++;
                    }

                    MissionOverride.AttackerGenerator.Run(__instance.MainAgent.Team);

                }



                if (!MissionOverride.IsPlanActive && __instance.MainAgent != null)
                {
                    foreach (Formation f in __instance.MainAgent.Team.Formations)
                    {
                        f.IsAIControlled = false;
                    }

                    MissionOverride.PlanCounter = 0;
                }

                if (EnemyFormationHandler.EnemyUseDefensiveSettings == 1)
                {
                    //InformationManager.DisplayMessage(new InformationMessage("Formation Orders IS RUNNING!"));
                    MissionOverride.DefenderGenerator.Run(__instance.PlayerEnemyTeam, true);
                }

            }
        }

        [HarmonyPatch(typeof(Mission))]
        [HarmonyPatch("AfterStart")]
        class AfterStartOverride
        {
            static void Postfix(Mission __instance)
            {
                MissionOverride.FrameCounter = 0;




                //string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

                //string finalPath = Path.Combine(path, "ModuleData", "configData.json");

                /*CunningLordsConfigData data;
                using (StreamReader file = File.OpenText(finalPath))
                {
                    JsonSerializer deserializer = new JsonSerializer();
                    data = (CunningLordsConfigData)deserializer.Deserialize(file, typeof(CunningLordsConfigData));
                }*/

                //GameMetricsController GMC= new GameMetricsController();

                //GMC.WriteToJson(GameMetricEnum.TotalBattles);

                /*if (CampaignInteraction.isCustomBattle)
                {
                    GMC.WriteToJson(GameMetricEnum.NumberOfTestBattles);
                }
                else
                {
                    if (data.AIActive)
                    {
                        GMC.WriteToJson(GameMetricEnum.BattlesUsingAI);
                    }
                    else
                    {
                        GMC.WriteToJson(GameMetricEnum.BattlesUsingNative);
                    }
                }*/
            }
        }
    }
}