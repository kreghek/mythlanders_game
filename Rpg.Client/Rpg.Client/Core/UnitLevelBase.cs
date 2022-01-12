namespace Rpg.Client.Core
{
    internal abstract class UnitLevelBase : IUnitLevelScheme
    {
        public abstract void Apply(Unit unit);

        public int Level { get; }

        protected UnitLevelBase(int level)
        {
            Level = level;
        }
    }
}