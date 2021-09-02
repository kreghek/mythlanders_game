﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core
{
    internal sealed class Globe
    {
        public Globe()
        {
            var biomNames = new Dictionary<string, string[]>
            {
                {
                    "Slavik", new[]
                    {
                        "Поле брани", "Дикое болото", "Черные топи", "Лес колдуна", "Нечистивая\nяма",
                        "Мыс страха", "Тропа\nпогибели", "Кладбише\nпроклятых", "Выжженая\nдеревня", "Холм тлена"
                    }
                },
                {
                    "China", new[]
                    {
                        "Поле брани", "Дикое болото", "Черные топи", "Лес колдуна", "Нечистивая\nяма",
                        "Мыс страха", "Тропа\nпогибели", "Кладбише\nпроклятых", "Выжженая\nдеревня", "Холм тлена"
                    }
                }
            };

            var biomes = new[]
            {
                new Biom
                {
                    Name = "Slavik",
                    IsAvailable = true,
                    Nodes = Enumerable.Range(0, 10).Select(x =>
                        new GlobeNode
                        {
                            Index = x,
                            Name = biomNames["Slavik"][x]
                        }
                    ).ToArray(),
                    UnlockBiom = "China"
                },
                new Biom
                {
                    Name = "China",
                    Nodes = Enumerable.Range(0, 10).Select(x =>
                        new GlobeNode
                        {
                            Index = x,
                            Name = biomNames["China"][x]
                        }
                    ).ToArray()
                }
            };

            Bioms = biomes;
            CurrentBiom = biomes[0];
        }

        public ActiveCombat? ActiveCombat { get; set; }

        public IEnumerable<Biom> Bioms { get; }

        public Biom? CurrentBiom { get; set; }

        public bool IsNodeInitialied { get; set; }

        public Player? Player { get; set; }
        public Dialog AvailableDialog { get; internal set; }

        public void UpdateNodes(IDice dice)
        {
            // Reset all combat states.
            var bioms = Bioms.Where(x => x.IsAvailable).ToArray();
            foreach (var biom in bioms)
            {
                foreach (var node in biom.Nodes)
                {
                    node.Combat = null;
                    node.AvailableDialog = null;
                }

                if (biom.IsComplete && biom.UnlockBiom is not null)
                {
                    var unlockedBiom = Bioms.Single(x => x.Name == biom.UnlockBiom);

                    unlockedBiom.IsAvailable = true;
                }
            }

            // Create new combats
            foreach (var biom in bioms)
            {
                if (biom.Level < 10)
                {
                    var nodesWithCombats = dice.RollFromList(biom.Nodes.ToList(), 3).ToArray();
                    var combatLevelAdditional = 0;
                    foreach (var node in nodesWithCombats)
                    {
                        var combatLevel = biom.Level + combatLevelAdditional;
                        var units = CreateReqularMonsters(node, dice, biom, combatLevel);

                        node.Combat = new Combat
                        {
                            Level = combatLevel,
                            EnemyGroup = new Group
                            {
                                Units = units
                            }
                        };

                        combatLevelAdditional++;
                    }
                }
                else
                {
                    var combatLevelAdditional = 0;

                    var nodesWithCombats = dice.RollFromList(biom.Nodes.ToList(), 3).ToArray();
                    foreach (var node in nodesWithCombats)
                    {
                        // boss level
                        if (node == nodesWithCombats.First())
                        {
                            var bossUnitScheme =
                                dice.RollFromList(
                                        UnitSchemeCatalog.AllUnits.Where(x => x.IsBoss && x.Biom == biom.Name).ToList(),
                                        1)
                                    .Single();
                            node.Combat = new Combat
                            {
                                IsBossLevel = true,
                                EnemyGroup = new Group
                                {
                                    Units = new[]
                                    {
                                        new Unit(
                                            bossUnitScheme,
                                            biom.Level)
                                    }
                                }
                            };
                        }
                        else
                        {
                            var combatLevel = biom.Level + combatLevelAdditional;
                            var units = CreateReqularMonsters(node, dice, biom, combatLevel);

                            node.Combat = new Combat
                            {
                                Level = combatLevel,
                                EnemyGroup = new Group
                                {
                                    Units = units
                                }
                            };
                        }

                        combatLevelAdditional++;
                    }
                }
            }

            // create dialogs of nodes with combat
            var nodesWithCombat = bioms.SelectMany(x => x.Nodes).Where(x => x.Combat is not null).ToArray();
            foreach (var node in nodesWithCombat)
            {
                var roll = dice.Roll(1, 10);
                if (roll > 5)
                {
                    node.AvailableDialog = dice.RollFromList(DialogCatalog.Dialogs.ToList(), 1).Single();
                }
            }
        }

        private static IEnumerable<Unit> CreateReqularMonsters(GlobeNode node, IDice dice, Biom biom, int combatLevel)
        {
            var availableMonsters = UnitSchemeCatalog.AllUnits
                .Where(x => !x.IsBoss && x.Biom == biom.Name && x.NodeIndexes.Contains(node.Index)).ToList();
            var rolledUnits = dice.RollFromList(availableMonsters, dice.Roll(1, Math.Min(3, availableMonsters.Count)));

            var uniqueIsUsed = false;
            var units = new List<Unit>();
            foreach (var unitScheme in rolledUnits)
            {
                if (unitScheme.IsUnique)
                {
                    if (uniqueIsUsed)
                    {
                        continue;
                    }

                    uniqueIsUsed = true;
                }

                var unit = new Unit(unitScheme, combatLevel);
                units.Add(unit);
            }

            return units;
        }
    }
}