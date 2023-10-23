using System;

using Client.Core;
using Client.Engine;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat.GameObjects.CommonStates;

internal class IdleActorVisualizationState : IActorVisualizationState
{
    public IdleActorVisualizationState(UnitGraphicsBase unitGraphics, CombatantVisualIdleState state)
    {
        var animationSid = GetAnimationSidByCombatantState(state);

        unitGraphics.PlayAnimation(animationSid);
    }

    private static PredefinedAnimationSid GetAnimationSidByCombatantState(CombatantVisualIdleState state)
    {
        return state switch
        {
            CombatantVisualIdleState.DefenseStance => PredefinedAnimationSid.DefenseStance,
            CombatantVisualIdleState.Idle => PredefinedAnimationSid.Idle,
            _ => throw new InvalidOperationException()
        };
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
        // Looped idle animation was started in constructor.
    }
}