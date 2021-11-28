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
                case EquipmentItemType.Warrior: return UnitSchemeCatalog.SwordsmanHero;
                case EquipmentItemType.Archer: return UnitSchemeCatalog.ArcherHero;
                case EquipmentItemType.Herbalist: return UnitSchemeCatalog.HerbalistHero;
                case EquipmentItemType.Priest: return UnitSchemeCatalog.PriestHero;
                case EquipmentItemType.Undefined:
                default:
                    Debug.Fail($"Unknown resource type {equipmentItemType}.");
                    return null;
            }
        }

        public static Rectangle GetUnitPortraitRect(UnitName unitName)
        {
            const int SIZE = 32;
            const int COLUMN_COUNT = 3;

            var index = GetUnitPortraitIndex(unitName);

            var indexZeroBased = index - 1;
            var x = indexZeroBased % COLUMN_COUNT;
            var y = indexZeroBased / COLUMN_COUNT;

            return new Rectangle(x * SIZE, y * SIZE, SIZE, SIZE);
        }

        private static int GetUnitPortraitIndex(UnitName unitName)
        { 
            return unitName switch
            {
                UnitName.Hq => 1,
                UnitName.Berimir => 2,
                UnitName.Hawk => 3,
                UnitName.Rada => 5,
                UnitName.Oldman => 4,
                UnitName.GuardianWoman => 5,
                UnitName.Aspid => 6,
                UnitName.GreyWolf => 7,
                UnitName.Bear => 8,
                UnitName.Wisp => 9,
                _ => 5
            };
        }
    }
}