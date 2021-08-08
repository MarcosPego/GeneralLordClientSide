using CunningLords.PlanDefinition;
using GeneralLord.FormationBattleTest;
using GeneralLord.FormationPlanHandler;
using GeneralLordWebApiClient.Model;
using Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;

namespace GeneralLord
{
    public class OpponentEntryTupleViewModel : ViewModel
    {
		public OpponentEntryTupleViewModel(Profile profile)
		{

			_profile = profile;
			ArmyContainer ac = Serializer.JsonDeserializeFromStringAc(_profile.ArmyContainer);
			_displayArmy = JsonBattleConfig.EnemyParty(ac, 2);
			this.Name = profile.Name;
			this.Elo = "Elo: " + profile.Elo.ToString();
			this.ArmyStrength = GetAverageStrength(profile);
			this.TotalArmyCount = "Troop Count: " + profile.TotalTroopCount.ToString();


			this.InfantryHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopInfo(_displayArmy, FormationClass.Infantry));
			this.CavalryHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopInfo(_displayArmy, FormationClass.Cavalry));
			this.RangedHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopInfo(_displayArmy, FormationClass.Ranged));
			this.HorseArcherHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopInfo(_displayArmy, FormationClass.HorseArcher));

			this.RefreshValues();
		}

		public void ExecuteChallenge()
		{
			BattleTestHandler.BattleTestEnabled = BattleTestHandler.BattleTestEnabledState.None;

			Serializer.JsonSerialize(_profile, "enemyProfile.json");
			ArmyContainer ac = Serializer.JsonDeserializeFromStringAc(_profile.ArmyContainer);

			if(false && _profile.SelectedFormation != -1)
            {
				EnemyFormationHandler.EnemySelectedFormation = _profile.SelectedFormation;

				string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));

				string finalPath = Path.Combine(path, "ModuleData", "enemydata.json");

				using (JsonReader reader = new JsonTextReader(new System.IO.StringReader(_profile.DefensiveFormation)))
				{
					JsonSerializer serializer = new JsonSerializer();
					List<PositionData> data = serializer.Deserialize<List<PositionData>>(reader);
					Serializer.JsonSerialize(data, finalPath);
				}
			}

			if(_profile.UseDefensiveOrder == 1)
            {
				EnemyFormationHandler.EnemyUseDefensiveSettings = 1;

				string defensiveOrders = _profile.DefensiveOrders;

				using (JsonReader reader = new JsonTextReader(new System.IO.StringReader(defensiveOrders)))
				{
					JsonSerializer serializer = new JsonSerializer();
					Plan plan =  serializer.Deserialize<Plan>(reader);
					Serializer.JsonSerialize(plan, "EnemyDecisiontree.json");
				}
				//Plan plan = Serializer.JsonDeserializeFromStringAc(_profile.DefensiveOrders);



				//Serializer.JsonSerialize(_profile.DefensiveFormation, "enemydata.json");
			} else
            {
				EnemyFormationHandler.EnemyUseDefensiveSettings = 0;
			}

			Settlement closestHideout = SettlementHelper.FindNearestSettlement((Settlement x) => x.IsHideout() && x.IsActive);
			Clan clan = Clan.BanditFactions.FirstOrDefault((Clan t) => t.Culture == closestHideout.Culture);

			//MobileParty mobileParty = MobilePartyHelper.SpawnLordParty(bestAvailableCommander, new Vec2(Hero.MainHero.GetPosition().x, Hero.MainHero.GetPosition().z), 1f);
			OpponentPartyHandler.RemoveOpponentParty();

			OpponentPartyHandler.PreBattleTroopRoster = JsonBattleConfig.EnemyParty(ac, _profile.UniqueUser);
			OpponentPartyHandler.CurrentOpponentParty = BanditPartyComponent.CreateBanditParty("EnemyClan", clan, closestHideout.Hideout, false);
			OpponentPartyHandler.CurrentOpponentParty.InitializeMobileParty(
						OpponentPartyHandler.PreBattleTroopRoster,
						OpponentPartyHandler.PreBattleTroopRoster,
						OpponentPartyHandler.CurrentOpponentParty.Position2D,
						0);

			JsonBattleConfig.copyOfTroopRosterPreviousToBattle = PartyBase.MainParty.MemberRoster.GetTroopRoster();

			PlayerEncounter.Start();

			//InformationManager.DisplayMessage(new InformationMessage(PartyBase.MainParty.IsSettlement.ToString()));
			PlayerEncounter.Current.SetupFields(PartyBase.MainParty, OpponentPartyHandler.CurrentOpponentParty.Party);
			PlayerEncounter.StartBattle();
			CampaignMission.OpenBattleMission(PlayerEncounter.GetBattleSceneForMapPosition(MobileParty.MainParty.Position2D));

			//ScreenManager.PopScreen();
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

			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			foreach (TroopRosterElement troopRosterElement in _displayArmy.GetTroopRoster())
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

		private Profile _profile;
		private string _name;
		private string _elo;
		private string _armyStrength;
		private string _totalArmyCount;

		private TroopRoster _displayArmy;

		private BasicTooltipViewModel _infantryHint;
		private BasicTooltipViewModel _rangedHint;
		private BasicTooltipViewModel _cavalryHint;
		private BasicTooltipViewModel _horseArcherHint;
		private int _infantryCount;
		private int _rangedCount;
		private int _cavalryCount;
		private int _horseArcherCount;

	}
}
