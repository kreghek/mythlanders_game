using Client.Engine;

using CombatDicesTeam.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.Ui;

internal sealed class CombatMovementButton : EntityButtonBase<CombatMovementInstance>
{
    private readonly Texture2D _icon;
    private readonly Rectangle? _iconRect;

    public CombatMovementButton(IconData iconData, CombatMovementInstance combatMovement) : base(combatMovement)
    {
        _icon = iconData.Spritesheet;
        _iconRect = iconData.SourceRect;
        IconData = iconData;
    }

    public IconData IconData { get; }

    public bool IsSelected => _buttonState == UiButtonState.Hover;

    protected override Point CalcTextureOffset()
    {
        return ControlTextures.CombatMove;
    }

    protected override Color CalculateColor()
    {
        return IsEnabled ? Color.White : Color.DarkGray;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
    {
        spriteBatch.Draw(_icon, contentRect, _iconRect, color);
        DrawBackground(spriteBatch, color);
    }
}