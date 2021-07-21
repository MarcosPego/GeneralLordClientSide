using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
//using CunningLords.Tactics;
using System.IO;
using Path = System.IO.Path;
using Newtonsoft.Json;
using System.Reflection;
//using CunningLords.Interaction;
using TaleWorlds.CampaignSystem;
//using CunningLords.PlanDefinition;

namespace GeneralLord.FormationBattleTest
{
    class MissionAI
    {
        public static bool missionAiActive = false;
        public static BattleSideEnum PlayerBattleSide { get; set; } = BattleSideEnum.None;

        [HarmonyPatch(typeof(MissionCombatantsLogic))]
        [HarmonyPatch("EarlyStart")]
        //This class is used to load tactics into the AI Teams, the tactics themselves determine the behaviour of each Formation within a Team
        public class TeamTacticsInitializer
        {
            static void Postfix(MissionCombatantsLogic __instance)
            {
                //string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

                /*string finalPath = Path.Combine(path, "ModuleData", "configData.json");

                CunningLordsConfigData data;
                using (StreamReader file = File.OpenText(finalPath))
                {
                    JsonSerializer deserializer = new JsonSerializer();
                    data = (CunningLordsConfigData)deserializer.Deserialize(file, typeof(CunningLordsConfigData));
                }*/

                //METER TEST BATTLE VERIFICATION HERE

                if (BattleTestHandler.BattleTestEnabled == BattleTestHandler.BattleTestEnabledState.BattleTest)
                {
                    //MissionAI.PlayerBattleSide = __instance.Mission.MainAgent.Team.Side; //Crashes

                    //InformationManager.DisplayMessage(new InformationMessage("tactic Level:" + data.TacticSill.ToString()));

                    List<Team> enemyTeams = Utils.GetAllEnemyTeams(__instance.Mission);

                    if (__instance.Mission.MissionTeamAIType == Mission.MissionTeamAITypeEnum.FieldBattle)
                    {
                        foreach (Team team in enemyTeams)
                        {
                                //InformationManager.DisplayMessage(new InformationMessage("Custom Battle"));

                                /*string path2 = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

                                string finalPath2 = Path.Combine(path2, "ModuleData", "DecisionTree.json");

                                Plan plan = new Plan();


                                var serializer = new JsonSerializer();
                                using (var sw = new StreamWriter(finalPath2))
                                using (JsonWriter writer = new JsonTextWriter(sw))
                                {
                                    serializer.Serialize(writer, plan);
                                }

                                Plan planFromJson;

                                using (StreamReader file = File.OpenText(finalPath))
                                {
                                    JsonSerializer serializer2 = new JsonSerializer();
                                    planFromJson = (Plan)serializer2.Deserialize(file, typeof(Plan));
                                }*/

                            team.ClearTacticOptions();
                            team.AddTacticOption(new TacticHoldGeneric(team));
                        }
                    }
                }
            }
        }
    }
}
