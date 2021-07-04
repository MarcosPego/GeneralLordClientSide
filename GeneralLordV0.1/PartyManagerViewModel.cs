using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.Core;

using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.InputSystem;
using TaleWorlds.CampaignSystem;

namespace GeneralLord
{
    internal class PartyManagerViewModel : ViewModel
    {

        public PartyManagerViewModel(PartyManagerLogic partyManagerLogic)
        {
            this._partyManagerLogic = partyManagerLogic;
            this._partyManager = new PartyManager();

            this._target = Formation.FormationA;
            this._formationASelected = true;


            this._doneText = new TextObject("{=ATDone}Done", null).ToString();
            this._cancelText = new TextObject("{=ATCancel}Cancel", null).ToString();
            this._leftSidePartyTitle = new TextObject("{=ATLeftSidePartyTitle} Left Party", null).ToString();
            this._rightSidePartyTitle = new TextObject("{=ATRightSidePartyTitle} Right Party", null).ToString();


            this._formationATitle = new TextObject("{=ATFormationATitle} Formation A", null).ToString();
            this._formationBTitle = new TextObject("{=ATFormationBTitle} Formation B", null).ToString();
            this._formationCTitle = new TextObject("{=ATFormationCTitle} Formation C", null).ToString();
            this._formationDTitle = new TextObject("{=ATFormationDTitle} Formation D", null).ToString();


            //this._leftPartyHeader = this._partyManagerLogic.RightSideRoster[0].GetCharacterAtIndex(0).GetName().ToString();


            this._rightPartyHeader = new TextObject("{=ATRightPartyHeader} Total Army Size", null).ToString();

            this._areMembersRelevantOnCurrentMode = true;

            this._isDoneDisabled = false;
            this._isCancelDisabled = false;


            this._rightParty = new MBBindingList<TroopVM>();
            this._formationsArmy = new MBBindingList<TroopVM>[4];
            //this._formationsArmy[0] = new MBBindingList<TroopVM>();
            //this._formationsArmy[1] = new MBBindingList<TroopVM>();
            //this._formationsArmy[2] = new MBBindingList<TroopVM>();

            for (int i = 0; i < this._formationsArmy.Length; i++)
            {
                this._formationsArmy[i] = new MBBindingList<TroopVM>();
            }
            InitializePartyList(this._rightParty, this._formationsArmy[(int) this._target], this._partyManagerLogic.RightSideRoster[0], PartyRosterSide.Right);
            //InitializePartyList(this._formationsArmy[0], this._rightParty, this._partyManagerLogic.LeftSideRoster[0], PartyRosterSide.Left);
            //InitializePartyList(this._formationsArmy[1], this._rightParty, this._partyManagerLogic.LeftSideRoster[1], PartyRosterSide.Left);
            //InitializePartyList(this._formationsArmy[2], this._rightParty, this._partyManagerLogic.LeftSideRoster[2], PartyRosterSide.Left);

            for (int i = 0; i < this._formationsArmy.Length; i++)
            {
                InitializePartyList(this._formationsArmy[i], this._rightParty, this._partyManagerLogic.LeftSideRoster[i], PartyRosterSide.Left);
            }

            this._config = new BattleGeneralConfig();
            this._config.UpdateArmyRosters(this._partyManagerLogic.LeftSideRoster, this._config.EnemyParty());

            this._formationASize = this._partyManagerLogic.LeftSideRoster[0].TotalManCount.ToString();
            this._formationBSize = this._partyManagerLogic.LeftSideRoster[1].TotalManCount.ToString();
            this._formationCSize = this._partyManagerLogic.LeftSideRoster[2].TotalManCount.ToString();
            this._formationDSize = this._partyManagerLogic.LeftSideRoster[3].TotalManCount.ToString();

            this._rightPartyHeader2 = this._partyManagerLogic.RightSideRoster[0].TotalManCount.ToString();



            this.RefreshValues();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            //InformationManager.DisplayMessage(new InformationMessage("Refresh"));



            this._formationASize = GetElementCount(this._formationsArmy[0]).ToString();
            this._formationBSize = GetElementCount(this._formationsArmy[1]).ToString();
            this._formationCSize = GetElementCount(this._formationsArmy[2]).ToString();
            this._formationDSize = GetElementCount(this._formationsArmy[3]).ToString();

            this._rightPartyHeader2 = GetElementCount(this._rightParty).ToString();



            base.OnPropertyChanged("FormationASize");
            base.OnPropertyChanged("FormationBSize");
            base.OnPropertyChanged("FormationCSize");
            base.OnPropertyChanged("FormationDSize");
            base.OnPropertyChanged("RightPartyHeader2");
        }


