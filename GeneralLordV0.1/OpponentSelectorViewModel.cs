using GeneralLord.Client.Web;
using GeneralLordWebApiClient;
using GeneralLordWebApiClient.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace GeneralLord
{
    public class OpponentSelectorViewModel : ViewModel
    {
        public OpponentSelectorViewModel(bool isRankingScreen = false)
        {
			_isRankingScreen = isRankingScreen;
			//_opponentProfiles = opponentProfiles;
			this.Opponents = new MBBindingList<OpponentEntryTupleViewModel>();

			this.HealthyInfantryHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopHealthyInfo(PartyBase.MainParty, FormationClass.Infantry));
			this.HealthyCavalryHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopHealthyInfo(PartyBase.MainParty, FormationClass.Cavalry));
			this.HealthyRangedHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopHealthyInfo(PartyBase.MainParty, FormationClass.Ranged));
			this.HealthyHorseArcherHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopHealthyInfo(PartyBase.MainParty, FormationClass.HorseArcher));
			this.HeroHealthHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetHeroHealthTooltip(PartyBase.MainParty.LeaderHero));

			JObject json = JObject.Parse(Serializer.ReadStringFromFile("playerprofile.json"));
			this.EloText = new TextObject("{=ATEloText}Elo: ", null).ToString();
			this.ExplanationText = new TextObject("{=ATExplanationText}Choose your opponent! After you defeat one opponent he will be unchallengeable for the next 12 hours. If no Opponent is available consider taking a rest and coming back later :)", null).ToString();
			this.Elo = json["Elo"].ToString();
			this.OppenentSortController = new OpponentSelectorSortControllerViewModel(ref this._opponents, isRankingScreen);
			this.OppenentSortController.SortByDefaultState();
			RefreshMembersList();
			RefreshValues();
		}

		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = PartyBase.MainParty.Name.ToString();
			GameTexts.SetVariable("LEFT", PartyBase.MainParty.MobileParty.MemberRoster.TotalManCount);
			GameTexts.SetVariable("RIGHT", PartyBase.MainParty.MobileParty.Party.PartySizeLimit);
			string text = GameTexts.FindText("str_LEFT_over_RIGHT", null).ToString();
			string content = GameTexts.FindText("str_party_morale_party_size", null).ToString();
			this.PartySizeText = text;
			GameTexts.SetVariable("LEFT", content);
			GameTexts.SetVariable("RIGHT", text);
			this.PartySizeSubTitleText = GameTexts.FindText("str_LEFT_colon_RIGHT", null).ToString();

			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;

			foreach (TroopRosterElement troopRosterElement in PartyBase.MainParty.MemberRoster.GetTroopRoster())
			{
				Hero heroObject = troopRosterElement.Character.HeroObject;
				if (heroObject != null && heroObject.Clan == Clan.PlayerClan)
				{

				}
				else if (troopRosterElement.Character.DefaultFormationClass.Equals(FormationClass.Infantry))
				{
					num += troopRosterElement.Number - troopRosterElement.WoundedNumber;
				}
				else if (troopRosterElement.Character.DefaultFormationClass.Equals(FormationClass.Ranged))
				{
					num2 += troopRosterElement.Number - troopRosterElement.WoundedNumber;
				}
				else if (troopRosterElement.Character.DefaultFormationClass.Equals(FormationClass.Cavalry))
				{
					num3 += troopRosterElement.Number - troopRosterElement.WoundedNumber;
				}
				else if (troopRosterElement.Character.DefaultFormationClass.Equals(FormationClass.HorseArcher))
				{
					num4 += troopRosterElement.Number - troopRosterElement.WoundedNumber;
				}
			}

			this.HealthyInfantryCount = num;
			this.HealthyRangedCount = num2;
			this.HealthyCavalryCount = num3;
			this.HealthyHorseArcherCount = num4;
		}

		public void RefreshMembersList()
		{
			this.Opponents.Clear();
			Profile userProfile = ProfileHandler.UpdateProfileAc();
			var task = Task.Run(async () => await ServerRequestsHandler.GetMatchHistory(userProfile.Id));
			task.Wait();
			IEnumerable<MatchHistory>  matchHistories = task.Result;
			var task1 = Task.Run(async () => await ServerRequestsHandler.GetMatchMakingProfiles(userProfile, _isRankingScreen));
			task1.Wait();
			IEnumerable<Profile> profiles = task1.Result;


			Dictionary<int, DateTime> blockedIds = new Dictionary<int, DateTime>();
			foreach (MatchHistory matchHistory in matchHistories)
			{
				if (matchHistory.Id == userProfile.Id)
				{
					double hourdifference = (DateTime.Now - matchHistory.LocalTimeDatePostMatch).TotalHours;
					if (hourdifference < JsonBattleConfig.rankedHourCooldown && matchHistory.BattleResult == "PlayerVictory")
					{
						blockedIds.Add(matchHistory.EnemyId, matchHistory.LocalTimeDatePostMatch);
					}
				}
			}

			int rankingPosition = 1;
			foreach (Profile profile in profiles)
			{
				bool isNotChallengeable = false;
				if (blockedIds.ContainsKey(profile.Id))
				{
					isNotChallengeable = true;
					
					this.Opponents.Add(new OpponentEntryTupleViewModel(profile, rankingPosition, _isRankingScreen, isNotChallengeable, blockedIds[profile.Id]));
				}
				else
                {

					this.Opponents.Add(new OpponentEntryTupleViewModel(profile, rankingPosition, _isRankingScreen, isNotChallengeable));
				}

				rankingPosition++;
			}
		}

		private void ExecuteLeave()
		{
			ScreenManager.PopScreen();
		}


		[DataSourceProperty]
		public string EloText
		{
			get
			{
				return this._eloText;
			}
			set
			{
				if (value != this._eloText)
				{
					this._eloText = value;
					base.OnPropertyChangedWithValue(value, "EloText");
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
		public string PartySizeText
		{
			get
			{
				return this._partySizeText;
			}
			set
			{
				if (value != this._partySizeText)
				{
					this._partySizeText = value;
					base.OnPropertyChanged("PartyStrengthText");
				}
			}
		}

		[DataSourceProperty]
		public string PartySizeSubTitleText
		{
			get
			{
				return this._partySizeSubTitleText;
			}
			set
			{
				if (value != this._partySizeSubTitleText)
				{
					this._partySizeSubTitleText = value;
					base.OnPropertyChangedWithValue(value, "PartySizeSubTitleText");
				}
			}
		}

		[DataSourceProperty]
		public string ExplanationText
		{
			get
			{
				return this._explanationText;
			}
			set
			{
				if (value != this._explanationText)
				{
					this._explanationText = value;
					base.OnPropertyChangedWithValue(value, "ExplanationText");
				}
			}
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

		[DataSourceProperty]
		public BasicTooltipViewModel HealthyInfantryHint
		{
			get
			{
				return this._healthyInfantryHint;
			}
			set
			{
				if (value != this._healthyInfantryHint)
				{
					this._healthyInfantryHint = value;
					base.OnPropertyChangedWithValue(value, "HealthyInfantryHint");
				}
			}
		}

		[DataSourceProperty]
		public BasicTooltipViewModel HealthyRangedHint
		{
			get
			{
				return this._healthyRangedHint;
			}
			set
			{
				if (value != this._healthyRangedHint)
				{
					this._healthyRangedHint = value;
					base.OnPropertyChangedWithValue(value, "HealthyRangedHint");
				}
			}
		}

		[DataSourceProperty]
		public BasicTooltipViewModel HealthyCavalryHint
		{
			get
			{
				return this._healthyCavalryHint;
			}
			set
			{
				if (value != this._healthyCavalryHint)
				{
					this._healthyCavalryHint = value;
					base.OnPropertyChangedWithValue(value, "HealthyCavalryHint");
				}
			}
		}

		[DataSourceProperty]
		public BasicTooltipViewModel HealthyHorseArcherHint
		{
			get
			{
				return this._healthyHorseArcherHint;
			}
			set
			{
				if (value != this._healthyHorseArcherHint)
				{
					this._healthyHorseArcherHint = value;
					base.OnPropertyChangedWithValue(value, "HealthyHorseArcherHint");
				}
			}
		}

		[DataSourceProperty]
		public int HealthyInfantryCount
		{
			get
			{
				return this._healthyInfantryCount;
			}
			set
			{
				if (value != this._healthyInfantryCount)
				{
					this._healthyInfantryCount = value;
					base.OnPropertyChangedWithValue(value, "HealthyInfantryCount");
				}
			}
		}

		[DataSourceProperty]
		public int HealthyRangedCount
		{
			get
			{
				return this._healthyRangedCount;
			}
			set
			{
				if (value != this._healthyRangedCount)
				{
					this._healthyRangedCount = value;
					base.OnPropertyChangedWithValue(value, "HealthyRangedCount");
				}
			}
		}

		[DataSourceProperty]
		public int HealthyCavalryCount
		{
			get
			{
				return this._healthyCavalryCount;
			}
			set
			{
				if (value != this._healthyCavalryCount)
				{
					this._healthyCavalryCount = value;
					base.OnPropertyChangedWithValue(value, "HealthyCavalryCount");
				}
			}
		}

		[DataSourceProperty]
		public int HealthyHorseArcherCount
		{
			get
			{
				return this._healthyHorseArcherCount;
			}
			set
			{
				if (value != this._healthyHorseArcherCount)
				{
					this._healthyHorseArcherCount = value;
					base.OnPropertyChangedWithValue(value, "HealthyHorseArcherCount");
				}
			}
		}

		[DataSourceProperty]
		public BasicTooltipViewModel HeroHealthHint
		{
			get
			{
				return this._heroHealthHint;
			}
			set
			{
				if (value != this._heroHealthHint)
				{
					this._heroHealthHint = value;
					base.OnPropertyChangedWithValue(value, "HeroHealthHint");
				}

			}
		}

		[DataSourceProperty]
		public int HeroHealth
		{
			get
			{
				return (int)Math.Ceiling((double) PartyBase.MainParty.LeaderHero.HitPoints * 100.0 / (double)PartyBase.MainParty.LeaderHero.CharacterObject.MaxHitPoints());
			}
		}

		[DataSourceProperty]
		public OpponentSelectorSortControllerViewModel OppenentSortController
		{
			get
			{
				return this._oppenentSortController;
			}
			set
			{
				if (value != this._oppenentSortController)
				{
					this._oppenentSortController = value;
					base.OnPropertyChangedWithValue(value, "OppenentSortController");
				}
			}
		}

		private string _eloText;
		private string _elo;

		private BasicTooltipViewModel _healthyInfantryHint;
		private BasicTooltipViewModel _healthyRangedHint;
		private BasicTooltipViewModel _healthyCavalryHint;
		private BasicTooltipViewModel _healthyHorseArcherHint;

		private BasicTooltipViewModel _heroHealthHint;


		private int _healthyInfantryCount;
		private int _healthyRangedCount;
		private int _healthyCavalryCount;
		private int _healthyHorseArcherCount;

		private MBBindingList<OpponentEntryTupleViewModel> _opponents;
		//private IEnumerable<Profile> _opponentProfiles;

		private string _partySizeText;
		private string _partySizeSubTitleText;
		private string _name;
        private OpponentSelectorSortControllerViewModel _oppenentSortController;

		private bool _isRankingScreen;
        private string _explanationText;
    }
}
