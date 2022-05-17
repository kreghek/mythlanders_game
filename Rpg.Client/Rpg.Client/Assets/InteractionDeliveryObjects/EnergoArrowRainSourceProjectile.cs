using System;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;
using Rpg.Client.Core.AnimationFrameSets;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.InteractionDeliveryObjects
{
    internal sealed class EnergoArrowRainSourceProjectile : ProjectileBase
    {
        private const double LIFETIME_DURATION_SECONDS = 0.3;
        private const int FPS = 8 * 3;

        public EnergoArrowRainSourceProjectile(Vector2 startPosition,
            GameObjectContentStorage contentStorage,
            AnimationBlocker? blocker,
            ICombatUnit? targetCombatUnit = null,
            Action<ICombatUnit>? interaction = null,
            double lifetimeDuration = LIFETIME_DURATION_SECONDS) :
            base(startPosition, startPosition - Vector2.UnitY * 400, contentStorage.GetBulletGraphics(),
                CreateFrameSet(), lifetimeDuration, blocker, targetCombatUnit, interaction)
        {
        }

        private static IAnimationFrameSet CreateFrameSet()
        {
            var rising = AnimationFrameSetFactory.CreateSequential(
                startFrameIndex: 4,
                frameCount: 5,
                speedMultiplicator: FPS,
                frameWidth: SfxSpriteConsts.Size64x32.WIDTH,
                frameHeight: SfxSpriteConsts.Size64x32.HEIGHT,
                textureColumns: SfxSpriteConsts.Size64x32.COL_COUNT,
                isLoop: false);

            var body = AnimationFrameSetFactory.CreateSequential(
                startFrameIndex: 9,
                frameCount: 5,
                speedMultiplicator: FPS,
                frameWidth: SfxSpriteConsts.Size64x32.WIDTH,
                frameHeight: SfxSpriteConsts.Size64x32.HEIGHT,
                textureColumns: SfxSpriteConsts.Size64x32.COL_COUNT,
                isLoop: true);

            var full = new CompositeAnimationFrameSet(new[] { rising, body });

            return full;
        }
    }
}