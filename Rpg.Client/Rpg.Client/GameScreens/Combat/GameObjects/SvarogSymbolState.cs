using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.GameObjects
{
    internal sealed class SvarogSymbolState : IUnitStateEngine
    {
        private const double DURATION = 2.5f;
        private readonly AnimationBlocker? _animationBlocker;
        private readonly UnitGraphics _graphics;
        private readonly SoundEffectInstance? _symbolAppearingSoundEffect;
        private double _counter;

        private SvarogSymbolState(UnitGraphics graphics)
        {
            _graphics = graphics;
        }

        public SvarogSymbolState(UnitGraphics graphics, AnimationBlocker animationBlocker,
            SoundEffectInstance symbolAppearingSoundEffect) :
            this(graphics)
        {
            _animationBlocker = animationBlocker;
            _symbolAppearingSoundEffect = symbolAppearingSoundEffect;
        }

        public bool CanBeReplaced => true;
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            if (_animationBlocker is not null)
            {
                _animationBlocker.Release();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_counter == 0)
            {
                _graphics.PlayAnimation(AnimationSid.Ult);
                _symbolAppearingSoundEffect?.Play();
            }

            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_counter > DURATION)
            {
                IsComplete = true;
            }
        }
    }
}