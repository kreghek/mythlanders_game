using Client.Assets;

using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs.Monsters;
using Rpg.Client.Assets.Skills.Monster;
using Rpg.Client.Core;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class BlackTrooperFactory : IMonsterFactory
    {
        public UnitName ClassName => UnitName.BlackTrooper;

        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                TankRank = 0.25f,
                DamageDealerRank = 0.75f,
                SupportRank = 0.0f,
                Resolve = 9,

                Name = UnitName.BlackTrooper,
                LocationSids = new[]
                {
                    LocationSid.Thicket, LocationSid.Swamp, LocationSid.Battleground, LocationSid.DeathPath,
                    LocationSid.Mines,

                    LocationSid.Desert, LocationSid.SacredPlace,

                    LocationSid.ShipGraveyard,

                    LocationSid.Monastery
                },
                IsMonster = true,

                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel<BlackRifleShotSkill>(1)
                },

                UnitGraphicsConfig = new BlackTrooperGraphicsConfig()
            };
        }

        public UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
        {
            return new BlackTrooperGraphicsConfig();
        }
    }
}