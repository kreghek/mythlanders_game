using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Assets.InteractionDeliveryObjects;
using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Assets.States.HeroSpecific.Primitives
{
    internal sealed class SvarogSymbolExplosionState : IUnitStateEngine
    {
        private readonly SoundEffectInstance _explosionSoundEffect;
        private readonly UnitGraphics _graphics;
        private readonly AnimationBlocker? _symbolAnimationBlocker;
        private bool _started;

        public SvarogSymbolExplosionState(UnitGraphics graphics,
            AnimationBlocker symbolAnimationBlocker, SoundEffectInstance explosionSoundEffect,
            SoundEffectInstance fireDamageEffect, SvarogSymbolObject svarogSymbolObject)
        {
            _graphics = graphics;
            _symbolAnimationBlocker = symbolAnimationBlocker;
            _explosionSoundEffect = explosionSoundEffect;

            svarogSymbolObject.InteractionPerformed += (_, _) =>
            {
                fireDamageEffect.Play();
            };

            _symbolAnimationBlocker.Released += (_, _) => { IsComplete = true; };
        }

        public bool CanBeReplaced => false;
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            if (_symbolAnimationBlocker is not null)
            {
                _symbolAnimationBlocker.Release();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!_started)
            {
                _graphics.PlayAnimation(PredefinedAnimationSid.Skill1);

                _explosionSoundEffect.Play();

                _started = true;
            }
        }
    }
}