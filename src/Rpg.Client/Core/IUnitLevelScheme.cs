namespace Rpg.Client.Core
{
    internal interface IUnitLevelScheme
    {
        int Level { get; }
        void Apply(Unit unit);
    }
}