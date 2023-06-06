using Client.Assets;
using Client.Assets.Monsters;
using Client.Core;
using Client.GameScreens;

using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs.Monsters;

namespace Client.Assets.Monsters.Slavic;

[UsedImplicitly]
internal sealed class VolkolakWarriorFactory : IMonsterFactory
{
    public UnitName ClassName => UnitName.VolkolakWarrior;

    public UnitScheme Create(IBalanceTable balanceTable)
    {
        return new UnitScheme(balanceTable.GetCommonUnitBasics())
        {
            TankRank = 0.5f,
            DamageDealerRank = 0.5f,
            SupportRank = 0.0f,

            Name = UnitName.VolkolakWarrior,
            LocationSids = new[] { LocationSids.Swamp },
            IsUnique = true,
            IsMonster = true,
            MinRequiredBiomeLevel = 5,

            SchemeAutoTransition = new UnitSchemeAutoTransition
            {
                HpShare = 0.5f,
                NextScheme = new UnitScheme(balanceTable.GetCommonUnitBasics())
                {
                    TankRank = 0.5f,
                    DamageDealerRank = 0.5f,
                    SupportRank = 0.0f,

                    Name = UnitName.Volkolak,
                    IsUnique = true,
                    IsMonster = true
                }
            }
        };
    }

    public UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
    {
        return new VolkolakWarriorGraphicsConfig(ClassName);
    }
}