using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CunningLords.Interaction
{
    public class TextViewModel : ViewModel
    {
        public TextObject TextObject
        {
            get
            {
                return this._textObject;
            }
            set
            {
                this._textObject = value;
            }
        }

        [DataSourceProperty]
        public string Text
        {
            get
            {
                return this._text;
            }
            set
            {
                if (this._text == value)
                {
                    return;
                }
                this._text = value;
                base.OnPropertyChanged("Text");
            }
        }


        public TextViewModel(TextObject text)
        {
            this.TextObject = text;
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            this.TextObject = this.TextObject;
        }

        private TextObject _textObject;

        private string _text;
    }
}
