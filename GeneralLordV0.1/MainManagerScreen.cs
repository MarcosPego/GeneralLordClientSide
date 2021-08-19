using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.TwoDimension;
using TaleWorlds.MountAndBlade.View.Screen;
using TaleWorlds.Engine;

using SandBox.View.Map;


namespace GeneralLord
{
	public class MainManagerScreen : ScreenBase, IGameStateListener
	{
		public MainManagerScreen()
		{
		}

		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._viewModel = new MainManagerViewModel();
			this._gauntletLayer = new GauntletLayer(1, "GauntletLayer");
			this._gauntletLayer.LoadMovie("MainViewer", this._viewModel);
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, TaleWorlds.Library.InputUsageMask.All);
			base.AddLayer(this._gauntletLayer);

		}

		void IGameStateListener.OnActivate()
		{
		}

		void IGameStateListener.OnDeactivate()
		{
		}

		void IGameStateListener.OnInitialize()
		{
		}

		void IGameStateListener.OnFinalize()
		{
		}

		protected override void OnActivate()
		{
			base.OnActivate();

			SpriteData spriteData = UIResourceManager.SpriteData;
			TwoDimensionEngineResourceContext resourceContext = UIResourceManager.ResourceContext;
			ResourceDepot uiresourceDepot = UIResourceManager.UIResourceDepot;
			this._clanCategory = spriteData.SpriteCategories["ui_clan"];
			this._clanCategory.Load(resourceContext, uiresourceDepot);
			this._partyscreenCategory = spriteData.SpriteCategories["ui_partyscreen"];
			this._partyscreenCategory.Load(resourceContext, uiresourceDepot);
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
			this._clanCategory.Unload();
			this._partyscreenCategory.Unload();
			base.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._viewModel = null;
		}

		private GauntletLayer _gauntletLayer;
		public MainManagerViewModel _viewModel;
		//private PartyManagerLogic _partyManagerLogic;
		private SpriteCategory _partyscreenCategory;
		private SpriteCategory _clanCategory;

		private MapNavigationHandler _mapNavigationHandler;
	}
}
