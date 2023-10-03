using Client.Core;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Assets.ActorVisualizationStates.Primitives;

internal sealed class BloodCombatVisualEffect : ICombatVisualEffect
{
    private readonly Vector2 _position;
    private readonly Texture2D _bloodTexture;
    private readonly bool _flipX;
    private readonly IAnimationFrameSet _animation;

    public BloodCombatVisualEffect(Vector2 position, Texture2D bloodTexture, bool flipX, IAnimationFrameSet animation)
    {
        _position = position;
        _bloodTexture = bloodTexture;
        _flipX = flipX;
        _animation = animation;
        _animation.End += (_, _) => { IsDestroyed = true; };
    }

    public bool IsDestroyed { get; private set; }

    public void DrawBack(SpriteBatch spriteBatch)
    {
        // Draw nothing
    }

    public void DrawFront(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_bloodTexture, _position, _animation.GetFrameRect(), Color.White, 0, Vector2.Zero, 1, _flipX ? SpriteEffects.FlipHorizontally: SpriteEffects.None, 0);
    }

    public void Update(GameTime gameTime)
    {
        if (IsDestroyed)
        {
            return;
        }

        _animation.Update(gameTime);
    }
}
