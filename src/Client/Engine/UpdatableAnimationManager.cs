using System.Collections.Generic;
using System.Linq;

namespace Client.Engine;

internal sealed class UpdatableAnimationManager : IAnimationManager
{
    private readonly IAnimationManager _baseAnimationManager;
    private readonly IList<IUpdatableAnimationBlocker> _updatableBlocks;

    public UpdatableAnimationManager(IAnimationManager baseAnimationManager)
    {
        _baseAnimationManager = baseAnimationManager;

        _updatableBlocks = new List<IUpdatableAnimationBlocker>();
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
}