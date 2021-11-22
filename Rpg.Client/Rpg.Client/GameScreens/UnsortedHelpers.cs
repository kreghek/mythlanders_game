using System.Diagnostics;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens
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

        public static Rectangle GetUnitPortraitRect(UnitName speaker)
        {
            return speaker switch
            {
                UnitName.Hq => new Rectangle(0, 0, 32, 32),
                UnitName.Berimir => new Rectangle(32, 0, 32, 32),
                UnitName.Hawk => new Rectangle(0, 32, 32, 32),
                UnitName.Oldman => new Rectangle(32, 32, 32, 32),
                UnitName.GuardianWoman => new Rectangle(32, 64, 32, 32),
                _ => Rectangle.Empty
            };
        }
    }
}