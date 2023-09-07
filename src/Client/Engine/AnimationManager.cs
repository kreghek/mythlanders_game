using System.Collections.Generic;

namespace Client.Engine;

internal class AnimationManager : IAnimationManager
{
    private readonly IList<IAnimationBlocker> _blockers = new List<IAnimationBlocker>();

    public bool HasBlockers => _blockers.Count > 0;

    public void RegisterBlocker(IAnimationBlocker blocker)
    {
        _blockers.Add(blocker);
        blocker.Released += (_, _) =>
        {
            _blockers.Remove(blocker);
        };
    }

    public void DropBlockers()
    {
        _blockers.Clear();
    }
}