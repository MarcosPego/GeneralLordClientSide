using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Categories;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.Engine.Screens;
using SandBox.View.Map;
using SandBox.GauntletUI;
using Helpers;

namespace GeneralLord
{
    class MainManagerViewModel : ViewModel
    {

        public MainManagerViewModel(PartyManagerLogic partyManagerLogic) {
			

			this._partyManagerLogic = partyManagerLogic;
			//this._partyManagerLogic.Initialize(this._partyManager.TestRosterLeft(), this._partyManager.TestRosterRight());

			this._clan = Hero.MainHero.Clan;

			this._name = new TextObject("{=ATName}Main Overview", null).ToString();

			this._overviewText = new TextObject("{=ATOverviewText}Character", null).ToString();
			this._partyText = new TextObject("{=ATPartyText}Party", null).ToString();
			this._formationText = new TextObject("{=ATFormationText}Formation Manager", null).ToString();
			this._recruitmentText = new TextObject("{=ATFormationText}Recruitment", null).ToString();
			this._shopText = new TextObject("{=ATShopText}Shop", null).ToString();

			this.MainOverview = new MainOverviewViewModel();
			this.Leader = new HeroVM(this._clan.Leader, false);
			UpdateBannerVisuals();

			this._mapNavigationHandler = new MapNavigationHandler();
			this._navigationHandler = this._mapNavigationHandler;
		}

		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = Hero.MainHero.Clan.Name.ToString();
			this.LeaderText = GameTexts.FindText("str_sort_by_leader_name_label", null).ToString();
			this.DoneLbl = GameTexts.FindText("str_done", null).ToString();
		}

		private void ExecuteDone()
		{
			//ScreenManager.PopScreen();
		}

		private void SetSelectedCategory(int value)
		{
			if(value == 0)
            {
				ScreenManager.PopScreen();
				this._navigationHandler.OpenCharacterDeveloper();
			}

			if(value == 1)
            {
				ScreenManager.PopScreen();
				this._navigationHandler.OpenParty();
			}
			if(value ==2)
            {
				//ScreenManager.PopScreen();
				//ScreenManager.PushScreen(new PartyManagerScreen(this._partyManagerLogic));
			}
			if (value == 3)
			{
				ScreenManager.PopScreen();
				PartyScreenState.currentState = PartyScreenStateEnum.RecruitmentScreen;
				RecruitmentManager.OpenRecruitmentRoster();

			}

			if (value == 4)
			{
				ScreenManager.PopScreen();
				Settlement closestHideout = SettlementHelper.FindNearestSettlement((Settlement x) => x.IsActive && x.IsTown);
				InformationManager.DisplayMessage(new InformationMessage(closestHideout.Name.ToString()));
				InventoryManager.OpenScreenAsTrade(closestHideout.ItemRoster, closestHideout.GetComponent<SettlementComponent>(), InventoryManager.InventoryCategoryType.None, null);

			}
		}

		[DataSourceProperty]
		public MainOverviewViewModel MainOverview
		{
			get
			{
				return this._mainOverview;
			}
			set
			{
				if (value != this._mainOverview)
				{
					this._mainOverview = value;
					base.OnPropertyChangedWithValue(value, "MainOverview");
				}
			}
		}

		[DataSourceProperty]
		public ImageIdentifierVM ClanBanner
		{
			get
			{
				return this._clanBanner;
			}
			set
			{
				if (value != this._clanBanner)
				{
					this._clanBanner = value;
					base.OnPropertyChangedWithValue(value, "ClanBanner");
				}
			}
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
		public string DoneLbl
		{
			get
			{
				return this._doneLbl;
			}
			set
			{
				if (value != this._doneLbl)
				{
					this._doneLbl = value;
					base.OnPropertyChangedWithValue(value, "DoneLbl");
				}
			}
		}

		[DataSourceProperty]
		public HintViewModel ClanBannerHint
		{
			get
			{
				return this._clanBannerHint;
			}
			set
			{
				if (value != this._clanBannerHint)
				{
					this._clanBannerHint = value;
					base.OnPropertyChangedWithValue(value, "ClanBannerHint");
				}
			}
		}

		[DataSourceProperty]
		public HeroVM Leader
		{
			get
			{
				return this._leader;
			}
			set
			{
				if (value != this._leader)
				{
					this._leader = value;
					base.OnPropertyChangedWithValue(value, "Leader");
				}
			}
		}

		[DataSourceProperty]
		public string LeaderText
		{
			get
			{
				return this._leaderText;
			}
			set
			{
				if (value != this._leaderText)
				{
					this._leaderText = value;
					base.OnPropertyChangedWithValue(value, "LeaderText");
				}
			}
		}

		[DataSourceProperty]
		public string OverviewText
		{
			get
			{
				return this._overviewText;
			}
			set
			{
				if (value != this._overviewText)
				{
					this._overviewText = value;
					base.OnPropertyChangedWithValue(value, "OverviewText");
				}
			}
		}

		[DataSourceProperty]
		public string FormationText
		{
			get
			{
				return this._formationText;
			}
			set
			{
				if (value != this._formationText)
				{
					this._formationText = value;
					base.OnPropertyChangedWithValue(value, "FormationText");
				}
			}
		}

		[DataSourceProperty]
		public string RecruitmentText
		{
			get
			{
				return this._recruitmentText;
			}
			set
			{
				if (value != this._recruitmentText)
				{
					this._recruitmentText = value;
					base.OnPropertyChangedWithValue(value, "RecruitmentText");
				}
			}
		}

		[DataSourceProperty]
		public string ShopText
		{
			get
			{
				return this._shopText;
			}
			set
			{
				if (value != this._shopText)
				{
					this._shopText = value;
					base.OnPropertyChangedWithValue(value, "ShopText");
				}
			}
		}

		[DataSourceProperty]
		public string PartyText
		{
			get
			{
				return this._partyText;
			}
			set
			{
				if (value != this._partyText)
				{
					this._partyText = value;
					base.OnPropertyChangedWithValue(value, "PartyText");
				}
			}
		}

		public void UpdateBannerVisuals()
		{
			this.ClanBanner = new ImageIdentifierVM(BannerCode.CreateFrom(this._clan.Banner), true);
			this.ClanBannerHint = new HintViewModel(new TextObject("{=t1lSXN9O}Your clan's standard carried into battle", null), null);
			this.RefreshValues();
		}

		private ImageIdentifierVM _clanBanner;
		private HintViewModel _clanBannerHint;
		private readonly Clan _clan;
		private HeroVM _leader;
		private string _leaderText;
		private string _doneLbl;
		private string _name;


		private MainOverviewViewModel _mainOverview;

		private string _overviewText;
		private string _formationText;
        private string _recruitmentText;
        private string _partyText;
		private string _shopText;

		private PartyManagerLogic _partyManagerLogic;

		private MapNavigationHandler _mapNavigationHandler;
		private INavigationHandler _navigationHandler;
		private PartyManager _partyManager = null;
	}
}
