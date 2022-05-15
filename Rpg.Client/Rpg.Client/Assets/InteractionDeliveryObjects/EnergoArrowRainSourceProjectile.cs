using System;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;
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
            base(startPosition, startPosition - Vector2.UnitY * 400, contentStorage.GetBulletGraphics(), CreateFrameSet(), lifetimeDuration, blocker, targetCombatUnit, interaction)
        {
        }

        private static IAnimationFrameSet CreateFrameSet()
        {
            return AnimationFrameSetFactory.CreateSequential(startFrameIndex: 9, frameCount: 5, speedMultiplicator: FPS, frameWidth: 64, frameHeight: 32, textureColumns: SfxSpriteConsts.Size64x32.COL_COUNT, isLoop: true);
        }
    }
}