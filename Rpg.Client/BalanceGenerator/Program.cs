using System;
using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Assets;
using Rpg.Client.Core;

namespace BalanceGenerator
{
    internal class Program
    {
        private static CombatSource CreateCombatSource(GlobeNode globeNode, Biome biome, LinearDice dice,
            UnitSchemeCatalog unitSchemeCatalog)
        {
            var combatSource = new CombatSource
            {
                EnemyGroup = new Group()
            };

            var monsters = MonsterGeneratorHelper.CreateMonsters(globeNode, dice, biome, 1, unitSchemeCatalog);
            for (var i = 0; i < monsters.Count; i++)
            {
                var monster = monsters[i];
                combatSource.EnemyGroup.Slots[i].Unit = monster;
            }

            return combatSource;
        }

        private static void Main(string[] args)
        {
            var balanceTable = new DynamicBalanceTable();

            balanceTable.SetTable(new Dictionary<UnitName, BalanceTableRecord>());
            var results = new List<ItemrationResult>();

            var globeNode = new GlobeNode
            { Sid = GlobeNodeSid.Thicket };
            var dice = new LinearDice();
            var biome = new Biome(1, BiomeType.Slavic);
            var unitSchemeCatalog = new UnitSchemeCatalog(balanceTable);

            for (var i = 0; i < 10; i++)
            {
                var combatSource = CreateCombatSource(globeNode, biome, dice, unitSchemeCatalog);
                var result = PlayIteration(balanceTable, globeNode, biome, dice, combatSource);
                results.Add(result);
            }

            foreach (var item in results)
            {
                Console.WriteLine(item.RoundCount);
            }
        }

        private static ItemrationResult PlayIteration(DynamicBalanceTable balanceTable, GlobeNode globeNode,
            Biome biome, IDice dice, CombatSource combatSource)
        {
            var unitSchemeCatalog = new UnitSchemeCatalog(balanceTable);

            var playerGroup = new Group();
            var playerUnitScheme = unitSchemeCatalog.Heroes[UnitName.Berimir];

            playerGroup.Slots[0].Unit = new Unit(playerUnitScheme, 1) { IsPlayerControlled = true };

            var combat = new Combat(playerGroup, globeNode, combatSource, biome, dice,
                isAutoplay: true);

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

                /*var attacker = combat.CurrentUnit.Unit;
                var skill = attacker.Skills[0];
                var target = combat.AliveUnits.Single(x => x.Unit != attacker);
                var targetSourceHitPoints = target.Unit.HitPoints;

                combat.UseSkill(skill, target);*/

                if (roundIndex > 10)
                {
                    // Fuse against infinite combats.
                    roundIndex = int.MaxValue;
                    break;
                }
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
        }
    }
}