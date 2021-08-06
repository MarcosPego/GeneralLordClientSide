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

using GeneralLord.HarmonyOverrides;
using GeneralLord.FormationBattleTest;

namespace CunningLords.Behaviors
{
    class BehaviorSkirmishMode : BehaviorComponent
    {
        private Formation mainFormation;
        public BehaviorSkirmishMode(Formation formation) : base(formation)
        {
            this.mainFormation = formation.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.Infantry);
            this.CalculateCurrentOrder();
        }
        
        protected override void CalculateCurrentOrder()
        {
            List<Tuple<Formation, float>> distances = Utils.GetDistanceFromAllEnemies(base.Formation);

            if (distances == null)
            {
                return;
            }

            List<Formation> tooCloseForConfort = new List<Formation>();

            foreach (Tuple<Formation, float> tup in distances)
            {
                if (tup.Item2 < (0.6 * base.Formation.QuerySystem.MissileRange))
                {
                    tooCloseForConfort.Add(tup.Item1);
                }
            }

            Vec2 escapeVector = new Vec2(0, 0);

            Vec2 targetDirection = new Vec2(0, 0);

            if (base.Formation == null || base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation == null)
            {
                return;
            }

            if (tooCloseForConfort.Count > 1) //Too close from more than 1 formation
            {
                //InformationManager.DisplayMessage(new InformationMessage(base.Formation.FormationIndex.ToString() + ": Kiting " + tooCloseForConfort.Count.ToString() + " enemies"));

                foreach (Formation f in tooCloseForConfort)
                {
                    escapeVector = Utils.AddVec2(escapeVector, base.Formation.QuerySystem.AveragePosition - f.QuerySystem.AveragePosition);
                }

                escapeVector = escapeVector.Normalized();

                escapeVector = Utils.MultVec2(5, escapeVector);

                escapeVector = Utils.AddVec2(escapeVector, base.Formation.QuerySystem.AveragePosition);

                targetDirection = -escapeVector.Normalized();
            }
            else if (tooCloseForConfort.Count == 1) //Too close too one formation
            {
                //InformationManager.DisplayMessage(new InformationMessage(base.Formation.FormationIndex.ToString() + ": Kiting 1 enemiy"));

                escapeVector = base.Formation.QuerySystem.AveragePosition - tooCloseForConfort.First().QuerySystem.AveragePosition;

                escapeVector = Utils.MultVec2(5, escapeVector);

                escapeVector = Utils.AddVec2(escapeVector, base.Formation.QuerySystem.AveragePosition);

                targetDirection = -escapeVector.Normalized();
            }
            else //No formations close, must approach
            {
                //InformationManager.DisplayMessage(new InformationMessage(base.Formation.FormationIndex.ToString() + ": approach enemy"));

                escapeVector = base.Formation.QuerySystem.AveragePosition - base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.MedianPosition.AsVec2;

                escapeVector = escapeVector.Normalized();

                escapeVector = Utils.MultVec2((base.Formation.QuerySystem.MissileRange * 0.75f), escapeVector);

                escapeVector = Utils.AddVec2(base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.MedianPosition.AsVec2, escapeVector);

                targetDirection = escapeVector.Normalized();
            }

            WorldPosition position = this.Formation.QuerySystem.MedianPosition;
            position.SetVec2(escapeVector);
            base.CurrentOrder = MovementOrder.MovementOrderMove(position);

            this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(targetDirection);
        }

        public override void TickOccasionally()
        {
            this.CalculateCurrentOrder();
            base.Formation.SetMovementOrder(base.CurrentOrder);
            base.Formation.FacingOrder = this.CurrentFacingOrder;
        }

        protected override void OnBehaviorActivatedAux()
        {
            this.CalculateCurrentOrder();
            base.Formation.SetMovementOrder(base.CurrentOrder);
            base.Formation.FacingOrder = this.CurrentFacingOrder;
            base.Formation.ArrangementOrder = ArrangementOrder.ArrangementOrderLoose;
            base.Formation.FiringOrder = FiringOrder.FiringOrderFireAtWill;
            base.Formation.FormOrder = FormOrder.FormOrderWide;
            base.Formation.WeaponUsageOrder = WeaponUsageOrder.WeaponUsageOrderUseAny;
        }

