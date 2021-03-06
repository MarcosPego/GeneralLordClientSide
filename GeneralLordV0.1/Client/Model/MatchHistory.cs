using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralLordWebApiClient.Model
{
    public class MatchHistory
    {
        public int EntryNumber;

        public int EnemyId;
        public int Id;
        public string EnemyName;
        public string PlayerName;
        public string BattleResult;

        public float PlayerArmyStrength;
        public int PlayerTroopCount;
        public int PlayerElo;
        public int PlayerEloChange;
        public float EnemyArmyStrength;
        public int EnemyElo;
        public int EnemyTroopCount;

        public int EnemyEloChange;


        public string PlayerArmyContainer;
        public string PlayerFallenArmyContainer;

        public string EnemyArmyContainer;
        public string EnemyFallenArmyContainer;

        public DateTime LocalTimeDatePostMatch;
    }
}
