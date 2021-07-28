using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement;
//using TaleWorlds.MountAndBlade.CustomBattle.CustomBattle;
//using TaleWorlds.MountAndBlade.CustomBattle;
using TaleWorlds.MountAndBlade;
using Helpers;
using EnhancedBattleTest;
using EnhancedBattleTest.Data;
using EnhancedBattleTest.Data.MissionData;
using EnhancedBattleTest.Config;
using EnhancedBattleTest.UI;
using EnhancedBattleTest.GameMode;
using TaleWorlds.Engine.Screens;
using GeneralLordWebApiClient.Model;
using System.Xml.Linq;
using GeneralLordWebApiClient;
using Newtonsoft.Json.Linq;
using TaleWorlds.Localization;
using MatchHistory = GeneralLordWebApiClient.Model.MatchHistory;
using GeneralLord.FormationBattleTest;

namespace GeneralLord
{
	internal class MainOverviewViewModel : ViewModel
	{
		public MainOverviewViewModel()
		{
			this._exitOnSaveOver = false;
			//this._partyManagerLogic = partyManagerLogic;

			/*GameTexts.SetVariable("DENAR_AMOUNT", content);
			GameTexts.SetVariable("GOLD_ICON", "{=!}<img src=\"Icons\\Coin@2x\" extend=\"8\">");*/

			this._faction = Hero.MainHero.Clan;
			//this._expectedGoldText = new TextObject("{=ATExpectedGoldText}Current Gold", null).ToString();
			//this._expectedGold = PartyBase.MainParty.LeaderHero.Gold.ToString();

			//this._config = BattleConfig.Deserialize(false);
			//_generalConfig = new BattleGeneralConfig();

			//BattleConfig.Instance = this._generalConfig._config;
			this.BuyRenown = new TextObject("{=ATBuyRenown} Purchase reputation for your clan: ", null).ToString();
			this.Train = new TextObject("{=ATTrain} Train your stewardship: ", null).ToString();

			CampaignEvents.OnSaveOverEvent.AddNonSerializedListener(this, new Action<bool>(this.OnSaveOver));
			this.RefreshValues();
		}

		public override void RefreshValues()
		{
			base.RefreshValues();
			this.SelectMainHero();
			this.SkillsText = GameTexts.FindText("str_skills", null).ToString();
			this.ExpectedGoldText = new TextObject("{=ATExpectedGoldText}Current Gold: ", null).ToString();
			this.ExpectedGold = PartyBase.MainParty.LeaderHero.Gold.ToString();

			JObject json = JObject.Parse(Serializer.ReadStringFromFile("playerprofile.json"));
			this.EloText = new TextObject("{=ATEloText}Elo: ", null).ToString();
			this.Elo = json["Elo"].ToString();

		}

		public void SelectMainHero()
		{
			//this.Family.Add(new ClanLordItemVM(Hero.MainHero, new Action<ClanLordItemVM>(this.OnMemberSelection)));
			ClanLordItemVM member = new ClanLordItemVM(Hero.MainHero, new Action<ClanLordItemVM>(this.OnMemberSelection));
			this.CurrentSelectedMember = member;
		}

		private void OnMemberSelection(ClanLordItemVM member)
		{
			if (this.CurrentSelectedMember != null)
			{
				this.CurrentSelectedMember.IsSelected = false;
			}
			this.CurrentSelectedMember = member;
			bool flag = member.GetHero() == Hero.MainHero;
			bool flag2 = this._faction.Companions.Contains(member.GetHero());
			bool flag3 = TaleWorlds.CampaignSystem.Campaign.Current.IssueManager.IssueSolvingCompanionList.Contains(member.GetHero());
			//this.CanKickCurrentMemberFromClan = (!flag && flag2 && !flag3);
			string kickFromClanReasonString = CampaignUIHelper.GetKickFromClanReasonString(flag, flag2, flag3);
			//this.KickFromClanActionHint.HintText = (string.IsNullOrEmpty(kickFromClanReasonString) ? TextObject.Empty : new TextObject("{=!}" + kickFromClanReasonString, null));
			if (member != null)
			{
				member.IsSelected = true;
			}
		}


		public void ExecuteTrain()
		{

			PartyCapacityLogicHandler.HandleRenownBuy();
			this.RefreshValues();
		}
		public void ExecuteBuyRenown()
		{


			PartyCapacityLogicHandler.HandleTrainSteward();
			this.RefreshValues();
		}

