using System.Collections.Generic;

using Client.Engine;

namespace Client.GameScreens.Combat;

/// <summary>
/// Factory of shade context.
/// </summary>
internal interface IShadeService
{
    /// <summary>
    /// Add focused targets to current targets.
    /// </summary>
    void AddTargets(IReadOnlyCollection<IActorAnimator> animators);

    /// <summary>
    /// Drop sshhading state.
    /// </summary>
    void DropTargets();

    /// <summary>
    /// Create instance of shading context.
    /// </summary>
    ICombatShadeContext CreateContext();
}
