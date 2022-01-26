using System;

namespace Rpg.Client.Core.Equipments
{
    internal sealed class BookOfHerbs : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.BookOfHerbs;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        float IEquipmentScheme.GetHitPointsMultiplier(int level)
        {
            return 1 + level * 0.05f;
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Herbalist;
    }
}