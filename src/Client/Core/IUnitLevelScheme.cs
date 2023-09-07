using Client.Core.Heroes;

namespace Client.Core;

internal interface IUnitLevelScheme
{
    int Level { get; }
    void Apply(Hero unit);
}