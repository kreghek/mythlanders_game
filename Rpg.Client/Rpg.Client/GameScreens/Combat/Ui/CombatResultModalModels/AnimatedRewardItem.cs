namespace Rpg.Client.GameScreens.Combat.Ui.CombatResultModalModels
{
    internal sealed class AnimatedRewardItem
    {
        public AnimatedRewardItem(CombatRewardsItem rewardItem)
        {
            Equipment = new AnimatedCountableUnitItemStat(rewardItem.Xp);
        }

        public AnimatedCountableUnitItemStat? Equipment { get; }

        public void Update()
        {
            Equipment?.Update();
        }
    }
}