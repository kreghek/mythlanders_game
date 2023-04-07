using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Assets;
using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Client.Assets.InteractionDeliveryObjects;

internal sealed class EnergyArrowProjectile : ProjectileBase
{
    private const double LIFETIME_DURATION_SECONDS = 0.3;
    private const int FPS = 8 * 3;

    private readonly ParticleSystem _tailParticleSystem;

    public EnergyArrowProjectile(Vector2 startPosition,
        Vector2 endPosition,
        Texture2D bulletTexture,
        double lifetimeDuration = LIFETIME_DURATION_SECONDS) :
        base(
            new ProjectileFunctions(
                new SlowDownMoveFunction(startPosition, endPosition),
                new RotateForward(startPosition, endPosition)),
            bulletTexture,
            CreateFrameSet(),
            lifetimeDuration)
    {
        var particleGenerator = new TailParticleGenerator(new[] { bulletTexture });
        _tailParticleSystem = new ParticleSystem(startPosition, particleGenerator);
    }

    protected override void DrawForegroundAdditionalEffects(SpriteBatch spriteBatch)
    {
        base.DrawForegroundAdditionalEffects(spriteBatch);

        _tailParticleSystem.Draw(spriteBatch);
    }

    protected override void UpdateAdditionalEffects(GameTime gameTime)
    {
        base.UpdateAdditionalEffects(gameTime);

        _tailParticleSystem.MoveEmitter(CurrentPosition);
        _tailParticleSystem.Update(gameTime);
    }

    private static IAnimationFrameSet CreateFrameSet()
    {
        return AnimationFrameSetFactory.CreateSequential(0, frameCount: 4, fps: FPS, frameWidth: 64,
            frameHeight: 32, textureColumns: SfxSpriteConsts.Size64x32.COL_COUNT, isLoop: true);
    }
}