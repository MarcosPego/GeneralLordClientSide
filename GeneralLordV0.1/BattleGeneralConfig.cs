using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using EnhancedBattleTest.Config;
using TaleWorlds.Core;

namespace GeneralLord
{
    public class BattleGeneralConfig
    {

        public BattleGeneralConfig()
        {
            _config = BattleConfig.Deserialize(false);
			_config.PlayerTeamConfig.Generals = new TroopGroupConfig(false, true)
			{
				Troops = new List<TroopConfig>
					{
						new TroopConfig(false, Hero.MainHero.StringId, 1, 0f),
					}
			};
			_config.PlayerTeamConfig.HasGeneral = true;


			_config.Serialize(false);
        }


		public TroopRoster[] EnemyParty()
		{
			//TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			TroopRoster troopRoster = new TroopRoster(PartyBase.MainParty);
			this.TryAddCharacterToRoster(troopRoster, "imperial_recruit", 4);
			//InformationManager.DisplayMessage(new InformationMessage(troopRoster.TotalManCount.ToString()));


			TroopRoster[] troopRosterlist = new TroopRoster[1];
			troopRosterlist[0] = troopRoster;

			return troopRosterlist;
		}
		public void TryAddCharacterToRoster(TroopRoster troopRoster, string characterId, int count)
		{

			CharacterObject characterObject = CharacterObject.Find(characterId);
			if (characterObject != null)
			{
				//InformationManager.DisplayMessage(new InformationMessage("Chegou" + characterId));
				troopRoster.AddToCounts(characterObject, count, false, 0, 0, true, -1);

			}
			else
			{
				InformationManager.DisplayMessage(new InformationMessage("CustomTroopRoster: " + characterId + " id not found."));
			}
		}


		public void UpdateArmyRosters(TroopRoster[] PartyArmy, TroopRoster[] EnemyArmy)
        {


			List<TroopConfig>[] TroopSegments = new List<TroopConfig>[PartyArmy.Length];
			//PartyArmy.GetTroopRoster()

			for(int i = 0; i < PartyArmy.Length; i++)
            {
				List<TroopConfig> TroopSegment = new List<TroopConfig>();
				foreach (TroopRosterElement troop in PartyArmy[i].GetTroopRoster())
				{
					TroopSegment.Add(new TroopConfig(false, troop.Character.StringId, troop.Number, 0f));
				}
				TroopSegments[i] = TroopSegment;
			}



			_config.PlayerTeamConfig.TroopGroups = new TroopGroupConfig[]
			{
					new TroopGroupConfig(false, false)
					{
						Troops = TroopSegments[0]
					},
					new TroopGroupConfig(false, false)
					{
						Troops = TroopSegments[1]
					},
					new TroopGroupConfig(false, false)
					{
						Troops = TroopSegments[2]
					},
					new TroopGroupConfig(false, false)
					{
						Troops = TroopSegments[3]
					},
					new TroopGroupConfig(false, false),
					new TroopGroupConfig(false, false),
					new TroopGroupConfig(false, false),
					new TroopGroupConfig(false, false)
			};

			_config.EnemyTeamConfig.Generals = new TroopGroupConfig(false, true)
			{
				Troops = new List<TroopConfig>
					{
					}
			};
			_config.EnemyTeamConfig.HasGeneral = false;

			List<TroopConfig> Troops = new List<TroopConfig>();
			foreach (TroopRosterElement troop in EnemyArmy[0].GetTroopRoster())
			{
				Troops.Add(new TroopConfig(false, troop.Character.StringId, troop.Number, 0f));
			}

			
			_config.EnemyTeamConfig.TroopGroups = new TroopGroupConfig[]
			{
					new TroopGroupConfig(false, false)
					{
						Troops = Troops
					},
					new TroopGroupConfig(false, false),
					new TroopGroupConfig(false, false),
					new TroopGroupConfig(false, false),
					new TroopGroupConfig(false, false),
					new TroopGroupConfig(false, false),
					new TroopGroupConfig(false, false),
					new TroopGroupConfig(false, false)
			};

			_config.Serialize(false);
		}

        public BattleConfig _config { get; set; }
    }
}
