using Rpg.Client.Core;
using Rpg.Client.Core.GraphicConfigs;

namespace Rpg.Client.Assets.Heroes
{
    internal abstract class HeroFactoryBase : IHeroBuilder
    {
        public abstract UnitName UnitName { get; }

        public UnitScheme Create(IBalanceTable balanceTable)
        {
            var record = balanceTable.GetRecord(UnitName);

            return new()
            {
                TankRank = record.TankRank,
                DamageDealerRank = record.DamageDealerRank,
                SupportRank = record.SupportRank,

                Name = UnitName,

                Levels = GetLevels(),

                Equipments = GetEquipment(),

                UnitGraphicsConfig = GetGraphicsConfig()
            };
        }

        protected abstract IEquipmentScheme[] GetEquipment();

        protected abstract IUnitLevelScheme[] GetLevels();

        protected virtual UnitGraphicsConfigBase GetGraphicsConfig()
        {
            return new GenericCharacterGraphicsConfig();
        }
    }
}