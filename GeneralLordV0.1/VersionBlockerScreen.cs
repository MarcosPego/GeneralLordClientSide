using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;

namespace GeneralLord
{
    public class VersionBlockerScreen : ScreenBase
    {
        public VersionBlockerScreen()
        {

        }

		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._viewModel = new VersionBlockerViewModel();
			this._gauntletLayer = new GauntletLayer(1, "GauntletLayer");
			this._gauntletLayer.LoadMovie("VersionBlocker", this._viewModel);
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, TaleWorlds.Library.InputUsageMask.All);
			base.AddLayer(this._gauntletLayer);

		}

		protected override void OnActivate()
		{
			base.OnActivate();
			ScreenManager.TrySetFocus(_gauntletLayer);
			LoadingWindow.DisableGlobalLoadingWindow();
		}

		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			_gauntletLayer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(_gauntletLayer);
		}

		protected override void OnFinalize()
		{
			base.OnFinalize();
			base.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._viewModel = null;
		}



		private GauntletLayer _gauntletLayer;
		private VersionBlockerViewModel _viewModel;

	}
}
