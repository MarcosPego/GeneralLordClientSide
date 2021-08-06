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

namespace CunningLords.Behaviors
{
    class BehaviorHideBehind : BehaviorComponent
    {
        private Formation mainFormation;

        public BehaviorHideBehind(Formation formation) : base(formation)
        {
            this.mainFormation = formation.Team.Formations.FirstOrDefault((Formation f) => f.FormationIndex == FormationClass.Infantry);
            this.CalculateCurrentOrder();
        }

        protected override void CalculateCurrentOrder()
        {
            if (mainFormation != null)
            {
                Vec2 escapeVector;

                Vec2 focusedPosition = mainFormation.QuerySystem.AveragePosition;

                Vec2 focusedDirection = mainFormation.Direction.Normalized();

                escapeVector = focusedPosition - (focusedDirection * 4 * (mainFormation.Depth + mainFormation.Depth));

                WorldPosition position = this.Formation.QuerySystem.MedianPosition;
                position.SetVec2(escapeVector);
                base.CurrentOrder = MovementOrder.MovementOrderMove(position);

                this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(focusedDirection);
            }
            else
            {
                base.CurrentOrder = MovementOrder.MovementOrderStop;

                this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
            }
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

        protected override float GetAiWeight()
        {
            return 1f;
        }
    }
}
