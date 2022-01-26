using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens
{
    internal static class UnsortedHelpers
    {
        public static bool CheckIsDisabled(UnitName name, GlobalUnitEffect effect)
        {
            var mapping = GetCharacterDisablingMap();

            foreach (var tuple in mapping)
            {
                if (name == tuple.Item1 && effect.Source.IsActive &&
                    effect.Source.GetRules().Contains(tuple.Item2))
                {
                    return true;
                }
            }

            return false;
        }

        public static IReadOnlyCollection<Tuple<UnitName, GlobeRule>> GetCharacterDisablingMap()
        {
            return new[]
            {
                new Tuple<UnitName, GlobeRule>(UnitName.Berimir, GlobeRule.DisableBerimir)
            };
        }

        public static IReadOnlyList<float> GetCombatSequenceXpBonuses()
        {
            return new[] { 1f, 0 /*not used*/, 1.25f, /*not used*/0, 1.5f };
        }

        public static EquipmentItemType? GetEquipmentItemTypeByUnitScheme(UnitScheme unitScheme)
        {
            switch (unitScheme.Name)
            {
                case UnitName.Berimir:
                    return EquipmentItemType.Warrior;
                default:
                    return null;
            }
        }

        public static UnitScheme? GetPlayerPersonSchemeByEquipmentType(IUnitSchemeCatalog unitSchemeCatalog,
            EquipmentItemType? equipmentItemType)
        {
            if (equipmentItemType is null)
            {
                return null;
            }

            switch (equipmentItemType)
            {
                case EquipmentItemType.Warrior: return unitSchemeCatalog.Heroes[UnitName.Berimir];
                case EquipmentItemType.Archer: return unitSchemeCatalog.Heroes[UnitName.Hawk];
                case EquipmentItemType.Herbalist: return unitSchemeCatalog.Heroes[UnitName.Rada];
                case EquipmentItemType.Priest: return unitSchemeCatalog.Heroes[UnitName.Kakhotep];
                default:
                    Debug.Fail($"Unknown resource type {equipmentItemType}.");
                    return null;
            }
        }

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
                UnitName.Rada => 4,
                UnitName.Maosin => 5,
                UnitName.Oldman => 6,
                UnitName.Aspid => 7,
                UnitName.GreyWolf => 8,
                UnitName.Bear => 9,
                UnitName.Wisp => 10,
                UnitName.Volkolak or UnitName.VolkolakWarrior => 11,
                _ => 12
            };
        }
    }
}