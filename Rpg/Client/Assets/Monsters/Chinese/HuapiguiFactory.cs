using Client.Assets;
using Client.Assets.GraphicConfigs.Monsters;
using Client.Core;
using Client.GameScreens;

using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs.Monsters;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class HuapiguiFactory : MonsterFactoryBase
    {
        public override UnitName ClassName => UnitName.Huapigui;

        public override CharacterCultureSid Culture => CharacterCultureSid.Chinese;

        public override UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                Name = UnitName.Huapigui,
                LocationSids = new[]
                {
                    LocationSids.Monastery
                },
                IsMonster = true,

                Levels = new IUnitLevelScheme[]
                {
                }
            };
        }

        public override UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
        {
            return new GenericMonsterGraphicsConfig(ClassName, Culture);
        }
    }
}