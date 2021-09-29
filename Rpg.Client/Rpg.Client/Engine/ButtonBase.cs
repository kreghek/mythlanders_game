﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rpg.Client.Engine
{
    internal abstract class ButtonBase
    {
        private const int CONTENT_MARGIN = 0;
        private UiButtonState _buttonState;
        private Rectangle _rect;

        protected ButtonBase(Texture2D texture, Rectangle rect)
        {
            Texture = texture;
            Rect = rect;
            _buttonState = UiButtonState.OutOfButton;
        }

        public bool IsEnabled { get; set; } = true;

        public Rectangle Rect
        {
            get => _rect;
            set
            {
                _rect = value;
                HandleRectChanges();
            }
        }

        public Texture2D Texture { get; }

        public void Click()
        {
            if (!IsEnabled)
            {
                return;
            }

            OnClick?.Invoke(this, EventArgs.Empty);
            PlayClickSoundIfExists();
        }

        protected virtual void DrawBackground(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(Texture, Rect, color);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var color = SelectColorByState();

            DrawBackground(spriteBatch, color);

            var contentRect = new Rectangle(
                CONTENT_MARGIN + Rect.Left,
                CONTENT_MARGIN + Rect.Top,
                Rect.Width - (CONTENT_MARGIN * 2),
                Rect.Height - (CONTENT_MARGIN * 2));

            DrawContent(spriteBatch, contentRect, color);
        }

        public void Update()
        {
            if (!IsEnabled)
            {
                return;
            }

            var mouseState = Mouse.GetState();
            if (CheckMouseOver())
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

        protected abstract void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color);

        protected virtual void HandleRectChanges()
        {
            // Used only by children.
        }

        private bool CheckMouseOver()
        {
            var mouseState = Mouse.GetState();
            var mousePosition = mouseState.Position;

            var mouseRect = new Rectangle(mousePosition.X, mousePosition.Y, 1, 1);

            return Rect.Intersects(mouseRect);
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
            var color = Color.White;

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