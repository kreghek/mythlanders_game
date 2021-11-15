using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal sealed class SvorogSymbolState : IUnitStateEngine
    {
        private const double DURATION = 1;
        private readonly AnimationBlocker? _animationBlocker;
        private readonly IList<IInteractionDelivery> _bulletList;
        private readonly UnitGraphics _graphics;
        private readonly SoundEffectInstance? _hitSound;
        private readonly int _index;
        private readonly ScreenShaker _screenShaker;
        private readonly IInteractionDelivery _interactionDelivery;
        private double _counter;

        private bool _interactionExecuted;

        public SvorogSymbolState(UnitGraphics graphics, IInteractionDelivery? interactionDelivery,
            IList<IInteractionDelivery> interactionDeliveryList,
            ScreenShaker screenShaker)
        {
            _graphics = graphics;
            _interactionDelivery = interactionDelivery;
            _bulletList = interactionDeliveryList;
            _screenShaker = screenShaker;
        }

        public SvorogSymbolState(UnitGraphics graphics, IInteractionDelivery? bulletGameObject,
            IList<IInteractionDelivery> interactionDeliveryList, AnimationBlocker animationBlocker,
            SoundEffectInstance? hitSound,
            int index,
            ScreenShaker screenShaker) :
            this(graphics, bulletGameObject, interactionDeliveryList, screenShaker)
        {
            _animationBlocker = animationBlocker;
            _hitSound = hitSound;
            _index = index;
        }

        public bool CanBeReplaced { get; }
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
                _graphics.PlayAnimation($"Skill{_index}");
                _screenShaker.Start(3, ShakeDirection.FadeOut);
            }

            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_counter > DURATION)
            {
                IsComplete = true;
            }
        }
    }
}