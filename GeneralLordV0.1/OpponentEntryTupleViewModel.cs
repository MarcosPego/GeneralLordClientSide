using GeneralLordWebApiClient.Model;
using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;

namespace GeneralLord
{
    public class OpponentEntryTupleViewModel : ViewModel
    {
		public OpponentEntryTupleViewModel(Profile profile)
		{

			_profile = profile;
			this.Name = profile.Name;
			this.Elo = "Elo: " + profile.Elo.ToString();
			this.ArmyStrength = GetAverageStrength(profile);
			this.TotalArmyCount = "Troop Count: " + profile.TotalTroopCount.ToString();

			this.RefreshValues();
		}

		public void ExecuteChallenge()
		{
			Serializer.JsonSerialize(_profile, "enemyProfile.json");
			ArmyContainer ac = Serializer.JsonDeserializeFromStringAc(_profile.ArmyContainer);
			CharacterHandler.saveLocationFile = "enemygeneral.xml";
			CharacterHandler.saveLocationPath = CharacterHandler.SaveLocationEnum.ModuleData;
			CharacterHandler.WriteToFile(ac.CharacterXML);
			CharacterHandler.LoadXML();


			Settlement closestHideout = SettlementHelper.FindNearestSettlement((Settlement x) => x.IsHideout() && x.IsActive);
			Clan clan = Clan.BanditFactions.FirstOrDefault((Clan t) => t.Culture == closestHideout.Culture);

			//MobileParty mobileParty = MobilePartyHelper.SpawnLordParty(bestAvailableCommander, new Vec2(Hero.MainHero.GetPosition().x, Hero.MainHero.GetPosition().z), 1f);
			OpponentPartyHandler.RemoveOpponentParty();

			OpponentPartyHandler.PreBattleTroopRoster = JsonBattleConfig.EnemyParty(ac);
			OpponentPartyHandler.CurrentOpponentParty = BanditPartyComponent.CreateBanditParty("EnemyClan", clan, closestHideout.Hideout, false);
			OpponentPartyHandler.CurrentOpponentParty.InitializeMobileParty(
						OpponentPartyHandler.PreBattleTroopRoster,
						OpponentPartyHandler.PreBattleTroopRoster,
						OpponentPartyHandler.CurrentOpponentParty.Position2D,
						0);

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

		private Profile _profile;
		private string _name;
		private string _elo;
		private string _armyStrength;
		private string _totalArmyCount;
	}
}
