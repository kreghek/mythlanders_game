using System.Collections.Generic;

namespace Rpg.Client.Engine
{
    internal class AnimationManager
    {
        private readonly IList<AnimationBlocker> _blockers = new List<AnimationBlocker>();

        public void AddBlocker(AnimationBlocker blocker)
        {
            _blockers.Add(blocker);
            blocker.Released += (s, e) => { _blockers.Remove(blocker); };
        }

        public bool HasBlockers => _blockers.Count > 0;

        internal void DropBlockers()
        {
            _blockers.Clear();
        }
    }
}
