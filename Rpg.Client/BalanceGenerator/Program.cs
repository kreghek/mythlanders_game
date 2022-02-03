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

            Iteration(balanceTable);
        }

        private static void Iteration(DynamicBalanceTable balanceTable)
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

            do
            {
                combat.Update();


                var attacker = combat.CurrentUnit.Unit;
                var skill = attacker.Skills[0];
                var target = combat.AliveUnits.Single(x => x.Unit != attacker);
                var targetSourceHitPoints = target.Unit.HitPoints;

                combat.UseSkill(skill, target);
            } while (!combat.Finished);
        }

        internal sealed class DynamicBalanceTable : IBalanceTable
        {
            private IDictionary<UnitName, BalanceTableRecord> _balanceDictionary;

            public void SetTable(IDictionary<UnitName, BalanceTableRecord> balanceDictionary)
            {
                _balanceDictionary = balanceDictionary;
            }

            public BalanceTableRecord GetRecord(UnitName unitName)
            {
                return _balanceDictionary[unitName];
            }
        }
    }
}
