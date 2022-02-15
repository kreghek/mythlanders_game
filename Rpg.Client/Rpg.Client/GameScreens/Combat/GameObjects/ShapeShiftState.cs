using Microsoft.Xna.Framework;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.GameObjects
{
    internal class ShapeShiftState : IUnitStateEngine
    {
        private double _counter;
        private readonly UnitGraphics _graphics;
        private readonly AnimationBlocker _shapeShiftBlocker;
        private readonly double _duration;

        public ShapeShiftState(UnitGraphics graphics, AnimationBlocker shapeShiftBlocker)
        {
            _graphics = graphics;
            _shapeShiftBlocker = shapeShiftBlocker;
            _duration = graphics.GetAnimationInfo(AnimationSid.ShapeShift).GetDuration();
        }

        /// <inheritdoc />
        /// <remarks> The state engine has no blockers. So we can't remove it with no aftermaths. </remarks>
        public bool CanBeReplaced => false;

        /// <summary>
        /// This engine is infinite.
        /// </summary>
        public bool IsComplete { get; private set; }

        public void Cancel()
        {
            _shapeShiftBlocker.Release();
        }

        public void Update(GameTime gameTime)
        {
            if (_counter == 0)
            {
                _graphics.PlayAnimation(AnimationSid.ShapeShift);
            }

            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_counter > _duration)
            {
                _graphics.IsDamaged = false;
                IsComplete = true;
                _shapeShiftBlocker.Release();
            }
        }
    }
}