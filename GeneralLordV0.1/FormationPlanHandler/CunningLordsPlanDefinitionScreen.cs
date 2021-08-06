using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using CunningLords.Interaction;

namespace CunningLords.Interaction
{
    public class CunningLordsPlanDefinitionScreen : ScreenBase
    {

        public CunningLordsPlanDefinitionScreen(bool isAttackerParty = true)
        {

            _isAttackerParty = isAttackerParty;



        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            this._viewModel = new CunningLordsPlanViewModel(_isAttackerParty);
            this._gauntletLayer = new GauntletLayer(1, "GauntletLayer");
            this._gauntletLayer.LoadMovie("PlanInterface", this._viewModel);
            this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            base.AddLayer(this._gauntletLayer);
        }


        private bool _isAttackerParty;
        private GauntletLayer _gauntletLayer;

        private CunningLordsPlanViewModel _viewModel;
    }
}
