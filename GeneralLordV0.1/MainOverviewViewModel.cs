﻿using System;
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
using TaleWorlds.Core.ViewModelCollection;
using System.Globalization;

namespace GeneralLord
{
	public class MainOverviewViewModel : ViewModel
	{
		public MainOverviewViewModel(MainManagerViewModel mainManagerViewModel)
		{
			Party = PartyBase.MainParty;
			this._mainManagerViewModel = mainManagerViewModel;
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
			this.HealHeroText = new TextObject("{=ATHealHeroText} Pay a visit to the doctor to heal your wounds: ", null).ToString();

			this.RenownCost = PartyCapacityLogicHandler.BuyRenownPrice.ToString();
			this.TrainCost = PartyCapacityLogicHandler.TrainStewardship.ToString();

			this.TimeUntilRecovery = new TextObject("{=ATTimeUntilRecovery} Time until next group recovery: ", null).ToString();


			//PartyBase.MainParty.MemberRoster.GetTroopRoster()

			CampaignEvents.OnSaveOverEvent.AddNonSerializedListener(this, new Action<bool>(this.OnSaveOver));
			this.RefreshValues();
		}

		public override void RefreshValues()
		{
			base.RefreshValues();
			_mainManagerViewModel.RefreshValues();


            if (PartyUtilsHandler.WoundedTroopArmy.WoundedTroopsGroup.Any())
            {
				WoundedTroopGroup woundedTroopGroup = PartyUtilsHandler.WoundedTroopArmy.WoundedTroopsGroup.First();
				RecoveryHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetTroopsToRecoverInfo(woundedTroopGroup));
				if (woundedTroopGroup.timeUntilRecovery <= DateTime.Now)
                {
					this.RecoveryCount = new TextObject("{=ATRecoveryCount}"+ woundedTroopGroup.totalWoundedTroops.ToString() + " will recover shortly!", null).ToString();
					this.Timer = "";
				} else
                {
					this.RecoveryCount = woundedTroopGroup.totalWoundedTroops.ToString();
					this.Timer = (woundedTroopGroup.timeUntilRecovery - DateTime.Now).ToString(@"hh\:mm");
				}
            } 
            else
            {
				this.RecoveryCount = "";
				this.Timer = new TextObject("{=ATRecoveryCount} All Troops are fully healed! ", null).ToString();
			}


			this.SelectMainHero();
			this.Name = this.Party.Name.ToString();
			this.SkillsText = GameTexts.FindText("str_skills", null).ToString();
			this.ExpectedGoldText = new TextObject("{=ATExpectedGoldText}Current Gold: ", null).ToString();
			this.ExpectedGold = PartyBase.MainParty.LeaderHero.Gold.ToString();

			JObject json = JObject.Parse(Serializer.ReadStringFromFile("playerprofile.json"));
			this.EloText = new TextObject("{=ATEloText}Elo: ", null).ToString();
			this.Elo = json["Elo"].ToString();

			if (CharacterHandler.PriceToFullHealth() > 0) { this.HealHeroCost = CharacterHandler.PriceToFullHealth().ToString(); }
			else { this.HealHeroCost = CharacterHandler.PriceToFullHealth().ToString(); }

			UpdateProperties();
		}

