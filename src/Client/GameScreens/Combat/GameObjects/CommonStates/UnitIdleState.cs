using System;

using Client.Core;
using Client.Engine;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat.GameObjects.CommonStates;

internal class UnitIdleState : IActorVisualizationState
{
    public UnitIdleState(UnitGraphicsBase unitGraphics, CombatUnitState state)
    {
        var animationSid = GetAnimationSidByCombatUnitState(state);

        unitGraphics.PlayAnimation(animationSid);
    }

    private static PredefinedAnimationSid GetAnimationSidByCombatUnitState(CombatUnitState state)
    {
        return state switch
        {
            CombatUnitState.Defense => PredefinedAnimationSid.DefenseStance,
            CombatUnitState.Idle => PredefinedAnimationSid.Idle,
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

    public event EventHandler? Completed;
}