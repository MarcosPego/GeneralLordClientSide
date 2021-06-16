using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.Core;

using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.InputSystem;
using TaleWorlds.CampaignSystem;

namespace GeneralLord
{
    internal class TroopVM : ViewModel
    {

        /*public TroopRosterElement Troop
        {
            get
            {
                return this._troop;
            }
            set
            {
                this._troop = value;
                this.Character = value.Character;
                this.TroopID = this.Character.StringId;
                this.CheckTransferAmountDefaultValue();
                this.TroopXPTooltip = new BasicTooltipViewModel(() => CampaignUIHelper.GetTroopXPTooltip(value));
                this.TroopConformityTooltip = new BasicTooltipViewModel(() => CampaignUIHelper.GetTroopConformityTooltip(value));
            }
        }*/

        public CharacterObject Character
        {
            get
            {
                return this._character;
            }
            set
            {
                this._character = value;
                CharacterCode characterCode = this.GetCharacterCode(value, PartyScreenLogic.TroopType.Member, PartyScreenLogic.PartyRosterSide.Right);
                this.Code = new ImageIdentifierVM(characterCode);
                CharacterObject[] upgradeTargets = this._character.UpgradeTargets;
                /*if (upgradeTargets != null && upgradeTargets.Length != 0)
                {
                    for (int i = 0; i < this._character.UpgradeTargets.Length; i++)
                    {
                        CharacterCode characterCode2 = this.GetCharacterCode(this._character.UpgradeTargets[i], this.Type, this.Side);
                        if (i == 0)
                        {
                            this.UpgradeTroop1 = new ImageIdentifierVM(characterCode2);
                        }
                        else if (i == 1)
                        {
                            this.UpgradeTroop2 = new ImageIdentifierVM(characterCode2);
                        }
                    }
                }*/
                //this.CheckTransferAmountDefaultValue();
            }
        }

        public TroopVM(PartyManagerViewModel pmvm,MBBindingList<TroopVM> own, MBBindingList<TroopVM> target, CharacterObject CharObj , string troopName, string troopId, string count, PartyRosterSide rosterSide, bool isHero)
        {
            this._pmvm = pmvm;
            this._ownList = own;
            this._targetList = target;
            this._troopName = troopName;
            this._troopID = troopId;
            this._troopNumber = count;
            this.Character = CharObj;

            this._isTroopTransferrableLeft = rosterSide == PartyRosterSide.Left;
            this._isTroopTransferrableRight = rosterSide == PartyRosterSide.Right;

            //this._isTroopTransferrableLeft = true;
            //this._isTroopTransferrableRight = false;

            this._isHero = isHero;

            this.TierIconData = CampaignUIHelper.GetCharacterTierData(this.Character, false);
            this.TypeIconData = CampaignUIHelper.GetCharacterTypeData(this.Character, false);
            
        }

        public void ThrowOnPropertyChanged()
        {
            base.OnPropertyChanged("TroopName");
            base.OnPropertyChanged("TroopNumber");
        }

