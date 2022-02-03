using System;
using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Assets;
using Rpg.Client.Core;

namespace BalanceGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var balanceTable = new DynamicBalanceTable();

            balanceTable.SetTable(new Dictionary<UnitName, BalanceTableRecord>());
            var results = new List<ItemrationResult>();
            for (var i = 0; i < 10; i++)
            {
                var result = Iteration(balanceTable);
                results.Add(result);
            }

            foreach (var item in results)
            {
                Console.WriteLine(item.RoundCount);
            }
        }

        private static ItemrationResult Iteration(DynamicBalanceTable balanceTable)
        {
            var unitSchemeCatalog = new UnitSchemeCatalog(balanceTable);

            var playerGroup = new Group();
            var playerUnitScheme = unitSchemeCatalog.Heroes[UnitName.Berimir];

            playerGroup.Slots[0].Unit = new Unit(playerUnitScheme, 1) { IsPlayerControlled = true };

            var globeNode = new GlobeNode();

            var monsterScheme = unitSchemeCatalog.AllMonsters.First(x => x.Name == UnitName.GreyWolf);
            var combatSource = new CombatSource
            {
                EnemyGroup = new Group()
            };
            combatSource.EnemyGroup.Slots[0].Unit = new Unit(monsterScheme, 1) { IsPlayerControlled = false };

            var dice = new LinearDice();

            var combat = new Combat(playerGroup, globeNode, combatSource, new Biome(0, BiomeType.Slavic), dice,
                isAutoplay: false);

            combat.Initialize();

            combat.ActionGenerated += (_, args) =>
            {
                args.Action();
            };

            var roundIndex = 1;
            combat.NextRoundStarted += (_, _) =>
            {
                roundIndex++;
            };

            do
            {
                combat.Update();

                var attacker = combat.CurrentUnit.Unit;
                var skill = attacker.Skills[0];
                var target = combat.AliveUnits.Single(x => x.Unit != attacker);
                var targetSourceHitPoints = target.Unit.HitPoints;

                combat.UseSkill(skill, target);
            } while (!combat.Finished);

            return new ItemrationResult
            {
                RoundCount = roundIndex
            };
        }

        internal sealed class ItemrationResult
        {
            public int RoundCount { get; init; }
        }

        internal sealed class DynamicBalanceTable : IBalanceTable
        {
            private IDictionary<UnitName, BalanceTableRecord> _balanceDictionary;
            private readonly BalanceTable _balanceTable;

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
                else
                {
                    return _balanceTable.GetRecord(unitName);
                }
            }
        }
    }
}
