using Client.Core;
using GameClient.Engine.Animations;
using GameClient.Engine.MoveFunctions;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Client.Assets.InteractionDeliveryObjects;

internal sealed class GunBulletProjectile : ProjectileBase
{
    private const double LIFETIME_DURATION_SECONDS = 0.15;
    private const int FPS = 8 * 3;

    public GunBulletProjectile(Vector2 startPosition,
        Vector2 endPosition,
        Texture2D bulletTexture,
        double lifetimeDuration = LIFETIME_DURATION_SECONDS) :
        base(
            new ProjectileFunctions(
                new LinearMoveFunction(startPosition, endPosition),
                new RotateForward(startPosition, endPosition)),
            bulletTexture,
            CreateFrameSet(),
            lifetimeDuration)
    {
    }

    protected override void DrawForegroundAdditionalEffects(SpriteBatch spriteBatch)
    {
        base.DrawForegroundAdditionalEffects(spriteBatch);
    }

    protected override void UpdateAdditionalEffects(GameTime gameTime)
    {
        base.UpdateAdditionalEffects(gameTime);
    }

    private static IAnimationFrameSet CreateFrameSet()
    {
        return AnimationFrameSetFactory.CreateSequential(4 * 4, frameCount: 4, fps: FPS, frameWidth: 64,
            frameHeight: 32, textureColumns: SfxSpriteConsts.Size64x32.COL_COUNT, isLoop: true);
    }
}