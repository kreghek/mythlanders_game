﻿using System.Collections.Generic;
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

            Name = UnitName.Berimir,
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

        public static UnitScheme MonkHero = new()
        {
            Hp = 100,
            HpPerLevel = 10,
            Power = 11,
            PowerPerLevel = 2,

            Name = UnitName.Maosin,
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

        public static UnitScheme SpearmanHero = new()
        {
            Hp = 100,
            HpPerLevel = 10,
            Power = 11,
            PowerPerLevel = 2,

            Name = UnitName.Ping,
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

        public static UnitScheme ScorpionHero = new()
        {
            Hp = 100,
            HpPerLevel = 10,
            Power = 11,
            PowerPerLevel = 2,

            Name = UnitName.Ping,
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
            Name = UnitName.Rada,
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
            Name = UnitName.Hawk,
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
                        new DefenseSkill(true)
                    }
                },
                new SkillSet
                {
                    Skills = new List<SkillBase>
                    {
                        new StrikeSkill(),
                        new DefenseSkill(true),
                        new ArrowRainSkill(true)
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

            Name = UnitName.Kakhotep,

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

            Name = UnitName.Cheng,

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

        public static IDictionary<UnitName, UnitScheme> PlayerUnits = new[]
        {
            ArcherHero,
            HerbalistHero,
            SwordmanHero
        }.ToDictionary(scheme => scheme.Name, scheme => scheme);

        static UnitSchemeCatalog()
        {
            var slavicMonsters = CreateSlavicMonsters();
            var chineseMonsters = CreateChineseMonsters();
            var egyptianMonsters = CreateEgyptianMonsters();
            var greekMonsters = CreateGreekMonsters();

            AllUnits = slavicMonsters.Concat(chineseMonsters).Concat(egyptianMonsters).Concat(greekMonsters);
        }

        public static IEnumerable<UnitScheme> AllUnits { get; }

        private static IEnumerable<UnitScheme> CreateChineseMonsters()
        {
            var biomeType = BiomeType.Chinese;
            return new[]
            {
                new UnitScheme
                {
                    Hp = 20,
                    HpPerLevel = 3,
                    Name = UnitName.GreyWolf,
                    Biom = biomeType,
                    NodeIndexes = new[] { 0, 1, 2 },
                    Power = 2,
                    PowerPerLevel = 1,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill()
                            }
                        }
                    }

                    //SchemeAudoTransiton= new UnitSchemeAutoTransition
                    //{
                    //    HpShare = 0.5f,
                    //    NextScheme = new UnitScheme
                    //    {
                    //        Hp = 20,
                    //        HpPerLevel = 3,
                    //        Name = "Grey Wolf 2",
                    //        Biom = BiomeType.Slavic,
                    //        NodeIndexes = new[] { 0, 1, 2 },
                    //        Power = 2,
                    //        PowerPerLevel = 1,

                    //        SkillSets = new List<SkillSet>
                    //        {
                    //            new SkillSet
                    //            {
                    //                Skills = new List<SkillBase>
                    //                {
                    //                    new MonsterAttackSkill(), // Bite
                    //                    new PowerUpSkill(), // Wolf howl
                    //                    new HealSkill() // lick wounds
                    //                }
                    //            }
                    //        }
                    //    },
                    //}
                },
                new UnitScheme
                {
                    Hp = 25,
                    HpPerLevel = 8,
                    Name = UnitName.Bear,
                    Biom = biomeType,
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
                    Name = UnitName.Wisp,
                    Biom = biomeType,
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
                    Name = UnitName.Volkolak,
                    Biom = biomeType,
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
                    Name = UnitName.Korgorush,
                    Biom = biomeType,
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
                    Name = UnitName.Stryga,
                    Biom = biomeType,
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
                    Name = UnitName.Vampire,
                    Biom = biomeType,
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
                    Name = UnitName.HornedFrog,
                    Biom = biomeType,
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
                    Name = UnitName.Basilisk,
                    Biom = biomeType,
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
                    Hp = 8,
                    HpPerLevel = 60,
                    Name = UnitName.Taote,
                    Biom = biomeType,

                    IsBoss = true,
                    Power = 13,
                    PowerPerLevel = 2,

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

        private static IEnumerable<UnitScheme> CreateEgyptianMonsters()
        {
            var biomeType = BiomeType.Egyptian;
            return new[]
            {
                new UnitScheme
                {
                    Hp = 20,
                    HpPerLevel = 3,
                    Name = UnitName.GreyWolf,
                    Biom = biomeType,
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

                    //SchemeAudoTransiton= new UnitSchemeAutoTransition
                    //{
                    //    HpShare = 0.5f,
                    //    NextScheme = new UnitScheme
                    //    {
                    //        Hp = 20,
                    //        HpPerLevel = 3,
                    //        Name = "Grey Wolf 2",
                    //        Biom = BiomeType.Slavic,
                    //        NodeIndexes = new[] { 0, 1, 2 },
                    //        Power = 2,
                    //        PowerPerLevel = 1,

                    //        SkillSets = new List<SkillSet>
                    //        {
                    //            new SkillSet
                    //            {
                    //                Skills = new List<SkillBase>
                    //                {
                    //                    new MonsterAttackSkill(), // Bite
                    //                    new PowerUpSkill(), // Wolf howl
                    //                    new HealSkill() // lick wounds
                    //                }
                    //            }
                    //        }
                    //    },
                    //}
                },
                new UnitScheme
                {
                    Hp = 25,
                    HpPerLevel = 8,
                    Name = UnitName.Bear,
                    Biom = biomeType,
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
                    Name = UnitName.Wisp,
                    Biom = biomeType,
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
                    Name = UnitName.Volkolak,
                    Biom = biomeType,
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
                    Name = UnitName.Korgorush,
                    Biom = biomeType,
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
                    Name = UnitName.Stryga,
                    Biom = biomeType,
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
                    Name = UnitName.Vampire,
                    Biom = biomeType,
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
                    Name = UnitName.HornedFrog,
                    Biom = biomeType,
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
                    Name = UnitName.Basilisk,
                    Biom = biomeType,
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
                    Hp = 8,
                    HpPerLevel = 60,
                    Name = UnitName.Sphynx,
                    Biom = biomeType,

                    IsBoss = true,
                    Power = 13,
                    PowerPerLevel = 2,

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

        private static IEnumerable<UnitScheme> CreateGreekMonsters()
        {
            var biomeType = BiomeType.Greek;
            return new[]
            {
                new UnitScheme
                {
                    Hp = 20,
                    HpPerLevel = 3,
                    Name = UnitName.GreyWolf,
                    Biom = biomeType,
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

                    //SchemeAudoTransiton= new UnitSchemeAutoTransition
                    //{
                    //    HpShare = 0.5f,
                    //    NextScheme = new UnitScheme
                    //    {
                    //        Hp = 20,
                    //        HpPerLevel = 3,
                    //        Name = "Grey Wolf 2",
                    //        Biom = BiomeType.Slavic,
                    //        NodeIndexes = new[] { 0, 1, 2 },
                    //        Power = 2,
                    //        PowerPerLevel = 1,

                    //        SkillSets = new List<SkillSet>
                    //        {
                    //            new SkillSet
                    //            {
                    //                Skills = new List<SkillBase>
                    //                {
                    //                    new MonsterAttackSkill(), // Bite
                    //                    new PowerUpSkill(), // Wolf howl
                    //                    new HealSkill() // lick wounds
                    //                }
                    //            }
                    //        }
                    //    },
                    //}
                },
                new UnitScheme
                {
                    Hp = 25,
                    HpPerLevel = 8,
                    Name = UnitName.Bear,
                    Biom = biomeType,
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
                    Name = UnitName.Wisp,
                    Biom = biomeType,
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
                    Name = UnitName.Volkolak,
                    Biom = biomeType,
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
                    Name = UnitName.Korgorush,
                    Biom = biomeType,
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
                    Name = UnitName.Stryga,
                    Biom = biomeType,
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
                    Name = UnitName.Vampire,
                    Biom = biomeType,
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
                    Name = UnitName.HornedFrog,
                    Biom = biomeType,
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
                    Name = UnitName.Basilisk,
                    Biom = biomeType,
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
                    Hp = 8,
                    HpPerLevel = 60,
                    Name = UnitName.Hydra,
                    Biom = biomeType,

                    IsBoss = true,
                    Power = 13,
                    PowerPerLevel = 2,

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

        private static IEnumerable<UnitScheme> CreateSlavicMonsters()
        {
            var biomeType = BiomeType.Slavic;
            return new[]
            {
                new UnitScheme
                {
                    Hp = 20,
                    HpPerLevel = 3,
                    Name = UnitName.GreyWolf,
                    Biom = biomeType,
                    NodeIndexes = new[] { 0, 1, 2 },
                    Power = 2,
                    PowerPerLevel = 1,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill() // Bite
                            }
                        }
                    }
                },
                new UnitScheme
                {
                    Hp = 25,
                    HpPerLevel = 8,
                    Name = UnitName.Bear,
                    Biom = biomeType,
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
                                new MonsterAttackSkill() // Bite
                            }
                        }
                    }
                },
                new UnitScheme
                {
                    Hp = 15,
                    HpPerLevel = 2,
                    Name = UnitName.Wisp,
                    Biom = biomeType,
                    NodeIndexes = new[] { 2, 3, 5 },
                    Power = 4,
                    PowerPerLevel = 1,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new ArrowRainSkill()
                            }
                        }
                    }
                },
                new UnitScheme
                {
                    Hp = 25,
                    HpPerLevel = 9,
                    Name = UnitName.Volkolak,
                    Biom = biomeType,
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
                                new WideSlashSkill()
                            }
                        }
                    }
                },
                new UnitScheme
                {
                    Hp = 15,
                    HpPerLevel = 3,
                    Name = UnitName.Korgorush,
                    Biom = biomeType,
                    NodeIndexes = new[] { 6, 7, 8 },
                    Power = 4,
                    PowerPerLevel = 1,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new HealSkill()
                            }
                        }
                    }
                },
                new UnitScheme
                {
                    Hp = 20,
                    HpPerLevel = 5,
                    Name = UnitName.Stryga,
                    Biom = biomeType,
                    NodeIndexes = new[] { 6, 7, 8 },
                    Power = 3,
                    PowerPerLevel = 1,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new MonsterAttackSkill()
                            }
                        }
                    }
                },
                new UnitScheme
                {
                    Hp = 20,
                    HpPerLevel = 10,
                    Name = UnitName.Vampire,
                    Biom = biomeType,
                    NodeIndexes = new[] { 6, 7, 8 },
                    IsUnique = true,
                    Power = 10,
                    PowerPerLevel = 1,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new VampiricBiteSkill()
                            }
                        }
                    }
                },
                new UnitScheme
                {
                    Hp = 20,
                    HpPerLevel = 3,
                    Name = UnitName.HornedFrog,
                    Biom = biomeType,
                    NodeIndexes = new[] { 7, 8, 9 },
                    Power = 3,
                    PowerPerLevel = 1,

                    SkillSets = new List<SkillSet>
                    {
                        new SkillSet
                        {
                            Skills = new List<SkillBase>
                            {
                                new DefenseSkill()
                            }
                        }
                    }
                },
                new UnitScheme
                {
                    Hp = 30,
                    HpPerLevel = 5,
                    Name = UnitName.Basilisk,
                    Biom = biomeType,
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
                                new WideSlashSkill()
                            }
                        }
                    }
                },
                new UnitScheme
                {
                    Hp = 8,
                    HpPerLevel = 55,
                    Name = UnitName.KosheyTheImmortal,
                    Biom = biomeType,

                    IsBoss = true,
                    Power = 13,
                    PowerPerLevel = 2,

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
                    },

                    SchemeAudoTransiton = new UnitSchemeAutoTransition
                    {
                        HpShare = 0.6f,
                        NextScheme = new UnitScheme
                        {
                            Hp = 8,
                            HpPerLevel = 55,
                            Name = UnitName.KosheyTheImmortal2,
                            Biom = BiomeType.Slavic,
                            NodeIndexes = new[] { 0, 1, 2 },
                            Power = 14,
                            PowerPerLevel = 2,

                            SkillSets = new List<SkillSet>
                            {
                                new SkillSet
                                {
                                    Skills = new List<SkillBase>
                                    {
                                        new MonsterAttackSkill(), // Bite
                                        new DefenseSkill(), // Dead one hard to die
                                        new HealSkill() // Eat a flash
                                    }
                                }
                            },

                            SchemeAudoTransiton = new UnitSchemeAutoTransition
                            {
                                HpShare = 0.3f,
                                NextScheme = new UnitScheme
                                {
                                    Hp = 8,
                                    HpPerLevel = 55,
                                    Name = UnitName.KosheyTheImmortal3,
                                    Biom = BiomeType.Slavic,
                                    NodeIndexes = new[] { 0, 1, 2 },
                                    Power = 15,
                                    PowerPerLevel = 2,

                                    SkillSets = new List<SkillSet>
                                    {
                                        new SkillSet
                                        {
                                            Skills = new List<SkillBase>
                                            {
                                                new ArrowRainSkill(), // Dark Wind
                                                new DopeHerbSkill(), // Scary Eyes
                                                new PowerUpSkill() // 1000-years hate
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}