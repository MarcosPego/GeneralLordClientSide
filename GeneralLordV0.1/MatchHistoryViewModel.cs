using GeneralLordWebApiClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;

namespace GeneralLord
{
    public class MatchHistoryViewModel : ViewModel
    {
        public MatchHistoryViewModel(IEnumerable<MatchHistory> completeMatchHistory)
        {
            _completeMatchHistory = completeMatchHistory;
            this.MatchHistory = new MBBindingList<MatchHistoryEntryViewModel>();
            RefreshMatchHistoryList();
            RefreshValues();
        }


        public override void RefreshValues()
        {
            base.RefreshValues();
        }

        public void RefreshMatchHistoryList()
        {
            this.MatchHistory.Clear();
            var reversedMatchHistoryList = _completeMatchHistory.Reverse();
            foreach (MatchHistory matchHistory in reversedMatchHistoryList)
            {
                //InformationManager.DisplayMessage(new InformationMessage(profile.Name.ToString()));
                this.MatchHistory.Add(new MatchHistoryEntryViewModel(matchHistory));
            }
        }

        private void ExecuteLeave()
        {
            ScreenManager.PopScreen();
        }

        [DataSourceProperty]
        public MBBindingList<MatchHistoryEntryViewModel> MatchHistory
        {
            get
            {
                return this._matchHistories;
            }
            set
            {
                if (value != this._matchHistories)
                {
                    this._matchHistories = value;
                    base.OnPropertyChangedWithValue(value, "MatchHistory");
                }
            }
        }

        private MBBindingList<MatchHistoryEntryViewModel> _matchHistories;
        private IEnumerable<MatchHistory> _completeMatchHistory;

    }
}