        private void ExecuteDone()
        {
            TroopRoster troopRoster = new TroopRoster(PartyBase.MainParty);
            foreach (TroopVM troopVM in this._formationsArmy[0])
            {

                this._partyManager.TryAddCharacterToRoster(troopRoster, troopVM._troopID, int.Parse(troopVM._troopNumber));
            }

            this._partyManagerLogic.LeftSideRoster[0] = troopRoster;


            troopRoster = new TroopRoster(PartyBase.MainParty);
            foreach (TroopVM troopVM in this._formationsArmy[1])
            {

                this._partyManager.TryAddCharacterToRoster(troopRoster, troopVM._troopID, int.Parse(troopVM._troopNumber));
            }

            this._partyManagerLogic.LeftSideRoster[1] = troopRoster;

            troopRoster = new TroopRoster(PartyBase.MainParty);
            foreach (TroopVM troopVM in this._formationsArmy[2])
            {

                this._partyManager.TryAddCharacterToRoster(troopRoster, troopVM._troopID, int.Parse(troopVM._troopNumber));
            }

            this._partyManagerLogic.LeftSideRoster[2] = troopRoster;

            troopRoster = new TroopRoster(PartyBase.MainParty);
            foreach (TroopVM troopVM in this._formationsArmy[3])
            {

                this._partyManager.TryAddCharacterToRoster(troopRoster, troopVM._troopID, int.Parse(troopVM._troopNumber));
            }

            this._partyManagerLogic.LeftSideRoster[3] = troopRoster;



            troopRoster = new TroopRoster(PartyBase.MainParty);
            foreach (TroopVM troopVM in this._rightParty)
            {

                if (!troopVM._isHero)
                {
                    this._partyManager.TryAddCharacterToRoster(troopRoster, troopVM._troopID, int.Parse(troopVM._troopNumber));
                }

                //this._partyManager.TryAddCharacterToRoster(troopRoster, troopVM._troopID, int.Parse(troopVM._troopNumber));

            }

            this._partyManagerLogic.RightSideRoster[0] = troopRoster;

            this._config.UpdateArmyRosters(this._partyManagerLogic.LeftSideRoster, this._config.EnemyParty());
            ScreenManager.PopScreen();
            Game.Current.GameStateManager.PushState(Game.Current.GameStateManager.CreateState<GeneralLordMainGameState>(new object[]
            {
                this._partyManagerLogic
            }));
            //ScreenManager.PushScreen(new MainManagerScreen(this._partyManagerLogic));
        }


        private void ExecuteCancel()
        {
            ScreenManager.PopScreen();
            //ScreenManager.PushScreen(new MainManagerScreen(this._partyManagerLogic));
            Game.Current.GameStateManager.PushState(Game.Current.GameStateManager.CreateState<GeneralLordMainGameState>(new object[]
            {
                this._partyManagerLogic
            }));
        }

        private void ExecuteReset()
        {
            this._rightParty = new MBBindingList<TroopVM>();
            //this._formationsArmy[0] = new MBBindingList<TroopVM>();
            //this._formationsArmy[1] = new MBBindingList<TroopVM>();
            //this._formationsArmy[2] = new MBBindingList<TroopVM>();

            for (int i = 0; i < this._formationsArmy.Length; i++)
            {
                this._formationsArmy[i] = new MBBindingList<TroopVM>();
            }


            //InitializePartyList(this._formationsArmy[0], this._rightParty, this._partyManagerLogic.LeftSideRoster[0], PartyRosterSide.Left);
            //InitializePartyList(this._formationsArmy[1], this._rightParty, this._partyManagerLogic.LeftSideRoster[1], PartyRosterSide.Left);
            //InitializePartyList(this._formationsArmy[2], this._rightParty, this._partyManagerLogic.LeftSideRoster[2], PartyRosterSide.Left);
            InitializePartyList(this._rightParty, this._formationsArmy[(int)this._target], this._partyManagerLogic.RightSideRoster[0], PartyRosterSide.Right);
            for (int i = 0; i < this._formationsArmy.Length; i++)
            {
                InitializePartyList(this._formationsArmy[i], this._rightParty, this._partyManagerLogic.LeftSideRoster[i], PartyRosterSide.Left);
            }

            base.OnPropertyChanged("RightParty");
            base.OnPropertyChanged("FormationArmyA");
            base.OnPropertyChanged("FormationArmyB");
            base.OnPropertyChanged("FormationArmyC");
            base.OnPropertyChanged("FormationArmyD");


            this._config.UpdateArmyRosters(this._partyManagerLogic.LeftSideRoster, this._config.EnemyParty());
            this.RefreshValues();
        }

