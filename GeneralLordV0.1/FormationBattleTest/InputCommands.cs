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
using System.IO;
using Path = System.IO.Path;
using Newtonsoft.Json;
using System.Reflection;

namespace GeneralLord.FormationBattleTest
{
    class InputCommands
    {
        public void ApplyActiontoFormation(Mission mission)
        {

            InformationManager.DisplayMessage(new InformationMessage("Infantry will now advance"));

            Team playerTeam = mission.MainAgent.Team;

            IEnumerable<Formation> playerFormations = playerTeam.FormationsIncludingSpecial;

            foreach(Formation f in playerFormations)
            {
                if(f.FormationIndex == FormationClass.Infantry)
                {
                    Vec2 escapeVector = f.QuerySystem.AveragePosition + (f.Direction.Normalized() * 10f);

                    WorldPosition position = f.QuerySystem.MedianPosition;
                    position.SetVec2(escapeVector);
                    f.SetMovementOrder(MovementOrder.MovementOrderMove(position));
                    f.FacingOrder = FacingOrder.FacingOrderLookAtDirection(f.Direction.Normalized());
                }
            }
        }

        public void ApplyOnStartPositions(Mission mission) 
        {
            InformationManager.DisplayMessage(new InformationMessage("Infantry will now advance"));

            Team playerTeam = mission.MainAgent.Team;

            IEnumerable<Formation> playerFormations = playerTeam.FormationsIncludingSpecial;

            foreach (Formation f in playerFormations)
            {
                if (f.FormationIndex == FormationClass.Cavalry)
                {
                    Vec2 escapeVector = f.QuerySystem.AveragePosition + (f.Direction.LeftVec().Normalized() * 20f);

                    WorldPosition position = f.QuerySystem.MedianPosition;
                    position.SetVec2(escapeVector);
                    f.SetMovementOrder(MovementOrder.MovementOrderMove(position));
                    f.FacingOrder = FacingOrder.FacingOrderLookAtDirection(f.Direction.Normalized());
                }
                else if (f.FormationIndex == FormationClass.Ranged)
                {
                    Vec2 escapeVector = f.QuerySystem.AveragePosition + (f.Direction.Normalized() * -20f);

                    WorldPosition position = f.QuerySystem.MedianPosition;
                    position.SetVec2(escapeVector);
                    f.SetMovementOrder(MovementOrder.MovementOrderMove(position));
                    f.FacingOrder = FacingOrder.FacingOrderLookAtDirection(f.Direction.Normalized());
                }
                else if (f.FormationIndex == FormationClass.HorseArcher)
                {
                    Vec2 escapeVector = f.QuerySystem.AveragePosition + (f.Direction.RightVec().Normalized() * 20f);

                    WorldPosition position = f.QuerySystem.MedianPosition;
                    position.SetVec2(escapeVector);
                    f.SetMovementOrder(MovementOrder.MovementOrderMove(position));
                    f.FacingOrder = FacingOrder.FacingOrderLookAtDirection(f.Direction.Normalized());
                }
            }
        }

        public void SaveOffsets(Mission mission, int index)
        {
            string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

            string finalPath = Path.Combine(path, "ModuleData", "data.json");

            List<PositionData> deserialized = Deserialize(finalPath);

            PositionData newData = GetCurrentPositionData(mission);
            deserialized[index] = newData;
            Serialize(deserialized, finalPath);

            InformationManager.DisplayMessage(new InformationMessage("Saved to index " + index.ToString()));
        }

