using Client.Engine;

using Core.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.Ui;

internal class ManeuverButton : ButtonBase
{
    public ManeuverButton(FieldCoords fieldCoords)
    {
        FieldCoords = fieldCoords;
    }

    public FieldCoords FieldCoords { get; }

    protected override Point CalcTextureOffset()
    {
        return ControlTextures.CombatMove;
    }

    protected override Color CalculateColor()
    {
        return IsEnabled ? Color.White : Color.Lerp(Color.DarkGray, Color.Transparent, 0.5f);
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
    }
}