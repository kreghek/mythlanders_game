using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;

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

        public static Rectangle GetIconRect(SkillSid sid, SkillVisualization visualization)
        {
            const int SPRITE_SHEET_COLUMN_COUNT = 6;
            const int ICON_SIZE = 64;

            var iconIndexNullable = GetIconOneBasedIndex(sid, visualization);

            Debug.Assert(iconIndexNullable is not null,
                $"Don't forget add combat power in {nameof(GetIconOneBasedIndex)}");

            var iconIndex = iconIndexNullable.GetValueOrDefault() - 1;

            var x = iconIndex % SPRITE_SHEET_COLUMN_COUNT;
            var y = iconIndex / SPRITE_SHEET_COLUMN_COUNT;
            var rect = new Rectangle(x * ICON_SIZE, y * ICON_SIZE, ICON_SIZE, ICON_SIZE);

            return rect;
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
                case EquipmentItemType.Archer: return unitSchemeCatalog.Heroes[UnitName.Archer];
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

        private static int? GetIconOneBasedIndex(SkillSid sid, SkillVisualization skillVisualization)
        {
            if (skillVisualization.IconOneBasedIndex > 0)
            {
                return skillVisualization.IconOneBasedIndex;
            }

            return sid switch
            {
                SkillSid.SuppressiveFire => 16,
                SkillSid.BlankShot => 16,
                SkillSid.DieBySword => 1,
                SkillSid.InspiringRush => 1,
                SkillSid.SwordSlashRandom => 1,
                SkillSid.SwordSlashDefensive => 1,
                SkillSid.SwordSlashInaccurate => 1,
                SkillSid.WideSwordSlash => 2,
                SkillSid.GroupProtection => 15,
                SkillSid.DefenseStance => 3,
                SkillSid.BlindDefense => 3,
                SkillSid.SvarogBlastFurnace => 4,

                SkillSid.EnergyShot => 5,
                SkillSid.RapidShot => 6,
                SkillSid.ArrowRain => 7,
                SkillSid.ZduhachMight => 8,

                SkillSid.HealingSalve => 9,
                SkillSid.ToxicGas => 10,
                SkillSid.DopeHerb => 11,
                SkillSid.MassHeal => 12,

                SkillSid.Heal => 9,
                SkillSid.StaffHit => 12,
                SkillSid.RestoreMantra => 13,
                SkillSid.PathOf1000Firsts => 14,
                SkillSid.MasterStaffHit => 17,
                SkillSid.GodNature => 21,

                SkillSid.DarkLighting => 9,
                SkillSid.ParalyticChoir => 10,
                SkillSid.FingerOfAnubis => 11,

                SkillSid.PowerUp => 1,

                SkillSid.PenetrationStrike => 18,
                SkillSid.StonePath => 19,
                SkillSid.DemonicTaunt => 20,

                _ => null
            };
        }

        private static int GetUnitPortraitOneBasedIndex(UnitName unitName)
        {
            return unitName switch
            {
                UnitName.Hq => 1,
                UnitName.Swordsman => 2,
                UnitName.Archer => 3,
                UnitName.Herbalist => 4,
                UnitName.Monk => 5,
                UnitName.Oldman => 6,
                UnitName.Aspid => 7,
                UnitName.DigitalWolf => 8,
                UnitName.Bear => 9,
                UnitName.Wisp => 10,
                UnitName.Volkolak or UnitName.VolkolakWarrior => 11,
                UnitName.Comissar => 13,
                UnitName.Assaulter => 14,
                _ => 12
            };
        }
    }
}