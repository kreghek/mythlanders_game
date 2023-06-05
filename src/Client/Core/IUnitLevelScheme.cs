using Client.Core.Heroes;

namespace Rpg.Client.Core
{
    internal interface IUnitLevelScheme
    {
        int Level { get; }
        void Apply(Hero unit);
    }
}