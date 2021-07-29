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
using GeneralLord.FormationBattleTest;

namespace GeneralLord
{
    public class MainManagerViewModel : ViewModel
    {

        public MainManagerViewModel(PartyManagerLogic partyManagerLogic) {

			ItemRosterGeneratorHandler.InitializeItemRosterForShop();

			this._partyManagerLogic = partyManagerLogic;
			//this._partyManagerLogic.Initialize(this._partyManager.TestRosterLeft(), this._partyManager.TestRosterRight());

			this._clan = Hero.MainHero.Clan;


			this._name = new TextObject("{=ATName}Main Overview", null).ToString();

			this._overviewText = new TextObject("{=ATOverviewText}Character", null).ToString();
			this._partyText = new TextObject("{=ATPartyText}Party", null).ToString();
			this._formationText = new TextObject("{=ATFormationText}Formation Manager", null).ToString();
			this._recruitmentText = new TextObject("{=ATFormationText}Recruitment", null).ToString();
			this._shopText = new TextObject("{=ATShopText}Shop", null).ToString();

			this.MainOverview = new MainOverviewViewModel(this);
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
			this.CurrentRenownText = GameTexts.FindText("str_clan_tier", null).ToString();
			this.CurrentRenown = (int)Clan.PlayerClan.Renown;
			this.CurrentTier = Clan.PlayerClan.Tier;
			if (Campaign.Current.Models.ClanTierModel.HasUpcomingTier(Clan.PlayerClan, false).Item2)
			{
				this.NextTierRenown = Clan.PlayerClan.RenownRequirementForNextTier;
				this.MinRenownForCurrentTier = Campaign.Current.Models.ClanTierModel.GetRequiredRenownForTier(this.CurrentTier);
				this.NextTier = Clan.PlayerClan.Tier + 1;
				this.IsRenownProgressComplete = false;
			}
			else
			{
				this.NextTierRenown = 1;
				this.MinRenownForCurrentTier = 1;
				this.NextTier = 0;
				this.IsRenownProgressComplete = true;
			}
			this.RenownHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetClanRenownTooltip(Clan.PlayerClan));
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
				BattleTestHandler.OpenBattleTestMission();
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
				InventoryManager.OpenScreenAsTrade(ItemRosterGeneratorHandler.itemRosterShop, closestHideout.GetComponent<SettlementComponent>(), InventoryManager.InventoryCategoryType.None, null);

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

		[DataSourceProperty]
		public int NextTierRenown
		{
			get
			{
				return this._nextTierRenown;
			}
			set
			{
				if (value != this._nextTierRenown)
				{
					this._nextTierRenown = value;
					base.OnPropertyChangedWithValue(value, "NextTierRenown");
				}
			}
		}

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06001719 RID: 5913 RVA: 0x00055CD2 File Offset: 0x00053ED2
		// (set) Token: 0x0600171A RID: 5914 RVA: 0x00055CDA File Offset: 0x00053EDA
		[DataSourceProperty]
		public int CurrentTier
		{
			get
			{
				return this._currentTier;
			}
			set
			{
				if (value != this._currentTier)
				{
					this._currentTier = value;
					base.OnPropertyChangedWithValue(value, "CurrentTier");
				}
			}
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x0600171B RID: 5915 RVA: 0x00055CFD File Offset: 0x00053EFD
		// (set) Token: 0x0600171C RID: 5916 RVA: 0x00055D05 File Offset: 0x00053F05
		[DataSourceProperty]
		public int MinRenownForCurrentTier
		{
			get
			{
				return this._minRenownForCurrentTier;
			}
			set
			{
				if (value != this._minRenownForCurrentTier)
				{
					this._minRenownForCurrentTier = value;
					base.OnPropertyChangedWithValue(value, "MinRenownForCurrentTier");
				}
			}
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x0600171D RID: 5917 RVA: 0x00055D28 File Offset: 0x00053F28
		// (set) Token: 0x0600171E RID: 5918 RVA: 0x00055D30 File Offset: 0x00053F30
		[DataSourceProperty]
		public int NextTier
		{
			get
			{
				return this._nextTier;
			}
			set
			{
				if (value != this._nextTier)
				{
					this._nextTier = value;
					base.OnPropertyChangedWithValue(value, "NextTier");
				}
			}
		}

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x0600171F RID: 5919 RVA: 0x00055D53 File Offset: 0x00053F53
		// (set) Token: 0x06001720 RID: 5920 RVA: 0x00055D5B File Offset: 0x00053F5B
		[DataSourceProperty]
		public int CurrentRenown
		{
			get
			{
				return this._currentRenown;
			}
			set
			{
				if (value != this._currentRenown)
				{
					this._currentRenown = value;
					base.OnPropertyChangedWithValue(value, "CurrentRenown");
				}
			}
		}

		[DataSourceProperty]
		public bool IsRenownProgressComplete
		{
			get
			{
				return this._isRenownProgressComplete;
			}
			set
			{
				if (value != this._isRenownProgressComplete)
				{
					this._isRenownProgressComplete = value;
					base.OnPropertyChangedWithValue(value, "IsRenownProgressComplete");
				}
			}
		}

		[DataSourceProperty]
		public BasicTooltipViewModel RenownHint
		{
			get
			{
				return this._renownHint;
			}
			set
			{
				if (value != this._renownHint)
				{
					this._renownHint = value;
					base.OnPropertyChangedWithValue(value, "RenownHint");
				}
			}
		}

		[DataSourceProperty]
		public string CurrentRenownText
		{
			get
			{
				return this._currentRenownText;
			}
			set
			{
				if (value != this._currentRenownText)
				{
					this._currentRenownText = value;
					base.OnPropertyChangedWithValue(value, "CurrentRenownText");
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

		private BasicTooltipViewModel _renownHint;
		private int _minRenownForCurrentTier;
		private int _currentRenown;
		private int _currentTier = -1;
		private int _nextTierRenown;
		private int _nextTier;
		private string _currentRenownText;
		private bool _isRenownProgressComplete;
	}
}
