using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal sealed class DeathState : IUnitStateEngine
    {
        private readonly SoundEffectInstance _deathSoundEffect;
        private readonly UnitGraphics _graphics;
        private double _counter;

        public DeathState(UnitGraphics graphics, SoundEffectInstance deathSoundEffect)
        {
            _graphics = graphics;
            _deathSoundEffect = deathSoundEffect;
        }

        public bool CanBeReplaced { get; }
        public bool IsComplete { get; }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            if (_counter == 0)
            {
                _deathSoundEffect.Play();
                _graphics.IsDamaged = true;
                _graphics.PlayAnimation("Death");
            }

            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_counter > 0.05)
            {
                _graphics.IsDamaged = false;
            }

            // Infinite
        }
    }
}