		public void ExecuteRanking()
		{

			this.RefreshValues();
		}

		public void ExecuteFormation()
		{


			BattleTestHandler.OpenBattleTestMission();
			//this.RefreshValues();
		}

		public void ExectuteLeaveGme()
        {
			InformationManager.DisplayMessage(new InformationMessage("Exiting to Main Menu"));

			//Campaign.Current.SaveHandler.QuickSaveCurrentGame();
			JsonBattleConfig.ExecuteSubmitAc();
			this._exitOnSaveOver = true;
		}

		public void ExecuteQueue()
        {
			JsonBattleConfig.ExecuteSubmitAc();

			IEnumerable<Profile> profiles;
			var task = Task.Run(async () =>
			{
				//var result = await WebRequests.RawMessageWebGet(UrlHandler.GetUrlFromString(UrlHandler.MultipleFromProfile));
				Profile profile = ProfileHandler.UpdateProfileAc();
				var result = await WebRequests.PostAsync<IEnumerable<Profile>>(UrlHandler.GetUrlFromString(UrlHandler.MultipleFromProfile), profile);
				//var result = await WebRequests.PostAsync<Profile>("http://localhost:40519/values/singleLast");

				profiles = result.ServerResponse;
				//Serializer.JsonSerialize(profiles, "enemyProfile.json");

				//InformationManager.DisplayMessage(new InformationMessage(result)); 
				//ScreenManager.PopScreen();
				ScreenManager.PushScreen(new OpponentSelectorScreen(profiles));

			});
			task.Wait();

		}

		/*public void ExecuteStart()
		{
			JsonBattleConfig.ExecuteSubmitAc();
			var task =  Task.Run(async () =>
			{
				//var result = await WebRequests.RawMessageWebGet(UrlHandler.GetUrlFromString(UrlHandler.MultipleFromProfile));
				Profile profile = ProfileHandler.UpdateProfileAc();
				var result = await WebRequests.PostAsync<IEnumerable<Profile>>(UrlHandler.GetUrlFromString(UrlHandler.MultipleFromProfile), profile);
				//var result = await WebRequests.PostAsync<Profile>("http://localhost:40519/values/singleLast");

				var profiles = result.ServerResponse.FirstOrDefault();
				Serializer.JsonSerialize(profiles, "enemyProfile.json");

				//InformationManager.DisplayMessage(new InformationMessage(result)); 

			});

			task.Wait();

			JObject json = JObject.Parse(Serializer.ReadStringFromFile("enemyProfile.json"));
			//InformationManager.DisplayMessage(new InformationMessage(json.ToString()));
			ArmyContainer ac = Serializer.JsonDeserializeFromStringAc((string)json["ArmyContainer"]);



			CharacterHandler.saveLocationFile = "enemygeneral.xml";
			CharacterHandler.saveLocationPath = CharacterHandler.SaveLocationEnum.ModuleData;
			CharacterHandler.WriteToFile(ac.CharacterXML);
			CharacterHandler.LoadXML();


			Settlement closestHideout = SettlementHelper.FindNearestSettlement((Settlement x) => x.IsHideout() && x.IsActive);
			Clan clan = Clan.BanditFactions.FirstOrDefault((Clan t) => t.Culture == closestHideout.Culture);

			//MobileParty mobileParty = MobilePartyHelper.SpawnLordParty(bestAvailableCommander, new Vec2(Hero.MainHero.GetPosition().x, Hero.MainHero.GetPosition().z), 1f);
			MobileParty mobileParty = BanditPartyComponent.CreateBanditParty("EnemyClan", clan, closestHideout.Hideout, false);
			mobileParty.InitializeMobileParty(
						JsonBattleConfig.EnemyParty(ac),
						JsonBattleConfig.EnemyParty(ac),
						mobileParty.Position2D,
						0);
			PlayerEncounter.Start();

			//InformationManager.DisplayMessage(new InformationMessage(PartyBase.MainParty.IsSettlement.ToString()));
			PlayerEncounter.Current.SetupFields(PartyBase.MainParty, mobileParty.Party);
			PlayerEncounter.StartBattle();
			CampaignMission.OpenBattleMission(PlayerEncounter.GetBattleSceneForMapPosition(MobileParty.MainParty.Position2D));


		}*/

