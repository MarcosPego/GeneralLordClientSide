using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace GeneralLord.FormationBattleTest
{
    public class TacticHoldGeneric : TacticComponent
    {
		public TacticHoldGeneric(Team team) : base(team)
		{
			AssignTacticFormations1121();
		}

		private bool hasBattleBeenJoined()
		{
			return false;
		}

		protected override void ManageFormationCounts()
		{
			base.AssignTacticFormations1121();
		}

		protected override void TickOccasionally()
		{
			if (!base.AreFormationsCreated)
			{
				return;
			}
			else 
			{
				this._mainInfantry.AI.ResetBehaviorWeights();
				/*this._archers.AI.ResetBehaviorWeights();
				this._leftCavalry.AI.ResetBehaviorWeights();
				this._rightCavalry.AI.ResetBehaviorWeights();
				this._rangedCavalry.AI.ResetBehaviorWeights();*/

				this._mainInfantry.AI.SetBehaviorWeight<BehaviorDefend>(2f);
				/*this._archers.AI.SetBehaviorWeight<BehaviorDefend>(2f);
				this._leftCavalry.AI.SetBehaviorWeight<BehaviorDefend>(2f);
				this._rightCavalry.AI.SetBehaviorWeight<BehaviorDefend>(2f);
				this._rangedCavalry.AI.SetBehaviorWeight<BehaviorDefend>(2f);*/
			}

			base.TickOccasionally();
		}

		internal float GetTacticWeight()
		{
			return 10f;
		}

		internal static void SetDefaultBehaviorWeights(Formation formation)
		{
			formation.AI.SetBehaviorWeight<BehaviorCharge>(1f);
			formation.AI.SetBehaviorWeight<BehaviorPullBack>(1f);
			formation.AI.SetBehaviorWeight<BehaviorStop>(1f);
			formation.AI.SetBehaviorWeight<BehaviorReserve>(1f);
		}
	}
}
