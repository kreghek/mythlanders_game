using System.Collections.Generic;

using Rpg.Client.Core.Effects;
using Rpg.Client.Core.Skills;
using Rpg.Client.Models.Combat.GameObjects;

namespace Rpg.Client.Core
{
    internal static class UnitSchemeCatalog
    {
        public static UnitScheme SwordmanHero = new()
        {
            Hp = 100,
            HpPerLevel = 10,
            Power = 11,
            PowerPerLevel = 2,
            
            Name = "Беримир",
            Skills = new SkillBase[]
            {
                new SlashSkill(),
                new WideSlashSkill()

                //new CombatSkill
                //{
                //    Sid = "Slash",
                //    DamageMin = 10,
                //    DamageMinPerLevel = 2,
                //    DamageMax = 12,
                //    DamageMaxPerLevel = 2,
                //    TargetType = SkillTarget.Enemy,
                //    Scope = SkillScope.Single,
                //    Range = CombatPowerRange.Melee
                //},
                //new CombatSkill
                //{
                //    Sid = "Wide Slash",
                //    DamageMin = 5,
                //    DamageMinPerLevel = 1,
                //    DamageMax = 7,
                //    DamageMaxPerLevel = 1,
                //    TargetType = SkillTarget.Enemy,
                //    Scope = SkillScope.AllEnemyGroup
                //}
            }
        };

        public static UnitScheme HerbalistHero = new()
        {
            Hp = 50,
            HpPerLevel = 6,
            Name = "Рада",
            Power = 6,
            PowerPerLevel = 1,
            Skills = new []
            {
                new HealSkill()
                //new CombatSkill
                //{
                //    Sid = "Heal",
                //    DamageMin = 5,
                //    DamageMinPerLevel = 1,
                //    DamageMax = 7,
                //    DamageMaxPerLevel = 1,
                //    TargetType = SkillTarget.Friendly,
                //    Scope = SkillScope.Single,
                //    Range = CombatPowerRange.Distant
                //}
            }
        };

        public static UnitScheme ArcherHero = new()
        {
            Hp = 50,
            HpPerLevel = 7,
            Name = "Соколинный глаз", // Hawkeye
            Power = 11,
            PowerPerLevel = 1,
            Skills = new SkillBase[]
            {
                new StrikeSkill(),
                new ArrowRainSkill()
                //new CombatSkill
                //{
                //    Sid = "Strike",
                //    DamageMin = 15,
                //    DamageMinPerLevel = 2,
                //    DamageMax = 17,
                //    DamageMaxPerLevel = 2,
                //    TargetType = SkillTarget.Enemy,
                //    Scope = SkillScope.Single,
                //    Range = CombatPowerRange.Distant
                //},
                //new CombatSkill
                //{
                //    Sid = "Arrow Rain",
                //    DamageMin = 5,
                //    DamageMinPerLevel = 1,
                //    DamageMax = 7,
                //    DamageMaxPerLevel = 1,
                //    TargetType = SkillTarget.Enemy,
                //    Scope = SkillScope.AllEnemyGroup,
                //    Range = CombatPowerRange.Distant
                //}
            }
        };

        //public static UnitScheme JuggernautHero = new()
        //{
        //    Hp = 150,
        //    HpPerLevel = 20,
        //    Name = "Smooky",
        //    Skills = new[]
        //    {
        //        new CombatSkill
        //        {
        //            Sid = "Player",
        //            DamageMin = 5,
        //            DamageMinPerLevel = 1,
        //            DamageMax = 7,
        //            DamageMaxPerLevel = 1,
        //            TargetType = SkillTarget.Enemy,
        //            Scope = SkillScope.AllEnemyGroup
        //        }
        //    }
        //};

        public static IEnumerable<UnitScheme> AllUnits = new[]
        {
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Grey Wolf",
                Biom = BiomeType.Slavic,
                NodeIndexes = new[] { 0, 1, 2 },
                Power = 2,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 25,
                HpPerLevel = 8,
                Name = "Bear",
                Biom = BiomeType.Slavic,
                NodeIndexes = new[] { 1, 2, 4 },
                IsUnique = true,
                Power = 1,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 15,
                HpPerLevel = 2,
                Name = "Wisp",
                Biom = BiomeType.Slavic,
                NodeIndexes = new[] { 2, 3, 5 },
                Power = 4,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 25,
                HpPerLevel = 9,
                Name = "Volkolak",
                Biom = BiomeType.Slavic,
                NodeIndexes = new[] { 2, 3, 5 },
                IsUnique = true,
                Power = 4,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 15,
                HpPerLevel = 3,
                Name = "Korgorush",
                Biom = BiomeType.Slavic,
                NodeIndexes = new[] { 6, 7, 8 },
                Power = 4,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 5,
                Name = "Stryga",
                Biom = BiomeType.Slavic,
                NodeIndexes = new[] { 6, 7, 8 },
                Power = 3,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 10,
                Name = "Vampire",
                Biom = BiomeType.Slavic,
                NodeIndexes = new[] { 6, 7, 8 },
                IsUnique = true,
                Power = 5,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Gaint frog",
                Biom = BiomeType.Slavic,
                NodeIndexes = new[] { 7, 8, 9 },
                Power = 3,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 30,
                HpPerLevel = 5,
                Name = "Basilisk",
                Biom = BiomeType.Slavic,
                NodeIndexes = new[] { 7, 8, 9 },
                IsUnique = true,
                Power = 4,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 1200,
                Name = "Zmey Gorynych",
                Biom = BiomeType.Slavic,

                IsBoss = true,
                Power = 13,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },

            // China
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Grey Wolf",
                Biom = BiomeType.China,
                NodeIndexes = new[] { 0, 1, 2 },
                Power = 3,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 25,
                HpPerLevel = 3,
                Name = "Bear",
                Biom = BiomeType.China,
                NodeIndexes = new[] { 1, 2, 4 },
                IsUnique = true,
                Power = 1,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 15,
                HpPerLevel = 3,
                Name = "Wisp",
                Biom = BiomeType.China,
                NodeIndexes = new[] { 2, 3, 5 },
                Power = 4,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 25,
                HpPerLevel = 3,
                Name = "Volkolak",
                Biom = BiomeType.China,
                NodeIndexes = new[] { 2, 3, 5 },
                IsUnique = true,
                Power = 4,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 15,
                HpPerLevel = 3,
                Name = "Korgorush",
                Biom = BiomeType.China,
                NodeIndexes = new[] { 6, 7, 8 },
                Power = 4,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Stryga",
                Biom = BiomeType.China,
                NodeIndexes = new[] { 6, 7, 8 },
                Power = 3,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Vampire",
                Biom = BiomeType.China,
                NodeIndexes = new[] { 6, 7, 8 },
                IsUnique = true,
                Power = 5,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Gaint frog",
                Biom = BiomeType.China,
                NodeIndexes = new[] { 7, 8, 9 },
                Power = 3,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 30,
                HpPerLevel = 3,
                Name = "Basilisk",
                Biom = BiomeType.China,
                NodeIndexes = new[] { 7, 8, 9 },
                IsUnique = true,
                Power = 4,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 1200,
                Name = "Dragon",
                Biom = BiomeType.China,

                IsBoss = true,
                Power = 13,
                PowerPerLevel = 2,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },

            // Egypt
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Jakal",
                Biom = BiomeType.Egypt,
                NodeIndexes = new[] { 0, 1, 2 },
                Power = 2,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 25,
                HpPerLevel = 3,
                Name = "Leopard",
                Biom = BiomeType.Egypt,
                NodeIndexes = new[] { 1, 2, 4 },
                IsUnique = true,
                Power = 1,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 15,
                HpPerLevel = 3,
                Name = "Bat",
                Biom = BiomeType.Egypt,
                NodeIndexes = new[] { 2, 3, 5 },
                Power = 4,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 25,
                HpPerLevel = 3,
                Name = "Volkolak",
                Biom = BiomeType.Egypt,
                NodeIndexes = new[] { 2, 3, 5 },
                IsUnique = true,
                Power = 4,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 15,
                HpPerLevel = 3,
                Name = "Korgorush",
                Biom = BiomeType.Egypt,
                NodeIndexes = new[] { 6, 7, 8 },
                Power = 4,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Stryga",
                Biom = BiomeType.Egypt,
                NodeIndexes = new[] { 6, 7, 8 },
                Power = 3,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Vampire",
                Biom = BiomeType.Egypt,
                NodeIndexes = new[] { 6, 7, 8 },
                IsUnique = true,
                Power = 5,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Gaint frog",
                Biom = BiomeType.Egypt,
                NodeIndexes = new[] { 7, 8, 9 },
                Power = 2,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 30,
                HpPerLevel = 3,
                Name = "Basilisk",
                Biom = BiomeType.Egypt,
                NodeIndexes = new[] { 7, 8, 9 },
                IsUnique = true,
                Power = 4,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 1200,
                Name = "Sphynx",
                Biom = BiomeType.Egypt,

                IsBoss = true,
                Power = 13,
                PowerPerLevel = 2,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },

            // Greek
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Jakal",
                Biom = BiomeType.Greek,
                NodeIndexes = new[] { 0, 1, 2 },
                Power = 3,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 25,
                HpPerLevel = 3,
                Name = "Leopard",
                Biom = BiomeType.Greek,
                NodeIndexes = new[] { 1, 2, 4 },
                IsUnique = true,
                Power = 1,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 15,
                HpPerLevel = 3,
                Name = "Bat",
                Biom = BiomeType.Greek,
                NodeIndexes = new[] { 2, 3, 5 },
                Power = 4,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 25,
                HpPerLevel = 3,
                Name = "Volkolak",
                Biom = BiomeType.Greek,
                NodeIndexes = new[] { 2, 3, 5 },
                IsUnique = true,
                Power = 4,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 15,
                HpPerLevel = 3,
                Name = "Korgorush",
                Biom = BiomeType.Greek,
                NodeIndexes = new[] { 6, 7, 8 },
                Power = 4,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Stryga",
                Biom = BiomeType.Greek,
                NodeIndexes = new[] { 6, 7, 8 },
                Power = 3,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Vampire",
                Biom = BiomeType.Greek,
                NodeIndexes = new[] { 6, 7, 8 },
                IsUnique = true,
                Power = 5,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Gaint frog",
                Biom = BiomeType.Greek,
                NodeIndexes = new[] { 7, 8, 9 },
                Power = 3,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 30,
                HpPerLevel = 3,
                Name = "Basilisk",
                Biom = BiomeType.Greek,
                NodeIndexes = new[] { 7, 8, 9 },
                IsUnique = true,
                Power = 5,
                PowerPerLevel = 1,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            },
            new UnitScheme
            {
                Hp = 1200,
                Name = "Sphynx",
                Biom = BiomeType.Greek,

                IsBoss = true,
                Power = 13,
                PowerPerLevel = 2,
                Skills = new[]
                {
                    new MonsterAttackSkill()
                }
            }
        };
    }
}