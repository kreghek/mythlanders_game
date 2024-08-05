using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core;
using Client.Engine;
using Client.GameScreens.TextDialogue.Ui;

using CombatDicesTeam.Engine.Ui;

using GameClient.Engine.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Client.ScreenManagement.Ui.TextEvents;

internal class DialogueOptions : ControlBase
{
    private const int OPTION_BUTTON_MARGIN = 5;

    private HintBase? _optionDescription;
    private ControlBase? _optionUnderHint;
    private readonly HoverController<DialogueOptionButton> _optionHoverController;
    private readonly IList<DialogueOptionButton> _options;

    public DialogueOptions() : base(UiThemeManager.UiContentStorage.GetControlBackgroundTexture())
    {
        _options = new List<DialogueOptionButton>();

        _optionHoverController = new HoverController<DialogueOptionButton>();

        _optionHoverController.Hover += (_, button) =>
        {
            if (button is not null)
            {
                HandleOptionHover(button);
            }
        };

        _optionHoverController.Leave += (_, button) =>
        {
            if (button is not null)
            {
                HandleOptionLeave(button);
            }
        };
    }

    private IReadOnlyList<DialogueOptionButton> Options => _options.ToArray();

    public void Clear()
    {
        _optionDescription = null;
        _optionUnderHint = null;

        _options.Clear();
    }

    public event EventHandler<DialogueOptionButton>? OptionHover;

    public void Add(DialogueOptionButton optionButton)
    {
        optionButton.OnHover += (sender, _) =>
        {
            _optionHoverController.HandleHover((DialogueOptionButton?)sender);
        };

        optionButton.OnLeave += (sender, _) =>
        {
            _optionHoverController.HandleLeave((DialogueOptionButton?)sender);
        };

        _options.Add(optionButton);
    }

    private void HandleOptionHover(DialogueOptionButton button)
    {
        var (text, isLocalized) = SpeechVisualizationHelper.PrepareLocalizedText(button.ResourceSid + "_Description");

        if (isLocalized)
        {
            _optionDescription = new TextHint(StringHelper.LineBreaking(text, 60));
            _optionUnderHint = button;
        }
        else
        {
            _optionDescription = null;
            _optionUnderHint = null;
        }

        OptionHover?.Invoke(this, button);
    }

    private void HandleOptionLeave(DialogueOptionButton button)
    {
        _optionDescription = null;
        _optionUnderHint = null;
    }


    public int GetHeight()
    {
        var sumOptionHeight = Options.Sum(x => CalcOptionButtonSize(x).Y) + OPTION_BUTTON_MARGIN;

        return sumOptionHeight;
    }

    public void SelectOption(int number)
    {
        Options.SingleOrDefault(x => x.Number == number)?.Click();
    }

    public void Update(IResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        foreach (var button in Options)
        {
            button.Update(resolutionIndependentRenderer);
        }

        DetectOptionDescription(resolutionIndependentRenderer);
    }

    private void DetectOptionDescription(IScreenProjection screenProjection)
    {
        var mouse = Mouse.GetState().Position;

        var rirPosition = screenProjection.ConvertScreenToWorldCoordinates(new Vector2(mouse.X, mouse.Y));

        foreach (var dialogueOptionButton in Options)
        {
            if (dialogueOptionButton.Rect.Contains(new Rectangle((int)rirPosition.X, (int)rirPosition.Y, 1, 1)))
            {
                HandleOptionHover(dialogueOptionButton);

                break;
            }
        }
    }

    protected override Point CalcTextureOffset()
    {
        return ControlTextures.PanelBlack;
    }

    protected override Color CalculateColor()
    {
        return Color.White;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        var lastTopButtonPosition = 0;
        foreach (var button in Options)
        {
            var optionButtonSize = CalcOptionButtonSize(button);
            var optionPosition = new Vector2(OPTION_BUTTON_MARGIN + contentRect.Left,
                lastTopButtonPosition + contentRect.Top).ToPoint();

            button.Rect = new Rectangle(optionPosition, optionButtonSize + new Point(1000, 0));

            button.Draw(spriteBatch);

            lastTopButtonPosition += optionButtonSize.Y;
        }

        if (_optionDescription is not null && _optionUnderHint is not null)
        {
            _optionDescription.Rect = new Rectangle(
                _optionUnderHint.Rect.Left,
                _optionUnderHint.Rect.Bottom,
                _optionDescription.Size.X,
                _optionDescription.Size.Y);

            _optionDescription.Draw(spriteBatch);
        }
    }

    private static Point CalcOptionButtonSize(DialogueOptionButton button)
    {
        var contentSize = button.GetContentSize();
        return (contentSize + Vector2.One * CONTENT_MARGIN + Vector2.UnitY * OPTION_BUTTON_MARGIN)
            .ToPoint();
    }
}