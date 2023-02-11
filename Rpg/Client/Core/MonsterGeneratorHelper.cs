using System.Collections.Generic;
using System.Linq;

using Core.Dices;

using Rpg.Client.Assets.Perks;

namespace Rpg.Client.Core
{
    internal static class MonsterGeneratorHelper
    {
        public static IReadOnlyList<Unit> CreateMonsters(GlobeNode node, IDice dice, int monsterLevel,
            IUnitSchemeCatalog unitSchemeCatalog, IMonsterGenerationGlobeContext globeContext)
        {
            var availableAllRegularMonsters =
                unitSchemeCatalog.AllMonsters.Where(x => !HasPerk<BossMonster>(x, monsterLevel));
            var availableAllBossMonsters = unitSchemeCatalog.AllMonsters.Where(x =>
                HasPerk<BossMonster>(x, monsterLevel) &&
                x.MinRequiredBiomeLevel is not null &&
                x.MinRequiredBiomeLevel.Value <= globeContext.GlobeProgressLevel);

            var allMonsters = availableAllRegularMonsters.Concat(availableAllBossMonsters);

            var filteredByBiomeMonsters = allMonsters;

            var filteredByLocationMonsters = filteredByBiomeMonsters.Where(x =>
                (x.LocationSids is null) || (x.LocationSids is not null && x.LocationSids.Contains(node.Sid)));

            var availableMonsters = filteredByLocationMonsters.ToList();

            if (availableMonsters.Any(x => HasPerk<BossMonster>(x, monsterLevel)))
            {
                // This location for a boss.
                // Boss has the highest priority so generate only one boss and ignore other units.
                var bossScheme = availableMonsters.Single(x => HasPerk<BossMonster>(x, monsterLevel));
                var unit = new Unit(bossScheme, monsterLevel);
                return new[] { unit };
            }

            var rolledUnits = new List<UnitScheme>();

            var predefinedMinMonsterCounts = GetPredefinedMonsterCounts(globeContext.GlobeProgressLevel);
            var predefinedMinMonsterCount = dice.RollFromList(predefinedMinMonsterCounts, 1).Single();
            var monsterCount = GetMonsterCount(node, predefinedMinMonsterCount);

            for (var i = 0; i < monsterCount; i++)
            {
                if (!availableMonsters.Any())
                {
                    break;
                }

                var scheme = dice.RollFromList(availableMonsters, 1).Single();

                var isRegularMonster = !HasPerk<BossMonster>(scheme, monsterLevel);

                if (isRegularMonster)
                {
                    rolledUnits.Add(scheme);

                    if (scheme.IsUnique)
                    {
                        // Remove all unique monsters from roll list.
                        availableMonsters.RemoveAll(x => x.IsUnique);
                    }
                }
                else
                {
                    // A big monster is alone on a battleground.
                    rolledUnits.Clear();
                    rolledUnits.Add(scheme);
                    break;
                }
            }

            var units = new List<Unit>();
            foreach (var unitScheme in rolledUnits)
            {
                var unitLevel = GetUnitLevel(monsterLevel);
                var unit = new Unit(unitScheme, unitLevel);
                units.Add(unit);
            }

            return units;
        }

        private static int GetMonsterCount(GlobeNode node,
            int predefinedMinMonsterCount)
        {
            //if (node.IsLast /* && !biome.IsComplete*/)
            //{
            //    return 1;
            //}

            return predefinedMinMonsterCount;
        }

        private static int[] GetPredefinedMonsterCounts(int globeProgressLevel)
        {
            return globeProgressLevel switch
            {
                >= 0 and <= 10 => new[] { 2, 3 },
                > 10 => new[] { 3, 3, 4, 4, 5 },
                _ => new[] { 3 }
            };
        }

        private static int GetUnitLevel(int combatLevel)
        {
            // +1 because combat starts with zero.
            // But a unit's level have to starts with 1.
            return combatLevel + 1;
        }

        private static bool HasPerk<TPerk>(UnitScheme unitScheme, int combatLevel)
        {
            var unit = new Unit(unitScheme, combatLevel);
            return unit.Perks.OfType<TPerk>().Any();
        }
    }
}