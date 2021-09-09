﻿using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal static class UnitSchemeCatalog
    {
        public static UnitScheme SwordmanHero = new()
        {
            Hp = 100,
            HpPerLevel = 10,
            Name = "Беримир",
            Skills = new[]
            {
                new CombatSkill
                {
                    DamageMin = 10,
                    DamageMinPerLevel = 2,
                    DamageMax = 12,
                    DamageMaxPerLevel = 2,
                    TargetType = SkillTarget.Enemy,
                    Scope = SkillScope.Single
                },
                new CombatSkill
                {
                    DamageMin = 5,
                    DamageMinPerLevel = 1,
                    DamageMax = 7,
                    DamageMaxPerLevel = 1,
                    TargetType = SkillTarget.Enemy,
                    Scope = SkillScope.AllEnemyGroup
                }
            }
        };

        public static UnitScheme HerbalistHero = new()
        {
            Hp = 50,
            HpPerLevel = 6,
            Name = "Рада",
            Skills = new[]
            {
                new CombatSkill
                {
                    DamageMin = 5,
                    DamageMinPerLevel = 1,
                    DamageMax = 7,
                    DamageMaxPerLevel = 1,
                    TargetType = SkillTarget.Friendly,
                    Scope = SkillScope.Single,
                    Range = Models.Combat.GameObjects.CombatPowerRange.Distant
                }
            }
        };

        public static UnitScheme ArcherHero = new()
        {
            Hp = 50,
            HpPerLevel = 7,
            Name = "Соколинный глаз", // Hawkeye
            Skills = new[]
            {
                new CombatSkill
                {
                    DamageMin = 15,
                    DamageMinPerLevel = 2,
                    DamageMax = 17,
                    DamageMaxPerLevel = 2,
                    TargetType = SkillTarget.Enemy,
                    Scope = SkillScope.Single,
                    Range = Models.Combat.GameObjects.CombatPowerRange.Distant
                },
                new CombatSkill
                {
                    DamageMin = 5,
                    DamageMinPerLevel = 1,
                    DamageMax = 7,
                    DamageMaxPerLevel = 1,
                    TargetType = SkillTarget.Enemy,
                    Scope = SkillScope.AllEnemyGroup,
                    Range = Models.Combat.GameObjects.CombatPowerRange.Distant
                }
            }
        };

        public static UnitScheme JuggernautHero = new()
        {
            Hp = 150,
            HpPerLevel = 20,
            Name = "Smooky",
            Skills = new[]
            {
                new CombatSkill
                {
                    DamageMin = 5,
                    DamageMinPerLevel = 1,
                    DamageMax = 7,
                    DamageMaxPerLevel = 1,
                    TargetType = SkillTarget.Enemy,
                    Scope = SkillScope.AllEnemyGroup
                }
            }
        };

        public static IEnumerable<UnitScheme> AllUnits = new[]
        {
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Grey Wolf",
                Biom = BiomType.Slavic,
                NodeIndexes = new[] { 0, 1, 2 },
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 2,
                        DamageMax = 4
                    }
                }
            },
            new UnitScheme
            {
                Hp = 25,
                HpPerLevel = 8,
                Name = "Bear",
                Biom = BiomType.Slavic,
                NodeIndexes = new[] { 1, 2, 4 },
                IsUnique = true,
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 1,
                        DamageMax = 2
                    }
                }
            },
            new UnitScheme
            {
                Hp = 15,
                HpPerLevel = 2,
                Name = "Wisp",
                Biom = BiomType.Slavic,
                NodeIndexes = new[] { 2, 3, 5 },
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 3,
                        DamageMax = 5
                    }
                }
            },
            new UnitScheme
            {
                Hp = 25,
                HpPerLevel = 9,
                Name = "Volkolak",
                Biom = BiomType.Slavic,
                NodeIndexes = new[] { 2, 3, 5 },
                IsUnique = true,
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 3,
                        DamageMax = 5
                    }
                }
            },
            new UnitScheme
            {
                Hp = 15,
                HpPerLevel = 3,
                Name = "Korgorush",
                Biom = BiomType.Slavic,
                NodeIndexes = new[] { 6, 7, 8 },
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 3,
                        DamageMax = 5
                    }
                }
            },
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 5,
                Name = "Stryga",
                Biom = BiomType.Slavic,
                NodeIndexes = new[] { 6, 7, 8 },
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 2,
                        DamageMax = 4
                    }
                }
            },
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 10,
                Name = "Vampire",
                Biom = BiomType.Slavic,
                NodeIndexes = new[] { 6, 7, 8 },
                IsUnique = true,
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 5,
                        DamageMax = 6
                    }
                }
            },
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Gaint frog",
                Biom = BiomType.Slavic,
                NodeIndexes = new[] { 7, 8, 9 },
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 2,
                        DamageMax = 4
                    }
                }
            },
            new UnitScheme
            {
                Hp = 30,
                HpPerLevel = 5,
                Name = "Basilisk",
                Biom = BiomType.Slavic,
                NodeIndexes = new[] { 7, 8, 9 },
                IsUnique = true,
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 3,
                        DamageMax = 6
                    }
                }
            },
            new UnitScheme
            {
                Hp = 1200,
                Name = "Zmey Gorynych",
                Biom = BiomType.Slavic,

                IsBoss = true,
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 10,
                        DamageMax = 15
                    }
                }
            },

            // China
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Grey Wolf",
                Biom = BiomType.China,
                NodeIndexes = new[] { 0, 1, 2 },
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 2,
                        DamageMax = 4
                    }
                }
            },
            new UnitScheme
            {
                Hp = 25,
                HpPerLevel = 3,
                Name = "Bear",
                Biom = BiomType.China,
                NodeIndexes = new[] { 1, 2, 4 },
                IsUnique = true,
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 1,
                        DamageMax = 2
                    }
                }
            },
            new UnitScheme
            {
                Hp = 15,
                HpPerLevel = 3,
                Name = "Wisp",
                Biom = BiomType.China,
                NodeIndexes = new[] { 2, 3, 5 },
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 3,
                        DamageMax = 5
                    }
                }
            },
            new UnitScheme
            {
                Hp = 25,
                HpPerLevel = 3,
                Name = "Volkolak",
                Biom = BiomType.China,
                NodeIndexes = new[] { 2, 3, 5 },
                IsUnique = true,
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 3,
                        DamageMax = 5
                    }
                }
            },
            new UnitScheme
            {
                Hp = 15,
                HpPerLevel = 3,
                Name = "Korgorush",
                Biom = BiomType.China,
                NodeIndexes = new[] { 6, 7, 8 },
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 3,
                        DamageMax = 5
                    }
                }
            },
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Stryga",
                Biom = BiomType.China,
                NodeIndexes = new[] { 6, 7, 8 },
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 2,
                        DamageMax = 4
                    }
                }
            },
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Vampire",
                Biom = BiomType.China,
                NodeIndexes = new[] { 6, 7, 8 },
                IsUnique = true,
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 5,
                        DamageMax = 6
                    }
                }
            },
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Gaint frog",
                Biom = BiomType.China,
                NodeIndexes = new[] { 7, 8, 9 },
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 2,
                        DamageMax = 4
                    }
                }
            },
            new UnitScheme
            {
                Hp = 30,
                HpPerLevel = 3,
                Name = "Basilisk",
                Biom = BiomType.China,
                NodeIndexes = new[] { 7, 8, 9 },
                IsUnique = true,
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 3,
                        DamageMax = 6
                    }
                }
            },
            new UnitScheme
            {
                Hp = 1200,
                Name = "Dragon",
                Biom = BiomType.China,

                IsBoss = true,
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 10,
                        DamageMax = 15
                    }
                }
            },

            // Egypt
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Jakal",
                Biom = BiomType.Egypt,
                NodeIndexes = new[] { 0, 1, 2 },
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 2,
                        DamageMax = 4
                    }
                }
            },
            new UnitScheme
            {
                Hp = 25,
                HpPerLevel = 3,
                Name = "Leopard",
                Biom = BiomType.Egypt,
                NodeIndexes = new[] { 1, 2, 4 },
                IsUnique = true,
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 1,
                        DamageMax = 2
                    }
                }
            },
            new UnitScheme
            {
                Hp = 15,
                HpPerLevel = 3,
                Name = "Bat",
                Biom = BiomType.Egypt,
                NodeIndexes = new[] { 2, 3, 5 },
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 3,
                        DamageMax = 5
                    }
                }
            },
            new UnitScheme
            {
                Hp = 25,
                HpPerLevel = 3,
                Name = "Volkolak",
                Biom = BiomType.Egypt,
                NodeIndexes = new[] { 2, 3, 5 },
                IsUnique = true,
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 3,
                        DamageMax = 5
                    }
                }
            },
            new UnitScheme
            {
                Hp = 15,
                HpPerLevel = 3,
                Name = "Korgorush",
                Biom = BiomType.Egypt,
                NodeIndexes = new[] { 6, 7, 8 },
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 3,
                        DamageMax = 5
                    }
                }
            },
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Stryga",
                Biom = BiomType.Egypt,
                NodeIndexes = new[] { 6, 7, 8 },
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 2,
                        DamageMax = 4
                    }
                }
            },
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Vampire",
                Biom = BiomType.Egypt,
                NodeIndexes = new[] { 6, 7, 8 },
                IsUnique = true,
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 5,
                        DamageMax = 6
                    }
                }
            },
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Gaint frog",
                Biom = BiomType.Egypt,
                NodeIndexes = new[] { 7, 8, 9 },
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 2,
                        DamageMax = 4
                    }
                }
            },
            new UnitScheme
            {
                Hp = 30,
                HpPerLevel = 3,
                Name = "Basilisk",
                Biom = BiomType.Egypt,
                NodeIndexes = new[] { 7, 8, 9 },
                IsUnique = true,
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 3,
                        DamageMax = 6
                    }
                }
            },
            new UnitScheme
            {
                Hp = 1200,
                Name = "Sphynx",
                Biom = BiomType.Egypt,

                IsBoss = true,
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 10,
                        DamageMax = 15
                    }
                }
            },

            // Greek
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Jakal",
                Biom = BiomType.Greek,
                NodeIndexes = new[] { 0, 1, 2 },
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 2,
                        DamageMax = 4
                    }
                }
            },
            new UnitScheme
            {
                Hp = 25,
                HpPerLevel = 3,
                Name = "Leopard",
                Biom = BiomType.Greek,
                NodeIndexes = new[] { 1, 2, 4 },
                IsUnique = true,
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 1,
                        DamageMax = 2
                    }
                }
            },
            new UnitScheme
            {
                Hp = 15,
                HpPerLevel = 3,
                Name = "Bat",
                Biom = BiomType.Greek,
                NodeIndexes = new[] { 2, 3, 5 },
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 3,
                        DamageMax = 5
                    }
                }
            },
            new UnitScheme
            {
                Hp = 25,
                HpPerLevel = 3,
                Name = "Volkolak",
                Biom = BiomType.Greek,
                NodeIndexes = new[] { 2, 3, 5 },
                IsUnique = true,
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 3,
                        DamageMax = 5
                    }
                }
            },
            new UnitScheme
            {
                Hp = 15,
                HpPerLevel = 3,
                Name = "Korgorush",
                Biom = BiomType.Greek,
                NodeIndexes = new[] { 6, 7, 8 },
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 3,
                        DamageMax = 5
                    }
                }
            },
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Stryga",
                Biom = BiomType.Greek,
                NodeIndexes = new[] { 6, 7, 8 },
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 2,
                        DamageMax = 4
                    }
                }
            },
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Vampire",
                Biom = BiomType.Greek,
                NodeIndexes = new[] { 6, 7, 8 },
                IsUnique = true,
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 5,
                        DamageMax = 6
                    }
                }
            },
            new UnitScheme
            {
                Hp = 20,
                HpPerLevel = 3,
                Name = "Gaint frog",
                Biom = BiomType.Greek,
                NodeIndexes = new[] { 7, 8, 9 },
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 2,
                        DamageMax = 4
                    }
                }
            },
            new UnitScheme
            {
                Hp = 30,
                HpPerLevel = 3,
                Name = "Basilisk",
                Biom = BiomType.Greek,
                NodeIndexes = new[] { 7, 8, 9 },
                IsUnique = true,
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 3,
                        DamageMax = 6
                    }
                }
            },
            new UnitScheme
            {
                Hp = 1200,
                Name = "Sphynx",
                Biom = BiomType.Greek,

                IsBoss = true,
                Skills = new[]
                {
                    new CombatSkill
                    {
                        DamageMin = 10,
                        DamageMax = 15
                    }
                }
            }
        };
    }
}