﻿using System.Collections.Generic;

namespace Rpg.Client.Engine
{
    internal class AnimationManager : IAnimationManager
    {
        private readonly IList<AnimationBlocker> _blockers = new List<AnimationBlocker>();

        private void AddBlocker(AnimationBlocker blocker)
        {
            _blockers.Add(blocker);
            blocker.Released += (_, _) =>
            {
                _blockers.Remove(blocker);
            };
        }

        public bool HasBlockers => _blockers.Count > 0;

        public AnimationBlocker CreateAndUseBlocker()
        {
            var blocker = new AnimationBlocker();

            AddBlocker(blocker);

            return blocker;
        }

        public void DropBlockers()
        {
            _blockers.Clear();
        }
    }
}