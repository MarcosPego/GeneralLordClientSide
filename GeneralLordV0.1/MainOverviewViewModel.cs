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
using TaleWorlds.MountAndBlade.CustomBattle;
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

namespace GeneralLord
{
	internal class MainOverviewViewModel : ViewModel
	{
		public MainOverviewViewModel()
		{
			EnhancedBattleTestPartyController.Initialize();


			//this._partyManagerLogic = partyManagerLogic;
			this._expectedGoldText = new TextObject("{=ATExpectedGoldText}Current Gold", null).ToString();
			this._expectedGold = PartyBase.MainParty.LeaderHero.Gold.ToString();

			this._faction = Hero.MainHero.Clan;


			this._state = new EnhancedBattleTestState();
			//this._config = BattleConfig.Deserialize(false);
			_generalConfig = new BattleGeneralConfig();

			BattleConfig.Instance = this._generalConfig._config;
			this._scenes = _state.Scenes;
			this.MapSelectionGroup = new MapSelectionGroupVM(this._scenes);


			this.SelectMainHero();
			this.RefreshValues();
		}

		public override void RefreshValues()
		{
			base.RefreshValues();
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



		// Token: 0x0600008D RID: 141 RVA: 0x00007021 File Offset: 0x00005221
		public void ExecuteStart()
		{
			//MainOverviewViewModel.CustomBattleHelper.StartGame(this.PrepareBattleData());
			//Debug.Print("EXECUTE START - PRESSED", 0, Debug.DebugColor.Green, 17592186044416UL);

			//if (!IsValid())
			//	return;
			/*if (!ApplyConfig())
            {
				InformationManager.DisplayMessage(new InformationMessage("Will Return"));
				return;
			}


			MapSelectionGroup.OnGameTypeChange(BattleType.Field);
			var sceneData = GetMap();
			if (sceneData == null)
				return;
			
			this._generalConfig._config.Serialize(false);
			GameTexts.SetVariable("MapName", sceneData.Name);
			//Utility.DisplayLocalizedText("str_ebt_current_map");
			EnhancedBattleTestPartyController.BattleConfig = this._generalConfig._config;

			//InformationManager.DisplayMessage(new InformationMessage(sceneData.Id.ToString()));

			//_config.PlayerTeamConfig.Generals = Hero.MainHero;

			this._generalConfig._config.PlayerTeamConfig.HasGeneral = true;

			this._generalConfig._config.BattleTypeConfig.PlayerType = PlayerType.Commander;

			ScreenManager.PopScreen();
			EnhancedBattleTestMissions.OpenSingleplayerMission(this._generalConfig._config, sceneData.Id);
			
			//Game.Current.GameStateManager.PopState();
			//Game.Current.GameStateManager.CleanStates();

			/*Game.Current.GameStateManager.PushState(Game.Current.GameStateManager.CreateState<GeneralLordMainGameState>(new object[]
			{
						this._partyManagerLogic
			}));*/
			/*InformationManager.DisplayMessage(new InformationMessage("pls start dude"));*/

			JsonBattleConfig.ExecuteSubmitAc();
			Task.Run(async () =>
			{
				var result = await WebRequests.RawMessageWebGet("http://localhost:40519/values/singleLast");
				//var result = await WebRequests.PostAsync<Profile>("http://localhost:40519/values/singleLast");
				Serializer.WriteJsonToFile(result, "enemyProfile.json");

				//InformationManager.DisplayMessage(new InformationMessage(result)); 
			});

			JObject json = JObject.Parse(Serializer.ReadStringFromFile("enemyProfile.json"));
			//InformationManager.DisplayMessage(new InformationMessage(json.ToString()));
			ArmyContainer ac = Serializer.JsonDeserializeFromStringAc((string)json["armyContainer"]) as ArmyContainer;

			//Serializer.JsonDeserialize("enemyProfile.json");
			//string jsonString = profile.ArmyContainer;
			Clan clan = Clan.All.First();
			Hero bestAvailableCommander = clan.Heroes.First();
			MobileParty mobileParty = MobilePartyHelper.SpawnLordParty(bestAvailableCommander, new Vec2(Hero.MainHero.GetPosition().x, Hero.MainHero.GetPosition().z), 1f);
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

		}

		public void ExecuteSubmit()
		{


			/*List<TroopContainer> troopContainers = new List<TroopContainer>();
			foreach (TroopRosterElement tre in PartyBase.MainParty.MemberRoster.GetTroopRoster())
			{

				troopContainers.Add(new TroopContainer { stringId = tre.Character.StringId, troopCount = tre.Number, troopXP = tre.Xp});

			}
			ArmyContainer ac = new ArmyContainer { TroopContainers = troopContainers };

			//XDocument xd = ArmyContainerSerializer.LoadArmyContainerXML(ac);
			Serializer.JsonSerialize(ac);
			*/
			/*Task.Run(async () =>
			{
				Profile profile = ProfileHandler.GetVerifyProfile();
				var result = await WebRequests.PostAsync<Profile>("http://localhost:40519/values/save", profile);
				Serializer.JsonSerialize(result.ServerResponse, "playerprofile.json");
			});*/

			//JsonBattleConfig.ExecuteSubmit();
			JsonBattleConfig.UpdateArmyAfterBattle();
		}

		private bool ApplyConfig()
		{
			this._generalConfig._config.MapConfig.MapNameSearchText = this.MapSelectionGroup.SearchText;
			if (this.MapSelectionGroup.SceneLevelSelection.SelectedItem != null)
			{
				this._generalConfig._config.MapConfig.SceneLevel = this.MapSelectionGroup.SceneLevelSelection.SelectedItem.Level;
			}
			if (this.MapSelectionGroup.WallHitpointSelection.SelectedItem != null)
			{
				this._generalConfig._config.MapConfig.BreachedWallCount = this.MapSelectionGroup.WallHitpointSelection.SelectedItem.BreachedWallCount;
			}
			if (this.MapSelectionGroup.SeasonSelection.SelectedItem != null)
			{
				this._generalConfig._config.MapConfig.Season = this.MapSelectionGroup.SelectedSeasonId;
			}
			if (this.MapSelectionGroup.TimeOfDaySelection.SelectedItem != null)
			{
				this._generalConfig._config.MapConfig.TimeOfDay = this.MapSelectionGroup.SelectedTimeOfDay;
			}

			if (this._generalConfig._config.BattleTypeConfig.BattleType == BattleType.Siege && (!this._generalConfig._config.PlayerTeamConfig.HasGeneral || this._generalConfig._config.PlayerTeamConfig.Generals.Troops.Count == 0))
			{
				Utility.DisplayLocalizedText("str_ebt_siege_no_player", null);
				return false;
			}
			return true;
		}

		private SceneData GetMap()
		{
			var selectedMap = MapSelectionGroup.SelectedMap;
			if (selectedMap == null)
			{
				MapSelectionGroup.RandomizeMap();
				selectedMap = MapSelectionGroup.SelectedMap;
				if (selectedMap == null)
				{
					Utility.DisplayLocalizedText("str_ebt_no_map");
					return null;
				}

				// Keep search text not changed.
				//MapSelectionGroup.SearchText = _config.MapConfig.MapNameSearchText;
			}
			return _scenes.First(data => data.Name.ToString() == selectedMap.MapName);
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
					base.OnPropertyChangedWithValue(value, "OverviewText");
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
					base.OnPropertyChangedWithValue(value, "OverviewText");
				}
			}
		}

		public MapSelectionGroupVM MapSelectionGroup { get; }
		private readonly EnhancedBattleTestState _state;
		private BattleGeneralConfig _generalConfig;
		private readonly List<SceneData> _scenes;
		//private BattleCreator _battleCreator;
		private CustomBattleState _customBattleState;
		private readonly Clan _faction;

		private bool _isAnyValidMemberSelected;
		private ClanLordItemVM _currentSelectedMember;
		private bool _isSelected;
		private PartyManagerLogic _partyManagerLogic;

		private string _expectedGoldText;
		private string _expectedGold;

		/*private static class CustomBattleHelper
		{
			// Token: 0x060001A5 RID: 421 RVA: 0x00009CD0 File Offset: 0x00007ED0
			public static void StartGame(CustomBattle.CustomBattleData data)
			{
				Game.Current.PlayerTroop = data.PlayerCharacter;
				BannerlordMissions.OpenCustomBattleMission(data.SceneId, data.PlayerCharacter, data.PlayerParty, data.EnemyParty, data.IsPlayerGeneral, data.PlayerSideGeneralCharacter, data.SceneLevel, data.SeasonId, data.TimeOfDay);
			}
		}*/
	}
}
