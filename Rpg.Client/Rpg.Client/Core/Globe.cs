using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core
{
    internal sealed class Globe
    {
        public Globe()
        {
            var biomes = new[] {
                new Biom{
                    Name = "Slavik",
                    Nodes = Enumerable.Range(1, 10).Select(x=>
                        new GlobeNode
                        {
                            Index = x
                        }
                    ).ToArray(),
                    UnlockBiom = "China"
                },
                new Biom{
                    Name = "China",
                    Nodes = Enumerable.Range(1, 10).Select(x=>
                        new GlobeNode
                        {
                            Index = x
                        }
                    ).ToArray()
                }
            };
        }

        public Player Player { get; set; }

        public ActiveCombat? ActiveCombat { get; set; }

        public bool IsNodeInitialied { get; set; }

        public IEnumerable<Biom> Bioms { get; private set; }

        public void UpdateNodes(IDice dice)
        {
            // Reset all combat states.
            foreach (var biom in Bioms.Where(x => x.IsAvailable).ToArray())
            {
                foreach (var node in biom.Nodes)
                {
                    node.Combat = null;
                }

                if (biom.IsComplete && biom.UnlockBiom is not null)
                {
                    var unlockedBiom = Bioms.Single(x => x.Name == biom.UnlockBiom);

                    unlockedBiom.IsAvailable = true;
                }
            }

            // Create new combats
            foreach (var biom in Bioms.Where(x => x.IsAvailable))
            {
                if (biom.Level < 10)
                {
                    var nodesWithCombats = dice.RollFromList(biom.Nodes.ToList(), 3);
                    foreach (var node in nodesWithCombats)
                    {
                        node.Combat = new Combat
                        {
                            EnemyGroup = new Group
                            {
                                Units = new[] {
                                    new Unit{
                                        Hp = 20,
                                        Name = "Enemy",
                                        Skills = new CombatSkill[]
                                        {
                                            new CombatSkill{
                                                DamageMin = 2,
                                                DamageMax = 4
                                            }
                                        }
                                    },
                                    new Unit{
                                        Hp = 25,
                                        Name = "Enemy",
                                        Skills = new CombatSkill[]
                                        {
                                            new CombatSkill{
                                                DamageMin = 1,
                                                DamageMax = 2
                                            }
                                        }
                                    },
                                    new Unit{
                                        Hp = 15,
                                        Name = "Enemy",
                                        Skills = new CombatSkill[]
                                        {
                                            new CombatSkill{
                                                DamageMin = 3,
                                                DamageMax = 5
                                            }
                                        }
                                    }
                                }
                            }
                        };
                    }
                }
                else
                {
                    var nodesWithCombats = dice.RollFromList(biom.Nodes.ToList(), 3);
                    foreach (var node in nodesWithCombats)
                    {
                        // boss level
                        if (node == nodesWithCombats.First())
                        {
                            node.Combat = new Combat
                            {
                                IsBossLevel = true,
                                EnemyGroup = new Group
                                {
                                    Units = new[] {
                                        new Unit{
                                            Hp = 1200,
                                            Name = "Enemy BOSS",
                                            Skills = new CombatSkill[]
                                            {
                                                new CombatSkill{
                                                    DamageMin = 10,
                                                    DamageMax = 14
                                                }
                                            }
                                        }
                                    }
                                }
                            };
                        }
                        else
                        {
                            node.Combat = new Combat
                            {
                                EnemyGroup = new Group
                                {
                                    Units = new[] {
                                        new Unit{
                                            Hp = 20,
                                            Name = "Enemy",
                                            Skills = new CombatSkill[]
                                            {
                                                new CombatSkill{
                                                    DamageMin = 2,
                                                    DamageMax = 4
                                                }
                                            }
                                        },
                                        new Unit{
                                            Hp = 25,
                                            Name = "Enemy",
                                            Skills = new CombatSkill[]
                                            {
                                                new CombatSkill{
                                                    DamageMin = 1,
                                                    DamageMax = 2
                                                }
                                            }
                                        },
                                        new Unit{
                                            Hp = 15,
                                            Name = "Enemy",
                                            Skills = new CombatSkill[]
                                            {
                                                new CombatSkill{
                                                    DamageMin = 3,
                                                    DamageMax = 5
                                                }
                                            }
                                        }
                                    }
                                }
                            };
                        }
                    }
                }
            }
        }
    }
}
