using GeneralLordWebApiClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
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

			InstantiateValues(matchHistory);
			this.RefreshValues();
		}

		public void InstantiateValues(MatchHistory matchHistory)
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

			if (matchHistory.PlayerName == PartyBase.MainParty.LeaderHero.Name.ToString())
			{
				this.PlayerSide = "Attacker";

				this.AttackerName = matchHistory.PlayerName;
				this.AttackerElo = "Elo: " + matchHistory.PlayerElo.ToString();
				this.AttackerTotalArmyCount = "Troops: " + matchHistory.PlayerTroopCount.ToString();
				this.AttackerArmyStrength = GetAverageStrength(matchHistory.PlayerArmyStrength, matchHistory.EnemyArmyStrength);

				this.DefenderName = matchHistory.EnemyName;
				this.DefenderElo = "Elo: " + matchHistory.EnemyElo.ToString();
				this.DefenderTotalArmyCount ="Troops: " + matchHistory.EnemyTroopCount.ToString();
				this.DefenderArmyStrength = GetAverageStrength(matchHistory.EnemyArmyStrength, matchHistory.PlayerArmyStrength);


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
				this.PlayerSide = "Defender";

				this.AttackerName = matchHistory.EnemyName;
				this.AttackerElo = "Elo: " + matchHistory.EnemyElo.ToString();
				this.AttackerTotalArmyCount = "Troops: " + matchHistory.EnemyTroopCount.ToString();
				this.AttackerArmyStrength = GetAverageStrength(matchHistory.EnemyArmyStrength, matchHistory.PlayerArmyStrength);

				this.DefenderName = matchHistory.PlayerName;
				this.DefenderElo = "Elo: " + matchHistory.PlayerElo.ToString();
				this.DefenderTotalArmyCount = "Troops: " + matchHistory.PlayerTroopCount.ToString();
				this.DefenderArmyStrength = GetAverageStrength(matchHistory.PlayerArmyStrength, matchHistory.EnemyArmyStrength);

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

		public MatchHistory _matchHistory;
    }
}
