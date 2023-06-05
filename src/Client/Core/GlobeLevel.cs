using System;

namespace Rpg.Client.Core
{
    internal sealed class GlobeLevel
    {
        public int Level { get; set; }

        public int MonsterLevel => (int)Math.Log(Level + 1, 32) + 1;
    }
}