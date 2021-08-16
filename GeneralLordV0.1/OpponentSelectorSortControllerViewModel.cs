using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace GeneralLord
{
    public class OpponentSelectorSortControllerViewModel : ViewModel
	{
        private int _troopState;
        private int _eloState;
        private int _nameState;
        private int _troopStrengthState;
        private bool _isNameSelected;
        private bool _isEloSelected;
        private bool _isTroopSelected;
        private bool _isTroopStrengthSelected;
        private bool _isRankingScreen;
        private MBBindingList<OpponentEntryTupleViewModel> _listToControl;
        private NameComparer _nameComparer;
        private EloComparer _eloComparer;
        private TroopComparer _troopComparer;
        private TroopStrengthComparer _troopStrengthComparer;
        private string _nameSortText;
        private string _eloSortText;
        private string _troopStrengthSortText;
        private string _troopSortText;

        public OpponentSelectorSortControllerViewModel(ref MBBindingList<OpponentEntryTupleViewModel> listToControl, bool isRankingScreen = false)
		{
			this._isRankingScreen = isRankingScreen;

			this._listToControl = listToControl;
			this._nameComparer = new OpponentSelectorSortControllerViewModel.NameComparer();
			this._nameComparer._isRankingScreen = isRankingScreen;
			this._eloComparer = new OpponentSelectorSortControllerViewModel.EloComparer();
			this._troopComparer = new OpponentSelectorSortControllerViewModel.TroopComparer();
			this._troopStrengthComparer = new OpponentSelectorSortControllerViewModel.TroopStrengthComparer();

			this.EloSortText = new TextObject("{=ATEloText}Elo", null).ToString();
			this.NameSortText = new TextObject("{=ATEloText}Name", null).ToString();
			this.TroopSortText = new TextObject("{=ATEloText}Troop Count", null).ToString();
			this.TroopStrengthSortText = new TextObject("{=ATEloText}Army Strength", null).ToString();


			this.RefreshValues();
		}

		private enum SortState
		{
			Default,
			Ascending,
			Descending
		}

		public void SortByDefaultState()
		{
			this.ExecuteSortByName();
		}

		public void SortByCurrentState()
		{
			if (this.IsNameSelected)
			{
				this._listToControl.Sort(this._nameComparer);
				return;
			}
			if (this.IsEloSelected)
			{
				this._listToControl.Sort(this._eloComparer);
				return;
			}
			if (this.IsTroopSelected)
			{
				this._listToControl.Sort(this._troopComparer);
				return;
			}
			if (this.IsTroopStrengthSelected)
			{
				this._listToControl.Sort(this._troopStrengthComparer);
			}
		}

		public void ExecuteSortByName()
		{
			int nameState = this.NameState;
			this.SetAllStates(OpponentSelectorSortControllerViewModel.SortState.Default);
			this.NameState = (nameState + 1) % 3;
			if (this.NameState == 0)
			{
				this.NameState++;
			}
			this._nameComparer.SetSortMode(this.NameState == 1);
			this._listToControl.Sort(this._nameComparer);
			this.IsNameSelected = true;
		}


		public void ExecuteSortByElo()
		{
			int typeState = this.EloState;
			this.SetAllStates(OpponentSelectorSortControllerViewModel.SortState.Default);
			this.EloState = (typeState + 1) % 3;
			if (this.EloState == 0)
			{
				this.EloState++;
			}
			this._eloComparer.SetSortMode(this.EloState == 1);
			this._listToControl.Sort(this._eloComparer);
			this.IsEloSelected = true;
		}


		public void ExecuteSortByTroop()
		{
			int troopState = this.TroopState;
			this.SetAllStates(OpponentSelectorSortControllerViewModel.SortState.Default);
			this.TroopState = (troopState + 1) % 3;
			if (this.TroopState == 0)
			{
				this.TroopState++;
			}
			this._troopComparer.SetSortMode(this.TroopState == 1);
			this._listToControl.Sort(this._troopComparer);
			this.IsTroopSelected = true;
		}


		public void ExecuteSortByTroopStrength()
		{
			int troopStrengthState = this.TroopStrengthState;
			this.SetAllStates(OpponentSelectorSortControllerViewModel.SortState.Default);
			this.TroopStrengthState = (troopStrengthState + 1) % 3;
			if (this.TroopStrengthState == 0)
			{
				this.TroopStrengthState++;
			}
			this._troopStrengthComparer.SetSortMode(this.TroopStrengthState == 1);
			this._listToControl.Sort(this._troopStrengthComparer);
			this.IsTroopStrengthSelected = true;
		}

		private void SetAllStates(OpponentSelectorSortControllerViewModel.SortState state)
		{
			this.NameState = (int)state;
			this.EloState = (int)state;
			this.TroopState = (int)state;
			this.TroopStrengthState = (int)state;
			this.IsNameSelected = false;
			this.IsEloSelected = false;
			this.IsTroopSelected = false;
			this.IsTroopStrengthSelected = false;
		}


		private abstract class ItemComparer : IComparer<OpponentEntryTupleViewModel>
		{
			public void SetSortMode(bool isAscending)
			{
				this._isAscending = isAscending;
			}
			public abstract int Compare(OpponentEntryTupleViewModel x, OpponentEntryTupleViewModel y);

			protected int ResolveEquality(OpponentEntryTupleViewModel x, OpponentEntryTupleViewModel y)
			{

				return x.Name.CompareTo(y.Name);
			}
			protected bool _isAscending;

			public bool _isRankingScreen = false;
		}

		[DataSourceProperty]
		public int NameState
		{
			get
			{
				return this._nameState;
			}
			set
			{
				if (value != this._nameState)
				{
					this._nameState = value;
					base.OnPropertyChangedWithValue(value, "NameState");
				}
			}
		}


		[DataSourceProperty]
		public int EloState
		{
			get
			{
				return this._eloState;
			}
			set
			{
				if (value != this._eloState)
				{
					this._eloState = value;
					base.OnPropertyChangedWithValue(value, "EloState");
				}
			}
		}


		[DataSourceProperty]
		public int TroopState
		{
			get
			{
				return this._troopState;
			}
			set
			{
				if (value != this._troopState)
				{
					this._troopState = value;
					base.OnPropertyChangedWithValue(value, "TroopState");
				}
			}
		}


		[DataSourceProperty]
		public int TroopStrengthState
		{
			get
			{
				return this._troopStrengthState;
			}
			set
			{
				if (value != this._troopStrengthState)
				{
					this._troopStrengthState = value;
					base.OnPropertyChangedWithValue(value, "TroopStrengthState");
				}
			}
		}

		[DataSourceProperty]
		public bool IsNameSelected
		{
			get
			{
				return this._isNameSelected;
			}
			set
			{
				if (value != this._isNameSelected)
				{
					this._isNameSelected = value;
					base.OnPropertyChangedWithValue(value, "IsNameSelected");
				}
			}
		}


		[DataSourceProperty]
		public bool IsEloSelected
		{
			get
			{
				return this._isEloSelected;
			}
			set
			{
				if (value != this._isEloSelected)
				{
					this._isEloSelected = value;
					base.OnPropertyChangedWithValue(value, "IsEloSelected");
				}
			}
		}


		[DataSourceProperty]
		public bool IsTroopSelected
		{
			get
			{
				return this._isTroopSelected;
			}
			set
			{
				if (value != this._isTroopSelected)
				{
					this._isTroopSelected = value;
					base.OnPropertyChangedWithValue(value, "IsTroopSelected");
				}
			}
		}


		[DataSourceProperty]
		public bool IsTroopStrengthSelected
		{
			get
			{
				return this._isTroopStrengthSelected;
			}
			set
			{
				if (value != this._isTroopStrengthSelected)
				{
					this._isTroopStrengthSelected = value;
					base.OnPropertyChangedWithValue(value, "IsTroopStrengthSelected");
				}
			}
		}


		[DataSourceProperty]
		public string NameSortText
		{
			get
			{
				return this._nameSortText;
			}
			set
			{
				if (value != this._nameSortText)
				{
					this._nameSortText = value;
					base.OnPropertyChangedWithValue(value, "NameSortText");
				}
			}
		}

		[DataSourceProperty]
		public string EloSortText
		{
			get
			{
				return this._eloSortText;
			}
			set
			{
				if (value != this._eloSortText)
				{
					this._eloSortText = value;
					base.OnPropertyChangedWithValue(value, "EloSortText");
				}
			}
		}

		[DataSourceProperty]
		public string TroopStrengthSortText
		{
			get
			{
				return this._troopStrengthSortText;
			}
			set
			{
				if (value != this._troopStrengthSortText)
				{
					this._troopStrengthSortText = value;
					base.OnPropertyChangedWithValue(value, "TroopStrengthSortText");
				}
			}
		}

		[DataSourceProperty]
		public string TroopSortText
		{
			get
			{
				return this._troopSortText;
			}
			set
			{
				if (value != this._troopSortText)
				{
					this._troopSortText = value;
					base.OnPropertyChangedWithValue(value, "TroopSortText");
				}
			}
		}


		private class NameComparer : OpponentSelectorSortControllerViewModel.ItemComparer
		{
			public override int Compare(OpponentEntryTupleViewModel x, OpponentEntryTupleViewModel y)
			{


				if (_isRankingScreen)
                {
					int xNumber = (int) Int32.Parse(x.Name.Split(':')[0]);
					int yNumber = (int) Int32.Parse(y.Name.Split(':')[0]);

					//InformationManager.DisplayMessage(new InformationMessage(x.Name.Split(';')[0]));
					//InformationManager.DisplayMessage(new InformationMessage(y.Name.Split(';')[0]));

					//int yNumber = 0;
					//int xNumber = 1;
					if (this._isAscending)
					{
						return yNumber.CompareTo(xNumber) * -1;
					}
					return yNumber.CompareTo(xNumber);
				}
				else
                {
					if (this._isAscending)
					{
						return y.Name.CompareTo(x.Name) * -1;
					}
					return y.Name.CompareTo(x.Name);
				}


			}
		}

		private class EloComparer : OpponentSelectorSortControllerViewModel.ItemComparer
		{
			public override int Compare(OpponentEntryTupleViewModel x, OpponentEntryTupleViewModel y)
			{
				int num = y.Elo.CompareTo(x.Elo);
				if (num != 0)
				{
					return num * (this._isAscending ? -1 : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		private class TroopComparer : OpponentSelectorSortControllerViewModel.ItemComparer
		{
			public override int Compare(OpponentEntryTupleViewModel x, OpponentEntryTupleViewModel y)
			{
				int num = y._troopCount.CompareTo(x._troopCount);
				if (num != 0)
				{
					return num * (this._isAscending ? -1 : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}

		private class TroopStrengthComparer : OpponentSelectorSortControllerViewModel.ItemComparer
		{
			public override int Compare(OpponentEntryTupleViewModel x, OpponentEntryTupleViewModel y)
			{
				int num = y._armyStrengthRatio.CompareTo(x._armyStrengthRatio);
				if (num != 0)
				{
					return num * (this._isAscending ? -1 : 1);
				}
				return base.ResolveEquality(x, y);
			}
		}
	}
}
