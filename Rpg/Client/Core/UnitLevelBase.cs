using Client.Core.Heroes;

namespace Rpg.Client.Core
{
    internal abstract class UnitLevelBase : IUnitLevelScheme
    {
        protected UnitLevelBase(int level)
        {
            Level = level;
        }

        public abstract void Apply(Hero unit);

        public int Level { get; }
    }
}