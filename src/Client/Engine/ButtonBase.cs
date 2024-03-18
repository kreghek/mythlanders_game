using System;

using CombatDicesTeam.Engine.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Client.Engine;

internal abstract class ButtonBase : UiElementContentBase
{
    protected UiButtonState _buttonState;

    protected ButtonBase() : base(UiThemeManager.UiContentStorage.GetControlBackgroundTexture())
    {
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

    public void Update(IScreenProjection screenProjection)
    {
        UpdateContent();

        if (!IsEnabled)
        {
            return;
        }

        var mouseState = Mouse.GetState();
        if (CheckMouseOver(screenProjection))
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
                    OnHover?.Invoke(this, EventArgs.Empty);
                }

                _buttonState = UiButtonState.Hover;
            }
        }
        else
        {
            if (_buttonState != UiButtonState.OutOfButton)
            {
                OnLeave?.Invoke(this, EventArgs.Empty);
            }

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

    protected virtual bool IsMouseOver(Rectangle mouseRect)
    {
        return Rect.Intersects(mouseRect);
    }

    protected virtual void UpdateContent()
    {
    }

    private bool CheckMouseOver(IScreenProjection screenProjection)
    {
        var mouseState = Mouse.GetState();
        var mousePosition = mouseState.Position.ToVector2();

        var rirPosition =
            screenProjection.ConvertScreenToWorldCoordinates(mousePosition);
        var mouseRect = new Rectangle(rirPosition.ToPoint(), new Point(1, 1));

        return IsMouseOver(mouseRect);
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

        if (!IsEnabled)
        {
            return Color.Lerp(color, Color.Gray, 0.5f);
        }

        return _buttonState switch
        {
            UiButtonState.OutOfButton => color, // Do not modify start color.
            UiButtonState.Hover => Color.Lerp(color, Color.Wheat, 0.25f),
            UiButtonState.Pressed => Color.Lerp(color, Color.Wheat, 0.75f),
            _ => Color.Red
        };
    }

    public event EventHandler? OnClick;
    public event EventHandler? OnHover;
    public event EventHandler? OnLeave;
}