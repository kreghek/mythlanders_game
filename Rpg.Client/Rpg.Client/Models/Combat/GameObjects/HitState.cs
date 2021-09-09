using System;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal sealed class HitState : IUnitStateEngine
    {
        private const double DURATION = 1;
        private readonly AttackInteraction _attackInteraction;
        private readonly AnimationBlocker? _animationBlocker;
        private readonly UnitGraphics _graphics;

        private double _counter;

        private bool _interactionExecuted;

        public HitState(UnitGraphics graphics, AttackInteraction attackInteraction)
        {
            _graphics = graphics;
            _attackInteraction = attackInteraction;
        }

        public HitState(UnitGraphics graphics, AttackInteraction attackInteraction, AnimationBlocker animationBlocker): this(graphics, attackInteraction)
        {
            _animationBlocker = animationBlocker;
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
                _graphics.PlayAnimation("Hit");
            }

            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_counter > DURATION)
            {
                IsComplete = true;

                if (_animationBlocker is not null)
                {
                    _animationBlocker.Release();
                }
            }
            else if (_counter > DURATION / 2)
            {
                if (!_interactionExecuted)
                {
                    _attackInteraction.Execute();

                    _interactionExecuted = true;
                }
            }
        }
    }

    internal sealed class WoundState : IUnitStateEngine
    {
        private readonly UnitGraphics _graphics;
        private double _counter;

        public WoundState(UnitGraphics graphics)
        {
            _graphics = graphics;
        }

        public bool CanBeReplaced { get; }
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            if (_counter == 0)
            {
                _graphics.PlayAnimation("Wound");
            }

            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_counter > 1)
            {
                IsComplete = true;
            }
        }
    }

    internal sealed class DeathState : IUnitStateEngine
    {
        private readonly UnitGraphics _graphics;
        private double _counter;

        public DeathState(UnitGraphics graphics)
        {
            _graphics = graphics;
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
                _graphics.PlayAnimation("Death");
            }

            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            // Infinite
        }
    }
}