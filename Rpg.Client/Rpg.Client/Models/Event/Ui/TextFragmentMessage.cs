using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.Models.Event.Ui
{
    internal sealed class TextFragmentMessage: ControlBase
    {
        private readonly SpriteFont _font;
        private readonly EventTextFragment _eventTextFragment;

        public TextFragmentMessage(Texture2D texture, SpriteFont font, EventTextFragment eventTextFragment) : base(texture)
        {
            _font = font;
            _eventTextFragment = eventTextFragment;
        }

        protected override Color CalculateColor()
        {
            return Color.White;
        }

        private static string GetLocalizedText(string text)
        {
            // The text in the event is localized from resources yet.
            return text;
        }
        
        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle clientRect, Color contentColor)
        {
            var fragment = _eventTextFragment;
            var localizedSpeakerText = GetLocalizedText(fragment.Text);
            
            spriteBatch.DrawString(_font, localizedSpeakerText, clientRect.Location.ToVector2(), contentColor);
        }
    }
}