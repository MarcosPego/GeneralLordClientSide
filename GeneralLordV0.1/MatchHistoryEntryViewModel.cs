using GeneralLordWebApiClient.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;

namespace GeneralLord
{
    public class MatchHistoryEntryViewModel : ViewModel
	{
		public static string VictoryBrush = "NameTitle.LargeGreen";
		public static string DefeatBrush = "NameTitle.LargeRed";
		public static string GreenBrush = "NameTitle.Green";
		public static string RedBrush = "NameTitle.Red";


		public static string DefaultBrush = "Clan.Tuple.Name.Text";

		public static string HighlightBrush = "NameTitle.Highlight";

		public MatchHistoryEntryViewModel(MatchHistory matchHistory){
            _matchHistory = matchHistory;

			ArmyContainer ac = JsonConvert.DeserializeObject<ArmyContainer>(matchHistory.PlayerArmyContainer);


			TroopRoster troopRoster = new TroopRoster(PartyBase.MainParty);
			foreach (TroopContainer tc in ac.TroopContainers)
			{
				if (tc.stringId != "main_hero" && tc.stringId != "tutorial_npc_brother")
				{
					JsonBattleConfig.TryAddCharacterToRoster(troopRoster, tc.stringId, tc.troopCount);
				}

			}
			_displayPlayerArmy = troopRoster;

			ac = JsonConvert.DeserializeObject<ArmyContainer>(matchHistory.EnemyArmyContainer);
			troopRoster = new TroopRoster(PartyBase.MainParty);
			foreach (TroopContainer tc in ac.TroopContainers)
			{
				if (tc.stringId != "main_hero" && tc.stringId != "tutorial_npc_brother")
				{
					JsonBattleConfig.TryAddCharacterToRoster(troopRoster, tc.stringId, tc.troopCount);
				}

			}
			_displayEnemyArmy = troopRoster;

			ac = JsonConvert.DeserializeObject<ArmyContainer>(matchHistory.PlayerFallenArmyContainer);
			troopRoster = new TroopRoster(PartyBase.MainParty);
			foreach (TroopContainer tc in ac.TroopContainers)
			{
				if (tc.stringId != "main_hero" && tc.stringId != "tutorial_npc_brother")
				{
					JsonBattleConfig.TryAddCharacterToRoster(troopRoster, tc.stringId, tc.troopCount);
				}

			}
			_displayFallenPlayerArmy = troopRoster;

			ac = JsonConvert.DeserializeObject<ArmyContainer>(matchHistory.EnemyFallenArmyContainer);
			troopRoster = new TroopRoster(PartyBase.MainParty);
			foreach (TroopContainer tc in ac.TroopContainers)
			{
				if (tc.stringId != "main_hero" && tc.stringId != "tutorial_npc_brother")
				{
					JsonBattleConfig.TryAddCharacterToRoster(troopRoster, tc.stringId, tc.troopCount);
				}

			}
			_displayFallenEnemyArmy = troopRoster;

			this.AttackerArmy = "Army Composition: ";
			this.FallenAttackerArmy = "Casualties: ";
			this.DefenderArmy = "Army Composition: ";
			this.FallenDefenderArmy = "Casualties: ";

			this.ExtraInformationVisible = true;
			InstantiateValues(matchHistory);
			this.RefreshValues();
		}

		public void ExecuteSetSelected()
        {
			this.ExtraInformationVisible = !this.ExtraInformationVisible;
		}


