using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal class UnitDistantAttackState : IUnitStateEngine
    {
        private readonly AnimationBlocker _blocker;
        private readonly IUnitStateEngine[] _subStates;

        private int _subStateIndex;

        public UnitDistantAttackState(UnitGraphics graphics, SpriteContainer graphicsRoot,
            SpriteContainer targetGraphicsRoot,
            AnimationBlocker blocker, Action attackInteraction,
            BulletGameObject? bullet, IList<BulletGameObject> bulletList, SoundEffectInstance hitSound, int index)
        {
            var targetPosition =
                targetGraphicsRoot.Position + new Vector2(-100 * (targetGraphicsRoot.FlipX ? 1 : -1), 0);
            _subStates = new IUnitStateEngine[]
            {
                new DistantHitState(graphics, attackInteraction, bullet, bulletList, blocker, hitSound, index)
            };
            _blocker = blocker;
        }

        public bool CanBeReplaced => false;
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            if (IsComplete)
            {
                return;
            }

            _blocker.Release();
        }

        public void Update(GameTime gameTime)
        {
            if (_subStateIndex < _subStates.Length)
            {
                var currentSubState = _subStates[_subStateIndex];
                if (currentSubState.IsComplete)
                {
                    _subStateIndex++;
                }
                else
                {
                    currentSubState.Update(gameTime);
                }
            }
            else
            {
                IsComplete = true;
            }
        }
    }
}