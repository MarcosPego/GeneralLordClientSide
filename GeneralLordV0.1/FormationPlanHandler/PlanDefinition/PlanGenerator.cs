using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Path = System.IO.Path;
using Newtonsoft.Json;
using System.Reflection;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;
using CunningLords.Patches;
using CunningLords.Behaviors;
using GeneralLordWebApiClient.Model;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using GeneralLord.FormationBattleTest;
using GeneralLord.FormationPlanHandler;

namespace CunningLords.PlanDefinition
{
    public class PlanGenerator
    {
        public Plan plan;

        public Formation infantry;
        public Formation archers;
        public Formation cavalry;
        public Formation horseArchers;
        public Formation skirmishers;
        public Formation heavyInfantry;
        public Formation lightCavalry;
        public Formation heavyCavalry;
        public int positionCounter = 0;
        private int engageCounterStart = -1;

        private bool isEngaged = false;

        private PlanStateEnum previousState = PlanStateEnum.Prepare;

        public int positionLimit = 1500;
        private bool _isPlayerSide;

        public PlanGenerator(bool isPlayerSide = true)
        {
            this._isPlayerSide = isPlayerSide;
            string finalPath; 
            if (isPlayerSide)
            {
                finalPath = Path.Combine(Serializer.SaveFolderPath(), "AttackDecisiontree.json");
            }
            else
            {
                finalPath = Path.Combine(Serializer.SaveFolderPath(), "EnemyDecisiontree.json");
            }

            if(!File.Exists(finalPath))
            {
                string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

                finalPath = Path.Combine(path, "ModuleData", "AttackDecisiontree.json");

            }

            using (StreamReader file = File.OpenText(finalPath))
            {
                JsonSerializer serializer2 = new JsonSerializer();
                this.plan = (Plan)serializer2.Deserialize(file, typeof(Plan));
            }

            if (Mission.Current != null)
            {
                if (Mission.Current.MainAgent != null)
                {
                    Team playerTeam = Mission.Current.MainAgent.Team;

                    foreach (Formation f in playerTeam.Formations)
                    {
                        //f.IsAIControlled = true;

                        switch (f.FormationIndex)
                        {
                            case FormationClass.Infantry:
                                this.infantry = f;
                                break;
                            case FormationClass.Ranged:
                                this.archers = f;
                                break;
                            case FormationClass.Cavalry:
                                this.cavalry = f;
                                break;
                            case FormationClass.HorseArcher:
                                this.horseArchers = f;
                                break;
                            case FormationClass.Skirmisher:
                                this.skirmishers = f;
                                break;
                            case FormationClass.HeavyInfantry:
                                this.heavyCavalry = f;
                                break;
                            case FormationClass.LightCavalry:
                                this.lightCavalry = f;
                                break;
                            case FormationClass.HeavyCavalry:
                                this.heavyCavalry = f;
                                break;
                        }
                    }
                }
            }

        }

