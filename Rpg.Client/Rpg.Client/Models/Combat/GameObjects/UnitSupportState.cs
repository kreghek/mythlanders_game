using Microsoft.Xna.Framework;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.GameObjects
{
    internal class UnitSupportState : IUnitStateEngine
    {
        private readonly AnimationBlocker _blocker;
        private readonly UnitGraphics _graphics;
        private readonly IUnitStateEngine[] _subStates;

        private int _subStateIndex;

        public UnitSupportState(UnitGraphics graphics, SpriteContainer graphicsRoot, SpriteContainer targetGraphicsRoot,
            AnimationBlocker blocker, HealInteraction healInteraction)
        {
            _graphics = graphics;
            _blocker = blocker;

            var targetPosition =
                targetGraphicsRoot.Position + new Vector2(-100 * (targetGraphicsRoot.FlipX ? 1 : -1), 0);
            _subStates = new IUnitStateEngine[]
            {
                new MoveToTarget(_graphics, graphicsRoot, targetPosition),
                new HealState(_graphics, healInteraction),
                new MoveBack(_graphics, graphicsRoot, targetPosition, _blocker)
            };
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