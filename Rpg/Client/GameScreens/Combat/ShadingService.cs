using System.Collections.Generic;

using Client.Engine;

namespace Client.GameScreens.Combat;

/// <summary>
/// Base implementation.
/// </summary>
internal sealed class ShadingService : IShadeService
{
    private readonly List<IActorAnimator> _currentActors = new List<IActorAnimator>();

    /// <inheritdoc/>
    public void AddTargets(IReadOnlyCollection<IActorAnimator> animators)
    {
        _currentActors.AddRange(animators);
    }

    /// <inheritdoc/>
    public void DropTargets()
    {
        _currentActors.Clear();
    }

    /// <inheritdoc/>
    public ICombatShadeContext CreateContext()
    {
        return new CombatShadeContext(_currentActors);
    }
}
