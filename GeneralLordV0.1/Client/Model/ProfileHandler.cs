using GeneralLord;
using GeneralLord.Client.Model;
using GeneralLord.FormationPlanHandler;
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

namespace GeneralLordWebApiClient.Model
{
    public class ProfileHandler
    {

        public static Profile GetVerifyProfile()
        {
            Serializer.EnsureSaveDirectory();
            var filePath = Path.Combine(Serializer.SaveFolderPath(), "playerprofile.json");
            try
            {
               
                Profile profile = (Profile) Serializer.JsonDeserializeProfile(filePath);
                //profile.Elo = 2000;
                //InformationManager.DisplayMessage(new InformationMessage(profile.Id.ToString()));
                return profile;

            }
            catch
            {
                Profile profile = new Profile { Name = PartyBase.MainParty.LeaderHero.Name.ToString(), Elo = 1500, ArmyContainer = Serializer.JsonString("armyConfig.json") };
                Serializer.JsonSerialize(profile, filePath);
                return profile;
            }
        }


        public static Profile UpdateProfileAc()
        {
            Serializer.EnsureSaveDirectory();
            var filePath = Path.Combine(Serializer.SaveFolderPath(), "playerprofile.json");
            try
            {

                Profile profile = (Profile)Serializer.JsonDeserializeProfile(filePath);
                profile.ArmyContainer = Serializer.JsonString("armyConfig.json");
                profile.ArmyStrength = PartyBase.MainParty.TotalStrength;
                profile.TotalTroopCount = PartyBase.MainParty.MemberRoster.TotalManCount;
                if (JsonBattleConfig.UniqueId != 0)
                {
                    profile.UniqueUser = JsonBattleConfig.UniqueId;
                    //InformationManager.DisplayMessage(new InformationMessage(profile.UniqueUser.ToString()));

                }


       


                //profile.DefensiveOrders = Serializer.JsonString("data.json");
                profile.UseDefensiveOrder = EnemyFormationHandler.UseDefensiveSettings;


                string formation_path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..", ".."));
                string formation_filePath = Path.Combine(formation_path, "ModuleData", "DefenderSelectedFormation.json");


                //formation_filePath = Path.Combine(formation_path, "ModuleData", "DefenseDecisiontree.json");

                string defenseFilePath = Path.Combine(Serializer.SaveFolderPath(), "DefenseDecisiontree.json");
                //profile.DefensiveOrders = File.ReadAllText(formation_filePath);
                if(File.Exists(defenseFilePath)) profile.DefensiveOrders = Serializer.JsonString("DefenseDecisiontree.json");
                profile.SelectedFormation = EnemyFormationHandler.DefensiveSelectedFormation;
  
                formation_filePath = Path.Combine(formation_path, "ModuleData", "data.json");
                profile.DefensiveFormation = File.ReadAllText(formation_filePath);
                //profile.Elo = 2000;
                //InformationManager.DisplayMessage(new InformationMessage(profile.Id.ToString()));

                try{
                    GameMetricsServer gms = JsonConvert.DeserializeObject<GameMetricsServer>(profile.GameMetrics);
                    gms.AddGameMetrics();
                    gms.remainingGold = PartyBase.MainParty.LeaderHero.Gold;
                    //1000 first gold + gold earned
                    int totalGold = 1000 + gms.totalGoldEarned;
                    gms.totalGoldSpent = totalGold  - gms.remainingGold;
                    string gameMetricsString = JsonConvert.SerializeObject(gms);
                    profile.GameMetrics = gameMetricsString;
                } catch
                {
                    string gameMetricsString = JsonConvert.SerializeObject(new GameMetricsServer());
                    profile.GameMetrics = gameMetricsString;
                }

                return profile;

            }
            catch
            {
                Profile profile;
                if (JsonBattleConfig.UniqueId == 0 )
                {
                    profile = new Profile { Name = PartyBase.MainParty.LeaderHero.Name.ToString(), Elo = 1500, ArmyContainer = Serializer.JsonString("armyConfig.json") };
                } else
                {
                    profile = new Profile { Name = PartyBase.MainParty.LeaderHero.Name.ToString(), Elo = 1500, ArmyContainer = Serializer.JsonString("armyConfig.json"), UniqueUser = JsonBattleConfig.UniqueId };
                }
                profile.ArmyStrength = PartyBase.MainParty.TotalStrength;

                profile.DefensiveFormation = "";
                profile.DefensiveOrders = "";
                profile.UseDefensiveOrder = EnemyFormationHandler.UseDefensiveSettings;
                profile.SelectedFormation = -1;

                string gameMetricsString = JsonConvert.SerializeObject(new GameMetricsServer());
                profile.GameMetrics = gameMetricsString;
                Serializer.JsonSerialize(profile, filePath);
                return profile;
            }
        }



    }
}
