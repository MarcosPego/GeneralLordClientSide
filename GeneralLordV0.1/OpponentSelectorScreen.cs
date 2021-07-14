﻿using GeneralLordWebApiClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace GeneralLord
{
    public class OpponentSelectorScreen : ScreenBase
	{

		public OpponentSelectorScreen(IEnumerable<Profile> opponentProfiles)
        {
			_OpponentProfiles = opponentProfiles;
		}

		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._viewModel = new OpponentSelectorViewModel(_OpponentProfiles);
			this._gauntletLayer = new GauntletLayer(1, "GauntletLayer");
			this._gauntletLayer.LoadMovie("OpponentSelector", this._viewModel);
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, TaleWorlds.Library.InputUsageMask.All);
			base.AddLayer(this._gauntletLayer);

		}

		protected override void OnActivate()
		{
			base.OnActivate();

			SpriteData spriteData = UIResourceManager.SpriteData;
			TwoDimensionEngineResourceContext resourceContext = UIResourceManager.ResourceContext;
			ResourceDepot uiresourceDepot = UIResourceManager.UIResourceDepot;
			this._clanCategory = spriteData.SpriteCategories["ui_clan"];
			this._clanCategory.Load(resourceContext, uiresourceDepot);
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
			base.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._viewModel = null;
		}


		private SpriteCategory _clanCategory;
		private GauntletLayer _gauntletLayer;
		private OpponentSelectorViewModel _viewModel;
		private IEnumerable<Profile> _OpponentProfiles;
	}
}
