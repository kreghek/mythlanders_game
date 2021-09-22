using System.Collections.Generic;
using System.Linq;

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
            SkillSets = new List<SkillSet>
            {
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new SlashSkill()
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new SlashSkill(),
                        new DefenseSkill(true)
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new SlashSkill(),
                        new DefenseSkill(true),
                        new WideSlashSkill(true)
                    }
                }
            }
        };

        public static UnitScheme HerbalistHero = new()
        {
            Hp = 50,
            HpPerLevel = 6,
            Name = "Рада",
            Power = 6,
            PowerPerLevel = 1,

            SkillSets = new List<SkillSet>
            {
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new PeriodicHealSkill()
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new PeriodicHealSkill(),
                        new DopeHerbSkill(true)
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new PeriodicHealSkill(),
                        new DopeHerbSkill(true),
                        new MassHealSkill(true)
                    }
                }
            }
        };

        public static UnitScheme ArcherHero = new()
        {
            Hp = 50,
            HpPerLevel = 7,
            Name = "Сокол", // Hawkeye
            Power = 13,
            PowerPerLevel = 2,

            SkillSets = new List<SkillSet>
            {
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new StrikeSkill()
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new StrikeSkill(),
                        new ArrowRainSkill(true)
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new StrikeSkill(),
                        new ArrowRainSkill(true),
                        new DefenseSkill(true)
                    }
                }
            }
        };

        public static UnitScheme PriestHero = new()
        {
            Hp = 50,
            HpPerLevel = 5,
            Power = 11,
            PowerPerLevel = 2,

            Name = "Кахотеп",

            SkillSets = new List<SkillSet>
            {
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new StrikeSkill()
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new StrikeSkill(),
                        new MassStunSkill(true)
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new StrikeSkill(),
                        new MassStunSkill(true),
                        new SlashSkill(true) // Finger of the Anubis
                    }
                }
            }
        };

        public static UnitScheme MissionaryHero = new()
        {
            Hp = 50,
            HpPerLevel = 5,
            Power = 11,
            PowerPerLevel = 2,

            Name = "Тао Чан",

            SkillSets = new List<SkillSet>
            {
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new DopeHerbSkill()
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new DopeHerbSkill(),
                        new PowerUpSkill(true)
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new DopeHerbSkill(), // No violence, please
                        new PowerUpSkill(true), // Trust
                        new HealSkill(true) // God Mercifull Touch
                    }
                }
            }
        };

        public static IDictionary<string, UnitScheme> PlayerUnits = new[]
        {
            ArcherHero,
            HerbalistHero,
            SwordmanHero
        }.ToDictionary(scheme => scheme.Name, scheme => scheme);

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

                SkillSets = new List<SkillSet>
                {
                    new SkillSet
                    {
                        Skills = new List<SkillBase>
                        {
                            new MonsterAttackSkill(), // Bite
                            new PowerUpSkill(), // Wolf howl
                            new HealSkill() // lick wounds
                        }
                    }
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

                SkillSets = new List<SkillSet>
                {
                    new SkillSet
                    {
                        Skills = new List<SkillBase>
                        {
                            new MonsterAttackSkill(), // Bite
                            new DefenseSkill(),
                            new WideSlashSkill()
                        }
                    }
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

                SkillSets = new List<SkillSet>
                {
                    new SkillSet
                    {
                        Skills = new List<SkillBase>
                        {
                            new StrikeSkill(),
                            new ArrowRainSkill(),
                            new PowerUpSkill()
                        }
                    }
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

                SkillSets = new List<SkillSet>
                {
                    new SkillSet
                    {
                        Skills = new List<SkillBase>
                        {
                            new MonsterAttackSkill(),
                            new WideSlashSkill(),
                            new PowerUpSkill()
                        }
                    }
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

                SkillSets = new List<SkillSet>
                {
                    new SkillSet
                    {
                        Skills = new List<SkillBase>
                        {
                            new MonsterAttackSkill(),
                            new HealSkill(),
                            new WideSlashSkill()
                        }
                    }
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

                SkillSets = new List<SkillSet>
                {
                    new SkillSet
                    {
                        Skills = new List<SkillBase>
                        {
                            new MonsterAttackSkill(),
                            new WideSlashSkill(),
                            new HealSkill()
                        }
                    }
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

                SkillSets = new List<SkillSet>
                {
                    new SkillSet
                    {
                        Skills = new List<SkillBase>
                        {
                            new MonsterAttackSkill(),
                            new DopeHerbSkill(),
                            new MassHealSkill()
                        }
                    }
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

                SkillSets = new List<SkillSet>
                {
                    new SkillSet
                    {
                        Skills = new List<SkillBase>
                        {
                            new MonsterAttackSkill(),
                            new DefenseSkill()
                        }
                    }
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

                SkillSets = new List<SkillSet>
                {
                    new SkillSet
                    {
                        Skills = new List<SkillBase>
                        {
                            new MonsterAttackSkill(),
                            new DopeHerbSkill(),
                            new WideSlashSkill()
                        }
                    }
                }
            },
            new UnitScheme
            {
                Hp = 400,
                Name = "Zmey Gorynych",
                Biom = BiomeType.Slavic,

                IsBoss = true,
                Power = 13,
                PowerPerLevel = 1,

                SkillSets = new List<SkillSet>
                {
                    new SkillSet
                    {
                        Skills = new List<SkillBase>
                        {
                            new MonsterAttackSkill(),
                            new DopeHerbSkill(),
                            new ArrowRainSkill()
                        }
                    }
                }
            }
        };
    }
}