		public void InstantiateValues(MatchHistory matchHistory)
        {
            this.DateOfMatch = matchHistory.LocalTimeDatePostMatch.ToString("dd MMM yyyy HH:mm:ss");

			TroopRoster attackerArmy = new TroopRoster(PartyBase.MainParty);
			TroopRoster attackerFallenArmy = new TroopRoster(PartyBase.MainParty);
			TroopRoster defenderArmy = new TroopRoster(PartyBase.MainParty);
			TroopRoster defenderFallenArmy = new TroopRoster(PartyBase.MainParty);

			if (matchHistory.PlayerName == PartyBase.MainParty.LeaderHero.Name.ToString())
			{
				if (matchHistory.BattleResult == "PlayerVictory")
				{
					this.BattleResult = "Victory!";
					BattleResultBrush = VictoryBrush;
				}
				else
				{
					this.BattleResult = "Defeat!";
					BattleResultBrush = DefeatBrush;
				}


				this.PlayerSide = "Attacker";

				this.AttackerName = matchHistory.PlayerName;
				this.AttackerElo = "Elo: " + matchHistory.PlayerElo.ToString();
				this.AttackerTotalArmyCount = "Troops: " + matchHistory.PlayerTroopCount.ToString();
				this.AttackerArmyStrength = GetAverageStrength(matchHistory.PlayerArmyStrength, matchHistory.EnemyArmyStrength);
				this.AttackerSidePartyName = matchHistory.PlayerName + "'s Party";

				attackerArmy = _displayPlayerArmy;
				attackerFallenArmy = _displayFallenPlayerArmy;
				defenderArmy = _displayEnemyArmy;
				defenderFallenArmy = _displayFallenEnemyArmy;

				this.DefenderName = matchHistory.EnemyName;
				this.DefenderElo = "Elo: " + matchHistory.EnemyElo.ToString();
				this.DefenderTotalArmyCount ="Troops: " + matchHistory.EnemyTroopCount.ToString();
				this.DefenderArmyStrength = GetAverageStrength(matchHistory.EnemyArmyStrength, matchHistory.PlayerArmyStrength);
				this.DefenderSidePartyName = matchHistory.EnemyName + "'s Party";


				this.EloChange = "EloChange: " + matchHistory.PlayerEloChange.ToString();
				
				if(matchHistory.PlayerEloChange >= 0)
                {
					this.EloChangeBrush = GreenBrush;

				} else
                {
					this.EloChangeBrush = RedBrush;

				}
				
				this.AttackerNameBrush = HighlightBrush;
				this.DefenderNameBrush = DefaultBrush;

			}
			else
			{
				if (matchHistory.BattleResult != "PlayerVictory")
				{
					this.BattleResult = "Victory!";
					BattleResultBrush = VictoryBrush;
				}
				else
				{
					this.BattleResult = "Defeat!";
					BattleResultBrush = DefeatBrush;
				}


				this.PlayerSide = "Defender";

				this.AttackerName = matchHistory.PlayerName;
				this.AttackerElo = "Elo: " + matchHistory.PlayerElo.ToString();
				this.AttackerTotalArmyCount = "Troops: " + matchHistory.PlayerTroopCount.ToString();
				this.AttackerArmyStrength = GetAverageStrength(matchHistory.PlayerArmyStrength, matchHistory.EnemyArmyStrength);
				this.AttackerSidePartyName = matchHistory.EnemyName + "'s Party";

				attackerArmy = _displayEnemyArmy;
				attackerFallenArmy = _displayFallenEnemyArmy;
				defenderArmy = _displayPlayerArmy;
				defenderFallenArmy = _displayFallenPlayerArmy;

				this.DefenderName = matchHistory.EnemyName;
				this.DefenderElo = "Elo: " + matchHistory.EnemyElo.ToString();
				this.DefenderTotalArmyCount = "Troops: " + matchHistory.EnemyTroopCount.ToString();
				this.DefenderArmyStrength = GetAverageStrength(matchHistory.EnemyArmyStrength, matchHistory.PlayerArmyStrength);
				this.DefenderSidePartyName = matchHistory.PlayerName + "'s Party";

				this.EloChange = "EloChange: " + matchHistory.EnemyEloChange.ToString();

				if (matchHistory.EnemyEloChange >= 0)
				{
					this.EloChangeBrush = GreenBrush;

				}
				else
				{
					this.EloChangeBrush = RedBrush;

				}

				this.AttackerNameBrush = DefaultBrush;
				this.DefenderNameBrush = HighlightBrush;
			}

			_attackerInfantryHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopInfo(attackerArmy, FormationClass.Infantry));
			_attackerRangedHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopInfo(attackerArmy, FormationClass.Cavalry));
			_attackerCavalryHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopInfo(attackerArmy, FormationClass.Ranged));
			_attackerHorseArcherHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopInfo(attackerArmy, FormationClass.HorseArcher));

			_defenderInfantryHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopInfo(defenderArmy, FormationClass.Infantry));
			_defenderRangedHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopInfo(defenderArmy, FormationClass.Cavalry));
			_defenderCavalryHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopInfo(defenderArmy, FormationClass.Ranged));
			_defenderHorseArcherHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopInfo(defenderArmy, FormationClass.HorseArcher));


			_fallenAttackerInfantryHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopInfo(attackerFallenArmy, FormationClass.Infantry));
			_fallenAttackerRangedHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopInfo(attackerFallenArmy, FormationClass.Cavalry));
			_fallenAttackerCavalryHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopInfo(attackerFallenArmy, FormationClass.Ranged));
			_fallenAttackerHorseArcherHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopInfo(attackerFallenArmy, FormationClass.HorseArcher));

			_fallenDefenderInfantryHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopInfo(defenderFallenArmy, FormationClass.Infantry));
			_fallenDefenderRangedHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopInfo(defenderFallenArmy, FormationClass.Cavalry));
			_fallenDefenderCavalryHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopInfo(defenderFallenArmy, FormationClass.Ranged));
			_fallenDefenderHorseArcherHint = new BasicTooltipViewModel(() => JsonBattleConfig.GetPartyTroopInfo(defenderFallenArmy, FormationClass.HorseArcher));


			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			foreach (TroopRosterElement troopRosterElement in attackerArmy.GetTroopRoster())
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

			this.AttackerInfantryCount = num;
			this.AttackerRangedCount = num2;
			this.AttackerCavalryCount = num3;
			this.AttackerHorseArcherCount = num4;

			num = 0;
			num2 = 0;
			num3 = 0;
			num4 = 0;
			foreach (TroopRosterElement troopRosterElement in defenderArmy.GetTroopRoster())
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

			this.DefenderInfantryCount = num;
			this.DefenderRangedCount = num2;
			this.DefenderCavalryCount = num3;
			this.DefenderHorseArcherCount = num4;


			num = 0;
			num2 = 0;
			num3 = 0;
			num4 = 0;
			foreach (TroopRosterElement troopRosterElement in attackerFallenArmy.GetTroopRoster())
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

			this.FallenAttackerInfantryCount = num;
			this.FallenAttackerRangedCount = num2;
			this.FallenAttackerCavalryCount = num3;
			this.FallenAttackerHorseArcherCount = num4;

			num = 0;
			num2 = 0;
			num3 = 0;
			num4 = 0;
			foreach (TroopRosterElement troopRosterElement in defenderFallenArmy.GetTroopRoster())
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

			this.FallenDefenderInfantryCount = num;
			this.FallenDefenderRangedCount = num2;
			this.FallenDefenderCavalryCount = num3;
			this.FallenDefenderHorseArcherCount = num4;
		}
		public string GetAverageStrength(float evaluatedArmyStrength, float otherArmyStrength)
		{
			float ratio = evaluatedArmyStrength / otherArmyStrength;

			if (ratio < 0.5f)
			{
				return "Weaker Army";
			}
			if (ratio > 2.0f)
			{
				return "Stronger Army";
			}

			return "Similar Army";
		}

		public override void RefreshValues()
		{
			base.RefreshValues();

		}

		public override void OnFinalize()
		{
			base.OnFinalize();
		}

		[DataSourceProperty]
		public string PlayerSide
		{
			get
			{
				return this._playerSide;
			}
			set
			{
				if (value != this._playerSide)
				{
					this._playerSide = value;
					base.OnPropertyChangedWithValue(value, "PlayerSide");
				}
			}
		}

		[DataSourceProperty]
		public string BattleResult
		{
			get
			{
				return this._battleResult;
			}
			set
			{
				if (value != this._battleResult)
				{
					this._battleResult = value;
					base.OnPropertyChangedWithValue(value, "BattleResult");
				}
			}
		}

		[DataSourceProperty]
		public string AttackerName
		{
			get
			{
				return this._attackerName;
			}
			set
			{
				if (value != this._attackerName)
				{
					this._attackerName = value;
					base.OnPropertyChangedWithValue(value, "AttackerName");
				}
			}
		}

		[DataSourceProperty]
		public string DefenderName
		{
			get
			{
				return this._defenderName;
			}
			set
			{
				if (value != this._defenderName)
				{
					this._defenderName = value;
					base.OnPropertyChangedWithValue(value, "DefenderName");
				}
			}
		}

		[DataSourceProperty]
		public string AttackerElo
		{
			get
			{
				return this._attackerElo;
			}
			set
			{
				if (value != this._attackerElo)
				{
					this._attackerElo = value;
					base.OnPropertyChangedWithValue(value, "AttackerElo");
				}
			}
		}

		[DataSourceProperty]
		public string DefenderElo
		{
			get
			{
				return this._defenderElo;
			}
			set
			{
				if (value != this._defenderElo)
				{
					this._defenderElo = value;
					base.OnPropertyChangedWithValue(value, "DefenderElo");
				}
			}
		}

		[DataSourceProperty]
		public string AttackerArmyStrength
		{
			get
			{
				return this._attackerArmyStrength;
			}
			set
			{
				if (value != this._attackerArmyStrength)
				{
					this._attackerArmyStrength = value;
					base.OnPropertyChangedWithValue(value, "AttackerArmyStrength");
				}
			}
		}

		[DataSourceProperty]
		public string AttackerTotalArmyCount
		{
			get
			{
				return this._attackerTotalArmyCount;
			}
			set
			{
				if (value != this._attackerTotalArmyCount)
				{
					this._attackerTotalArmyCount = value;
					base.OnPropertyChangedWithValue(value, "AttackerTotalArmyCount");
				}
			}
		}

		[DataSourceProperty]
		public string DefenderArmyStrength
		{
			get
			{
				return this._defenderArmyStrength;
			}
			set
			{
				if (value != this._defenderArmyStrength)
				{
					this._defenderArmyStrength = value;
					base.OnPropertyChangedWithValue(value, "DefenderArmyStrength");
				}
			}
		}

		[DataSourceProperty]
		public string DefenderTotalArmyCount
		{
			get
			{
				return this._defenderTotalArmyCount;
			}
			set
			{
				if (value != this._defenderTotalArmyCount)
				{
					this._defenderTotalArmyCount = value;
					base.OnPropertyChangedWithValue(value, "DefenderTotalArmyCount");
				}
			}
		}

		[DataSourceProperty]
		public string EloChange
		{
			get
			{
				return this._eloChange;
			}
			set
			{
				if (value != this._eloChange)
				{
					this._eloChange = value;
					base.OnPropertyChangedWithValue(value, "EloChange");
				}
			}
		}


		//Brushes
		[DataSourceProperty]
		public string BattleResultBrush
		{
			get
			{
				return this._battleResultBrush;
			}
			set
			{
				if (value != this._battleResultBrush)
				{
					this._battleResultBrush = value;
					base.OnPropertyChangedWithValue(value, "BattleResultBrush");
				}
			}
		}

		[DataSourceProperty]
		public string EloChangeBrush
		{
			get
			{
				return this._eloChangeBrush;
			}
			set
			{
				if (value != this._eloChangeBrush)
				{
					this._eloChangeBrush = value;
					base.OnPropertyChangedWithValue(value, "EloChangeBrush");
				}
			}
		}


		[DataSourceProperty]
		public string AttackerSidePartyName
		{
			get
			{
				return this._attackerSidePartyName;
			}
			set
			{
				if (value != this._attackerSidePartyName)
				{
					this._attackerSidePartyName = value;
					base.OnPropertyChangedWithValue(value, "AttackerSidePartyName");
				}
			}
		}

		[DataSourceProperty]
		public string DefenderSidePartyName
		{
			get
			{
				return this._defenderSidePartyName;
			}
			set
			{
				if (value != this._defenderSidePartyName)
				{
					this._defenderSidePartyName = value;
					base.OnPropertyChangedWithValue(value, "DefenderSidePartyName");
				}
			}
		}


		[DataSourceProperty]
		public string AttackerNameBrush
		{
			get
			{
				return this._attackerNameBrush;
			}
			set
			{
				if (value != this._attackerNameBrush)
				{
					this._attackerNameBrush = value;
					base.OnPropertyChangedWithValue(value, "AttackerNameBrush");
				}
			}
		}


		[DataSourceProperty]
		public string DefenderNameBrush
		{
			get
			{
				return this._defenderNameBrush;
			}
			set
			{
				if (value != this._defenderNameBrush)
				{
					this._defenderNameBrush = value;
					base.OnPropertyChangedWithValue(value, "DefenderNameBrush");
				}
			}
		}

		public string DateOfMatch
		{
			get
			{
				return this._dateOfMatch;
			}
			set
			{
				if (value != this._dateOfMatch)
				{
					this._dateOfMatch = value;
					base.OnPropertyChangedWithValue(value, "DateOfMatch");
				}
			}
		}

		[DataSourceProperty]
		public bool ExtraInformationVisible
		{
			get
			{
				return this._extraInformationVisible;
			}
			set
			{
				if (value != this._extraInformationVisible)
				{
					this._extraInformationVisible = value;
					base.OnPropertyChangedWithValue(value, "ExtraInformationVisible");
				}
			}
		}

		[DataSourceProperty]
		public BasicTooltipViewModel AttackerInfantryHint
		{
			get
			{
				return this._attackerInfantryHint;
			}
			set
			{
				if (value != this._attackerInfantryHint)
				{
					this._attackerInfantryHint = value;
					base.OnPropertyChangedWithValue(value, "AttackerInfantryHint");
				}
			}
		}

		[DataSourceProperty]
		public BasicTooltipViewModel AttackerRangedHint
		{
			get
			{
				return this._attackerRangedHint;
			}
			set
			{
				if (value != this._attackerRangedHint)
				{
					this._attackerRangedHint = value;
					base.OnPropertyChangedWithValue(value, "AttackerRangedHint");
				}
			}
		}

		[DataSourceProperty]
		public BasicTooltipViewModel AttackerCavalryHint
		{
			get
			{
				return this._attackerCavalryHint;
			}
			set
			{
				if (value != this._attackerCavalryHint)
				{
					this._attackerCavalryHint = value;
					base.OnPropertyChangedWithValue(value, "AttackerCavalryHint");
				}
			}
		}

		[DataSourceProperty]
		public BasicTooltipViewModel AttackerHorseArcherHint
		{
			get
			{
				return this._attackerHorseArcherHint;
			}
			set
			{
				if (value != this._attackerHorseArcherHint)
				{
					this._attackerHorseArcherHint = value;
					base.OnPropertyChangedWithValue(value, "AttackerHorseArcherHint");
				}
			}
		}
		[DataSourceProperty]
		public BasicTooltipViewModel DefenderInfantryHint
		{
			get
			{
				return this._defenderInfantryHint;
			}
			set
			{
				if (value != this._defenderInfantryHint)
				{
					this._defenderInfantryHint = value;
					base.OnPropertyChangedWithValue(value, "DefenderInfantryHint");
				}
			}
		}

		[DataSourceProperty]
		public BasicTooltipViewModel DefenderRangedHint
		{
			get
			{
				return this._defenderRangedHint;
			}
			set
			{
				if (value != this._defenderRangedHint)
				{
					this._defenderRangedHint = value;
					base.OnPropertyChangedWithValue(value, "DefenderRangedHint");
				}
			}
		}

		[DataSourceProperty]
		public BasicTooltipViewModel DefenderCavalryHint
		{
			get
			{
				return this._defenderCavalryHint;
			}
			set
			{
				if (value != this._defenderCavalryHint)
				{
					this._defenderCavalryHint = value;
					base.OnPropertyChangedWithValue(value, "DefenderCavalryHint");
				}
			}
		}

		[DataSourceProperty]
		public BasicTooltipViewModel DefenderHorseArcherHint
		{
			get
			{
				return this._defenderHorseArcherHint;
			}
			set
			{
				if (value != this._defenderHorseArcherHint)
				{
					this._defenderHorseArcherHint = value;
					base.OnPropertyChangedWithValue(value, "DefenderHorseArcherHint");
				}
			}
		}
		[DataSourceProperty]
		public int AttackerInfantryCount
		{
			get
			{
				return this._attackerInfantryCount;
			}
			set
			{
				if (value != this._attackerInfantryCount)
				{
					this._attackerInfantryCount = value;
					base.OnPropertyChangedWithValue(value, "AttackerInfantryCount");
				}
			}
		}

		[DataSourceProperty]
		public int AttackerRangedCount
		{
			get
			{
				return this._attackerRangedCount;
			}
			set
			{
				if (value != this._attackerRangedCount)
				{
					this._attackerRangedCount = value;
					base.OnPropertyChangedWithValue(value, "AttackerRangedCount");
				}
			}
		}

		[DataSourceProperty]
		public int AttackerCavalryCount
		{
			get
			{
				return this._attackerCavalryCount;
			}
			set
			{
				if (value != this._attackerCavalryCount)
				{
					this._attackerCavalryCount = value;
					base.OnPropertyChangedWithValue(value, "AttackerCavalryCount");
				}
			}
		}

		[DataSourceProperty]
		public int AttackerHorseArcherCount
		{
			get
			{
				return this._attackerHorseArcherCount;
			}
			set
			{
				if (value != this._attackerHorseArcherCount)
				{
					this._attackerHorseArcherCount = value;
					base.OnPropertyChangedWithValue(value, "AttackerHorseArcherCount");
				}
			}
		}
		[DataSourceProperty]
		public int DefenderInfantryCount
		{
			get
			{
				return this._defenderInfantryCount;
			}
			set
			{
				if (value != this._defenderInfantryCount)
				{
					this._defenderInfantryCount = value;
					base.OnPropertyChangedWithValue(value, "DefenderInfantryCount");
				}
			}
		}

		[DataSourceProperty]
		public int DefenderRangedCount
		{
			get
			{
				return this._defenderRangedCount;
			}
			set
			{
				if (value != this._defenderRangedCount)
				{
					this._defenderRangedCount = value;
					base.OnPropertyChangedWithValue(value, "DefenderRangedCount");
				}
			}
		}

		[DataSourceProperty]
		public int DefenderCavalryCount
		{
			get
			{
				return this._defenderCavalryCount;
			}
			set
			{
				if (value != this._defenderCavalryCount)
				{
					this._defenderCavalryCount = value;
					base.OnPropertyChangedWithValue(value, "DefenderCavalryCount");
				}
			}
		}

		[DataSourceProperty]
		public int DefenderHorseArcherCount
		{
			get
			{
				return this._defenderHorseArcherCount;
			}
			set
			{
				if (value != this._defenderHorseArcherCount)
				{
					this._defenderHorseArcherCount = value;
					base.OnPropertyChangedWithValue(value, "DefenderHorseArcherCount");
				}
			}
		}

		[DataSourceProperty]
		public BasicTooltipViewModel FallenAttackerInfantryHint
		{
			get
			{
				return this._fallenAttackerInfantryHint;
			}
			set
			{
				if (value != this._fallenAttackerInfantryHint)
				{
					this._fallenAttackerInfantryHint = value;
					base.OnPropertyChangedWithValue(value, "FallenAttackerInfantryHint");
				}
			}
		}

		[DataSourceProperty]
		public BasicTooltipViewModel FallenAttackerRangedHint
		{
			get
			{
				return this._fallenAttackerRangedHint;
			}
			set
			{
				if (value != this._fallenAttackerRangedHint)
				{
					this._fallenAttackerRangedHint = value;
					base.OnPropertyChangedWithValue(value, "FallenAttackerRangedHint");
				}
			}
		}

		[DataSourceProperty]
		public BasicTooltipViewModel FallenAttackerCavalryHint
		{
			get
			{
				return this._fallenAttackerCavalryHint;
			}
			set
			{
				if (value != this._fallenAttackerCavalryHint)
				{
					this._fallenAttackerCavalryHint = value;
					base.OnPropertyChangedWithValue(value, "FallenAttackerCavalryHint");
				}
			}
		}

		[DataSourceProperty]
		public BasicTooltipViewModel FallenAttackerHorseArcherHint
		{
			get
			{
				return this._fallenAttackerHorseArcherHint;
			}
			set
			{
				if (value != this._fallenAttackerHorseArcherHint)
				{
					this._fallenAttackerHorseArcherHint = value;
					base.OnPropertyChangedWithValue(value, "FallenAttackerHorseArcherHint");
				}
			}
		}
		[DataSourceProperty]
		public BasicTooltipViewModel FallenDefenderInfantryHint
		{
			get
			{
				return this._fallenDefenderInfantryHint;
			}
			set
			{
				if (value != this._fallenDefenderInfantryHint)
				{
					this._fallenDefenderInfantryHint = value;
					base.OnPropertyChangedWithValue(value, "FallenDefenderInfantryHint");
				}
			}
		}

		[DataSourceProperty]
		public BasicTooltipViewModel FallenDefenderRangedHint
		{
			get
			{
				return this._fallenDefenderRangedHint;
			}
			set
			{
				if (value != this._fallenDefenderRangedHint)
				{
					this._fallenDefenderRangedHint = value;
					base.OnPropertyChangedWithValue(value, "FallenDefenderRangedHint");
				}
			}
		}

		[DataSourceProperty]
		public BasicTooltipViewModel FallenDefenderCavalryHint
		{
			get
			{
				return this._fallenDefenderCavalryHint;
			}
			set
			{
				if (value != this._fallenDefenderCavalryHint)
				{
					this._fallenDefenderCavalryHint = value;
					base.OnPropertyChangedWithValue(value, "FallenDefenderCavalryHint");
				}
			}
		}

		[DataSourceProperty]
		public BasicTooltipViewModel FallenDefenderHorseArcherHint
		{
			get
			{
				return this._fallenDefenderHorseArcherHint;
			}
			set
			{
				if (value != this._fallenDefenderHorseArcherHint)
				{
					this._fallenDefenderHorseArcherHint = value;
					base.OnPropertyChangedWithValue(value, "FallenDefenderHorseArcherHint");
				}
			}
		}
		[DataSourceProperty]
		public int FallenAttackerInfantryCount
		{
			get
			{
				return this._fallenAttackerInfantryCount;
			}
			set
			{
				if (value != this._fallenAttackerInfantryCount)
				{
					this._fallenAttackerInfantryCount = value;
					base.OnPropertyChangedWithValue(value, "FallenAttackerInfantryCount");
				}
			}
		}

		[DataSourceProperty]
		public int FallenAttackerRangedCount
		{
			get
			{
				return this._fallenAttackerRangedCount;
			}
			set
			{
				if (value != this._fallenAttackerRangedCount)
				{
					this._fallenAttackerRangedCount = value;
					base.OnPropertyChangedWithValue(value, "FallenAttackerRangedCount");
				}
			}
		}

		[DataSourceProperty]
		public int FallenAttackerCavalryCount
		{
			get
			{
				return this._fallenAttackerCavalryCount;
			}
			set
			{
				if (value != this._fallenAttackerCavalryCount)
				{
					this._fallenAttackerCavalryCount = value;
					base.OnPropertyChangedWithValue(value, "FallenAttackerCavalryCount");
				}
			}
		}

		[DataSourceProperty]
		public int FallenAttackerHorseArcherCount
		{
			get
			{
				return this._fallenAttackerHorseArcherCount;
			}
			set
			{
				if (value != this._fallenAttackerHorseArcherCount)
				{
					this._fallenAttackerHorseArcherCount = value;
					base.OnPropertyChangedWithValue(value, "FallenAttackerHorseArcherCount");
				}
			}
		}
		[DataSourceProperty]
		public int FallenDefenderInfantryCount
		{
			get
			{
				return this._fallenDefenderInfantryCount;
			}
			set
			{
				if (value != this._fallenDefenderInfantryCount)
				{
					this._fallenDefenderInfantryCount = value;
					base.OnPropertyChangedWithValue(value, "FallenDefenderInfantryCount");
				}
			}
		}

		[DataSourceProperty]
		public int FallenDefenderRangedCount
		{
			get
			{
				return this._fallenDefenderRangedCount;
			}
			set
			{
				if (value != this._fallenDefenderRangedCount)
				{
					this._fallenDefenderRangedCount = value;
					base.OnPropertyChangedWithValue(value, "FallenDefenderRangedCount");
				}
			}
		}

		[DataSourceProperty]
		public int FallenDefenderCavalryCount
		{
			get
			{
				return this._fallenDefenderCavalryCount;
			}
			set
			{
				if (value != this._fallenDefenderCavalryCount)
				{
					this._fallenDefenderCavalryCount = value;
					base.OnPropertyChangedWithValue(value, "FallenDefenderCavalryCount");
				}
			}
		}

		[DataSourceProperty]
		public int FallenDefenderHorseArcherCount
		{
			get
			{
				return this._fallenDefenderHorseArcherCount;
			}
			set
			{
				if (value != this._fallenDefenderHorseArcherCount)
				{
					this._fallenDefenderHorseArcherCount = value;
					base.OnPropertyChangedWithValue(value, "FallenDefenderHorseArcherCount");
				}
			}
		}

		[DataSourceProperty]
		public string AttackerArmy
		{
			get
			{
				return this._attackerArmy;
			}
			set
			{
				if (value != this._attackerArmy)
				{
					this._attackerArmy = value;
					base.OnPropertyChangedWithValue(value, "AttackerName");
				}
			}
		}

		[DataSourceProperty]
		public string FallenAttackerArmy
		{
			get
			{
				return this._fallenAttackerArmy;
			}
			set
			{
				if (value != this._fallenAttackerArmy)
				{
					this._fallenAttackerArmy = value;
					base.OnPropertyChangedWithValue(value, "AttackerName");
				}
			}
		}

		[DataSourceProperty]
		public string DefenderArmy
		{
			get
			{
				return this._defenderArmy;
			}
			set
			{
				if (value != this._defenderArmy)
				{
					this._defenderArmy = value;
					base.OnPropertyChangedWithValue(value, "AttackerName");
				}
			}
		}

		[DataSourceProperty]
		public string FallenDefenderArmy
		{
			get
			{
				return this._fallenDefenderArmy;
			}
			set
			{
				if (value != this._fallenDefenderArmy)
				{
					this._fallenDefenderArmy = value;
					base.OnPropertyChangedWithValue(value, "FallenDefenderArmy");
				}
			}
		}

		private string _dateOfMatch;

		private string _attackerSidePartyName;
		private string _defenderSidePartyName;

		private bool _extraInformationVisible;

		private string _battleResult;
		private string _playerSide;
		
		private string _attackerName;
		private string _defenderName;
		private string _attackerElo;
		private string _defenderElo;
		private string _defenderArmyStrength;
		private string _defenderTotalArmyCount;
		private string _attackerArmyStrength;
		private string _attackerTotalArmyCount;

		private string _eloChange;

		private string _battleResultBrush;
		private string _attackerNameBrush;
		private string _defenderNameBrush;
		private string _eloChangeBrush;

		private int _attackerInfantryCount;
		private int _attackerRangedCount;
		private int _attackerCavalryCount;
		private int _attackerHorseArcherCount;

		private int _defenderInfantryCount;
		private int _defenderRangedCount;
		private int _defenderCavalryCount;
		private int _defenderHorseArcherCount;


		private string _attackerArmy;
		private string _fallenAttackerArmy;
		private string _defenderArmy;
		private string _fallenDefenderArmy;

		private BasicTooltipViewModel _attackerInfantryHint;
		private BasicTooltipViewModel _attackerRangedHint;
		private BasicTooltipViewModel _attackerCavalryHint;
		private BasicTooltipViewModel _attackerHorseArcherHint;

		private BasicTooltipViewModel _defenderInfantryHint;
		private BasicTooltipViewModel _defenderRangedHint;
		private BasicTooltipViewModel _defenderCavalryHint;
		private BasicTooltipViewModel _defenderHorseArcherHint;



		private int _fallenAttackerInfantryCount;
		private int _fallenAttackerRangedCount;
		private int _fallenAttackerCavalryCount;
		private int _fallenAttackerHorseArcherCount;

		private int _fallenDefenderInfantryCount;
		private int _fallenDefenderRangedCount;
		private int _fallenDefenderCavalryCount;
		private int _fallenDefenderHorseArcherCount;

		private BasicTooltipViewModel _fallenAttackerInfantryHint;
		private BasicTooltipViewModel _fallenAttackerRangedHint;
		private BasicTooltipViewModel _fallenAttackerCavalryHint;
		private BasicTooltipViewModel _fallenAttackerHorseArcherHint;

		private BasicTooltipViewModel _fallenDefenderInfantryHint;
		private BasicTooltipViewModel _fallenDefenderRangedHint;
		private BasicTooltipViewModel _fallenDefenderCavalryHint;
		private BasicTooltipViewModel _fallenDefenderHorseArcherHint;


		public MatchHistory _matchHistory;

		public TroopRoster _displayPlayerArmy;
		public TroopRoster _displayEnemyArmy;
		public TroopRoster _displayFallenPlayerArmy;
		public TroopRoster _displayFallenEnemyArmy;
	}
}
