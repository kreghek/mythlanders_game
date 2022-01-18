using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Event.Ui
{
    internal sealed class TextFragmentMessage : ControlBase
    {
        private readonly EventTextFragment _eventTextFragment;
        private readonly SpriteFont _font;

        public TextFragmentMessage(Texture2D texture, SpriteFont font, EventTextFragment eventTextFragment) :
            base(texture)
        {
            _font = font;
            _eventTextFragment = eventTextFragment;
            
            _localizedText = GetLocalizedText(_eventTextFragment.Text);
            _textToPrintBuilder = new StringBuilder();
        }

        public Vector2 CalculateSize()
        {
            var localizedText = GetLocalizedText(_textToPrintBuilder.ToString());
            var size = _font.MeasureString(localizedText);
            // TODO use margin
            return size + Vector2.One * (2 * 4);
        }

        protected override Color CalculateColor()
        {
            return Color.White;
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle clientRect, Color contentColor)
        {
            spriteBatch.DrawString(_font, _textToPrintBuilder.ToString(), clientRect.Location.ToVector2() + Vector2.UnitX * 2,
                Color.SaddleBrown);
        }

        private static string GetLocalizedText(string text)
        {
            // The text in the event is localized from resources yet.
            return text;
        }

        private double _characterCounter;
        private readonly string _localizedText;
        private readonly StringBuilder _textToPrintBuilder;
        private int _index;

        public void Update(GameTime gameTime)
        {
            if (_characterCounter <= 0.01f)
            {
                _characterCounter += gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                if (_index < _localizedText.Length - 1)
                {
                    _textToPrintBuilder.Append(_localizedText[_index]);
                    _characterCounter = 0;
                    _index++;
                }
                else
                {
                    IsComplete = true;
                }
            }
        }

        public bool IsComplete { get; private set; }
    }
}