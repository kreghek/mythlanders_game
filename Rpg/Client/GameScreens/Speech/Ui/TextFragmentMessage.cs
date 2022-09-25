using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Speech.Ui
{
    internal sealed class TextFragmentMessage : ControlBase
    {
        private readonly SpriteFont _font;
        private readonly bool _isCharacterSpeech;

        private readonly Speech _speech;

        public TextFragmentMessage(EventTextFragment eventTextFragment,
            SoundEffect textSoundEffect, IDice dice, bool isCharacterSpeech)
        {
            _font = UiThemeManager.UiContentStorage.GetTitlesFont();

            var speechText = SpeechVisualizationHelper.PrepareLocalizedText(eventTextFragment.TextSid);
            _speech = new Speech(speechText, new SpeechSoundWrapper(textSoundEffect), new SpeechRandomProvider(dice));
            _isCharacterSpeech = isCharacterSpeech;
        }

        public bool IsComplete => _speech.IsComplete;

        public Vector2 CalculateSize()
        {
            var size = _font.MeasureString(_speech.FullText);
            // TODO use margin
            return size + Vector2.One * (2 * 4);
        }

        public void MoveToCompletion()
        {
            _speech.MoveToCompletion();
        }

        public void Update(GameTime gameTime)
        {
            _speech.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        protected override Point CalcTextureOffset()
        {
            if (_isCharacterSpeech)
            {
                return ControlTextures.Speech;
            }

            return ControlTextures.Shadow;
        }

        protected override Color CalculateColor()
        {
            return Color.White;
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle clientRect, Color contentColor)
        {
            spriteBatch.DrawString(_font, _speech.GetCurrentText(),
                clientRect.Location.ToVector2() + Vector2.UnitX * 2,
                Color.SaddleBrown);
        }
    }
}