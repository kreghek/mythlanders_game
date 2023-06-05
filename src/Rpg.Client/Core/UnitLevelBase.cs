namespace Rpg.Client.Core
{
    internal abstract class UnitLevelBase : IUnitLevelScheme
    {
        protected UnitLevelBase(int level)
        {
            Level = level;
        }

        public abstract void Apply(Unit unit);

        public int Level { get; }
    }
}