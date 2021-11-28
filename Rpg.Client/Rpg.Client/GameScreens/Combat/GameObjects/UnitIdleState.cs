using Microsoft.Xna.Framework;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.Combat.GameObjects
{
    internal class UnitIdleState : IUnitStateEngine
    {
        public UnitIdleState(UnitGraphics unitGraphics, CombatUnitState state)
        {
            if (state == CombatUnitState.Defense)
            {
                unitGraphics.PlayAnimation(AnimationSid.Defense);
            }
            else
            {
                unitGraphics.PlayAnimation(AnimationSid.Idle);
            }
        }

        /// <inheritdoc />
        /// <remarks> The state engine has no blockers. So we can't remove it with no aftermaths. </remarks>
        public bool CanBeReplaced => true;

        /// <summary>
        /// This engine is infinite.
        /// </summary>
        public bool IsComplete => false;

        public void Cancel()
        {
            // There is no blockers. So do nothing.
        }

        public void Update(GameTime gameTime)
        {
            // TODO Run idle animation
        }
    }
}