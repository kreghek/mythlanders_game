using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Client.GameScreens.Combat.Ui;

internal sealed class BurningCombatMovement : ControlBase
{
    private readonly IconData _iconData;
    private double _lifetime = 1;

    public BurningCombatMovement(IconData iconData, int handSlotIndex)
    {
        _iconData = iconData;
        HandSlotIndex = handSlotIndex;
    }

    protected override Point CalcTextureOffset() => ControlTextures.CombatMove;

    protected override Color CalculateColor() => Color.Lerp(Color.Transparent, Color.White, (float)_lifetime);

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        spriteBatch.Draw(_iconData.Spritesheet, contentRect, _iconData.SourceRect, contentColor);
        DrawBackground(spriteBatch, contentColor);
    }

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

    public bool IsComplete => _lifetime <= 0;

    public int HandSlotIndex { get; }
}