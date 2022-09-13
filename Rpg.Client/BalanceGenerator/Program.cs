using System;
using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Assets;
using Rpg.Client.Assets.Catalogs;
using Rpg.Client.Core;

namespace BalanceGenerator
{
    internal static class Program
    {
        private static CombatSource CreateCombatSource(GlobeNode globeNode, IDice dice,
            UnitSchemeCatalog unitSchemeCatalog)
        {
            var combatSource = new CombatSource
            {
                EnemyGroup = new Group()
            };

            var globeLevel = new GlobeLevel { Level = 1 };

            var globeContext = new MonsterGenerationGlobeContext(globeLevel/*, new[] { new Biome(BiomeType.Slavic) }*/);

            var monsters =
                MonsterGeneratorHelper.CreateMonsters(globeNode, dice, 1, unitSchemeCatalog, globeContext);
            for (var i = 0; i < monsters.Count; i++)
            {
                var monster = monsters[i];
                combatSource.EnemyGroup.Slots[i].Unit = monster;
            }

            return combatSource;
        }

        private static void Main()
        {
            var balanceTable = new DynamicBalanceTable();

            balanceTable.SetTable(new Dictionary<UnitName, BalanceTableRecord>());
            var results = new List<IterationsResult>();

            var globeNode = new GlobeNode
            {
                Sid = GlobeNodeSid.Thicket
            };

            var dice = new LinearDice();
            var unitSchemeCatalog = new UnitSchemeCatalog(balanceTable, isDemo: false);

            for (var i = 0; i < 10; i++)
            {
                var combatSource = CreateCombatSource(globeNode, dice, unitSchemeCatalog);
                var result = PlayIteration(balanceTable, globeNode, dice, combatSource);
                results.Add(result);
            }

            foreach (var item in results)
            {
                Console.WriteLine($@"Rounds: {item.RoundCount}; Heroes victory: {item.IsHeroesVictory}");
            }
        }

        private static IterationsResult PlayIteration(IBalanceTable balanceTable, GlobeNode globeNode, IDice dice,
            CombatSource combatSource)
        {
            var unitSchemeCatalog = new UnitSchemeCatalog(balanceTable, isDemo: false);

            var playerGroup = new Group();
            var playerUnitScheme = unitSchemeCatalog.Heroes[UnitName.Swordsman];

            playerGroup.Slots[0].Unit = new Unit(playerUnitScheme, 1) { IsPlayerControlled = true };

            var combat = new Combat(playerGroup, globeNode, combatSource, dice, isAutoplay: true);

            combat.Initialize();

            combat.ActionGenerated += (_, args) =>
            {
                foreach (var skillRuleInteraction in args.Action.SkillRuleInteractions)
                {
                    foreach (var target in skillRuleInteraction.Targets)
                    {
                        skillRuleInteraction.Action(target);
                    }
                }

                args.Action.SkillComplete();
            };

            var isHeroesVictory = false;
            combat.Finish += (_, args) =>
            {
                isHeroesVictory = args.Victory;
            };

            var roundIndex = 1;
            combat.NextRoundStarted += (_, _) =>
            {
                // ReSharper disable once AccessToModifiedClosure
                // Justification: We need to count the rounds. We must to change outer variable.
                roundIndex++;
            };

            do
            {
                combat.Update();

                var attacker = combat.CurrentUnit;
                if (attacker is null)
                {
                    throw new InvalidOperationException("Current unit can't be null after the combat starts.");
                }

                var skill = attacker.CombatCards[0];
                var target = combat.AliveUnits.FirstOrDefault(x => x != attacker);

                if (target is null)
                {
                    throw new InvalidOperationException("The combat ends but Finished property is not equal true.");
                }

                combat.UseSkill(skill, target);

                if (roundIndex > 10)
                {
                    // Fuse against infinite combats.
                    break;
                }
            } while (!combat.Finished);

            return new IterationsResult
            {
                RoundCount = roundIndex,
                IsHeroesVictory = isHeroesVictory
            };
        }

        private sealed class IterationsResult
        {
            public bool IsHeroesVictory { get; init; }
            public int RoundCount { get; init; }
        }

        private sealed class DynamicBalanceTable : IBalanceTable
        {
            private readonly BalanceTable _balanceTable;
            private IDictionary<UnitName, BalanceTableRecord> _balanceDictionary;

            public DynamicBalanceTable()
            {
                _balanceTable = new BalanceTable();
            }

            public void SetTable(IDictionary<UnitName, BalanceTableRecord> balanceDictionary)
            {
                _balanceDictionary = balanceDictionary;
            }

            public BalanceTableRecord GetRecord(UnitName unitName)
            {
                if (_balanceDictionary.TryGetValue(unitName, out var record))
                {
                    return record;
                }

                return _balanceTable.GetRecord(unitName);
            }

            public CommonUnitBasics GetCommonUnitBasics()
            {
                return new CommonUnitBasics();
            }
        }
    }
}