        private void ExecuteTransfer()
        {
            
            //InformationManager.DisplayMessage(new InformationMessage(this._troopName));
            if (int.Parse(this._troopNumber) > 0)
            {

                int changeAmount = 1;
                int thisTroopCount = int.Parse(this._troopNumber);
                //int rightValue = int.Parse(this._RightItemEntry[value]);

                if (Input.IsKeyDown(InputKey.LeftShift))
                {
                    changeAmount = 5;

                    if (thisTroopCount < 5)
                    {
                        changeAmount = thisTroopCount;
                    }
                }

                if (Input.IsKeyDown(InputKey.LeftControl))
                {
                    changeAmount = thisTroopCount;
                }


                
                if (thisTroopCount - changeAmount == 0)
                {
                    this._ownList.Remove(this);
                }
                else
                {
                    this._troopNumber = (thisTroopCount - changeAmount).ToString();
                }

                //this._RightItemEntry[value] = (rightValue - changeAmount).ToString();

                //this._leftPartyHeader2 = (int.Parse(this._leftPartyHeader2) + changeAmount).ToString();
                //this._rightPartyHeader2 = (int.Parse(this._rightPartyHeader2) - changeAmount).ToString();

                //base.OnPropertyChanged("ItemEntry" + value.ToString());
                //base.OnPropertyChanged("RightItemEntry" + value.ToString());

                //base.OnPropertyChanged("LeftPartyHeader2");
                //base.OnPropertyChanged("RightPartyHeader2");
                //InformationManager.DisplayMessage(new InformationMessage(this._targetList.Contains(this).ToString()));

                if (this._targetList.Contains(this))
                {
                    int index = this._targetList.IndexOf(this);
                    if (int.Parse(this._targetList.ElementAt(index)._troopNumber) + changeAmount == 0)
                    {
                        this._targetList.RemoveAt(0);
                    }
                    else
                    {
                        
                    }
                    this._targetList.ElementAt(index)._troopNumber = (int.Parse(this._targetList.ElementAt(index)._troopNumber) + changeAmount).ToString();
                    this._targetList.ElementAt(index).ThrowOnPropertyChanged();
                }
                else
                {
                    var side = PartyRosterSide.None;
                    if (this._isTroopTransferrableRight)
                    {
                        side = PartyRosterSide.Left;
                    } else if (this._isTroopTransferrableLeft)
                    {
                        side = PartyRosterSide.Right;
                    }


                    if (side == PartyRosterSide.Right)
                    {
                        this._targetList.Add(new TroopVM(this._pmvm, this._targetList, this._pmvm._formationsArmy[(int)this._pmvm._target], this._character, this._troopName, this._troopID, changeAmount.ToString(), side, false));
                    }

                    if (side == PartyRosterSide.Left)
                    {
                        this._targetList.Add(new TroopVM(this._pmvm, this._targetList, this._ownList, this._character, this._troopName, this._troopID, changeAmount.ToString(), side, false));
                    }

                }

                //_pmvm._leftPartyHeader2 = GetElementCount(_pmvm._leftParty).ToString();
                //_pmvm._rightPartyHeader2 = GetElementCount(_pmvm._rightParty).ToString();

                _pmvm.RefreshValues();

                base.OnPropertyChanged("TroopNumber");
            }
        }


        private void ExecuteRightTransfer()
        {
            if (int.Parse(this._troopNumber) > 0)
            {

                int changeAmount = 1;
                int thisTroopCount = int.Parse(this._troopNumber);
                //int rightValue = int.Parse(this._RightItemEntry[value]);

                if (Input.IsKeyDown(InputKey.LeftShift))
                {
                    changeAmount = 5;

                    if (thisTroopCount < 5)
                    {
                        changeAmount = thisTroopCount;
                    }
                }

                if (Input.IsKeyDown(InputKey.LeftControl))
                {
                    changeAmount = thisTroopCount;
                }


                this._troopNumber = (thisTroopCount - changeAmount).ToString();
                //this._RightItemEntry[value] = (rightValue - changeAmount).ToString();

                //this._leftPartyHeader2 = (int.Parse(this._leftPartyHeader2) + changeAmount).ToString();
                //this._rightPartyHeader2 = (int.Parse(this._rightPartyHeader2) - changeAmount).ToString();

                //base.OnPropertyChanged("ItemEntry" + value.ToString());
                //base.OnPropertyChanged("RightItemEntry" + value.ToString());

                //base.OnPropertyChanged("LeftPartyHeader2");
                //base.OnPropertyChanged("RightPartyHeader2");
                //InformationManager.DisplayMessage(new InformationMessage(this._targetList.Contains(this).ToString()));

                if (this._targetList.Contains(this))
                {
                    int index = this._targetList.IndexOf(this);
                    this._targetList.ElementAt(index)._troopNumber = (int.Parse(this._targetList.ElementAt(index)._troopNumber) + changeAmount).ToString();
                    this._targetList.ElementAt(index).ThrowOnPropertyChanged();
                }
                //this._targetList.Contains(this);
                //this._targetList.Add(this);


                base.OnPropertyChanged("TroopNumber");
            }
        }


