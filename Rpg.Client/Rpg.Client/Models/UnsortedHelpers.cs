using System.Diagnostics;

using Rpg.Client.Core;

namespace Rpg.Client.Models
{
    internal static class UnsortedHelpers
    {
        public static UnitScheme? GetPlayerPersonSchemeByEquipmentType(EquipmentItemType? equipmentItemType)
        {
            if (equipmentItemType is null)
            {
                return null;
            }

            switch (equipmentItemType)
            {
                case EquipmentItemType.Warrior: return UnitSchemeCatalog.SwordmanHero;
                case EquipmentItemType.Archer: return UnitSchemeCatalog.ArcherHero;
                case EquipmentItemType.Herbalist: return UnitSchemeCatalog.HerbalistHero;
                case EquipmentItemType.Priest: return UnitSchemeCatalog.PriestHero;
                case EquipmentItemType.Undefined:
                default:
                    Debug.Fail($"Unknown resource type {equipmentItemType}.");
                    return null;
            }
        }
    }
}