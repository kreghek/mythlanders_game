using Client.Assets;
using Client.Assets.GraphicConfigs.Monsters;
using Client.Core;

using JetBrains.Annotations;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class ChaserFactory : MonsterFactoryBase
    {
        public override UnitName ClassName => UnitName.Chaser;

        public override CharacterCultureSid Culture => CharacterCultureSid.Egyptian;

        public override UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                Name = UnitName.Chaser,
                LocationSids = new[]
                {
                    LocationSids.Desert
                },
                IsMonster = true,

                Levels = new IUnitLevelScheme[]
                {
                }
            };
        }
    }
}