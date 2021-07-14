using GeneralLordWebApiClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;

namespace GeneralLord
{
    public class OpponentSelectorViewModel : ViewModel
    {
        public OpponentSelectorViewModel(IEnumerable<Profile> opponentProfiles)
        {

            _opponentProfiles = opponentProfiles;
			this.Opponents = new MBBindingList<OpponentEntryTupleViewModel>();
			RefreshMembersList();
			RefreshValues();
		}

		public override void RefreshValues()
		{
			base.RefreshValues();
		}

		public void RefreshMembersList()
		{
			this.Opponents.Clear();
			foreach (Profile profile in _opponentProfiles)
			{
				//InformationManager.DisplayMessage(new InformationMessage(profile.Name.ToString()));
				this.Opponents.Add(new OpponentEntryTupleViewModel(profile));
			}
		}

		private void ExecuteLeave()
		{
			ScreenManager.PopScreen();
		}



		[DataSourceProperty]
		public MBBindingList<OpponentEntryTupleViewModel> Opponents
		{
			get
			{
				return this._opponents;
			}
			set
			{
				if (value != this._opponents)
				{
					this._opponents = value;
					base.OnPropertyChangedWithValue(value, "Opponents");
				}
			}
		}

		private MBBindingList<OpponentEntryTupleViewModel> _opponents;
		private IEnumerable<Profile> _opponentProfiles;
    }
}
