using System.Collections.Generic;

namespace Rpg.Client.Engine
{
    public class AnimationManager
    {
        private readonly IList<AnimationBlocker> _blockers = new List<AnimationBlocker>();

        public bool HasBlockers => _blockers.Count > 0;

        public void AddBlocker(AnimationBlocker blocker)
        {
            _blockers.Add(blocker);
            blocker.Released += (s, e) => { _blockers.Remove(blocker); };
        }

        internal void DropBlockers()
        {
            _blockers.Clear();
        }
    }
}