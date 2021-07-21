using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade.View.Missions;
using SandBox.View.Missions;
using SandBox.ViewModelCollection;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.Data;


namespace GeneralLord.FormationBattleTest.BattleTestCustomView
{
	[OverrideView(typeof(BattleTestEmptyView))]
	public class BattleTestView : MissionView
	{
		public override void OnMissionScreenInitialize() 
		{
			base.OnMissionScreenInitialize();
			this._dataSource = new BattleTestViewModel();
			this._gauntletLayer = new GauntletLayer(this.ViewOrderPriorty, "GauntletLayer", false);
			this._movie = this._gauntletLayer.LoadMovie("MissionBattleTest", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
		}

		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
		}

		public override void OnMissionScreenFinalize()
		{
			this._dataSource.OnFinalize();
			this._gauntletLayer.ReleaseMovie(this._movie);
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			base.OnMissionScreenFinalize();
		}

		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			this._gauntletLayer._gauntletUIContext.ContextAlpha = 0f;
		}

		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			this._gauntletLayer._gauntletUIContext.ContextAlpha = 1f;
		}

		private BattleTestViewModel _dataSource;

		private GauntletLayer _gauntletLayer;

		private IGauntletMovie _movie;
	}
}
