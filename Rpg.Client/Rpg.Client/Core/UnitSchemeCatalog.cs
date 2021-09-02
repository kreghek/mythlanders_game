using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal static class UnitSchemeCatalog
    {
        public static UnitScheme SlavikHero = new()
        {
            Hp = 100,
            HpPerLevel = 10,
            Name = "Svatoslav",
            Skills = new[]{
                new CombatSkill
                {
                    DamageMin = 10,
                    DamageMinPerLevel = 2,
                    DamageMax = 12,
                    DamageMaxPerLevel = 2
                }
            }
        };

        public static IEnumerable<UnitScheme> AllUnits = new[]{
            new UnitScheme{
                Hp = 20,
                HpPerLevel = 3,
                Name = "Grey Wolf",
                Biom = "Slavik",
                NodeIndexes = new[]{ 0, 1, 2 },
                Skills = new CombatSkill[]
                {
                    new CombatSkill{
                        DamageMin = 2,
                        DamageMax = 4
                    }
                }
            },
            new UnitScheme{
                Hp = 25,
                HpPerLevel = 8,
                Name = "Bear",
                Biom = "Slavik",
                NodeIndexes = new[]{ 1, 2, 4 },
                IsUnique = true,
                Skills = new CombatSkill[]
                {
                    new CombatSkill{
                        DamageMin = 1,
                        DamageMax = 2
                    }
                }
            },
            new UnitScheme{
                Hp = 15,
                HpPerLevel = 2,
                Name = "Wisp",
                Biom = "Slavik",
                NodeIndexes = new[]{ 2, 3, 5 },
                Skills = new CombatSkill[]
                {
                    new CombatSkill{
                        DamageMin = 3,
                        DamageMax = 5
                    }
                }
            },
            new UnitScheme{
                Hp = 25,
                HpPerLevel = 9,
                Name = "Volkolak",
                Biom = "Slavik",
                NodeIndexes = new[]{ 2, 3, 5 },
                IsUnique = true,
                Skills = new CombatSkill[]
                {
                    new CombatSkill{
                        DamageMin = 3,
                        DamageMax = 5
                    }
                }
            },
            new UnitScheme{
                Hp = 15,
                HpPerLevel = 3,
                Name = "Korgorush",
                Biom = "Slavik",
                NodeIndexes = new[]{ 6, 7, 8 },
                Skills = new CombatSkill[]
                {
                    new CombatSkill{
                        DamageMin = 3,
                        DamageMax = 5
                    }
                }
            },
            new UnitScheme{
                Hp = 20,
                HpPerLevel = 5,
                Name = "Stryga",
                Biom = "Slavik",
                NodeIndexes = new[]{ 6, 7, 8 },
                Skills = new CombatSkill[]
                {
                    new CombatSkill{
                        DamageMin = 2,
                        DamageMax = 4
                    }
                }
            },
            new UnitScheme{
                Hp = 20,
                HpPerLevel = 10,
                Name = "Vampire",
                Biom = "Slavik",
                NodeIndexes = new[]{ 6, 7, 8 },
                IsUnique = true,
                Skills = new CombatSkill[]
                {
                    new CombatSkill{
                        DamageMin = 5,
                        DamageMax = 6
                    }
                }
            },
            new UnitScheme{
                Hp = 20,
                HpPerLevel = 3,
                Name = "Gaint frog",
                Biom = "Slavik",
                NodeIndexes = new[]{ 7, 8, 9 },
                Skills = new CombatSkill[]
                {
                    new CombatSkill{
                        DamageMin = 2,
                        DamageMax = 4
                    }
                }
            },
            new UnitScheme{
                Hp = 30,
                HpPerLevel = 5,
                Name = "Basilisk",
                Biom = "Slavik",
                NodeIndexes = new[]{ 7, 8, 9 },
                IsUnique = true,
                Skills = new CombatSkill[]
                {
                    new CombatSkill{
                        DamageMin = 3,
                        DamageMax = 6
                    }
                }
            },
            new UnitScheme{
                Hp = 1200,
                Name = "Zmey Gorynych",
                Biom = "Slavik",

                IsBoss = true,
                Skills = new CombatSkill[]
                {
                    new CombatSkill{
                        DamageMin = 10,
                        DamageMax = 15
                    }
                }
            },

            // China
            new UnitScheme{
                Hp = 20,
                HpPerLevel = 3,
                Name = "Grey Wolf",
                Biom = "China",
                NodeIndexes = new[]{ 0, 1, 2 },
                Skills = new CombatSkill[]
                {
                    new CombatSkill{
                        DamageMin = 2,
                        DamageMax = 4
                    }
                }
            },
            new UnitScheme{
                Hp = 25,
                HpPerLevel = 3,
                Name = "Bear",
                Biom = "China",
                NodeIndexes = new[]{ 1, 2, 4 },
                IsUnique = true,
                Skills = new CombatSkill[]
                {
                    new CombatSkill{
                        DamageMin = 1,
                        DamageMax = 2
                    }
                }
            },
            new UnitScheme{
                Hp = 15,
                HpPerLevel = 3,
                Name = "Wisp",
                Biom = "China",
                NodeIndexes = new[]{ 2, 3, 5 },
                Skills = new CombatSkill[]
                {
                    new CombatSkill{
                        DamageMin = 3,
                        DamageMax = 5
                    }
                }
            },
            new UnitScheme{
                Hp = 25,
                HpPerLevel = 3,
                Name = "Volkolak",
                Biom = "China",
                NodeIndexes = new[]{ 2, 3, 5 },
                IsUnique = true,
                Skills = new CombatSkill[]
                {
                    new CombatSkill{
                        DamageMin = 3,
                        DamageMax = 5
                    }
                }
            },
            new UnitScheme{
                Hp = 15,
                HpPerLevel = 3,
                Name = "Korgorush",
                Biom = "China",
                NodeIndexes = new[]{ 6, 7, 8 },
                Skills = new CombatSkill[]
                {
                    new CombatSkill{
                        DamageMin = 3,
                        DamageMax = 5
                    }
                }
            },
            new UnitScheme{
                Hp = 20,
                HpPerLevel = 3,
                Name = "Stryga",
                Biom = "China",
                NodeIndexes = new[]{ 6, 7, 8 },
                Skills = new CombatSkill[]
                {
                    new CombatSkill{
                        DamageMin = 2,
                        DamageMax = 4
                    }
                }
            },
            new UnitScheme{
                Hp = 20,
                HpPerLevel = 3,
                Name = "Vampire",
                Biom = "China",
                NodeIndexes = new[]{ 6, 7, 8 },
                IsUnique = true,
                Skills = new CombatSkill[]
                {
                    new CombatSkill{
                        DamageMin = 5,
                        DamageMax = 6
                    }
                }
            },
            new UnitScheme{
                Hp = 20,
                HpPerLevel = 3,
                Name = "Gaint frog",
                Biom = "China",
                NodeIndexes = new[]{ 7, 8, 9 },
                Skills = new CombatSkill[]
                {
                    new CombatSkill{
                        DamageMin = 2,
                        DamageMax = 4
                    }
                }
            },
            new UnitScheme{
                Hp = 30,
                HpPerLevel = 3,
                Name = "Basilisk",
                Biom = "China",
                NodeIndexes = new[]{ 7, 8, 9 },
                IsUnique = true,
                Skills = new CombatSkill[]
                {
                    new CombatSkill{
                        DamageMin = 3,
                        DamageMax = 6
                    }
                }
            },
            new UnitScheme{
                Hp = 1200,
                Name = "Dragon",
                Biom = "China",

                IsBoss = true,
                Skills = new CombatSkill[]
                {
                    new CombatSkill{
                        DamageMin = 10,
                        DamageMax = 15
                    }
                }
            },
        };
    }
}