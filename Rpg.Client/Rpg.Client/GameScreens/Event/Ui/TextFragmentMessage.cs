using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Event.Ui
{
    internal sealed class TextFragmentMessage : ControlBase
    {
        private readonly EventTextFragment _eventTextFragment;
        private readonly SpriteFont _font;
        private readonly string _localizedText;
        private readonly SoundEffect _textSoundEffect;
        private readonly StringBuilder _textToPrintBuilder;

        private double _characterCounter;
        private SoundEffectInstance? _currentSound;
        private double _delayCounter;
        private int _index;

        public TextFragmentMessage(Texture2D texture, SpriteFont font, EventTextFragment eventTextFragment,
            Microsoft.Xna.Framework.Audio.SoundEffect textSoundEffect) :
            base(texture)
        {
            _font = font;
            _eventTextFragment = eventTextFragment;
            _textSoundEffect = textSoundEffect;
            _localizedText = GetLocalizedText(_eventTextFragment.Text);
            _textToPrintBuilder = new StringBuilder();
        }

        public bool IsComplete { get; private set; }

        public Vector2 CalculateSize()
        {
            // var localizedText = GetLocalizedText(_textToPrintBuilder.ToString());
            // var size = _font.MeasureString(localizedText);
            var size = _font.MeasureString(_localizedText);
            // TODO use margin
            return size + Vector2.One * (2 * 4);
        }

        public void MoveToCompletion()
        {
            IsComplete = true;
            _textToPrintBuilder.Clear();
            _textToPrintBuilder.Append(_localizedText);
        }

        public void Update(GameTime gameTime)
        {
            if (IsComplete)
            {
                return;
            }

            var duration = _eventTextFragment.Speaker == UnitName.Environment ? 0.01f : 0.01f;
            if (_characterCounter <= duration)
            {
                _characterCounter += gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                if (_index < _localizedText.Length - 1)
                {
                    PlayTextSound();

                    _textToPrintBuilder.Append(_localizedText[_index]);
                    _characterCounter = 0;
                    _index++;
                }
                else
                {
                    if (_delayCounter <= 1)
                    {
                        _delayCounter += gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else
                    {
                        IsComplete = true;
                    }
                }
            }
        }

        protected override Color CalculateColor()
        {
            return Color.White;
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle clientRect, Color contentColor)
        {
            spriteBatch.DrawString(_font, _textToPrintBuilder.ToString(),
                clientRect.Location.ToVector2() + Vector2.UnitX * 2,
                Color.SaddleBrown);
        }

        private static string GetLocalizedText(string text)
        {
            // The text in the event is localized from resources yet.
            return text;
        }

        private void PlayTextSound()
        {
            if (_currentSound is null || _currentSound.State == SoundState.Stopped)
            {
                _currentSound = _textSoundEffect.CreateInstance();
                _currentSound.Volume = 0.5f;
                _currentSound.Play();
            }
        }
    }
}