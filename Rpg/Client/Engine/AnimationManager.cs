using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Engine;

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

internal sealed class UpdatableAnimationManager : IAnimationManager
{
    private readonly IAnimationManager _baseAnimationManager;
    private readonly IList<IUpdatableAnimationBlocker> _updatableBlocks;

    public UpdatableAnimationManager(IAnimationManager baseAnimationManager)
    {
        _baseAnimationManager = baseAnimationManager;

        _updatableBlocks = new List<IUpdatableAnimationBlocker>();
    }

    public bool HasBlockers => _baseAnimationManager.HasBlockers;

    public void DropBlockers()
    {
        _updatableBlocks.Clear();
        _baseAnimationManager.DropBlockers();
    }

    public void RegisterBlocker(IAnimationBlocker blocker)
    {
        if (blocker is IUpdatableAnimationBlocker updatable)
        {
            _updatableBlocks.Add(updatable);
        }

        _baseAnimationManager.RegisterBlocker(blocker);
    }

    public void Update(double elapsedSeconds)
    {
        foreach (var item in _updatableBlocks.ToArray())
        {
            item.Update(elapsedSeconds);

            if (item.State == AnimationBlockerState.Released)
            {
                _updatableBlocks.Remove(item);
            }
        }
    }
}