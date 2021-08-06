using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace GeneralLord.FormationBattleTest
{
    class PositionData
    {

        public ArrangementOrder.ArrangementOrderEnum InfantryArrangementOrder = ArrangementOrder.ArrangementOrderEnum.Line;

        public float ArchersXOffset { get; set; } = 0f;

        public float ArchersYOffset { get; set; } = -20f;

        public ArrangementOrder.ArrangementOrderEnum ArchersArrangementOrder = ArrangementOrder.ArrangementOrderEnum.Line;

        public float CavalryXOffset { get; set; } = -20f;

        public float CavalryYOffset { get; set; } = 0f;

        public ArrangementOrder.ArrangementOrderEnum CavalryArrangementOrder = ArrangementOrder.ArrangementOrderEnum.Line;

        public float HorseArchersXOffset { get; set; } = 20f;

        public float HorseArchersYOffset { get; set; } = 0f;

        public ArrangementOrder.ArrangementOrderEnum HorseArchersArrangementOrder = ArrangementOrder.ArrangementOrderEnum.Line;

        public float SkirmisherXOffset { get; set; } = 0f;

        public float SkirmisherYOffset { get; set; } = 0f;

        public ArrangementOrder.ArrangementOrderEnum SkirmisherArrangementOrder = ArrangementOrder.ArrangementOrderEnum.Line;

        public float HeavyInfantryXOffset { get; set; } = 0f;

        public float HeavyInfantryYOffset { get; set; } = 0f;

        public ArrangementOrder.ArrangementOrderEnum HeavyInfantryArrangementOrder = ArrangementOrder.ArrangementOrderEnum.Line;

        public float LightCavalryXOffset { get; set; } = 0f;

        public float LightCavalryYOffset { get; set; } = 0f;

        public ArrangementOrder.ArrangementOrderEnum LightCavalryArrangementOrder = ArrangementOrder.ArrangementOrderEnum.Line;

        public float HeavyCavalryXOffset { get; set; } = 0f;

        public float HeavyCavalryYOffset { get; set; } = 0f;

        public ArrangementOrder.ArrangementOrderEnum HeavyCavalryArrangementOrder = ArrangementOrder.ArrangementOrderEnum.Line;

    }
}
