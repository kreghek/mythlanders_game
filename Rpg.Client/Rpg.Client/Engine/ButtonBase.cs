using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rpg.Client.Engine
{
    internal abstract class ButtonBase : ControlBase
    {
        private UiButtonState _buttonState;

        protected ButtonBase(Texture2D texture, Rectangle rect) : base(texture)
        {
            Rect = rect;
            _buttonState = UiButtonState.OutOfButton;
        }

        public bool IsEnabled { get; set; } = true;

        public void Click()
        {
            if (!IsEnabled)
            {
                return;
            }

            OnClick?.Invoke(this, EventArgs.Empty);
            PlayClickSoundIfExists();
        }

        public void Update(ResolutionIndependentRenderer? resolutionIndependentRenderer)
        {
            UpdateContent();

            if (!IsEnabled)
            {
                return;
            }

            var mouseState = Mouse.GetState();
            if (CheckMouseOver(resolutionIndependentRenderer))
            {
                if (_buttonState == UiButtonState.Hover && mouseState.LeftButton == ButtonState.Pressed)
                {
                    _buttonState = UiButtonState.Pressed;
                }
                else if (mouseState.LeftButton == ButtonState.Released && _buttonState == UiButtonState.Pressed)
                {
                    _buttonState = UiButtonState.Hover;
                    Click();
                }
                else if (mouseState.LeftButton == ButtonState.Released)
                {
                    if (_buttonState == UiButtonState.OutOfButton)
                    {
                        PlayHoverSoundIfExists();
                    }

                    _buttonState = UiButtonState.Hover;
                }
            }
            else
            {
                _buttonState = UiButtonState.OutOfButton;
            }
        }

        protected override Color CalculateColor()
        {
            return SelectColorByState();
        }

        protected virtual Color GetStartColor()
        {
            return Color.White;
        }

        protected virtual void UpdateContent()
        {
        }

        private bool CheckMouseOver(ResolutionIndependentRenderer? resolutionIndependentRenderer)
        {
            var mouseState = Mouse.GetState();
            var mousePosition = mouseState.Position;

            if (resolutionIndependentRenderer is null)
            {
                var mouseRect = new Rectangle(mousePosition.X, mousePosition.Y, 1, 1);

                return Rect.Intersects(mouseRect);
            }
            else
            {
                var rirPosition =
                    resolutionIndependentRenderer.ScaleMouseToScreenCoordinates(mousePosition.ToVector2());
                var mouseRect = new Rectangle(rirPosition.ToPoint(), new Point(1, 1));

                return Rect.Intersects(mouseRect);
            }
        }

        private static void PlayClickSoundIfExists()
        {
            if (UiThemeManager.SoundStorage is null)
            {
                // See the description in PlayHoverSoundIfExists method.
                return;
            }

            var soundEffect = UiThemeManager.SoundStorage.GetButtonClickEffect();
            soundEffect.Play();
        }

        private static void PlayHoverSoundIfExists()
        {
            if (UiThemeManager.SoundStorage is null)
            {
                // Sound content was not loaded.
                // This may occured by a few reasons:
                // - Content was not loaded yet. This is not critical to skip effect playing once. May be next time sound effect will be ready to play.
                // - There is test environment. So there is need no sounds to auto-tests.
                // - Developer forget to load content and assign UiThemeManager.SoundStorage. This is error bu no way to differ is from two other reasons above.
                // So just do nothing.
                return;
            }

            var soundEffect = UiThemeManager.SoundStorage.GetButtonHoverEffect();
            soundEffect.Play();
        }

        private Color SelectColorByState()
        {
            var color = GetStartColor();

            return _buttonState switch
            {
                UiButtonState.OutOfButton => color, // Do not modify start color.
                UiButtonState.Hover => Color.Lerp(color, Color.Wheat, 0.25f),
                UiButtonState.Pressed => Color.Lerp(color, Color.Wheat, 0.75f),
                _ => Color.Red
            };
        }

        public event EventHandler? OnClick;
    }
}