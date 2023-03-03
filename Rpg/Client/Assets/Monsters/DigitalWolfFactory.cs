using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs.Monsters;
using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.Skills.Monster;
using Rpg.Client.Core;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class DigitalWolfFactory : IMonsterFactory
    {
        public UnitName ClassName => UnitName.DigitalWolf;

        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                Name = UnitName.DigitalWolf,
                LocationSids = new[]
                {
                    LocationSid.Thicket, LocationSid.Battleground, LocationSid.DestroyedVillage,
                    LocationSid.Swamp
                },
                IsMonster = true,

                Levels = new IUnitLevelScheme[]
                {
                    new AddSkillUnitLevel<DigitalBiteSkill>(1),
                    new AddPerkUnitLevel<CriticalHit>(3)
                },

                UnitGraphicsConfig = new DigitalWolfGraphicsConfig()
            };
        }

        public UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
        {
            return new DigitalWolfGraphicsConfig();
        }
    }
}