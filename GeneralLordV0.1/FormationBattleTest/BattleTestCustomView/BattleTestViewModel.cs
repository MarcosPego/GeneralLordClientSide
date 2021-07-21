using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace GeneralLord.FormationBattleTest.BattleTestCustomView
{
    public class BattleTestViewModel : ViewModel
    {
        public BattleTestViewModel()
        {
			this.FirstText = new TextObject("{=ATFirstText} This is a simulator where you can define your army's formations, after concluding kill the lone enemy to conclude the battle", null).ToString();
			this.SecondText = new TextObject("{=ATSecondText} Place each formation where you desire (being aware that the formation I is the reference point)", null).ToString();
			this.ThirdText = new TextObject("{=ATThirdText}Save the final formation using \n Ctrl + (F10 / F11 / F12) to save to the slot 0 / 1 / 2", null).ToString();
			this.FourthText = new TextObject("{=ATFourthText}You can load the saved formations anytime using \n  F10 / F11 / F12 to load the slot 0 / 1 / 2", null).ToString();
		}

		[DataSourceProperty]
		public string FirstText
		{
			get
			{
				return this._firstText;
			}
			set
			{
				if (this._firstText != value)
				{
					this._firstText = value;
					base.OnPropertyChangedWithValue(value, "FirstText");
				}
			}
		}

		[DataSourceProperty]
		public string SecondText
		{
			get
			{
				return this._secondText;
			}
			set
			{
				if (this._secondText != value)
				{
					this._secondText = value;
					base.OnPropertyChangedWithValue(value, "SecondText");
				}
			}
		}

		[DataSourceProperty]
		public string ThirdText
		{
			get
			{
				return this._thirdText;
			}
			set
			{
				if (this._thirdText != value)
				{
					this._thirdText = value;
					base.OnPropertyChangedWithValue(value, "ThirdText");
				}
			}
		}

		public string FourthText
		{
			get
			{
				return this._fourthText;
			}
			set
			{
				if (this._fourthText != value)
				{
					this._fourthText = value;
					base.OnPropertyChangedWithValue(value, "FourthText");
				}
			}
		}

		private string _firstText;
		private string _secondText;
		private string _thirdText;
		private string _fourthText;
	}
}