        public PositionData GetCurrentPositionData(Mission mission)
        {
            PositionData result = new PositionData();

            Formation mainFormation = GetFormationPriority(mission);

            foreach (Formation f in mission.MainAgent.Team.Formations)
            {
                if (f.FormationIndex == FormationClass.Infantry)
                {
                    result.InfantryArrangementOrder = f.ArrangementOrder.OrderEnum;
                }
                else if(f.FormationIndex == FormationClass.Ranged)
                {
                    result.ArchersXOffset = CalculateXYOffsetsBasedOnWorldPosition(mainFormation, f).X;
                    result.ArchersYOffset = CalculateXYOffsetsBasedOnWorldPosition(mainFormation, f).Y;

                    result.ArchersArrangementOrder = f.ArrangementOrder.OrderEnum;
                }
                else if (f.FormationIndex == FormationClass.Cavalry)
                {
                    result.CavalryXOffset = CalculateXYOffsetsBasedOnWorldPosition(mainFormation, f).X;
                    result.CavalryYOffset = CalculateXYOffsetsBasedOnWorldPosition(mainFormation, f).Y;

                    result.CavalryArrangementOrder = f.ArrangementOrder.OrderEnum;
                }
                else if (f.FormationIndex == FormationClass.HorseArcher)
                {
                    result.HorseArchersXOffset = CalculateXYOffsetsBasedOnWorldPosition(mainFormation, f).X;
                    result.HorseArchersYOffset = CalculateXYOffsetsBasedOnWorldPosition(mainFormation, f).Y;

                    result.HorseArchersArrangementOrder = f.ArrangementOrder.OrderEnum;
                }
                else if (f.FormationIndex == FormationClass.Skirmisher)
                {
                    result.SkirmisherXOffset = CalculateXYOffsetsBasedOnWorldPosition(mainFormation, f).X;
                    result.SkirmisherYOffset = CalculateXYOffsetsBasedOnWorldPosition(mainFormation, f).Y;

                    result.SkirmisherArrangementOrder = f.ArrangementOrder.OrderEnum;
                }
                else if (f.FormationIndex == FormationClass.HeavyInfantry)
                {
                    result.HeavyInfantryXOffset = CalculateXYOffsetsBasedOnWorldPosition(mainFormation, f).X;
                    result.HeavyInfantryYOffset = CalculateXYOffsetsBasedOnWorldPosition(mainFormation, f).Y;

                    result.HeavyInfantryArrangementOrder = f.ArrangementOrder.OrderEnum;
                }
                else if (f.FormationIndex == FormationClass.LightCavalry)
                {
                    result.LightCavalryXOffset = CalculateXYOffsetsBasedOnWorldPosition(mainFormation, f).X;
                    result.LightCavalryYOffset = CalculateXYOffsetsBasedOnWorldPosition(mainFormation, f).Y;

                    result.LightCavalryArrangementOrder = f.ArrangementOrder.OrderEnum;
                }
                else if (f.FormationIndex == FormationClass.HeavyCavalry)
                {
                    result.HeavyCavalryXOffset = CalculateXYOffsetsBasedOnWorldPosition(mainFormation, f).X;
                    result.HeavyCavalryYOffset = CalculateXYOffsetsBasedOnWorldPosition(mainFormation, f).Y;

                    result.HeavyCavalryArrangementOrder = f.ArrangementOrder.OrderEnum;
                }
            }

            return result;
        }