        private void SetSelectedCategory(int value)
        {

            bool updateRightTarget = false;
            if(value == 0 && this._target != Formation.FormationA)
            {
                this._target = Formation.FormationA;

                this._formationASelected = true;
                this._formationBSelected = false;
                this._formationCSelected = false;
                this._formationDSelected = false;

                updateRightTarget = true;
            }
            if (value == 1 && this._target != Formation.FormationB)
            {
                this._target = Formation.FormationB;

                this._formationASelected = false;
                this._formationBSelected = true;
                this._formationCSelected = false;
                this._formationDSelected = false;

                updateRightTarget = true;
            }
            if (value == 2 && this._target != Formation.FormationC)
            {
                this._target = Formation.FormationC;

                this._formationASelected = false;
                this._formationBSelected = false;
                this._formationCSelected = true;
                this._formationDSelected = false;

                updateRightTarget = true;
            }
            if (value == 3 && this._target != Formation.FormationD)
            {
                this._target = Formation.FormationD;

                this._formationASelected = false;
                this._formationBSelected = false;
                this._formationCSelected = false;
                this._formationDSelected = true;

                updateRightTarget = true;
            }

            base.OnPropertyChanged("FormationASelected");
            base.OnPropertyChanged("FormationBSelected");
            base.OnPropertyChanged("FormationCSelected");
            base.OnPropertyChanged("FormationDSelected");

            InformationManager.DisplayMessage(new InformationMessage(this._target.ToString()));

            if (updateRightTarget)
            {
                foreach(TroopVM troopVm in this._rightParty)
                {
                    troopVm._targetList = this._formationsArmy[(int)this._target];
                }
            }
        }


        [DataSourceProperty]
        public string CancelText
        {
            get
            {
                return this._cancelText;
            }
        }

        [DataSourceProperty]
        public string DoneText
        {
            get
            {
                return this._doneText;
            }
        }

        [DataSourceProperty]
        public string FormationATitle
        {
            get
            {
                return this._formationATitle;
            }
        }

        [DataSourceProperty]
        public string FormationASize
        {
            get
            {
                return this._formationASize;
            }
        }

        [DataSourceProperty]
        public string FormationBTitle
        {
            get
            {
                return this._formationBTitle;
            }
        }

        [DataSourceProperty]
        public string FormationBSize
        {
            get
            {
                return this._formationBSize;
            }
        }

        [DataSourceProperty]
        public string FormationCTitle
        {
            get
            {
                return this._formationCTitle;
            }
        }

        [DataSourceProperty]
        public string FormationCSize
        {
            get
            {
                return this._formationCSize;
            }
        }

        [DataSourceProperty]
        public string FormationDTitle
        {
            get
            {
                return this._formationDTitle;
            }
        }

        [DataSourceProperty]
        public string FormationDSize
        {
            get
            {
                return this._formationDSize;
            }
        }

        [DataSourceProperty]
        public string RightPartyHeader
        {
            get
            {
                return this._rightPartyHeader;
            }
        }

        [DataSourceProperty]
        public string RightPartyHeader2
        {
            get
            {
                return this._rightPartyHeader2;
            }
        }

        [DataSourceProperty]
        public bool IsCancelDisabled
        {
            get
            {
                return this._isCancelDisabled;
            }
        }

        [DataSourceProperty]
        public bool IsDoneDisabled
        {
            get
            {
                return this._isDoneDisabled;
            }
        }


        [DataSourceProperty]
        public bool AreMembersRelevantOnCurrentMode
        {
            get
            {
                return this._areMembersRelevantOnCurrentMode;
            }
        }

        [DataSourceProperty]
        public MBBindingList<TroopVM> RightParty
        {
            get
            {
                return this._rightParty;
            }
            set
            {
                if (value != this._rightParty)
                {
                    this._rightParty = value;
                    base.OnPropertyChangedWithValue(value, "RightParty");
                }
            }
        }

        [DataSourceProperty]
        public MBBindingList<TroopVM> FormationArmyA
        {
            get
            {
                return this._formationsArmy[0];
            }
            set
            {
                if (value != this._formationsArmy[0])
                {
                    this._formationsArmy[0] = value;
                    base.OnPropertyChangedWithValue(value, "FormationArmyA");
                }
            }
        }

        [DataSourceProperty]
        public MBBindingList<TroopVM> FormationArmyB
        {
            get
            {
                return this._formationsArmy[1];
            }
            set
            {
                if (value != this._formationsArmy[1])
                {
                    this._formationsArmy[1] = value;
                    base.OnPropertyChangedWithValue(value, "FormationArmyB");
                }
            }
        }