		public void UpdateProperties()
        {
			GameTexts.SetVariable("LEFT", this.Party.MobileParty.MemberRoster.TotalManCount);
			GameTexts.SetVariable("RIGHT", this.Party.MobileParty.Party.PartySizeLimit);
			string text = GameTexts.FindText("str_LEFT_over_RIGHT", null).ToString();
			string content = GameTexts.FindText("str_party_morale_party_size", null).ToString();
			this.PartySizeText = text;
			GameTexts.SetVariable("LEFT", content);
			GameTexts.SetVariable("RIGHT", text);
			this.PartySizeSubTitleText = GameTexts.FindText("str_LEFT_colon_RIGHT", null).ToString();

			this.InfantryHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopInfo(Party.MemberRoster, FormationClass.Infantry));
			this.CavalryHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopInfo(Party.MemberRoster, FormationClass.Cavalry));
			this.RangedHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopInfo(Party.MemberRoster, FormationClass.Ranged));
			this.HorseArcherHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopInfo(Party.MemberRoster, FormationClass.HorseArcher));

			this.HealthyInfantryHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopHealthyInfo(Party, FormationClass.Infantry));
			this.HealthyCavalryHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopHealthyInfo(Party, FormationClass.Cavalry));
			this.HealthyRangedHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopHealthyInfo(Party, FormationClass.Ranged));
			this.HealthyHorseArcherHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopHealthyInfo(Party, FormationClass.HorseArcher));
			this.HeroHealthHint =new BasicTooltipViewModel(() => CampaignUIHelper.GetHeroHealthTooltip(Party.LeaderHero));

			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			foreach (TroopRosterElement troopRosterElement in this.Party.MemberRoster.GetTroopRoster())
			{
				Hero heroObject = troopRosterElement.Character.HeroObject;
				if (heroObject != null && heroObject.Clan == Clan.PlayerClan)
				{

				}
				else if (troopRosterElement.Character.DefaultFormationClass.Equals(FormationClass.Infantry))
				{
					num += troopRosterElement.Number;
				}
				else if (troopRosterElement.Character.DefaultFormationClass.Equals(FormationClass.Ranged))
				{
					num2 += troopRosterElement.Number;
				}
				else if (troopRosterElement.Character.DefaultFormationClass.Equals(FormationClass.Cavalry))
				{
					num3 += troopRosterElement.Number;
				}
				else if (troopRosterElement.Character.DefaultFormationClass.Equals(FormationClass.HorseArcher))
				{
					num4 += troopRosterElement.Number;
				}
			}

			this.InfantryCount = num;
			this.RangedCount = num2;
			this.CavalryCount = num3;
			this.HorseArcherCount = num4;


			num = 0;
			num2 = 0;
			num3 = 0;
			num4 = 0;

			foreach (TroopRosterElement troopRosterElement in this.Party.MemberRoster.GetTroopRoster())
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

		public void ExecuteHealHero()
		{

			CharacterHandler.HandleHealthBuy();
			this.RefreshValues();
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
			InformationManager.DisplayMessage(new InformationMessage("To Be Implemented!"));
			this.RefreshValues();
		}

		public void ExecuteFormation()
		{


			BattleTestHandler.OpenBattleTestMission();
			//this.RefreshValues();
		}

		public void ExectuteLeaveGme()
        {
			InformationManager.DisplayMessage(new InformationMessage("Exiting to Main Menu!"));

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

		[DataSourceProperty]
		public string RenownCost
		{
			get
			{
				return this._buyRenownCost;
			}
			set
			{
				if (value != this._buyRenownCost)
				{
					this._buyRenownCost = value;
					base.OnPropertyChangedWithValue(value, "RenownCost");
				}
			}
		}

		[DataSourceProperty]
		public string TrainCost
		{
			get
			{
				return this._trainCost;
			}
			set
			{
				if (value != this._trainCost)
				{
					this._trainCost = value;
					base.OnPropertyChangedWithValue(value, "TrainCost");
				}
			}
		}


		[DataSourceProperty]
		public string HealHeroText
		{
			get
			{
				return this._healHeroText;
			}
			set
			{
				if (value != this._healHeroText)
				{
					this._healHeroText = value;
					base.OnPropertyChangedWithValue(value, "HealHeroText");
				}
			}
		}

		[DataSourceProperty]
		public string HealHeroCost
		{
			get
			{
				return this._healHeroCost;
			}
			set
			{
				if (value != this._healHeroCost)
				{
					this._healHeroCost = value;
					base.OnPropertyChangedWithValue(value, "HealHeroCost");
				}
			}
		}

		[DataSourceProperty]
		public BasicTooltipViewModel RecoveryHint
		{
			get
			{
				return this._recoveryHint;
			}
			set
			{
				if (value != this._recoveryHint)
				{
					this._recoveryHint = value;
					base.OnPropertyChangedWithValue(value, "RecoveryHint");
				}
			}
		}


		[DataSourceProperty]
		public BasicTooltipViewModel InfantryHint
		{
			get
			{
				return this._infantryHint;
			}
			set
			{
				if (value != this._infantryHint)
				{
					this._infantryHint = value;
					base.OnPropertyChangedWithValue(value, "InfantryHint");
				}
			}
		}

		[DataSourceProperty]
		public BasicTooltipViewModel RangedHint
		{
			get
			{
				return this._rangedHint;
			}
			set
			{
				if (value != this._rangedHint)
				{
					this._rangedHint = value;
					base.OnPropertyChangedWithValue(value, "RangedHint");
				}
			}
		}

		[DataSourceProperty]
		public BasicTooltipViewModel CavalryHint
		{
			get
			{
				return this._cavalryHint;
			}
			set
			{
				if (value != this._cavalryHint)
				{
					this._cavalryHint = value;
					base.OnPropertyChangedWithValue(value, "CavalryHint");
				}
			}
		}

		[DataSourceProperty]
		public BasicTooltipViewModel HorseArcherHint
		{
			get
			{
				return this._horseArcherHint;
			}
			set
			{
				if (value != this._horseArcherHint)
				{
					this._horseArcherHint = value;
					base.OnPropertyChangedWithValue(value, "HorseArcherHint");
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
		public int InfantryCount
		{
			get
			{
				return this._infantryCount;
			}
			set
			{
				if (value != this._infantryCount)
				{
					this._infantryCount = value;
					base.OnPropertyChangedWithValue(value, "InfantryCount");
				}
			}
		}

		[DataSourceProperty]
		public int RangedCount
		{
			get
			{
				return this._rangedCount;
			}
			set
			{
				if (value != this._rangedCount)
				{
					this._rangedCount = value;
					base.OnPropertyChangedWithValue(value, "RangedCount");
				}
			}
		}

		[DataSourceProperty]
		public int CavalryCount
		{
			get
			{
				return this._cavalryCount;
			}
			set
			{
				if (value != this._cavalryCount)
				{
					this._cavalryCount = value;
					base.OnPropertyChangedWithValue(value, "CavalryCount");
				}
			}
		}

		[DataSourceProperty]
		public int HorseArcherCount
		{
			get
			{
				return this._horseArcherCount;
			}
			set
			{
				if (value != this._horseArcherCount)
				{
					this._horseArcherCount = value;
					base.OnPropertyChangedWithValue(value, "HorseArcherCount");
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
				return (int)Math.Ceiling((double)this.Party.LeaderHero.HitPoints * 100.0 / (double)this.Party.LeaderHero.CharacterObject.MaxHitPoints());
			}
		}



		[DataSourceProperty]
		public string TimeUntilRecovery
		{
			get
			{
				return this._timeUntilRecovery;
			}
			set
			{
				if (value != this._timeUntilRecovery)
				{
					this._timeUntilRecovery = value;
					base.OnPropertyChangedWithValue(value, "TimeUntilRecovery");
				}
			}
		}

		[DataSourceProperty]
		public string RecoveryCount
		{
			get
			{
				return this._recoveryCount;
			}
			set
			{
				if (value != this._recoveryCount)
				{
					this._recoveryCount = value;
					base.OnPropertyChangedWithValue(value, "RecoveryCount");
				}
			}
		}

		[DataSourceProperty]
		public string Timer
		{
			get
			{
				return this._timer;
			}
			set
			{
				if (value != this._timer)
				{
					this._timer = value;
					base.OnPropertyChangedWithValue(value, "Timer");
				}
			}
		}
		public PartyBase Party { get; }


		private string _timeUntilRecovery;
		private string _recoveryCount;
		private string _timer;


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
		private string _name;

		private string _buyRenown;
		private string _train;

		private string _buyRenownCost;
		private string _trainCost;

		private string _healHeroText;
		private string _healHeroCost;

		private string _eloText;
		private string _elo;
		private bool _exitOnSaveOver;
		private MainManagerViewModel _mainManagerViewModel;

		private BasicTooltipViewModel _recoveryHint;
		private BasicTooltipViewModel _infantryHint;
		private BasicTooltipViewModel _rangedHint;
		private BasicTooltipViewModel _cavalryHint;
		private BasicTooltipViewModel _horseArcherHint;

		private BasicTooltipViewModel _healthyInfantryHint;
		private BasicTooltipViewModel _healthyRangedHint;
		private BasicTooltipViewModel _healthyCavalryHint;
		private BasicTooltipViewModel _healthyHorseArcherHint;

		private BasicTooltipViewModel _heroHealthHint;

		private int _infantryCount;
		private int _rangedCount;
		private int _cavalryCount;
		private int _horseArcherCount;

		private int _healthyInfantryCount;
		private int _healthyRangedCount;
		private int _healthyCavalryCount;
		private int _healthyHorseArcherCount;

		private string _partySizeText;
		private string _partySizeSubTitleText;
	}
}