        private CharacterCode GetCharacterCode(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side)
        {
            IFaction faction = null;
            if (type != PartyScreenLogic.TroopType.Prisoner)
            {
                /*if (side == PartyScreenLogic.PartyRosterSide.Left && this._partyScreenLogic.LeftOwnerParty != null)
                {
                    faction = this._pmvm._partyScreenLogic.LeftOwnerParty.MapFaction;
                }
                else if (this.Side == PartyScreenLogic.PartyRosterSide.Right && this._partyScreenLogic.RightOwnerParty != null)
                {
                    faction = this._partyScreenLogic.RightOwnerParty.MapFaction;
                }*/


                //if (this._targetList.Contains(this))
                //{
                //    int index = this._targetList.IndexOf(this);
                //    this._targetList.ElementAt(index);
                //}

                //faction = 
            }
            uint color = Color.White.ToUnsignedInteger();
            uint color2 = Color.White.ToUnsignedInteger();
            if (faction != null)
            {
                color = faction.Color;
                color2 = faction.Color2;
            }
            else if (character.Culture != null)
            {
                color = character.Culture.Color;
                color2 = character.Culture.Color2;
            }
            Equipment equipment = character.Equipment;
            string equipmentCode = (equipment != null) ? equipment.CalculateEquipmentCode() : null;
            BodyProperties bodyProperties = character.GetBodyProperties(character.Equipment, -1);
            return CharacterCode.CreateFrom(equipmentCode, bodyProperties, character.IsFemale, character.IsHero, color, color2, character.DefaultFormationClass);
        }

        [DataSourceProperty]
        public bool IsHero
        {
            get
            {
                return this._isHero;
            }
            set
            {
            }
        }

        [DataSourceProperty]
        public ImageIdentifierVM Code
        {
            get
            {
                return this._code;
            }
            set
            {
                if (value != this._code)
                {
                    this._code = value;
                    base.OnPropertyChangedWithValue(value, "Code");
                }
            }
        }


        [DataSourceProperty]
        public StringItemWithHintVM TierIconData
        {
            get
            {
                return this._tierIconData;
            }
            set
            {
                if (value != this._tierIconData)
                {
                    this._tierIconData = value;
                    base.OnPropertyChangedWithValue(value, "TierIconData");
                }
            }
        }

        [DataSourceProperty]
        public bool IsTroopTransferrableLeft
        {
            get
            {
                return this._isTroopTransferrableLeft;
            }
            set
            {

            }
        }

        [DataSourceProperty]
        public bool IsTroopTransferrableRight
        {
            get
            {
                return this._isTroopTransferrableRight;
            }
            set
            {

            }
        }

        [DataSourceProperty]
        public StringItemWithHintVM TypeIconData
        {
            get
            {
                return this._typeIconData;
            }
            set
            {
                if (value != this._typeIconData)
                {
                    this._typeIconData = value;
                    base.OnPropertyChangedWithValue(value, "TypeIconData");
                }
            }
        }

        private StringItemWithHintVM _tierIconData;
        private StringItemWithHintVM _typeIconData;


        [DataSourceProperty]
        public string TroopName
        {
            get
            {
                return this._troopName;
            }
        }

        [DataSourceProperty]
        public string TroopNumber
        {
            get
            {
                return this._troopNumber;
            }
        }

        [DataSourceProperty]
        public string TroopId
        {
            get
            {
                return this._troopID;
            }
        }


        /* [DataSourceProperty]
         public string TroopID
         {
             get
             {
                 return this._troopID;
             }
             set
             {
                 if (TroopID != this._troopID)
                 {
                     this._troopID = value;
                     base.OnPropertyChangedWithValue(TroopID, "TroopID");
                 }
             }
         }*/

        private int GetElementCount(MBBindingList<TroopVM> partyList)
        {
            int value = 0;
            foreach (TroopVM troopVM in partyList)
            {
                value += int.Parse(troopVM.TroopNumber);
            }

            return value;

        }

        public bool Equals(TroopVM other)
        {
            return _troopID == other._troopID;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as TroopVM);
        }


        private CharacterObject _character;
        private ImageIdentifierVM _code;

        private PartyManagerViewModel _pmvm;
        private MBBindingList<TroopVM> _ownList;
        public  MBBindingList<TroopVM> _targetList;

        public bool _isTroopTransferrableLeft;
        public bool _isTroopTransferrableRight;
        public bool _isHero;

        public string _troopID { get; set; }
        public string _troopName { get; set; }
        public string _troopNumber { get; set; }
    }
}
