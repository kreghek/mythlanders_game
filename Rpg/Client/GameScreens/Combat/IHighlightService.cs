using System.Collections.Generic;

using Client.Engine;

namespace Client.GameScreens.Combat;

internal interface IHighlightService
{
    void AddTargets(IReadOnlyCollection<IActorAnimator> animators);
    void ClearTargets();
    ICombatSceneContext CreateContext();
}
