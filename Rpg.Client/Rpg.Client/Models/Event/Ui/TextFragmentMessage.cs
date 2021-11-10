using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.Models.Event.Ui
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
        }

        public Vector2 CalculateSize()
        {
            var fragment = _eventTextFragment;
            var localizedText = GetLocalizedText(fragment.Text);
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
            var fragment = _eventTextFragment;
            var localizedText = GetLocalizedText(fragment.Text);

            spriteBatch.DrawString(_font, localizedText, clientRect.Location.ToVector2(), contentColor);
        }

        private static string GetLocalizedText(string text)
        {
            // The text in the event is localized from resources yet.
            return text;
        }
    }
}