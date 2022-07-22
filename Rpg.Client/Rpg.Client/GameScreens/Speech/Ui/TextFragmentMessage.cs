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

        private readonly Speech _speech;

        public TextFragmentMessage(Texture2D texture, SpriteFont font, EventTextFragment eventTextFragment,
            SoundEffect textSoundEffect, IDice dice) :
            base(texture)
        {
            _font = font;

            var speechText = SpeechVisualizationHelper.PrepareLocalizedText(eventTextFragment.TextSid);
            _speech = new Speech(speechText, new SpeechSoundWrapper(textSoundEffect), new SpeechRandomProvider(dice));
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