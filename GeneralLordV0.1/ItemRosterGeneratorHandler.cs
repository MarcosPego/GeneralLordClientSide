using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace GeneralLord
{
    public class ItemRosterGeneratorHandler
    {
		public static ItemRoster itemRosterShop = new ItemRoster();

		public static bool VerifyIfValidCulture(string stringIdObject)
        {
			//InformationManager.DisplayMessage(new InformationMessage(CharacterTierHandler.CharacterMainCulture.StringId));

			if (PartyBase.MainParty.LeaderHero.Culture.StringId == stringIdObject)
            {
				return true;
            }

			if (PartyBase.MainParty.LeaderHero.Clan.Tier >= 1)
            {
				return true;
			}

			return false;
        }

		public static void InitializeItemRosterForShop()
		{
			/*PIKES AND LANCES NEEDED*/

			itemRosterShop = new ItemRoster();
			GlobalItems();
			if (VerifyIfValidCulture("aserai")) AseraiCultureItems();
			if (VerifyIfValidCulture("battania")) BattanianCultureItems();
			if (VerifyIfValidCulture("khuzait")) KhuzaitCultureItems();
			if (VerifyIfValidCulture("vlandia")) VlandianCultureItems();
			if (VerifyIfValidCulture("sturgia")) SturgianCultureItems();
			if (VerifyIfValidCulture("empire")) EmpireCultureItems();
		}

		public static void GlobalItems()
        {
			//Horse Saddles
			TryAddItemToRoster(itemRosterShop, "bandit_saddle_steppe", 999);
			TryAddItemToRoster(itemRosterShop, "light_harness", 999);
			TryAddItemToRoster(itemRosterShop, "chain_horse_harness", 999);

			//Horses
			TryAddItemToRoster(itemRosterShop, "sumpter_horse", 999);
			TryAddItemToRoster(itemRosterShop, "mule", 999);
			TryAddItemToRoster(itemRosterShop, "old_horse", 999);
			TryAddItemToRoster(itemRosterShop, "hunter", 999);


			//Arm_Armour
			TryAddItemToRoster(itemRosterShop, "ragged_armwraps", 999);

			//Shields
			TryAddItemToRoster(itemRosterShop, "old_kite_shield", 999);
			TryAddItemToRoster(itemRosterShop, "leather_bound_kite_shield", 999);
			TryAddItemToRoster(itemRosterShop, "reinforced_kite_shield", 999);
			TryAddItemToRoster(itemRosterShop, "old_horsemans_kite_shield", 999);
			TryAddItemToRoster(itemRosterShop, "heavy_horsemans_kite_shield", 999);

			//Weapons
			TryAddItemToRoster(itemRosterShop, "torch", 999);
			TryAddItemToRoster(itemRosterShop, "training_bow", 999);
			TryAddItemToRoster(itemRosterShop, "training_longbow", 999);
			TryAddItemToRoster(itemRosterShop, "small_spurred_axe_t2", 999);
			TryAddItemToRoster(itemRosterShop, "tzkurion_axe_t3", 999);
			TryAddItemToRoster(itemRosterShop, "large_franziskaa_axe_t3", 999);
			TryAddItemToRoster(itemRosterShop, "battle_axe_t4", 999);

			TryAddItemToRoster(itemRosterShop, "simple_sparth_axe_t2", 999);
			TryAddItemToRoster(itemRosterShop, "bearded_axe_t3", 999);

			TryAddItemToRoster(itemRosterShop, "triangluar_spear_t3", 999);

			TryAddItemToRoster(itemRosterShop, "falchion_sword_t2", 999);

			TryAddItemToRoster(itemRosterShop, "composite_bow", 999);
			TryAddItemToRoster(itemRosterShop, "noble_long_bow", 999);
			TryAddItemToRoster(itemRosterShop, "noble_bow", 999);

			TryAddItemToRoster(itemRosterShop, "bodkin_arrows_a", 999);
			TryAddItemToRoster(itemRosterShop, "piercing_arrows", 999);
			TryAddItemToRoster(itemRosterShop, "heavy_steppe_arrows", 999);
			TryAddItemToRoster(itemRosterShop, "barbed_arrows", 999);
		}

		public static void AseraiCultureItems()
        {
			//Horse Saddles
			TryAddItemToRoster(itemRosterShop, "bandit_saddle_desert", 999);


			//Horses
			TryAddItemToRoster(itemRosterShop, "aserai_horse", 999);
			TryAddItemToRoster(itemRosterShop, "t2_aserai_horse", 999);
			TryAddItemToRoster(itemRosterShop, "camel", 999);
			TryAddItemToRoster(itemRosterShop, "war_camel", 999);

			//BodyArmour
			TryAddItemToRoster(itemRosterShop, "long_desert_robe", 999);
			TryAddItemToRoster(itemRosterShop, "desert_padded_cloth", 999);
			TryAddItemToRoster(itemRosterShop, "studded_leather_coat", 999);
			TryAddItemToRoster(itemRosterShop, "leather_strips_over_padded_robe", 999);
			TryAddItemToRoster(itemRosterShop, "aserai_horseman_armor", 999);

			//ArmArmour
			TryAddItemToRoster(itemRosterShop, "guarded_padded_vambrace", 999);

			//HeadArmour
			TryAddItemToRoster(itemRosterShop, "headscarf_d", 999);
			TryAddItemToRoster(itemRosterShop, "tied_head_wrapping", 999);
			TryAddItemToRoster(itemRosterShop, "open_desert_helmet", 999);
			TryAddItemToRoster(itemRosterShop, "pointed_skullcap_over_mail", 999);
			TryAddItemToRoster(itemRosterShop, "emirs_helmet", 999);
			TryAddItemToRoster(itemRosterShop, "southern_lord_helmet", 999);

			//LegArmour
			TryAddItemToRoster(itemRosterShop, "southern_moccasins", 999);
			TryAddItemToRoster(itemRosterShop, "wrapped_shoes", 999);

			//ShoulderArmour
			TryAddItemToRoster(itemRosterShop, "southern_shawl", 999);
			TryAddItemToRoster(itemRosterShop, "desert_fabric_shoulderpad", 999);
			TryAddItemToRoster(itemRosterShop, "aserai_horseman_shoulder", 999);
			TryAddItemToRoster(itemRosterShop, "desert_scale_shoulders", 999);

			//Shields
			TryAddItemToRoster(itemRosterShop, "oval_shield", 999);
			TryAddItemToRoster(itemRosterShop, "desert_round_shield", 999);
			TryAddItemToRoster(itemRosterShop, "bound_desert_round_shield", 999);
			TryAddItemToRoster(itemRosterShop, "bound_desert_round_sparring_shield", 999);


			//Weapons
			TryAddItemToRoster(itemRosterShop, "aserai_axe_1_t2", 999);
			TryAddItemToRoster(itemRosterShop, "aserai_axe_2_t2", 999);

			TryAddItemToRoster(itemRosterShop, "aserai_2haxe_1_t3", 999);
			TryAddItemToRoster(itemRosterShop, "aserai_2haxe_2_t4", 999);
			TryAddItemToRoster(itemRosterShop, "aserai_2haxe_3_t5", 999);

			TryAddItemToRoster(itemRosterShop, "aserai_sword_1_t2", 999);
			TryAddItemToRoster(itemRosterShop, "aserai_sword_2_t2", 999);
			//TryAddItemToRoster(itemRosterShop, "aserai_blade_3", 999);

			TryAddItemToRoster(itemRosterShop, "tribal_bow", 999);
			TryAddItemToRoster(itemRosterShop, "longbow_recurve_desert_bow", 999);
			TryAddItemToRoster(itemRosterShop, "southern_spear_3_t3", 999);
			TryAddItemToRoster(itemRosterShop, "southern_spear_3_t4", 999);
		}

		public static void BattanianCultureItems()
		{
			//Horse Saddles
			TryAddItemToRoster(itemRosterShop, "bandit_saddle_highland", 999);
			TryAddItemToRoster(itemRosterShop, "celtic_frost", 999);

			//Horses
			TryAddItemToRoster(itemRosterShop, "battania_horse", 999);
			TryAddItemToRoster(itemRosterShop, "t2_battania_horse", 999);

			//BodyArmour
			TryAddItemToRoster(itemRosterShop, "baggy_trunks", 999);
			TryAddItemToRoster(itemRosterShop, "armored_baggy_trunks", 999);
			TryAddItemToRoster(itemRosterShop, "studded_fur_armor", 999);
			TryAddItemToRoster(itemRosterShop, "armored_bearskin", 999);
			TryAddItemToRoster(itemRosterShop, "western_scale_mail", 999);
			TryAddItemToRoster(itemRosterShop, "battania_noble_armor", 999);
			TryAddItemToRoster(itemRosterShop, "battania_warlord_armor", 999);

			//ArmArmour
			TryAddItemToRoster(itemRosterShop, "guarded_armwraps", 999);
			TryAddItemToRoster(itemRosterShop, "roughtied_leather_bracers", 999);
			TryAddItemToRoster(itemRosterShop, "battania_noble_bracers", 999);
			TryAddItemToRoster(itemRosterShop, "battania_warlord_bracers", 999);

			//HeadArmour
			TryAddItemToRoster(itemRosterShop, "thinhide_coif", 999);
			TryAddItemToRoster(itemRosterShop, "leather_studdedhelm", 999);
			TryAddItemToRoster(itemRosterShop, "wolfhead", 999);
			TryAddItemToRoster(itemRosterShop, "battania_fur_helmet", 999);
			TryAddItemToRoster(itemRosterShop, "bearhead", 999);
			TryAddItemToRoster(itemRosterShop, "battanian_crowned_helmet", 999);
			TryAddItemToRoster(itemRosterShop, "battania_battle_crown", 999);

			//LegArmour
			TryAddItemToRoster(itemRosterShop, "highland_leg_wrappings", 999);
			TryAddItemToRoster(itemRosterShop, "battania_fur_boots", 999);
			TryAddItemToRoster(itemRosterShop, "wrapped_leather_boots", 999);
			TryAddItemToRoster(itemRosterShop, "belted_leather_boots", 999);
			TryAddItemToRoster(itemRosterShop, "battania_warlord_boots", 999);

			//ShoulderArmour
			TryAddItemToRoster(itemRosterShop, "battania_shoulder_strap", 999);
			TryAddItemToRoster(itemRosterShop, "battania_cloak", 999);
			TryAddItemToRoster(itemRosterShop, "wolf_shoulder", 999);
			TryAddItemToRoster(itemRosterShop, "battania_warlord_pauldrons", 999);

			TryAddItemToRoster(itemRosterShop, "battania_shoulder_furr", 999);
			TryAddItemToRoster(itemRosterShop, "battanian_chainmail_shoulder_a", 999);

			//Shields
			TryAddItemToRoster(itemRosterShop, "battania_large_shield_a", 999);
			TryAddItemToRoster(itemRosterShop, "battania_targe_b", 999);


			//Weapons
			TryAddItemToRoster(itemRosterShop, "battania_axe_1_t2", 999);
			TryAddItemToRoster(itemRosterShop, "woodland_axe_t3", 999);
			TryAddItemToRoster(itemRosterShop, "battania_axe_2_t4", 999);
			TryAddItemToRoster(itemRosterShop, "battania_axe_3_t5", 999);

			TryAddItemToRoster(itemRosterShop, "battania_2haxe_1_t2", 999);


			TryAddItemToRoster(itemRosterShop, "highland_spear_t2", 999);
			TryAddItemToRoster(itemRosterShop, "highland_spear_3_t3", 999);
			TryAddItemToRoster(itemRosterShop, "highland_spear_4_t4", 999);



			TryAddItemToRoster(itemRosterShop, "battania_sword_1_t2", 999);

			TryAddItemToRoster(itemRosterShop, "hunting_bow", 999);
			TryAddItemToRoster(itemRosterShop, "mountain_hunting_bow", 999);
			TryAddItemToRoster(itemRosterShop, "glen_ranger_bow", 999);
			TryAddItemToRoster(itemRosterShop, "highland_ranger_bow", 999);

			TryAddItemToRoster(itemRosterShop, "woodland_yew_bow", 999);
			TryAddItemToRoster(itemRosterShop, "woodland_longbow", 999);
		}
		public static void KhuzaitCultureItems()
		{
			//Horses
			TryAddItemToRoster(itemRosterShop, "khuzait_horse", 999);
			TryAddItemToRoster(itemRosterShop, "t2_khuzait_horse", 999);


			//BodyArmour
			TryAddItemToRoster(itemRosterShop, "steppe_armor", 999);
			TryAddItemToRoster(itemRosterShop, "steppe_robe", 999);
			TryAddItemToRoster(itemRosterShop, "khuzait_fortified_armor", 999);
			TryAddItemToRoster(itemRosterShop, "khuzait_sturdy_armor", 999);
			TryAddItemToRoster(itemRosterShop, "eastern_lamellar_armor", 999);
			TryAddItemToRoster(itemRosterShop, "eastern_plated_leather_vest", 999);

			//ArmArmour
			TryAddItemToRoster(itemRosterShop, "eastern_wrapped_armguards", 999);
			TryAddItemToRoster(itemRosterShop, "studded_vambraces", 999);
			TryAddItemToRoster(itemRosterShop, "eastern_plated_leather_vambraces", 999);

			//HeadArmour
			TryAddItemToRoster(itemRosterShop, "upturned_fur_cap", 999);
			TryAddItemToRoster(itemRosterShop, "fur_hood", 999);
			TryAddItemToRoster(itemRosterShop, "plumed_fur_lined_helmet", 999);
			TryAddItemToRoster(itemRosterShop, "nomad_helmet", 999);
			TryAddItemToRoster(itemRosterShop, "plumed_lamellar_helmet", 999);
			TryAddItemToRoster(itemRosterShop, "spiked_helmet_with_facemask", 999);
			TryAddItemToRoster(itemRosterShop, "khuzait_noble_helmet_with_fur", 999);

			//LegArmour
			TryAddItemToRoster(itemRosterShop, "eastern_leather_boots", 999);
			TryAddItemToRoster(itemRosterShop, "khuzait_curved_boots", 999);
			TryAddItemToRoster(itemRosterShop, "steppe_leather_boots", 999);
			TryAddItemToRoster(itemRosterShop, "reinforced_suede_boots", 999);


			//ShoulderArmour
			TryAddItemToRoster(itemRosterShop, "bolted_leather_strips", 999);
			TryAddItemToRoster(itemRosterShop, "steppe_leather_shoulders", 999);
			TryAddItemToRoster(itemRosterShop, "khuzait_leather_pauldron", 999);
			TryAddItemToRoster(itemRosterShop, "lamellar_shoulders", 999);
			TryAddItemToRoster(itemRosterShop, "a_metal_lamellar_armor_shoulder_a", 999);

			//Shields
			TryAddItemToRoster(itemRosterShop, "steel_round_shield", 999);
			TryAddItemToRoster(itemRosterShop, "decorated_steppe_shield", 999);
			TryAddItemToRoster(itemRosterShop, "noyans_shield", 999);

			TryAddItemToRoster(itemRosterShop, "eastern_spear_1_t2", 999);
			TryAddItemToRoster(itemRosterShop, "eastern_spear_2_t3", 999);
			TryAddItemToRoster(itemRosterShop, "eastern_spear_3_t3", 999);
			TryAddItemToRoster(itemRosterShop, "eastern_spear_4_t4", 999);
			TryAddItemToRoster(itemRosterShop, "eastern_throwing_spear_1_t3", 999);
			TryAddItemToRoster(itemRosterShop, "eastern_throwing_spear_2_t4", 999);
			TryAddItemToRoster(itemRosterShop, "eastern_spear_5_t5", 999);

			TryAddItemToRoster(itemRosterShop, "khuzait_sword_1_t2", 999);
			TryAddItemToRoster(itemRosterShop, "simple_sabre_sword_t2", 999);

			TryAddItemToRoster(itemRosterShop, "steppe_bow", 999);
			TryAddItemToRoster(itemRosterShop, "mountain_hunting_bow", 999);
			TryAddItemToRoster(itemRosterShop, "composite_steppe_bow", 999);
			TryAddItemToRoster(itemRosterShop, "steppe_heavy_bow", 999);
			TryAddItemToRoster(itemRosterShop, "steppe_war_bow", 999);


			TryAddItemToRoster(itemRosterShop, "steppe_arrows", 999);
			TryAddItemToRoster(itemRosterShop, "heavy_steppe_arrows", 999);
		}

		public static void VlandianCultureItems()
		{
			//Horse Saddles
			TryAddItemToRoster(itemRosterShop, "halfchain_barding", 999);
			TryAddItemToRoster(itemRosterShop, "chain_barding", 999);

			//Horses

			TryAddItemToRoster(itemRosterShop, "vlandia_horse", 999);
			TryAddItemToRoster(itemRosterShop, "t2_vlandia_horse", 999);


			//BodyArmour
			TryAddItemToRoster(itemRosterShop, "sackcloth_tunic", 999);
			TryAddItemToRoster(itemRosterShop, "padded_leather_shirt", 999);
			TryAddItemToRoster(itemRosterShop, "leather_scale_armor", 999);
			TryAddItemToRoster(itemRosterShop, "veteran_mercenary_armor", 999);
			TryAddItemToRoster(itemRosterShop, "mail_shirt", 999);
			TryAddItemToRoster(itemRosterShop, "coat_of_plates_over_mail", 999);

			//ArmArmour
			TryAddItemToRoster(itemRosterShop, "mail_mitten", 999);
			TryAddItemToRoster(itemRosterShop, "reinforced_mail_mitten", 999);
			TryAddItemToRoster(itemRosterShop, "lordly_mail_mitten", 999);
			TryAddItemToRoster(itemRosterShop, "reinforced_leather_vambraces", 999);

			//HeadArmour
			TryAddItemToRoster(itemRosterShop, "arming_cap", 999);
			TryAddItemToRoster(itemRosterShop, "padded_coif", 999);
			TryAddItemToRoster(itemRosterShop, "cervelliere_over_arming_coif", 999);
			TryAddItemToRoster(itemRosterShop, "kettle_helmet_over_padded_coif", 999);
			TryAddItemToRoster(itemRosterShop, "plumed_lamellar_helmet", 999);
			TryAddItemToRoster(itemRosterShop, "full_helm_over_arming_coif", 999);
			TryAddItemToRoster(itemRosterShop, "segmented_skullcap_over_mail_coif", 999);


			//LegArmour
			TryAddItemToRoster(itemRosterShop, "leather_cavalier_boots", 999);
			TryAddItemToRoster(itemRosterShop, "mail_cavalier_boots", 999);

			//ShoulderArmour
			TryAddItemToRoster(itemRosterShop, "green_hood", 999);
			TryAddItemToRoster(itemRosterShop, "leather_lamellar_shoulders", 999);
			TryAddItemToRoster(itemRosterShop, "hood", 999);
			TryAddItemToRoster(itemRosterShop, "padded_leather_shoulders", 999);



			//Shields
			TryAddItemToRoster(itemRosterShop, "pavise_shield", 999);
			TryAddItemToRoster(itemRosterShop, "horsemans_heater_shield", 999);
			TryAddItemToRoster(itemRosterShop, "small_heater_shield", 999);
			TryAddItemToRoster(itemRosterShop, "jousting_shield", 999);

			//Weapons
			TryAddItemToRoster(itemRosterShop, "vlandia_axe_1_t3", 999);
			TryAddItemToRoster(itemRosterShop, "vlandia_axe_2_t4", 999);

			TryAddItemToRoster(itemRosterShop, "vlandia_2haxe_1_t4", 999);
			TryAddItemToRoster(itemRosterShop, "hooked_axe_t4", 999);

			TryAddItemToRoster(itemRosterShop, "western_spear_1_t2", 999);
			TryAddItemToRoster(itemRosterShop, "western_spear_2_t2", 999);
			TryAddItemToRoster(itemRosterShop, "western_spear_3_t3", 999);

			TryAddItemToRoster(itemRosterShop, "vlandia_sword_1_t2", 999);
			TryAddItemToRoster(itemRosterShop, "lowland_longbow", 999);

			TryAddItemToRoster(itemRosterShop, "vlandic_arrows", 999);
		}

		public static void SturgianCultureItems()
		{
			//Horses
			TryAddItemToRoster(itemRosterShop, "sturgia_horse", 999);
			TryAddItemToRoster(itemRosterShop, "t2_sturgia_horse", 999);

			//BodyArmour
			TryAddItemToRoster(itemRosterShop, "northern_tunic", 999);
			TryAddItemToRoster(itemRosterShop, "fur_trimmed_tunic", 999);
			TryAddItemToRoster(itemRosterShop, "nordic_sloven_leather", 999);
			TryAddItemToRoster(itemRosterShop, "northern_padded_gambeson", 999);
			TryAddItemToRoster(itemRosterShop, "northern_leather_vest", 999);
			TryAddItemToRoster(itemRosterShop, "northern_lamellar_armor", 999);
			TryAddItemToRoster(itemRosterShop, "sturgian_fortified_armor", 999);
			TryAddItemToRoster(itemRosterShop, "decorated_nordic_hauberk", 999);


			//ArmArmour
			TryAddItemToRoster(itemRosterShop, "northern_brass_bracers", 999);
			TryAddItemToRoster(itemRosterShop, "northern_plated_gloves", 999);

			//HeadArmour
			TryAddItemToRoster(itemRosterShop, "nordic_leather_cap", 999);
			TryAddItemToRoster(itemRosterShop, "nordic_fur_cap", 999);
			TryAddItemToRoster(itemRosterShop, "nasalhelm_over_leather", 999);
			TryAddItemToRoster(itemRosterShop, "sturgian_crown", 999);
			TryAddItemToRoster(itemRosterShop, "nordic_helmet", 999);
			TryAddItemToRoster(itemRosterShop, "decorated_goggled_helmet", 999);
			TryAddItemToRoster(itemRosterShop, "northern_warlord_helmet", 999);

			//LegArmour
			TryAddItemToRoster(itemRosterShop, "sturgia_boots_a", 999);
			TryAddItemToRoster(itemRosterShop, "mail_chausses", 999);
			TryAddItemToRoster(itemRosterShop, "strapped_mail_chausses", 999);
			TryAddItemToRoster(itemRosterShop, "northern_plated_boots", 999);


			//ShoulderArmour
			TryAddItemToRoster(itemRosterShop, "stitched_leather_shoulders", 999);
			TryAddItemToRoster(itemRosterShop, "mail_shoulders", 999);
			TryAddItemToRoster(itemRosterShop, "brass_scale_shoulders", 999);

			//Weapons
			TryAddItemToRoster(itemRosterShop, "sturgia_axe_2_t2", 999);
			TryAddItemToRoster(itemRosterShop, "sturgia_axe_3_t3", 999);
			TryAddItemToRoster(itemRosterShop, "sturgia_axe_4_t4", 999);
			TryAddItemToRoster(itemRosterShop, "sturgia_axe_5_t5", 999);

			TryAddItemToRoster(itemRosterShop, "northern_axe_t3", 999);
			TryAddItemToRoster(itemRosterShop, "sturgia_2haxe_1_t4", 999);
			TryAddItemToRoster(itemRosterShop, "sturgia_2haxe_2_t5", 999);

			TryAddItemToRoster(itemRosterShop, "northern_spear_1_t2", 999);
			TryAddItemToRoster(itemRosterShop, "northern_spear_2_t3", 999);
			TryAddItemToRoster(itemRosterShop, "northern_spear_3_t4", 999);
			TryAddItemToRoster(itemRosterShop, "northern_spear_4_t5", 999);
			TryAddItemToRoster(itemRosterShop, "military_fork_t2", 999);

			TryAddItemToRoster(itemRosterShop, "sturgia_sword_1_t2", 999);

			TryAddItemToRoster(itemRosterShop, "nordic_shortbow", 999);
		}
		public static void EmpireCultureItems()
		{
			//Horse Saddles
			TryAddItemToRoster(itemRosterShop, "fortunas_choice", 999);
			TryAddItemToRoster(itemRosterShop, "chain_barding", 999);
			TryAddItemToRoster(itemRosterShop, "stripped_leather_harness", 999);
			TryAddItemToRoster(itemRosterShop, "half_scale_barding", 999);
			TryAddItemToRoster(itemRosterShop, "imperial_scale_barding", 999);

			//Horses
			TryAddItemToRoster(itemRosterShop, "empire_horse", 999);
			TryAddItemToRoster(itemRosterShop, "t2_empire_horse", 999);


			//BodyArmour
			TryAddItemToRoster(itemRosterShop, "fine_town_tunic", 999);
			TryAddItemToRoster(itemRosterShop, "tunic_with_shoulder_pads", 999);
			TryAddItemToRoster(itemRosterShop, "padded_cloth_with_strips", 999);
			TryAddItemToRoster(itemRosterShop, "empire_warrior_padded_armor_c", 999);
			TryAddItemToRoster(itemRosterShop, "basic_imperial_leather_armor", 999);
			TryAddItemToRoster(itemRosterShop, "imperial_mail_over_leather", 999);
			TryAddItemToRoster(itemRosterShop, "imperial_scale_armor", 999);
			TryAddItemToRoster(itemRosterShop, "legionary_mail", 999);


			//ArmArmour
			TryAddItemToRoster(itemRosterShop, "padded_mitten", 999);
			TryAddItemToRoster(itemRosterShop, "plated_strip_gauntlets", 999);
			TryAddItemToRoster(itemRosterShop, "lordly_padded_mitten", 999);

			//HeadArmour
			TryAddItemToRoster(itemRosterShop, "imperial_padded_coif", 999);
			TryAddItemToRoster(itemRosterShop, "laced_cloth_coif", 999);
			TryAddItemToRoster(itemRosterShop, "tall_helmet", 999);
			TryAddItemToRoster(itemRosterShop, "imperial_mail_coif", 999);
			TryAddItemToRoster(itemRosterShop, "imperial_goggled_helmet", 999);
			TryAddItemToRoster(itemRosterShop, "heavy_nasalhelm_over_imperial_mail", 999);
			TryAddItemToRoster(itemRosterShop, "helmet_with_faceguard", 999);
			TryAddItemToRoster(itemRosterShop, "empire_lord_helmet", 999);
			TryAddItemToRoster(itemRosterShop, "empire_jewelled_helmet", 999);

			//LegArmour
			TryAddItemToRoster(itemRosterShop, "strapped_leather_boots", 999);
			TryAddItemToRoster(itemRosterShop, "empire_horseman_boots", 999);
			TryAddItemToRoster(itemRosterShop, "plated_strip_boots", 999);
			TryAddItemToRoster(itemRosterShop, "lamellar_plate_boots", 999);

			//ShoulderArmour
			TryAddItemToRoster(itemRosterShop, "pauldron_cape_a", 999);
			TryAddItemToRoster(itemRosterShop, "empire_warrior_padded_armor_shoulder", 999);
			TryAddItemToRoster(itemRosterShop, "woven_leather_shoulders", 999);
			TryAddItemToRoster(itemRosterShop, "imperial_lamellar_shoulders", 999);
			TryAddItemToRoster(itemRosterShop, "empire_plate_armor_shoulder_a", 999);

			//Shields
			TryAddItemToRoster(itemRosterShop, "heavy_round_shield", 999);
			TryAddItemToRoster(itemRosterShop, "leather_round_shield", 999);
			TryAddItemToRoster(itemRosterShop, "strapped_round_shield", 999);
			TryAddItemToRoster(itemRosterShop, "sturgia_old_shield_a", 999);

			//Weapons
			TryAddItemToRoster(itemRosterShop, "imperial_axe_t3", 999);


			TryAddItemToRoster(itemRosterShop, "imperial_spear_t2", 999);
			TryAddItemToRoster(itemRosterShop, "western_spear_4_t4", 999);
			TryAddItemToRoster(itemRosterShop, "imperial_throwing_spear_1_t4", 999);

			TryAddItemToRoster(itemRosterShop, "empire_sword_1_t2", 999);


			TryAddItemToRoster(itemRosterShop, "arrow_emp_1_a", 999);
		}


		public static void TryAddItemToRoster(ItemRoster itemRoster, string itemId, int count)
		{
			foreach (ItemObject item in Items.All)
			{
				if (item.StringId == itemId)
				{
					itemRoster.AddToCounts(item, 999);
					return;
				}

			}

			InformationManager.DisplayMessage(new InformationMessage("Item Id: " + itemId + " id not found."));
		}
	}
}
