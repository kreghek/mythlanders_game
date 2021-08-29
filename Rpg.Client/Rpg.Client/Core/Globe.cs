using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core
{
    internal class Globe
    {
        public Globe()
        {
            Nodes = Enumerable.Range(1, 10).Select(x=>
                new GlobeNode
                {
                    Index = x
                }
            ).ToArray();
        }

        public Group PlayerGroup { get; set; }

        public ActiveCombat? ActiveCombat { get; set; }

        public IEnumerable<GlobeNode> Nodes { get; private set; }

        public bool IsNodeInitialied { get; set; }



        public void UpdateNodes(IDice dice)
        {
            // Reset all combat states.
            foreach (var node in Nodes)
            {
                node.Combat = null;
            }

            // Create new combats
            var nodesWithCombats = dice.RollFromList(Nodes.ToList(), 3);
            foreach (var node in nodesWithCombats)
            {
                node.Combat = new Combat
                {
                    EnemyGroup = new Group { 
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
