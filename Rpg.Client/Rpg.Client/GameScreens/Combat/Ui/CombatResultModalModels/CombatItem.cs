using System.Collections.Generic;

namespace Rpg.Client.GameScreens.Combat.Ui.CombatResultModalModels
{
    internal sealed class CombatItem
    {
        public CombatItem(AnimatedProgressionUnitItemStat biomeProgress,
            IReadOnlyCollection<AnimatedRewardItem> unitItems)
        {
            BiomeProgress = biomeProgress;
            UnitItems = unitItems;
        }

        public AnimatedProgressionUnitItemStat BiomeProgress { get; }
        public IReadOnlyCollection<AnimatedRewardItem> UnitItems { get; }

        public void Update()
        {
            BiomeProgress?.Update();

            UpdateUnitItems();
        }

        private void UpdateUnitItems()
        {
            foreach (var unitItem in UnitItems)
            {
                unitItem.Update();
            }
        }
    }
}