        public void ApplyPosition(Mission mission, int index, bool playerTeam = true, string file = "data.json")
        {
            InformationManager.DisplayMessage(new InformationMessage("Load index is player " + playerTeam.ToString()));

            string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

            string finalPath = Path.Combine(path, "ModuleData", file);

            List<PositionData> deserialized = Deserialize(finalPath);

            Formation mainFormation = GetFormationPriority(mission);

            Team team;
            if (playerTeam)
            {
                team = mission.MainAgent.Team;
            } else
            {
                team = mission.PlayerEnemyTeam;
            }


            foreach (Formation f in team.Formations)
            {
                if (f.FormationIndex == FormationClass.Infantry)
                {
                    WorldPosition position = CalculateWorldpositionsBasedOnOffset(mainFormation, mission, 0, 0);
                    f.SetMovementOrder(MovementOrder.MovementOrderMove(position));

                    ApplyArrangement(f, deserialized[index].InfantryArrangementOrder);
                }
                else if (f.FormationIndex == FormationClass.Ranged && f.FormationIndex != mainFormation.FormationIndex)
                {
                    WorldPosition position = CalculateWorldpositionsBasedOnOffset(mainFormation, mission, deserialized[index].ArchersXOffset, deserialized[index].ArchersYOffset);
                    f.SetMovementOrder(MovementOrder.MovementOrderMove(position));

                    ApplyArrangement(f, deserialized[index].ArchersArrangementOrder);
                }
                else if (f.FormationIndex == FormationClass.Cavalry && f.FormationIndex != mainFormation.FormationIndex)
                {
                    WorldPosition position = CalculateWorldpositionsBasedOnOffset(mainFormation, mission, deserialized[index].CavalryXOffset, deserialized[index].CavalryYOffset);
                    f.SetMovementOrder(MovementOrder.MovementOrderMove(position));

                    ApplyArrangement(f, deserialized[index].CavalryArrangementOrder);
                }
                else if (f.FormationIndex == FormationClass.HorseArcher && f.FormationIndex != mainFormation.FormationIndex)
                {
                    WorldPosition position = CalculateWorldpositionsBasedOnOffset(mainFormation, mission, deserialized[index].HorseArchersXOffset, deserialized[index].HorseArchersYOffset);
                    f.SetMovementOrder(MovementOrder.MovementOrderMove(position));

                    ApplyArrangement(f, deserialized[index].HorseArchersArrangementOrder);
                }
                else if (f.FormationIndex == FormationClass.Skirmisher && f.FormationIndex != mainFormation.FormationIndex)
                {
                    WorldPosition position = CalculateWorldpositionsBasedOnOffset(mainFormation, mission, deserialized[index].SkirmisherXOffset, deserialized[index].SkirmisherYOffset);
                    f.SetMovementOrder(MovementOrder.MovementOrderMove(position));

                    ApplyArrangement(f, deserialized[index].SkirmisherArrangementOrder);
                }
                else if (f.FormationIndex == FormationClass.HeavyInfantry && f.FormationIndex != mainFormation.FormationIndex)
                {
                    WorldPosition position = CalculateWorldpositionsBasedOnOffset(mainFormation, mission, deserialized[index].HeavyInfantryXOffset, deserialized[index].HeavyInfantryYOffset);
                    f.SetMovementOrder(MovementOrder.MovementOrderMove(position));

                    ApplyArrangement(f, deserialized[index].HeavyInfantryArrangementOrder);
                }
                else if (f.FormationIndex == FormationClass.LightCavalry && f.FormationIndex != mainFormation.FormationIndex)
                {
                    WorldPosition position = CalculateWorldpositionsBasedOnOffset(mainFormation, mission, deserialized[index].LightCavalryXOffset, deserialized[index].LightCavalryYOffset);
                    f.SetMovementOrder(MovementOrder.MovementOrderMove(position));

                    ApplyArrangement(f, deserialized[index].LightCavalryArrangementOrder);
                }
                else if (f.FormationIndex == FormationClass.HeavyCavalry && f.FormationIndex != mainFormation.FormationIndex)
                {
                    WorldPosition position = CalculateWorldpositionsBasedOnOffset(mainFormation, mission, deserialized[index].HeavyCavalryXOffset, deserialized[index].HeavyCavalryYOffset);
                    f.SetMovementOrder(MovementOrder.MovementOrderMove(position));

                    ApplyArrangement(f, deserialized[index].HeavyCavalryArrangementOrder);
                }
            }
            InformationManager.DisplayMessage(new InformationMessage("Load index " + index.ToString()));
        }

