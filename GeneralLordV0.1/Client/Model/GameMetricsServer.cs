using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralLord.Client.Model
{
    public class GameMetricsServer
    {
        public GameMetricsServer()
        {
            timesHeroHealed = GameMetrics.timesHeroHealed;
            renownPurchased = GameMetrics.renownPurchased;
            stewardship = GameMetrics.stewardship;
            recruitmentScreenOpened = GameMetrics.recruitmentScreenOpened;
            shopScreenOpened = GameMetrics.shopScreenOpened;
            characterScreenOpened = GameMetrics.characterScreenOpened;
            partyScreenOpened = GameMetrics.partyScreenOpened;
            garrisonScreenOpened = GameMetrics.garrisonScreenOpened;
            woundedScreenOpened = GameMetrics.woundedScreenOpened;
            defensiveScreenOpened = GameMetrics.defensiveScreenOpened;
            attackScreenOpened = GameMetrics.attackScreenOpened;
            defensiveBoolPressed = GameMetrics.defensiveBoolPressed;
            formationScreenOpened = GameMetrics.formationScreenOpened;
            rankingScreenOpened = GameMetrics.rankingScreenOpened;
            matchScreenOpened = GameMetrics.matchScreenOpened;
            findOponentsScreenOpened = GameMetrics.findOponentsScreenOpened;
            totalGoldEarned = GameMetrics.totalGoldEarned;
            remainingGold = GameMetrics.remainingGold;
            totalGoldSpent = GameMetrics.totalGoldSpent;
            numberOfPlansActivated = GameMetrics.numberOfPlansActivated;
            numberOfLoadoutsUsed = GameMetrics.numberOfLoadoutsUsed;
            numberOfLoadoutsSaved = GameMetrics.numberOfLoadoutsSaved;

            savedAndExited = GameMetrics.savedAndExited;
            currentLastPlaythroughStart = GameMetrics.currentLastPlaythroughStart;
            currentLastPlaythroughEnd = GameMetrics.currentLastPlaythroughEnd;
        }



        public void AddGameMetrics()
        {

            timesHeroHealed += GameMetrics.timesHeroHealed;
            renownPurchased += GameMetrics.renownPurchased;
            stewardship += GameMetrics.stewardship;
            recruitmentScreenOpened += GameMetrics.recruitmentScreenOpened;
            shopScreenOpened += GameMetrics.shopScreenOpened;
            characterScreenOpened += GameMetrics.characterScreenOpened;
            partyScreenOpened += GameMetrics.partyScreenOpened;
            garrisonScreenOpened += GameMetrics.garrisonScreenOpened;
            woundedScreenOpened += GameMetrics.woundedScreenOpened;
            defensiveScreenOpened += GameMetrics.defensiveScreenOpened;
            attackScreenOpened += GameMetrics.attackScreenOpened;
            defensiveBoolPressed += GameMetrics.defensiveBoolPressed;
            formationScreenOpened += GameMetrics.formationScreenOpened;
            rankingScreenOpened += GameMetrics.rankingScreenOpened;
            matchScreenOpened += GameMetrics.matchScreenOpened;
            findOponentsScreenOpened += GameMetrics.findOponentsScreenOpened;
            totalGoldEarned += GameMetrics.totalGoldEarned;
            remainingGold += GameMetrics.remainingGold;
            totalGoldSpent += GameMetrics.totalGoldSpent;
            numberOfPlansActivated += GameMetrics.numberOfPlansActivated;
            numberOfLoadoutsUsed += GameMetrics.numberOfLoadoutsUsed;
            numberOfLoadoutsSaved += GameMetrics.numberOfLoadoutsSaved;

            savedAndExited += GameMetrics.savedAndExited;

            timesOpenedTheGame += GameMetrics.timesOpenedTheGame;
            timePlayed += GameMetrics.timePlayed;
            currentLastPlaythroughStart = GameMetrics.currentLastPlaythroughStart;
            currentLastPlaythroughEnd = GameMetrics.currentLastPlaythroughEnd;


            GameMetrics.timesHeroHealed = 0;
            GameMetrics.renownPurchased = 0;
            GameMetrics.stewardship = 0;
            GameMetrics.recruitmentScreenOpened = 0;
            GameMetrics.shopScreenOpened = 0;
            GameMetrics.characterScreenOpened = 0;
            GameMetrics.partyScreenOpened = 0;
            GameMetrics.garrisonScreenOpened = 0;
            GameMetrics.woundedScreenOpened = 0;
            GameMetrics.defensiveScreenOpened = 0;
            GameMetrics.attackScreenOpened = 0;
            GameMetrics.defensiveBoolPressed = 0;
            GameMetrics.formationScreenOpened = 0;
            GameMetrics.rankingScreenOpened = 0;
            GameMetrics.matchScreenOpened = 0;
            GameMetrics.findOponentsScreenOpened = 0;
            GameMetrics.totalGoldEarned = 0;
            GameMetrics.remainingGold = 0;
            GameMetrics.totalGoldSpent = 0;
            GameMetrics.numberOfPlansActivated = 0;
            GameMetrics.numberOfLoadoutsUsed = 0;
            GameMetrics.numberOfLoadoutsSaved = 0;

            GameMetrics.savedAndExited = 0;
            GameMetrics.timesOpenedTheGame = 0;
            GameMetrics.timePlayed = TimeSpan.Zero;
        }


        public void LoadGameMetrics()
        {
            GameMetrics.timesHeroHealed = timesHeroHealed;
            GameMetrics.renownPurchased = renownPurchased;
            GameMetrics.stewardship = stewardship;
            GameMetrics.recruitmentScreenOpened = recruitmentScreenOpened;
            GameMetrics.shopScreenOpened = shopScreenOpened;
            GameMetrics.characterScreenOpened = characterScreenOpened;
            GameMetrics.partyScreenOpened = partyScreenOpened;
            GameMetrics.garrisonScreenOpened = garrisonScreenOpened;
            GameMetrics.woundedScreenOpened = woundedScreenOpened;
            GameMetrics.defensiveScreenOpened = defensiveScreenOpened;
            GameMetrics.attackScreenOpened = attackScreenOpened;
            GameMetrics.defensiveBoolPressed = defensiveBoolPressed;
            GameMetrics.formationScreenOpened = formationScreenOpened;
            GameMetrics.rankingScreenOpened = rankingScreenOpened;
            GameMetrics.matchScreenOpened = matchScreenOpened;
            GameMetrics.findOponentsScreenOpened = findOponentsScreenOpened;
            GameMetrics.totalGoldEarned = totalGoldEarned;
            GameMetrics.remainingGold = remainingGold;
            GameMetrics.totalGoldSpent = totalGoldSpent;
            GameMetrics.numberOfPlansActivated = numberOfPlansActivated;
            GameMetrics.numberOfLoadoutsUsed = numberOfLoadoutsUsed;
            GameMetrics.numberOfLoadoutsSaved = numberOfLoadoutsSaved;

            GameMetrics.savedAndExited = savedAndExited;
        }


        public int timesHeroHealed;
        public int renownPurchased;
        public int stewardship;


        public int recruitmentScreenOpened;
        public int shopScreenOpened;

        public int characterScreenOpened;
        public int partyScreenOpened;
        public int garrisonScreenOpened;
        public int woundedScreenOpened;
        public int defensiveScreenOpened;
        public int attackScreenOpened;
        public int defensiveBoolPressed;

        public int formationScreenOpened;
        public int rankingScreenOpened;
        public int matchScreenOpened;
        public int findOponentsScreenOpened;

        public int totalGoldEarned;
        public int remainingGold;
        public int totalGoldSpent;

        public int numberOfPlansActivated;
        public int numberOfLoadoutsUsed;
        public int numberOfLoadoutsSaved;

        public int savedAndExited;

        public int timesOpenedTheGame;
        public TimeSpan timePlayed;
        public DateTime currentLastPlaythroughStart;
        public DateTime currentLastPlaythroughEnd;
    }
}