		public void ExecuteMatchHistory()
		{


			IEnumerable<MatchHistory> matchHistories;
			var task = Task.Run(async () =>
			{
				//var result = await WebRequests.RawMessageWebGet(UrlHandler.GetUrlFromString(UrlHandler.MultipleFromProfile));
				Profile profile = ProfileHandler.UpdateProfileAc();
				var result = await WebRequests.PostAsync<IEnumerable<MatchHistory>>(UrlHandler.GetUrlFromString(UrlHandler.GetMatchHistory), profile.Id);
				//var result = await WebRequests.PostAsync<Profile>("http://localhost:40519/values/singleLast");

				matchHistories = result.ServerResponse;
				//Serializer.JsonSerialize(profiles, "enemyProfile.json");

				//InformationManager.DisplayMessage(new InformationMessage(result)); 
				//ScreenManager.PopScreen();
				ScreenManager.PushScreen(new MatchHistoryScreen(matchHistories));

			});
			task.Wait();
		
		}

		private void OnSaveOver(bool isSuccessful)
		{
			if (this._exitOnSaveOver)
			{
				MBGameManager.EndGame();
			}
		}

		[DataSourceProperty]
		public bool IsAnyValidMemberSelected
		{
			get
			{
				return this._isAnyValidMemberSelected;
			}
			set
			{
				if (value != this._isAnyValidMemberSelected)
				{
					this._isAnyValidMemberSelected = value;
					base.OnPropertyChangedWithValue(value, "IsAnyValidMemberSelected");
				}
			}
		}

		[DataSourceProperty]
		public ClanLordItemVM CurrentSelectedMember
		{
			get
			{
				return this._currentSelectedMember;
			}
			set
			{
				if (value != this._currentSelectedMember)
				{
					this._currentSelectedMember = value;
					base.OnPropertyChangedWithValue(value, "CurrentSelectedMember");
					this.IsAnyValidMemberSelected = (value != null);
				}
			}
		}

		[DataSourceProperty]
        public bool IsSelected
        {
            get
            {
                return this._isSelected;
            }
            set
            {
                if (value != this._isSelected)
                {
                    this._isSelected = value;
                    base.OnPropertyChangedWithValue(value, "IsSelected");
                }
            }
        }

		[DataSourceProperty]
		public string ExpectedGoldText
		{
			get
			{
				return this._expectedGoldText;
			}
			set
			{
				if (value != this._expectedGoldText)
				{
					this._expectedGoldText = value;
					base.OnPropertyChangedWithValue(value, "ExpectedGoldText");
				}
			}
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
		public string ExpectedGold
		{
			get
			{
				return this._expectedGold;
			}
			set
			{
				if (value != this._expectedGold)
				{
					this._expectedGold = value;
					base.OnPropertyChangedWithValue(value, "ExpectedGold");
				}
			}
		}

		[DataSourceProperty]
		public string SkillsText
		{
			get
			{
				return this._skillsText;
			}
			set
			{
				if (value != this._skillsText)
				{
					this._skillsText = value;
					base.OnPropertyChangedWithValue(value, "SkillsText");
				}
			}
		}


		[DataSourceProperty]
		public string BuyRenown
		{
			get
			{
				return this._buyRenown;
			}
			set
			{
				if (value != this._buyRenown)
				{
					this._buyRenown = value;
					base.OnPropertyChangedWithValue(value, "BuyRenown");
				}
			}
		}

		[DataSourceProperty]
		public string Train
		{
			get
			{
				return this._train;
			}
			set
			{
				if (value != this._train)
				{
					this._train = value;
					base.OnPropertyChangedWithValue(value, "Train");
				}
			}
		}



		public MapSelectionGroupVM MapSelectionGroup { get; }
		//private readonly EnhancedBattleTestState _state;
		//private BattleGeneralConfig _generalConfig;
		//private readonly List<SceneData> _scenes;
		private readonly Clan _faction;

		private bool _isAnyValidMemberSelected;
		private ClanLordItemVM _currentSelectedMember;
		private bool _isSelected;
		private PartyManagerLogic _partyManagerLogic;
		private string _skillsText;
		private string _expectedGoldText;
		private string _expectedGold;

		private string _buyRenown;
		private string _train;

		private string _eloText;
		private string _elo;
		private bool _exitOnSaveOver;
	}
}