        /*
        private void ExecuteActions()
        {
            Formation mainThreat = null;

            mainThreat = Utils.GetSkirmishersGreatestEnemy(this.Formation); //Careful if null

            List<Tuple<Formation, float>> distances = Utils.GetDistanceFromAllEnemies(this.Formation);

            List<Formation> tooCloseForConfort = new List<Formation>();

            foreach (Tuple<Formation, float> tup in distances)
            {
                if (tup.Item2 < (0.6 * this.Formation.QuerySystem.MissileRange))
                {
                    tooCloseForConfort.Add(tup.Item1);
                }
            }

            Vec2 escapeVector = new Vec2(0, 0);

            Vec2 targetDirection = new Vec2(0, 0);

            if (tooCloseForConfort.Count > 1) //Too close from more than 1 formation
            {
                //InformationManager.DisplayMessage(new InformationMessage(this.Formation.FormationIndex.ToString() + ": Kiting " + tooCloseForConfort.Count.ToString() + " enemies"));

                foreach (Formation f in tooCloseForConfort)
                {
                    escapeVector = Utils.AddVec2(escapeVector, this.Formation.QuerySystem.AveragePosition - f.QuerySystem.AveragePosition);
                }

                escapeVector = escapeVector.Normalized();

                escapeVector = Utils.MultVec2(5, escapeVector);

                escapeVector = Utils.AddVec2(escapeVector, this.Formation.QuerySystem.AveragePosition);

                targetDirection = -escapeVector.Normalized();
            }
            else if (tooCloseForConfort.Count == 1) //Too close too one formation
            {
                //InformationManager.DisplayMessage(new InformationMessage(this.Formation.FormationIndex.ToString() + ": Kiting 1 enemiy"));

                escapeVector = this.Formation.QuerySystem.AveragePosition - tooCloseForConfort.First().QuerySystem.AveragePosition;

                escapeVector = Utils.MultVec2(5, escapeVector);

                escapeVector = Utils.AddVec2(escapeVector, this.Formation.QuerySystem.AveragePosition);

                targetDirection = -escapeVector.Normalized();
            }
            else //No formations close, must approach
            {
                //InformationManager.DisplayMessage(new InformationMessage(this.Formation.FormationIndex.ToString() + ": approach enemy"));

                escapeVector = this.Formation.QuerySystem.AveragePosition - mainThreat.QuerySystem.AveragePosition;

                escapeVector = escapeVector.Normalized();

                escapeVector = Utils.MultVec2((this.Formation.QuerySystem.MissileRange * 0.75f), escapeVector);

                escapeVector = Utils.AddVec2(mainThreat.QuerySystem.AveragePosition, escapeVector);

                targetDirection = -escapeVector.Normalized();
            }

            WorldPosition position = this.Formation.QuerySystem.MedianPosition;
            position.SetVec2(escapeVector);
            this.Formation.MovementOrder = MovementOrder.MovementOrderMove(position);

            this.Formation.FacingOrder = FacingOrder.FacingOrderLookAtDirection(targetDirection);
            MaintainPersistentOrders();
        }

        private void MaintainPersistentOrders()
        {
            this.Formation.ArrangementOrder = ArrangementOrder.ArrangementOrderLoose;
            this.Formation.FiringOrder = FiringOrder.FiringOrderFireAtWill;
            this.Formation.FormOrder = FormOrder.FormOrderWide;
            this.Formation.WeaponUsageOrder = WeaponUsageOrder.WeaponUsageOrderUseAny;
        }

        protected override void TickOccasionally()
        {
            ExecuteActions();
        }

        protected override void OnBehaviorActivatedAux()
        {
        }*/

        protected override float GetAiWeight()
        {
            return 1f;
        }
    }
}
