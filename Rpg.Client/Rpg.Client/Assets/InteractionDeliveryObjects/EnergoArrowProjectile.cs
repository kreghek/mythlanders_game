using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.InteractionDeliveryObjects
{
    internal sealed class EnergoArrowProjectile : ProjectileBase
    {
        private const double LIFETIME_DURATION_SECONDS = 0.3;
        private const int FPS = 8 * 3;
        
        private readonly ParticleSystem _tailParticleSystem;

        public EnergoArrowProjectile(Vector2 startPosition,
            Vector2 endPosition,
            GameObjectContentStorage contentStorage,
            AnimationBlocker? blocker,
            ICombatUnit? targetCombatUnit = null,
            Action<ICombatUnit>? interaction = null,
            double lifetimeDuration = LIFETIME_DURATION_SECONDS):
            base(startPosition, endPosition, contentStorage.GetBulletGraphics(), CreateFrameSet(), lifetimeDuration, blocker, targetCombatUnit, interaction)
        {
            var particleGenerator = new TailParticleGenerator(new[] { contentStorage.GetParticlesTexture() });
            _tailParticleSystem = new ParticleSystem(startPosition, particleGenerator);
        }

        private static IAnimationFrameSet CreateFrameSet()
        {
            return AnimationFrameSetFactory.CreateSequential(0, frameCount: 4, speedMultiplicator: FPS, frameWidth: 64, frameHeight: 32, textureColumns: SfxSpriteConsts.Size64x32.COL_COUNT, isLoop: true);
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
    }
}