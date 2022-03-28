namespace Rpg.Client.GameScreens.Combat.Ui.CombatResultModalModels
{
    internal sealed class AnimatedRewardItem
    {
        public AnimatedRewardItem(ResourceReward rewardItem)
        {
            Equipment = new AnimatedCountableUnitItemStat(rewardItem);
        }

        public AnimatedCountableUnitItemStat? Equipment { get; }

        public void Update()
        {
            Equipment?.Update();
        }
    }
}