        public void ApplyArrangement(Formation f, ArrangementOrder.ArrangementOrderEnum ae)
        {
            switch (ae)
            {
                case ArrangementOrder.ArrangementOrderEnum.Circle:
                    f.ArrangementOrder = ArrangementOrder.ArrangementOrderCircle;
                    break;
                case ArrangementOrder.ArrangementOrderEnum.Column:
                    f.ArrangementOrder = ArrangementOrder.ArrangementOrderColumn;
                    break;
                case ArrangementOrder.ArrangementOrderEnum.Line:
                    f.ArrangementOrder = ArrangementOrder.ArrangementOrderLine;
                    break;
                case ArrangementOrder.ArrangementOrderEnum.Loose:
                    f.ArrangementOrder = ArrangementOrder.ArrangementOrderLoose;
                    break;
                case ArrangementOrder.ArrangementOrderEnum.Scatter:
                    f.ArrangementOrder = ArrangementOrder.ArrangementOrderScatter;
                    break;
                case ArrangementOrder.ArrangementOrderEnum.ShieldWall:
                    f.ArrangementOrder = ArrangementOrder.ArrangementOrderShieldWall;
                    break;
                case ArrangementOrder.ArrangementOrderEnum.Skein:
                    f.ArrangementOrder = ArrangementOrder.ArrangementOrderSkein;
                    break;
                case ArrangementOrder.ArrangementOrderEnum.Square:
                    f.ArrangementOrder = ArrangementOrder.ArrangementOrderSquare;
                    break;
            }
        }