        public void Run(Team team, bool isAiTeam = false)
        {
            PlanStateEnum state = GetMissionState();

            if (this.previousState != state)
            {
                if (isAiTeam)
                {
                    //InformationManager.DisplayMessage(new InformationMessage("AI Mission has entered " + state.ToString() + " State"));
                } else
                {
                    InformationManager.DisplayMessage(new InformationMessage("Mission has entered " + state.ToString() + " State"));
                }


                this.previousState = state;
            }


            
            if (Mission.Current != null)
            {
                if (Mission.Current.MainAgent != null)
                {
                    //Team playerTeam = Mission.Current.MainAgent.Team;

                    
                    foreach (Formation f in team.Formations)
                    {
                        f.IsAIControlled = false;
                        if (positionCounter > positionLimit)
                        {
                            f.IsAIControlled = true;
                        }


                        int index; //METER AQUI O INDICE DA POSITION QUE SE QUER!!!

                        string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

                        string finalPath;
                        List<PositionData> deserialized;
                        bool usePosition = true;
                        if (!isAiTeam)
                        {
                            index = EnemyFormationHandler.AttackSelectedFormation; //METER AQUI O INDICE DA POSITION QUE SE QUER!!!
                            finalPath = Path.Combine(path, "ModuleData", "data.json");
                            deserialized = Deserialize(finalPath);
                        } else
                        {
                            index = EnemyFormationHandler.EnemySelectedFormation;
                            finalPath = Path.Combine(path, "ModuleData", "enemydata.json");
                            deserialized = Deserialize(finalPath);
                        }
                        if (index == -1) usePosition = false;
                        if (usePosition && state == PlanStateEnum.Position)
                        {

                            if (positionCounter < 3)
                            {
                                
                                Mission mission = Mission.Current;

                                Formation mainFormation = GetFormationPriority(mission, team);

                                if (f.FormationIndex == FormationClass.Infantry)
                                {
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
                        }
                        else if (positionCounter == positionLimit || !usePosition)
                        {
                            f.IsAIControlled = true;
                        }


                        //f.IsAIControlled = true;
                        switch (f.FormationIndex)
                        {
                            case FormationClass.Infantry:
                                if (state == PlanStateEnum.Prepare)
                                {
                                    ApplyBehavior(f, plan.infantryPhasePrepare);
                                }
                                else if (state == PlanStateEnum.Ranged)
                                {
                                    ApplyBehavior(f, plan.infantryPhaseRanged);
                                }
                                else if (state == PlanStateEnum.Engage)
                                {
                                    ApplyBehavior(f, plan.infantryPhaseEngage);
                                }
                                else if (state == PlanStateEnum.Winning)
                                {
                                    ApplyBehavior(f, plan.infantryPhaseWinning);
                                }
                                else if (state == PlanStateEnum.Losing)
                                {
                                    ApplyBehavior(f, plan.infantryPhaseLosing);
                                }
                                break;
                            case FormationClass.Ranged:
                                if (state == PlanStateEnum.Prepare)
                                {
                                    ApplyBehavior(f, plan.archersPhasePrepare);
                                }
                                else if (state == PlanStateEnum.Ranged)
                                {
                                    ApplyBehavior(f, plan.archersPhaseRanged);
                                }
                                else if (state == PlanStateEnum.Engage)
                                {
                                    ApplyBehavior(f, plan.archersPhaseEngage);
                                }
                                else if (state == PlanStateEnum.Winning)
                                {
                                    ApplyBehavior(f, plan.archersPhaseWinning);
                                }
                                else if (state == PlanStateEnum.Losing)
                                {
                                    ApplyBehavior(f, plan.archersPhaseLosing);
                                }
                                break;
                            case FormationClass.Cavalry:
                                if (state == PlanStateEnum.Prepare)
                                {
                                    ApplyBehavior(f, plan.cavalryPhasePrepare);
                                }
                                else if (state == PlanStateEnum.Ranged)
                                {
                                    ApplyBehavior(f, plan.cavalryPhaseRanged);
                                }
                                else if (state == PlanStateEnum.Engage)
                                {
                                    ApplyBehavior(f, plan.cavalryPhaseEngage);
                                }
                                else if (state == PlanStateEnum.Winning)
                                {
                                    ApplyBehavior(f, plan.cavalryPhaseWinning);
                                }
                                else if (state == PlanStateEnum.Losing)
                                {
                                    ApplyBehavior(f, plan.cavalryPhaseLosing);
                                }
                                break;
                            case FormationClass.HorseArcher:
                                if (state == PlanStateEnum.Prepare)
                                {
                                    ApplyBehavior(f, plan.horseArchersPhasePrepare);
                                }
                                else if (state == PlanStateEnum.Ranged)
                                {
                                    ApplyBehavior(f, plan.horseArchersPhaseRanged);
                                }
                                else if (state == PlanStateEnum.Engage)
                                {
                                    ApplyBehavior(f, plan.horseArchersPhaseEngage);
                                }
                                else if (state == PlanStateEnum.Winning)
                                {
                                    ApplyBehavior(f, plan.horseArchersPhaseWinning);
                                }
                                else if (state == PlanStateEnum.Losing)
                                {
                                    ApplyBehavior(f, plan.horseArchersPhaseLosing);
                                }
                                break;
                            case FormationClass.Skirmisher:
                                if (state == PlanStateEnum.Prepare)
                                {
                                    ApplyBehavior(f, plan.skirmishersPhasePrepare);
                                }
                                else if (state == PlanStateEnum.Ranged)
                                {
                                    ApplyBehavior(f, plan.skirmishersPhaseRanged);
                                }
                                else if (state == PlanStateEnum.Engage)
                                {
                                    ApplyBehavior(f, plan.skirmishersPhaseEngage);
                                }
                                else if (state == PlanStateEnum.Winning)
                                {
                                    ApplyBehavior(f, plan.skirmishersPhaseWinning);
                                }
                                else if (state == PlanStateEnum.Losing)
                                {
                                    ApplyBehavior(f, plan.skirmishersPhaseLosing);
                                }
                                break;
                            case FormationClass.HeavyInfantry:
                                if (state == PlanStateEnum.Prepare)
                                {
                                    ApplyBehavior(f, plan.heavyInfantryPhasePrepare);
                                }
                                else if (state == PlanStateEnum.Ranged)
                                {
                                    ApplyBehavior(f, plan.heavyInfantryPhaseRanged);
                                }
                                else if (state == PlanStateEnum.Engage)
                                {
                                    ApplyBehavior(f, plan.heavyInfantryPhaseEngage);
                                }
                                else if (state == PlanStateEnum.Winning)
                                {
                                    ApplyBehavior(f, plan.heavyInfantryPhaseWinning);
                                }
                                else if (state == PlanStateEnum.Losing)
                                {
                                    ApplyBehavior(f, plan.heavyInfantryPhaseLosing);
                                }
                                break;
                            case FormationClass.LightCavalry:
                                if (state == PlanStateEnum.Prepare)
                                {
                                    ApplyBehavior(f, plan.lightCavalryPhasePrepare);
                                }
                                else if (state == PlanStateEnum.Ranged)
                                {
                                    ApplyBehavior(f, plan.lightCavalryPhaseRanged);
                                }
                                else if (state == PlanStateEnum.Engage)
                                {
                                    ApplyBehavior(f, plan.lightCavalryPhaseEngage);
                                }
                                else if (state == PlanStateEnum.Winning)
                                {
                                    ApplyBehavior(f, plan.lightCavalryPhaseWinning);
                                }
                                else if (state == PlanStateEnum.Losing)
                                {
                                    ApplyBehavior(f, plan.lightCavalryPhaseLosing);
                                }
                                break;
                            case FormationClass.HeavyCavalry:
                                if (state == PlanStateEnum.Prepare)
                                {
                                    ApplyBehavior(f, plan.heavyCavalryPhasePrepare);
                                }
                                else if (state == PlanStateEnum.Ranged)
                                {
                                    ApplyBehavior(f, plan.heavyCavalryPhaseRanged);
                                }
                                else if (state == PlanStateEnum.Engage)
                                {
                                    ApplyBehavior(f, plan.heavyCavalryPhaseEngage);
                                }
                                else if (state == PlanStateEnum.Winning)
                                {
                                    ApplyBehavior(f, plan.heavyCavalryPhaseWinning);
                                }
                                else if (state == PlanStateEnum.Losing)
                                {
                                    ApplyBehavior(f, plan.heavyCavalryPhaseLosing);
                                }
                                break;
                        }
                        //InformationManager.DisplayMessage(new InformationMessage(f.FormationIndex + "ai is" + f.IsAIControlled.ToString()));
                        //f.IsAIControlled = false;
                    }

                    if (positionCounter == positionLimit)
                    {
                        positionCounter++;
                    }


                }
            }
        }

        public void ApplyBehavior(Formation f, PlanOrderEnum order)
        {
            switch (order)
            {
                case PlanOrderEnum.Charge:
                    f.AI.ResetBehaviorWeights();
                    f.AI.SetBehaviorWeight<BehaviorCharge>(2f);
                    break;
                case PlanOrderEnum.HoldPosition:
                    f.AI.ResetBehaviorWeights();
                    f.AI.SetBehaviorWeight<BehaviorDefend>(2f);
                    break;
                case PlanOrderEnum.Flank:
                    f.AI.ResetBehaviorWeights();
                    f.AI.SetBehaviorWeight<BehaviorFlank>(2f);
                    break;
                case PlanOrderEnum.Skirmish:
                    if(f.FormationIndex == FormationClass.HorseArcher)
                    {
                        f.AI.ResetBehaviorWeights();
                        f.AI.SetBehaviorWeight<BehaviorHorseArcherSkirmish>(2f);
                    }
                    else
                    {
                        f.AI.ResetBehaviorWeights();
                        f.AI.SetBehaviorWeight<BehaviorSkirmishMode>(2f);
                    }
                    break;
                case PlanOrderEnum.HideBehind:
                    f.AI.ResetBehaviorWeights();
                    f.AI.SetBehaviorWeight<BehaviorHideBehind>(2f);
                    break;
                case PlanOrderEnum.ProtectFlank:
                    f.AI.ResetBehaviorWeights();
                    f.AI.SetBehaviorWeight<BehaviorProtectFlank>(2f);
                    break;
                case PlanOrderEnum.Advance:
                    f.AI.ResetBehaviorWeights();
                    f.AI.SetBehaviorWeight<BehaviorAdvance>(2f);
                    break;
                case PlanOrderEnum.CautiousAdvance:
                    f.AI.ResetBehaviorWeights();
                    f.AI.SetBehaviorWeight<BehaviorCautiousAdvance>(2f);
                    break;
            }
        }

        public PlanStateEnum GetMissionState()
        {
            PlanStateEnum result = PlanStateEnum.Prepare;

            if (Mission.Current != null)
            {
                if (Mission.Current.MainAgent != null)
                {
                    float alliedCasualityRatio = 0.0f;

                    int numberOfFormations = 0;

                    if (positionCounter < positionLimit)
                    {
                        positionCounter++;
                        return PlanStateEnum.Position;
                    }


                    if (!isEngaged)
                    {
                        foreach (Formation f in Mission.Current.MainAgent.Team.Formations)
                        {
                            Formation closestsFormation;
                            if (f.QuerySystem.ClosestEnemyFormation != null)
                            {
                                closestsFormation = f.QuerySystem.ClosestEnemyFormation.Formation;
                            }
                            else
                            {
                                return PlanStateEnum.Prepare;
                            }

                            if (closestsFormation != null)
                            {
                                float distance = f.QuerySystem.AveragePosition.Distance(closestsFormation.QuerySystem.AveragePosition);

                                if (distance < 20.0f)
                                {
                                    if (engageCounterStart == -1 && isEngaged == false)
                                    {
                                        engageCounterStart = MissionOverride.FrameCounter;
                                        isEngaged = true;
                                    }
                                    return PlanStateEnum.Engage;
                                }
                                else if (distance >= 20.0f && distance < 60.0f)
                                {
                                    return PlanStateEnum.Ranged;
                                }

                                alliedCasualityRatio += f.QuerySystem.CasualtyRatio;
                                numberOfFormations++;
                            }
                            else
                            {
                                return PlanStateEnum.Prepare;
                            }
                        }
                    }
                    else if (isEngaged && (MissionOverride.FrameCounter - engageCounterStart) > 2500)
                    {
                        List<Team> enemyTeams = (from t in Mission.Current.Teams where t.Side != Mission.Current.MainAgent.Team.Side select t).ToList<Team>();

                        List<Team> alliedTeams = (from t in Mission.Current.Teams where t.Side == Mission.Current.MainAgent.Team.Side select t).ToList<Team>();

                        float enemyCasualityRatio = 0.0f;

                        int numberOfEnemyFormations = 0;

                        float enemyPowerRatio = 0.0f;

                        float alliedPowerRatio = 0.0f;

                        foreach (Team te in enemyTeams)
                        {
                            if (te != null)
                            {
                                foreach (Formation fe in te.Formations)
                                {
                                    enemyCasualityRatio += fe.QuerySystem.CasualtyRatio;
                                    numberOfEnemyFormations++;
                                }
                                enemyPowerRatio += te.QuerySystem.TeamPower;
                            }
                            else
                            {
                                return PlanStateEnum.Prepare;
                            }
                        }

                        foreach (Team te in alliedTeams)
                        {
                            if (te != null)
                            {
                                alliedPowerRatio += te.QuerySystem.TeamPower;
                            }
                            else
                            {
                                return PlanStateEnum.Prepare;
                            }
                        }

                        float averageCasualities = alliedCasualityRatio / numberOfFormations;

                        float averageEnemyCasualities = enemyCasualityRatio / numberOfEnemyFormations;

                        /*if (averageCasualities > averageEnemyCasualities)
                        {
                            return PlanStateEnum.Losing;
                        }
                        else
                        {
                            return PlanStateEnum.Winning;
                        }*/

                        float averageAlliedPower = alliedPowerRatio / alliedTeams.Count;

                        float averageEnemyPower = enemyPowerRatio / enemyTeams.Count;

                        //InformationManager.DisplayMessage(new InformationMessage("ALLIED: " + averageAlliedPower.ToString()));

                        //InformationManager.DisplayMessage(new InformationMessage("ENEMY: " + averageEnemyPower.ToString()));

                        if ((averageEnemyPower * 0.75) > averageAlliedPower)
                        {
                            return PlanStateEnum.Losing;
                        }
                        else if ((averageAlliedPower * 0.75) > averageEnemyPower)
                        {
                            return PlanStateEnum.Winning;
                        }
                        else
                        {
                            return PlanStateEnum.Engage;
                        }
                    }
                    else
                    {
                        return PlanStateEnum.Prepare;
                    }
                } 
            }

            return result;
        }



        public Formation GetFormationPriority(Mission mission, Team team)
        {
            Formation infantry = team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.Infantry);
            Formation archers = team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.Ranged);
            Formation cavalry = team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.Cavalry);
            Formation horseArcher = team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.HorseArcher);
            Formation skirmisher = team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.Skirmisher);
            Formation heavyInfantry = team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.HeavyInfantry);
            Formation lightCavalry = team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.LightCavalry);
            Formation heavyCavalry = team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.HeavyInfantry);

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
                if (f != null)
                {
                    return f;
                }
            }

            return null;
        }


        private List<PositionData> Deserialize(string path)
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
    }
}
