using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.Models.Event.Ui
{
    internal sealed class TextFragment: ControlBase
    {
        const int PORTRAIT_SIZE = 32;
        
        private readonly SpriteFont _font;
        private readonly Texture2D _portraitsTexture;
        private readonly TextFragmentMessage _message;
        private readonly UnitName _speaker;
        private readonly string? _localizedSpeakerName;

        public TextFragment(Texture2D texture, SpriteFont font, EventTextFragment eventTextFragment, Texture2D portraitsTexture) : base(texture)
        {
            _font = font;
            _portraitsTexture = portraitsTexture;
            _speaker = eventTextFragment.Speaker;
            _localizedSpeakerName = GetSpeaker(_speaker);
            _message = new TextFragmentMessage(texture, font, eventTextFragment);
        }

        protected override Color CalculateColor()
        {
            return Color.White;
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle clientRect, Color contentColor)
        {
            if (_speaker != UnitName.Environment)
            {
                DrawSpeaker(spriteBatch, clientRect.Location.ToVector2());
            }

            var textPosition = clientRect.Location.ToVector2() + Vector2.UnitX * PORTRAIT_SIZE;
            _message.Rect = new Rectangle(textPosition.ToPoint(),
                new Point(clientRect.Width - PORTRAIT_SIZE, PORTRAIT_SIZE));
            _message.Draw(spriteBatch);
        }

        protected override void DrawBackground(SpriteBatch spriteBatch, Color color)
        {
            // Do nothing to draw transparent background.
            //base.DrawBackground(spriteBatch, color);
        }

        private void DrawSpeaker(SpriteBatch spriteBatch, Vector2 position)
        {
            var portrainSourceRect = UnsortedHelpers.GetUnitPortraitRect(_speaker);
            spriteBatch.Draw(_portraitsTexture, position, portrainSourceRect, Color.White);
            spriteBatch.DrawString(_font, _localizedSpeakerName, position + Vector2.UnitY * PORTRAIT_SIZE,
                Color.White);
        }
        
        private static string? GetSpeaker(UnitName speaker)
        {
            if (speaker == UnitName.Environment)
            {
                return null;
            }

            if (speaker == UnitName.Undefined)
            {
                Debug.Fail("Speaker is undefined.");
                return null;
            }

            var unitName = speaker;
            var name = GameObjectHelper.GetLocalized(unitName);

            return name;
        }

        public Vector2 CalculateSize()
        {
            var messageSize = _message.CalculateSize();
            var portraitSize = new Vector2(PORTRAIT_SIZE, PORTRAIT_SIZE);
            // TODO use margin
            return Vector2.Max(messageSize, portraitSize) + Vector2.One * (2 * 4);
        }
    }
}