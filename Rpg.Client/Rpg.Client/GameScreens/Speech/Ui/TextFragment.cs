using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Speech.Ui
{
    internal sealed class TextFragment : ControlBase
    {
        private const int PORTRAIT_SIZE = 32;

        private readonly SpriteFont _font;
        private readonly string? _localizedSpeakerName;
        private readonly TextFragmentMessage _message;
        private readonly Texture2D _portraitsTexture;
        private readonly UnitName _speaker;

        public TextFragment(EventTextFragment eventTextFragment, Texture2D portraitsTexture, SoundEffect textSoundEffect, IDice dice)
        {
            _font = UiThemeManager.UiContentStorage.GetMainFont();
            _portraitsTexture = portraitsTexture;
            _speaker = eventTextFragment.Speaker;
            _localizedSpeakerName = GetSpeaker(_speaker);
            _message = new TextFragmentMessage(eventTextFragment, textSoundEffect, dice, _speaker != UnitName.Environment);
        }

        public bool IsComplete => _message.IsComplete;

        public Vector2 CalculateSize()
        {
            var messageSize = _message.CalculateSize();
            var portraitSize = new Vector2(PORTRAIT_SIZE, PORTRAIT_SIZE);

            var width = Math.Max(messageSize.X, portraitSize.X);
            var height = Math.Max(messageSize.Y, portraitSize.Y);

            // TODO use margin
            return new Vector2(width, height) + Vector2.One * (2 * 4);
        }

        public void MoveToCompletion()
        {
            _message.MoveToCompletion();
        }

        public void Update(GameTime gameTime)
        {
            _message.Update(gameTime);
        }

        protected override Color CalculateColor()
        {
            return Color.White;
        }

        protected override void DrawBackground(SpriteBatch spriteBatch, Color color)
        {
            // Do nothing to draw transparent background.
            //base.DrawBackground(spriteBatch, color);
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle clientRect, Color contentColor)
        {
            if (_speaker != UnitName.Environment)
            {
                DrawSpeaker(spriteBatch, clientRect.Location.ToVector2());
            }

            var textPosition = clientRect.Location.ToVector2() + Vector2.UnitX * PORTRAIT_SIZE;
            _message.Rect = new Rectangle(textPosition.ToPoint(),
                new Point(clientRect.Width, clientRect.Height));
            _message.Draw(spriteBatch);
        }

        private void DrawSpeaker(SpriteBatch spriteBatch, Vector2 position)
        {
            var portraitSourceRect = UnsortedHelpers.GetUnitPortraitRect(_speaker);
            spriteBatch.Draw(_portraitsTexture, position, portraitSourceRect, Color.White);

            var speakerNameTextSize = _font.MeasureString(_localizedSpeakerName);
            var speakerNameTextPosition = position + Vector2.UnitY * PORTRAIT_SIZE;

            var xDiff = speakerNameTextSize.X - PORTRAIT_SIZE;

            var alignedSpeakerNameTextPosition =
                new Vector2(speakerNameTextPosition.X - xDiff, speakerNameTextPosition.Y);

            spriteBatch.DrawString(_font, _localizedSpeakerName, alignedSpeakerNameTextPosition, Color.White);
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

            return GameObjectHelper.GetLocalized(speaker);
        }

        protected override Point CalcTextureOffset() => Point.Zero;
    }
}