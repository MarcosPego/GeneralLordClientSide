using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralLordWebApiClient.Model
{
    public class MatchHistory
    {
        public int EnemyId;
        public int Id;

        public string BattleResult;

        public float PlayerArmyStrength;
        public int PlayerTroopCount;
        public int PlayerElo;
        public float EnemyArmyStrength;
        public int EnemyElo;
        public int EnemyTroopCount;
    }
}
