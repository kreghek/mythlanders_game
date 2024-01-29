using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.Ui;

internal sealed class BurningCombatMovement : ControlBase
{
    private readonly IconData _iconData;
    private double _lifetime = 1;

    public BurningCombatMovement(IconData iconData, int handSlotIndex) : base(UiThemeManager.UiContentStorage.GetControlBackgroundTexture())
    {
        _iconData = iconData;
        HandSlotIndex = handSlotIndex;
    }

    public int HandSlotIndex { get; }

    public bool IsComplete => _lifetime <= 0;

    public void Update(GameTime gameTime)
    {
        if (_lifetime > 0)
        {
            _lifetime -= gameTime.ElapsedGameTime.TotalSeconds * 2.5;
        }
        else
        {
            _lifetime = 0;
        }
    }

    protected override Point CalcTextureOffset()
    {
        return ControlTextures.CombatMove;
    }

    protected override Color CalculateColor()
    {
        return Color.Lerp(Color.Transparent, Color.White, (float)_lifetime);
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        spriteBatch.Draw(_iconData.Spritesheet, contentRect, _iconData.SourceRect, contentColor);
        DrawBackground(spriteBatch, contentColor);
    }
}