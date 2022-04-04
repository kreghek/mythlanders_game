using FluentAssertions;

using NUnit.Framework;

using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Combat.Ui.CombatResultModalModels;

namespace Rpg.Client.Tests.GameScreens.Combat.Ui
{
    [TestFixture]
    public class AnimatedCountableUnitItemStatTests
    {
        [Test]
        public void Update_ResourceWasGathered_CurrentValueEqualsStartPlusRewardValues(
            [Values(0, 1, 2, 333, 1000, -1)]
            int rewardAmount,
            [Values(0, 1, 2, 333, 1000, -1)]
            int startAmount)
        {
            // ARRANGE

            var sourceReward = new ResourceReward
            {
                Amount = rewardAmount,
                StartValue = startAmount
            };

            var fact = new AnimatedCountableUnitItemStat(sourceReward);

            // ACT

            var fuseCounter = 0;

            while (!fact.IsComplete)
            {
                fact.Update();

                fuseCounter++;

                if (fuseCounter > 1000)
                {
                    Assert.Fail();
                }
            }

            // ASSERT
            fact.CurrentValue.Should().Be(startAmount + rewardAmount);
        }
    }
}