using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens
{
    internal static class UnsortedHelpers
    {
        public static UnitScheme? GetPlayerPersonSchemeByEquipmentType(IUnitSchemeCatalog unitSchemeCatalog,
            EquipmentItemType? equipmentItemType)
        {
            if (equipmentItemType is null)
            {
                return null;
            }

            switch (equipmentItemType)
            {
                case EquipmentItemType.Warrior: return unitSchemeCatalog.PlayerUnits[UnitName.Berimir];
                case EquipmentItemType.Archer: return unitSchemeCatalog.PlayerUnits[UnitName.Hawk];
                case EquipmentItemType.Herbalist: return unitSchemeCatalog.PlayerUnits[UnitName.Rada];
                case EquipmentItemType.Priest: return unitSchemeCatalog.PlayerUnits[UnitName.Kakhotep];
                default:
                    Debug.Fail($"Unknown resource type {equipmentItemType}.");
                    return null;
            }
        }

        public static IReadOnlyList<float> GetCombatSequenceXpBonuses() =>
            new[] { 1f, 0 /*not used*/, 1.25f, /*not used*/0, 1.5f };

        public static Rectangle GetUnitPortraitRect(UnitName unitName)
        {
            const int SIZE = 32;
            const int COLUMN_COUNT = 3;

            var index = GetUnitPortraitOneBasedIndex(unitName);

            var indexZeroBased = index - 1;
            var x = indexZeroBased % COLUMN_COUNT;
            var y = indexZeroBased / COLUMN_COUNT;

            return new Rectangle(x * SIZE, y * SIZE, SIZE, SIZE);
        }

        private static int GetUnitPortraitOneBasedIndex(UnitName unitName)
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
                UnitName.Volkolak => 10,
                _ => 5
            };
        }
    }
}