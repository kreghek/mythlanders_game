using Client.Core;

using Microsoft.Xna.Framework.Graphics;

namespace Client.Assets.InteractionDeliveryObjects;

internal sealed class Projectile : ProjectileBase
{
    private const double DEFAULT_LIFETIME_DURATION_SECONDS = 0.3;

    public Projectile(ProjectileFunctions projectileFunctions,
        Texture2D bulletTexture,
        IAnimationFrameSet animation,
        double lifetimeDuration = DEFAULT_LIFETIME_DURATION_SECONDS) :
        base(
            projectileFunctions,
            bulletTexture,
            animation,
            lifetimeDuration)
    {
    }
}