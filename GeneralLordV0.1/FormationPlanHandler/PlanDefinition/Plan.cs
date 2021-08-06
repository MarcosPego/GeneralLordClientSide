using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CunningLords.PlanDefinition
{
    public class Plan
    {
        public PlanOrderEnum infantryPhasePrepare = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum infantryPhaseRanged = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum infantryPhaseEngage = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum infantryPhaseWinning = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum infantryPhaseLosing = PlanOrderEnum.HoldPosition;

        public PlanOrderEnum archersPhasePrepare = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum archersPhaseRanged = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum archersPhaseEngage = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum archersPhaseWinning = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum archersPhaseLosing = PlanOrderEnum.HoldPosition;

        public PlanOrderEnum cavalryPhasePrepare = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum cavalryPhaseRanged = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum cavalryPhaseEngage = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum cavalryPhaseWinning = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum cavalryPhaseLosing = PlanOrderEnum.HoldPosition;

        public PlanOrderEnum horseArchersPhasePrepare = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum horseArchersPhaseRanged = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum horseArchersPhaseEngage = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum horseArchersPhaseWinning = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum horseArchersPhaseLosing = PlanOrderEnum.HoldPosition;

        public PlanOrderEnum skirmishersPhasePrepare = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum skirmishersPhaseRanged = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum skirmishersPhaseEngage = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum skirmishersPhaseWinning = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum skirmishersPhaseLosing = PlanOrderEnum.HoldPosition;

        public PlanOrderEnum heavyInfantryPhasePrepare = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum heavyInfantryPhaseRanged = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum heavyInfantryPhaseEngage = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum heavyInfantryPhaseWinning = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum heavyInfantryPhaseLosing = PlanOrderEnum.HoldPosition;

        public PlanOrderEnum lightCavalryPhasePrepare = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum lightCavalryPhaseRanged = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum lightCavalryPhaseEngage = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum lightCavalryPhaseWinning = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum lightCavalryPhaseLosing = PlanOrderEnum.HoldPosition;

        public PlanOrderEnum heavyCavalryPhasePrepare = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum heavyCavalryPhaseRanged = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum heavyCavalryPhaseEngage = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum heavyCavalryPhaseWinning = PlanOrderEnum.HoldPosition;
        public PlanOrderEnum heavyCavalryPhaseLosing = PlanOrderEnum.HoldPosition;
    }
}
