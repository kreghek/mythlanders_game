﻿using Rpg.Client.Core;
using Rpg.Client.Core.GraphicConfigs;

namespace Rpg.Client.Assets.Heroes
{
    internal abstract class HeroFactoryBase : IHeroFactory
    {
        protected abstract IEquipmentScheme[] GetEquipment();

        protected virtual UnitGraphicsConfigBase GetGraphicsConfig()
        {
            return new GenericCharacterGraphicsConfig();
        }

        protected abstract IUnitLevelScheme[] GetLevels();
        public abstract UnitName HeroName { get; }

        public UnitScheme Create(IBalanceTable balanceTable)
        {
            var record = balanceTable.GetRecord(HeroName);

            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                TankRank = record.TankRank,
                DamageDealerRank = record.DamageDealerRank,
                SupportRank = record.SupportRank,

                Name = HeroName,

                Levels = GetLevels(),

                Equipments = GetEquipment(),

                UnitGraphicsConfig = GetGraphicsConfig()
            };
        }
    }
}