﻿using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Client.GameScreens.VoiceCombat;

internal class VoiceCombatOptions : ControlBase
{
    private const int OPTION_BUTTON_MARGIN = 5;

    public VoiceCombatOptions()
    {
        Options = new List<VoiceCombatOptionButton>();
    }

    public IList<VoiceCombatOptionButton> Options { get; }

    public int GetHeight()
    {
        var sumOptionHeight = Options.Sum(x => CalcOptionButtonSize(x).Y) + OPTION_BUTTON_MARGIN;

        return sumOptionHeight;
    }

    public void SelectOption(int number)
    {
        Options.SingleOrDefault(x => x.Number == number)?.Click();
    }

    public void Update(ResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        foreach (var button in Options.ToArray())
        {
            button.Update(resolutionIndependentRenderer);
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
    }

    private static Point CalcOptionButtonSize(VoiceCombatOptionButton button)
    {
        var contentSize = button.GetContentSize();
        return (contentSize + Vector2.One * CONTENT_MARGIN + Vector2.UnitY * OPTION_BUTTON_MARGIN)
            .ToPoint();
    }
}
