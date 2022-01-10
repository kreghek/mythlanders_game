namespace Rpg.Client.Core
{
    internal interface IUnitLevelScheme
    {
        void Apply(Unit unit);
        int Level { get; }
    }
}