        [DataSourceProperty]
        public MBBindingList<TroopVM> FormationArmyC
        {
            get
            {
                return this._formationsArmy[2];
            }
            set
            {
                if (value != this._formationsArmy[2])
                {
                    this._formationsArmy[2] = value;
                    base.OnPropertyChangedWithValue(value, "FormationArmyC");
                }
            }
        }

        [DataSourceProperty]
        public MBBindingList<TroopVM> FormationArmyD
        {
            get
            {
                return this._formationsArmy[3];
            }
            set
            {
                if (value != this._formationsArmy[3])
                {
                    this._formationsArmy[3] = value;
                    base.OnPropertyChangedWithValue(value, "FormationArmyD");
                }
            }
        }

        [DataSourceProperty]
        public string LeftSidePartyTitle
        {
            get
            {
                return this._leftSidePartyTitle;
            }
        }

        [DataSourceProperty]
        public string RigthSidePartyTitle
        {
            get
            {
                return this._rightSidePartyTitle;
            }
        }

        [DataSourceProperty]
        public bool FormationASelected
        {
            get
            {
                return this._formationASelected;
            }
            set
            {

            }
        }

        [DataSourceProperty]
        public bool FormationBSelected
        {
            get
            {
                return this._formationBSelected;
            }
            set
            {

            }
        }

        [DataSourceProperty]
        public bool FormationCSelected
        {
            get
            {
                return this._formationCSelected;
            }
            set
            {

            }
        }

        [DataSourceProperty]
        public bool FormationDSelected
        {
            get
            {
                return this._formationDSelected;
            }
            set
            {

            }
        }



        private void InitializePartyList(MBBindingList<TroopVM> partyList, MBBindingList<TroopVM> target, TroopRoster currentTroopRoster, PartyRosterSide rosterSide)
        {

            partyList.Clear();

            if (rosterSide == PartyRosterSide.Right)
            {
                TroopVM troopVM = new TroopVM(this, this._rightParty, this._rightParty, Hero.MainHero.CharacterObject, Hero.MainHero.CharacterObject.Name.ToString(), Hero.MainHero.CharacterObject.StringId.ToString(), "1", PartyRosterSide.None, true);
                partyList.Add(troopVM);
                troopVM.ThrowOnPropertyChanged();
            }



            for (int i = 0; i < currentTroopRoster.Count; i++)
            {
                TroopRosterElement elementCopyAtIndex = currentTroopRoster.GetElementCopyAtIndex(i);
                CharacterObject CharObj = currentTroopRoster.GetCharacterAtIndex(i);
                TroopVM troopVM = new TroopVM(this ,partyList, target, CharObj, CharObj.Name.ToString(), CharObj.StringId.ToString(), currentTroopRoster.GetTroopCount(CharObj).ToString(), rosterSide, false);
                partyList.Add(troopVM);
                troopVM.ThrowOnPropertyChanged();
            }
        }

        private int GetElementCount(MBBindingList<TroopVM> partyList)
        {
            int value = 0;
            foreach (TroopVM troopVM in partyList)
            {
                value += int.Parse(troopVM.TroopNumber);
            }

            return value;

        }
        private int GetArrayCount(string[] array)
        {
            int value = 0;
            for (int i = 0; i < array.Length; i++)
            {
                value += int.Parse(array[i]);
            }

            return value;

        }




        private string _doneText;
        private string _cancelText;

        //private string _leftPartyHeader;
        //public string _leftPartyHeader2;


        private string _formationATitle;
        public string _formationASize;

        private string _formationBTitle;
        public string _formationBSize;

        private string _formationCTitle;
        public string _formationCSize;

        private string _formationDTitle;
        public string _formationDSize;

        private string _rightPartyHeader;
        public string _rightPartyHeader2;
        private string _leftSidePartyTitle;
        private string _rightSidePartyTitle;

        private bool _formationASelected;
        private bool _formationBSelected;
        private bool _formationCSelected;
        private bool _formationDSelected;

        private bool _isDoneDisabled;
        private bool _isCancelDisabled;
        private bool _areMembersRelevantOnCurrentMode;
        private PartyManagerLogic _partyManagerLogic;
        private PartyManager _partyManager;

        public MBBindingList<TroopVM> _rightParty;
        
        public MBBindingList<TroopVM>[] _formationsArmy;

        //public MBBindingList<TroopVM> _formationArmyA;
        //public MBBindingList<TroopVM> _formationArmyB;
        //public MBBindingList<TroopVM> _formationArmyC;

        private BattleGeneralConfig _config;

        public Formation _target;

    }
}
