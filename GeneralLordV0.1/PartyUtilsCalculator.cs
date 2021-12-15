using GeneralLordWebApiClient.Model;
using Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace GeneralLord
{
    public class PartyUtilsCalculator
    {
        static List<Dictionary<string, float>> allplayers = new List<Dictionary<string, float>>();
        static string originText = "C:\\Users\\Utilizador 1\\Desktop\\PartyUtilsCalculate\\";
        static string destinText = "C:\\Users\\Utilizador 1\\Desktop\\PartyUtilsCalculate\\Sheet\\partyutilssheet3.csv";

        public static void calculateIntoSheet()
        {
            var csv = new StringBuilder();

            var firstLine = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};" +
            "{13};{14};{15};{16};{17};{18};{19};{20};{21};{22};{23};{24};{25};{26};{27};{28};{29};{30}",


            "id","day1", "day2", "day3", "day4", "day5", "day6", "day7", "day8", "day9", "day10", "day11", "day12",
            "day13", "day14", "day15", "day16", "day17", "day18", "day19", "day20", "day21", "day22", "day23",
            "day24", "day25", "day26", "day27", "day28", "day29", "day30"); ;
            csv.AppendLine(firstLine);

            for (int i = 1; i < 301; i++)
            {
                Dictionary<string, float> newDict = new Dictionary<string, float>();
            
                newDict["id"] = i;
                for (int j = 1; j < 31; j++)
                {

                    string daytxt = "day" + j;
                    newDict[daytxt] = 0;
                }
                allplayers.Add(newDict);
            }

            for (int i = 0; i < 30; i++)
            {
                int day = i + 1;

                string text = File.ReadAllText(originText + "partyutils"+ day + ".txt");
                InformationManager.DisplayMessage(new InformationMessage(text));

                try { var array = JArray.Parse(text); foreach (var value in array)
                    {

                        iteratePlayer(value.ToString(), day);
                    }
                }
                catch { InformationManager.DisplayMessage(new InformationMessage(text + "failed")); 

               
                }
            }
            foreach(var dic in allplayers)
            {

                try
                {
                    var newLine = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};" +
                    "{13};{14};{15};{16};{17};{18};{19};{20};{21};{22};{23};{24};{25};{26};{27};{28};{29};{30}",


                    dic["id"],dic["day1"], dic["day2"], dic["day3"], dic["day4"], dic["day5"], dic["day6"], dic["day7"], dic["day8"], dic["day9"], dic["day10"], dic["day11"], dic["day12"],
                    dic["day13"], dic["day14"], dic["day15"], dic["day16"], dic["day17"], dic["day18"], dic["day19"], dic["day20"], dic["day21"], dic["day22"], dic["day23"],
                    dic["day24"], dic["day25"], dic["day26"], dic["day27"], dic["day28"], dic["day29"], dic["day30"]);
                    csv.AppendLine(newLine);
                }
                catch
                {

                }

            }
            File.WriteAllText(destinText, csv.ToString());
        }

        public static void iteratePlayer(string resultFromArray, int day)
        {
            //JObject result = JObject.Parse(resultFromArray);


            PartyUtils partyUtils = JsonConvert.DeserializeObject<PartyUtils>(resultFromArray);
            ArmyContainer ac = JsonConvert.DeserializeObject<ArmyContainer>(partyUtils.GarrisonedTroops);


            TroopRoster troopRoster = new TroopRoster(PartyBase.MainParty);
            foreach (TroopContainer tc in ac.TroopContainers)
            {
                if (tc.stringId != "main_hero" && tc.stringId != "tutorial_npc_brother")
                {
                    
                    //JsonBattleConfig.TryAddCharacterToRoster(troopRoster, tc.stringId, tc.troopCount);
                }

            }

            //PartyUtilsHandler.GarrisonedTroops = troopRoster;
            WoundedTroopArmy WoundedTroopArmyTest = new WoundedTroopArmy();
            WoundedTroopArmyTest = JsonConvert.DeserializeObject<WoundedTroopArmy>(partyUtils.WoundedTroopsGroup);

            foreach (WoundedTroopGroup woundedTroopGroupToRecover in WoundedTroopArmyTest.WoundedTroopsGroup)
            {
                foreach (WoundedTroop wt in woundedTroopGroupToRecover.woundedTroops)
                {
                    CharacterObject characterObject = CharacterObject.Find(wt.stringId);
                    if (characterObject != null)
                    {
                        if (wt.troopCount > 0) troopRoster.AddToCounts(characterObject, wt.troopCount);
                    }
                    //WoundedTroops.AddMember(characterObject, wt.troopCount);
                }
            }

            Settlement closestHideout = SettlementHelper.FindNearestSettlement((Settlement x) => x.IsHideout && x.IsActive);
            Clan clan = Clan.BanditFactions.FirstOrDefault((Clan t) => t.Culture == closestHideout.Culture);

            //InformationManager.DisplayMessage(new InformationMessage(troopRoster.TotalManCount.ToString()));
            if (troopRoster.TotalManCount > 0)
            {
                
                MobileParty currentOpponentParty = BanditPartyComponent.CreateBanditParty("EnemyClan"+ partyUtils.Id+ day, clan, closestHideout.Hideout, false);
                currentOpponentParty.InitializeMobileParty(
                            troopRoster,
                            troopRoster,
                            currentOpponentParty.Position2D,
                            0);

                //var dic = allplayers[day];
                string daytxt = "day" + day;
                //dic[daytxt] = currentOpponentParty.Party.TotalStrength;
                int index = partyUtils.Id - 1;
                //allplayers[index][daytxt] = currentOpponentParty.Party.TotalStrength;
                allplayers[index][daytxt] = troopRoster.TotalManCount;
                //allplayers[index][daytxt] = currentOpponentParty.Party.TotalStrength;
            }



        }
    }
}
