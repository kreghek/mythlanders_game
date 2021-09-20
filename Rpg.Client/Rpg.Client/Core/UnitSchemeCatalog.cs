using System.Collections.Generic;

using Rpg.Client.Core.Skills;

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
            }
        };

        public static UnitScheme HerbalistHero = new()
        {
            Hp = 50,
            HpPerLevel = 6,
            Name = "Рада",
            Power = 6,
            PowerPerLevel = 1,
            Skills = new SkillBase[]
            {
                new HealSkill(),
                new DopeHerbSkill(),
                new PeriodicHealSkill()
            }
        };

        public static UnitScheme ArcherHero = new()
        {
            Hp = 50,
            HpPerLevel = 7,
            Name = "Сокол", // Hawkeye
            Power = 11,
            PowerPerLevel = 1,
            Skills = new SkillBase[]
            {
                new StrikeSkill(),
                new ArrowRainSkill()
            }
        };

        public static UnitScheme PriestHero = new()
        {
            Hp = 50,
            HpPerLevel = 5,
            Power = 11,
            PowerPerLevel = 2,

            Name = "Кахотеп",
            Skills = new SkillBase[]
            {
                new MassStunSkill()
            }
        };

        public static UnitScheme MissionaryHero = new()
        {
            Hp = 50,
            HpPerLevel = 5,
            Power = 11,
            PowerPerLevel = 2,

            Name = "Тао Чан",
            Skills = new SkillBase[]
            {
                new DopeHerbSkill(),
                new PowerUpSkill(),
                new HealSkill()
            }
        };

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
                Skills = new SkillBase[]
                {
                    new StrikeSkill(),
                    new ArrowRainSkill()
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
                Skills = new SkillBase[]
                {
                    new MonsterAttackSkill(),
                    new WideSlashSkill()
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
                Skills = new SkillBase[]
                {
                    new MonsterAttackSkill(),
                    new WideSlashSkill()
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
                Skills = new SkillBase[]
                {
                    new MonsterAttackSkill(),
                    new DopeHerbSkill()
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
                Skills = new SkillBase[]
                {
                    new MonsterAttackSkill(),
                    new DopeHerbSkill(),
                    new WideSlashSkill()
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
                Skills = new SkillBase[]
                {
                    new MonsterAttackSkill(),
                    new DopeHerbSkill(),
                    new ArrowRainSkill()
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