        public Formation GetFormationPriority(Mission mission)
        {
            Formation infantry = mission.MainAgent.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.Infantry);
            Formation archers = mission.MainAgent.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.Ranged);
            Formation cavalry = mission.MainAgent.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.Cavalry);
            Formation horseArcher = mission.MainAgent.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.HorseArcher);
            Formation skirmisher = mission.MainAgent.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.Skirmisher);
            Formation heavyInfantry = mission.MainAgent.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.HeavyInfantry);
            Formation lightCavalry = mission.MainAgent.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.LightCavalry);
            Formation heavyCavalry = mission.MainAgent.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.HeavyInfantry);

            List<Formation> formations = new List<Formation>();

            formations.Add(infantry);
            formations.Add(archers);
            formations.Add(cavalry);
            formations.Add(horseArcher);
            formations.Add(skirmisher);
            formations.Add(heavyInfantry);
            formations.Add(lightCavalry);
            formations.Add(heavyCavalry);

            foreach (Formation f in formations) 
            {
                if(f != null)
                {
                    return f;
                }
            }

            return null;
        }

        public void Serialize(List<PositionData> data, string finalPath)
        {
            var serializer = new JsonSerializer();
            using (var sw = new StreamWriter(finalPath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, data);
            }
        }

        public List<PositionData> Deserialize(string path)
        {
            using (StreamReader file = File.OpenText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                List<PositionData> data = (List<PositionData>)serializer.Deserialize(file, typeof(List<PositionData>));
                return data;
            }
        }

        private static WorldPosition CalculateWorldpositionsBasedOnOffset(Formation formation, Mission __instance, float XOffset, float YOffset)
        {
            float num = formation.Direction.x * YOffset;
            float num2 = formation.Direction.y * YOffset;
            float num3 = formation.Direction.y * XOffset;
            float num4 = -formation.Direction.x * XOffset;
            Vec2 currentPosition = formation.CurrentPosition;
            Vec3 position = new Vec3(currentPosition.X + num + num3, currentPosition.Y + num2 + num4, 0f, -1f);
            WorldPosition result = new WorldPosition(__instance.Scene, position);
            return result;
        }

        private static Vec2 CalculateXYOffsetsBasedOnWorldPosition(Formation mainFormation, Formation formation)
        {
            double num = (double)formation.CurrentPosition.X;
            double num2 = (double)formation.CurrentPosition.Y;
            double num3 = (double)mainFormation.CurrentPosition.X;
            double num4 = (double)mainFormation.CurrentPosition.Y;
            double num5 = Math.Atan2((double)mainFormation.Direction.Y, (double)mainFormation.Direction.X);
            num -= num3;
            num2 -= num4;
            double num6 = num * Math.Sin(num5) - num2 * Math.Cos(num5);
            double num7 = num * Math.Cos(num5) + num2 * Math.Sin(num5);
            return new Vec2((float)num6, (float)num7);
        }

        /*public void SetInitialFormationOrders(Mission mission)
        {
            //ToDo: 
            //Create a function which receives a formation and a Ordertype and applies that ordertype -> OrderController.SetOrder()
            //Create a Json which can contain the order
            //Apply it on first Tick

            string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

            string finalPath = Path.Combine(path, "ModuleData", "startdata.json");

            StartingOrderData data;

            using (StreamReader file = File.OpenText(finalPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                data = (StartingOrderData)serializer.Deserialize(file, typeof(StartingOrderData));
            }

            foreach (Formation f in mission.MainAgent.Team.Formations)
            {
                if (f.FormationIndex == FormationClass.Infantry)
                {
                    ApplyOrderToFormation(f, data.InfantryOrder);
                }
                else if (f.FormationIndex == FormationClass.Ranged)
                {
                    ApplyOrderToFormation(f, data.ArcherOrder);
                }
                else if (f.FormationIndex == FormationClass.Cavalry)
                {
                    ApplyOrderToFormation(f, data.CavalryOrder);
                }
                else if (f.FormationIndex == FormationClass.HorseArcher)
                {
                    ApplyOrderToFormation(f, data.HorseArcherOrder);
                }
                else if (f.FormationIndex == FormationClass.Skirmisher)
                {
                    ApplyOrderToFormation(f, data.SkirmisherOrder);
                }
                else if (f.FormationIndex == FormationClass.HeavyInfantry)
                {
                    ApplyOrderToFormation(f, data.HeavyInfantryOrder);
                }
                else if (f.FormationIndex == FormationClass.LightCavalry)
                {
                    ApplyOrderToFormation(f, data.LightCavalryOrder);
                }
                else if (f.FormationIndex == FormationClass.HeavyCavalry)
                {
                    ApplyOrderToFormation(f, data.HeavyCavalryOrder);
                }
            }
        }*/

        public void ApplyOrderToFormation(Formation formation, OrderType order)
        {
            if(formation == null)
            {
                return;
            }
            switch (order)
            {
                case OrderType.Charge:
                    formation.SetMovementOrder(MovementOrder.MovementOrderCharge);
                    break;
                case OrderType.ChargeWithTarget:
                case OrderType.FollowMe:
                    if (formation.Team.Leader != null)
                    {
                        formation.SetMovementOrder(MovementOrder.MovementOrderFollow(formation.Team.Leader));
                    }
                    else
                    {
                        formation.SetMovementOrder(MovementOrder.MovementOrderStop);
                    }
                    break;
                case OrderType.FollowEntity:
                case OrderType.GuardMe:
                //case OrderType.Attach:
                case OrderType.LookAtDirection:
                case OrderType.FormCustom:
                case OrderType.CohesionHigh:
                case OrderType.CohesionMedium:
                case OrderType.CohesionLow:
                case OrderType.RideFree:
                case OrderType.None:
                case OrderType.StandYourGround:
                    formation.SetMovementOrder(MovementOrder.MovementOrderStop);
                    break;
                case OrderType.Retreat:
                    formation.SetMovementOrder(MovementOrder.MovementOrderRetreat);
                    break;
                case OrderType.Advance:
                    formation.SetMovementOrder(MovementOrder.MovementOrderAdvance);
                    break;
                case OrderType.FallBack:
                    formation.SetMovementOrder(MovementOrder.MovementOrderFallBack);
                    break;
                default:
                    formation.SetMovementOrder(MovementOrder.MovementOrderStop);
                    break;
            }
        }
    }
}
