using Client.GameScreens.Combat;
using Client.GameScreens.Combat.Ui.CombatResultModalModels;

using FluentAssertions;

using NUnit.Framework;

namespace Client.Tests.GameScreens.Combat.Ui;

[TestFixture]
public class AnimatedCountableUnitItemStatTests
{
    [Test]
    public void Update_ResourceWasGathered_CurrentValueEqualsStartPlusRewardValues(
        [Values(0, 1, 2, 333, 1000, -1)] int rewardAmount,
        [Values(0, 1, 2, 333, 1000, -1)] int startAmount)
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