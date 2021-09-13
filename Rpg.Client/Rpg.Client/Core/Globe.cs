using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Rpg.Client.Core
{
    internal sealed class Globe
    {
        public Globe()
        {
            var biomNames = new Dictionary<BiomeType, string[]>
            {
                {
                    BiomeType.Slavic, new[]
                    {
                        "Поле брани", "Дикое болото", "Черные топи", "Лес колдуна", "Нечистивая\nяма",
                        "Мыс страха", "Тропа\nпогибели", "Кладбише\nпроклятых", "Выжженая\nдеревня", "Холм тлена"
                    }
                },
                {
                    BiomeType.China, new[]
                    {
                        "Поле брани", "Дикое болото", "Черные топи", "Лес колдуна", "Нечистивая\nяма",
                        "Мыс страха", "Тропа\nпогибели", "Кладбише\nпроклятых", "Выжженая\nдеревня", "Холм тлена"
                    }
                },
                {
                    BiomeType.Egypt, new[]
                    {
                        "Поле брани", "Дикое болото", "Черные топи", "Лес колдуна", "Нечистивая\nяма",
                        "Мыс страха", "Тропа\nпогибели", "Кладбише\nпроклятых", "Выжженая\nдеревня", "Холм тлена"
                    }
                },
                {
                    BiomeType.Greek, new[]
                    {
                        "Поле брани", "Дикое болото", "Черные топи", "Лес колдуна", "Нечистивая\nяма",
                        "Мыс страха", "Тропа\nпогибели", "Кладбише\nпроклятых", "Выжженая\nдеревня", "Холм тлена"
                    }
                }
            };
            var biomes = GenerateBiomes(biomNames);

            Bioms = biomes;
            CurrentBiome = biomes.Single(x => x.IsStartBiom);
        }

        [JsonIgnore]
        public ActiveCombat? ActiveCombat { get; set; }

        // TODO Save dialog id to fix nullification of active dialog after saves.
        [JsonIgnore]
        public Event? AvailableDialog { get; internal set; }

        public IEnumerable<Biom> Bioms { get; }

        [JsonIgnore]
        public Biom? CurrentBiome { get; set; }

        public bool IsNodeInitialied { get; set; }

        public Player? Player { get; set; }

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
                    var unlockedBiom = Bioms.Single(x => x.Type == biom.UnlockBiom);

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
                                        UnitSchemeCatalog.AllUnits.Where(x => x.IsBoss && x.Biom == biom.Type).ToList(),
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
            // TODO Use Counter to get unused dialogs first.
            var availableDialogs = EventCatalog.Dialogs.Where(x => (x.IsUnique && x.Counter == 0) || (!x.IsUnique))
                .OrderBy(x => x.Counter);
            foreach (var node in nodesWithCombat)
            {
                var availableDialogsList = availableDialogs.ToList();
                var roll = dice.Roll(1, 10);
                if (roll > 5)
                {
                    node.AvailableDialog = dice.RollFromList(availableDialogsList, 1).Single();
                    availableDialogsList.Remove(node.AvailableDialog);
                }
            }
        }

        private static IEnumerable<Unit> CreateReqularMonsters(GlobeNode node, IDice dice, Biom biom, int combatLevel)
        {
            var availableMonsters = UnitSchemeCatalog.AllUnits
                .Where(x => !x.IsBoss && x.Biom == biom.Type && x.NodeIndexes.Contains(node.Index)).ToList();
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

                var unitLevel = combatLevel + 1; //+1 because combat starts with zero.
                var unit = new Unit(unitScheme, unitLevel);
                units.Add(unit);
            }

            return units;
        }

        private static Biom[] GenerateBiomes(Dictionary<BiomeType, string[]> biomNames)
        {
            return new[]
            {
                new Biom
                {
                    Type = BiomeType.Slavic,
                    IsAvailable = true,
                    Nodes = Enumerable.Range(0, 10).Select(x =>
                        new GlobeNode
                        {
                            Index = x,
                            Name = biomNames[BiomeType.Slavic][x]
                        }
                    ).ToArray(),
                    UnlockBiom = BiomeType.China,
                    IsStartBiom = true
                },
                new Biom
                {
                    Type = BiomeType.China,
                    Nodes = Enumerable.Range(0, 10).Select(x =>
                        new GlobeNode
                        {
                            Index = x,
                            Name = biomNames[BiomeType.China][x]
                        }
                    ).ToArray(),
                    UnlockBiom = BiomeType.Egypt
                },
                new Biom
                {
                    Type = BiomeType.Egypt,
                    Nodes = Enumerable.Range(0, 10).Select(x =>
                        new GlobeNode
                        {
                            Index = x,
                            Name = biomNames[BiomeType.Egypt][x]
                        }
                    ).ToArray(),
                    UnlockBiom = BiomeType.Greek
                },
                new Biom
                {
                    Type = BiomeType.Greek,
                    Nodes = Enumerable.Range(0, 10).Select(x =>
                        new GlobeNode
                        {
                            Index = x,
                            Name = biomNames[BiomeType.Greek][x]
                        }
                    ).ToArray(),
                    IsFinalBiom = true
                }
            };
        }
    }
}