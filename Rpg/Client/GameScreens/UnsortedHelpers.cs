using System;
using System.Collections.Generic;
using System.Diagnostics;

using Client.Assets.CombatMovements;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens
{
    internal static class UnsortedHelpers
    {
        public static IReadOnlyCollection<Tuple<UnitName, GlobeRule>> GetCharacterDisablingMap()
        {
            return new[]
            {
                new Tuple<UnitName, GlobeRule>(UnitName.Swordsman, GlobeRule.DisableBerimir)
            };
        }

        public static IReadOnlyList<float> GetCombatSequenceXpBonuses()
        {
            return new[] { 1f, 1f, 1.25f, 1.25f, 1.5f };
        }

        public static EquipmentItemType? GetEquipmentItemTypeByUnitScheme(UnitScheme unitScheme)
        {
            switch (unitScheme.Name)
            {
                case UnitName.Swordsman:
                    return EquipmentItemType.Warrior;
                default:
                    return null;
            }
        }

        public static Rectangle GetIconRect(int iconIndex)
        {
            const int SPRITE_SHEET_COLUMN_COUNT = 6;
            const int ICON_SIZE = 64;

            var x = iconIndex % SPRITE_SHEET_COLUMN_COUNT;
            var y = iconIndex / SPRITE_SHEET_COLUMN_COUNT;
            var rect = new Rectangle(x * ICON_SIZE, y * ICON_SIZE, ICON_SIZE, ICON_SIZE);

            return rect;
        }

        public static Rectangle GetIconRect(CombatMovementIcon icon)
        {
            const int ICON_SIZE = 64;
            return new Rectangle(icon.X * ICON_SIZE, icon.Y * ICON_SIZE, ICON_SIZE, ICON_SIZE);
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
                case EquipmentItemType.Warrior: return unitSchemeCatalog.Heroes[UnitName.Swordsman];
                case EquipmentItemType.Archer: return unitSchemeCatalog.Heroes[UnitName.Robber];
                case EquipmentItemType.Herbalist: return unitSchemeCatalog.Heroes[UnitName.Herbalist];
                case EquipmentItemType.Priest: return unitSchemeCatalog.Heroes[UnitName.Priest];
                default:
                    Debug.Fail($"Unknown resource type {equipmentItemType}.");
                    return null;
            }
        }

        public static Rectangle GetUnitPortraitRect(UnitName unitName)
        {
            const int SIZE = 32;
            const int COLUMN_COUNT = 4;

            var index = GetUnitPortraitOneBasedIndex(unitName);

            var indexZeroBased = index - 1;
            var x = indexZeroBased % COLUMN_COUNT;
            var y = indexZeroBased / COLUMN_COUNT;

            return new Rectangle(x * SIZE, y * SIZE, SIZE, SIZE);
        }
        //
        // private static int? GetIconOneBasedIndex(SkillSid sid, SkillVisualization skillVisualization)
        // {
        //     if (skillVisualization.IconOneBasedIndex > 0)
        //     {
        //         return skillVisualization.IconOneBasedIndex;
        //     }
        //
        //     return sid switch
        //     {
        //         SkillSid.SuppressiveFire => 16,
        //         SkillSid.BlankShot => 16,
        //         SkillSid.DieBySword => 1,
        //         SkillSid.InspiringRush => 1,
        //         SkillSid.SwordSlashRandom => 1,
        //         SkillSid.SwordSlashDefensive => 1,
        //         SkillSid.SwordSlashInaccurate => 1,
        //         SkillSid.WideSwordSlash => 2,
        //         SkillSid.GroupProtection => 15,
        //         SkillSid.DefenseStance => 3,
        //         SkillSid.BlindDefense => 3,
        //         SkillSid.SvarogBlastFurnace => 4,
        //
        //         SkillSid.EnergyShot => 5,
        //         SkillSid.RapidShot => 6,
        //         SkillSid.ArrowRain => 7,
        //         SkillSid.ZduhachMight => 8,
        //
        //         SkillSid.HealingSalve => 9,
        //         SkillSid.ToxicGasBomb => 10,
        //         SkillSid.DopeHerb => 11,
        //         SkillSid.MassHeal => 12,
        //
        //         SkillSid.Heal => 9,
        //         SkillSid.StaffHit => 12,
        //         SkillSid.RestoreMantra => 13,
        //         SkillSid.PathOf1000Firsts => 14,
        //         SkillSid.MasterStaffHit => 17,
        //         SkillSid.GodNature => 21,
        //
        //         SkillSid.DarkLighting => 9,
        //         SkillSid.ParalyticChoir => 10,
        //         SkillSid.FingerOfAnubis => 11,
        //
        //         SkillSid.PowerUp => 1,
        //
        //         SkillSid.PenetrationStrike => 18,
        //         SkillSid.StonePath => 19,
        //         SkillSid.DemonicTaunt => 20,
        //
        //         _ => null
        //     };
        // }

        private static int GetUnitPortraitOneBasedIndex(UnitName unitName)
        {
            return unitName switch
            {
                UnitName.Hq => 1,
                UnitName.Swordsman => 2,
                UnitName.Robber => 3,
                UnitName.Herbalist => 4,
                UnitName.Monk => 5,
                UnitName.Oldman => 6,
                UnitName.Aspid => 7,
                UnitName.DigitalWolf => 8,
                UnitName.Bear => 9,
                UnitName.Wisp => 10,
                UnitName.Volkolak or UnitName.VolkolakWarrior => 11,
                UnitName.Partisan => 13,
                UnitName.Assaulter => 14,
                _ => 12
            };
        }
    }
}