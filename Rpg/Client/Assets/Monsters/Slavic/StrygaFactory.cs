using Client.Assets;
using Client.Assets.GraphicConfigs.Monsters;

using JetBrains.Annotations;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class StrygaFactory : MonsterFactoryBase
    {
        public override UnitName ClassName => UnitName.Stryga;

        public override CharacterCultureSid Culture => CharacterCultureSid.Slavic;

        public override UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                Name = UnitName.Stryga,
                LocationSids = new[]
                {
                    LocationSids.Pit, LocationSids.Swamp
                },
                IsMonster = true,

                Levels = new IUnitLevelScheme[]
                {
                }
            };
        }
    }
}