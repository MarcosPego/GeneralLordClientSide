using GeneralLordWebApiClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace GeneralLord
{
    public class OpponentEntryTupleViewModel : ViewModel
    {
		public OpponentEntryTupleViewModel(Profile profile)
		{
			this.Name = profile.Name;
			this.Elo = "Elo: " + profile.Elo.ToString();
			this.ArmyStrength = GetAverageStrength(profile);
			this.TotalArmyCount = "Troop Count: " + profile.TotalTroopCount.ToString();

			this.RefreshValues();
		}

		public string GetAverageStrength(Profile profile)
        {
			float ratio = profile.ArmyStrength / PartyBase.MainParty.TotalStrength;

			if(ratio < 0.5f)
            {
				return "Weaker Army";
            }
			if(ratio > 1.7f)
            {
				return "Stronger Army";
			}

			return "Similar Army";
		}

		public override void RefreshValues()
		{
			base.RefreshValues();

		}

		public override void OnFinalize()
		{
			base.OnFinalize();
		}

		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue(value, "Name");
				}
			}
		}

		[DataSourceProperty]
		public string Elo
		{
			get
			{
				return this._elo;
			}
			set
			{
				if (value != this._elo)
				{
					this._elo = value;
					base.OnPropertyChangedWithValue(value, "Elo");
				}
			}
		}

		[DataSourceProperty]
		public string ArmyStrength
		{
			get
			{
				return this._armyStrength;
			}
			set
			{
				if (value != this._armyStrength)
				{
					this._armyStrength = value;
					base.OnPropertyChangedWithValue(value, "ArmyStrength");
				}
			}
		}

		[DataSourceProperty]
		public string TotalArmyCount
		{
			get
			{
				return this._totalArmyCount;
			}
			set
			{
				if (value != this._totalArmyCount)
				{
					this._totalArmyCount = value;
					base.OnPropertyChangedWithValue(value, "TotalArmyCount");
				}
			}
		}

		private string _name;
		private string _elo;
		private string _armyStrength;
		private string _totalArmyCount;
	}
}
