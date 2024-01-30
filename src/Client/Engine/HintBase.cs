using CombatDicesTeam.Engine.Ui;

using Microsoft.Xna.Framework;

namespace Client.Engine;

internal abstract class HintBase : ControlBase
{
    protected override Color CalculateColor()
    {
        return Color.Lerp(Color.Transparent, Color.SlateBlue, 0.85f);
    }

    protected HintBase() : base(UiThemeManager.UiContentStorage.GetControlBackgroundTexture())
    {
    }
}