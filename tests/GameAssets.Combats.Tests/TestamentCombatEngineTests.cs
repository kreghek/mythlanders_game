using CombatDicesTeam.Combats;
using CombatDicesTeam.Dices;

using FluentAssertions;

using Moq;

using NUnit.Framework;

namespace GameAssets.Combats.Tests;

public class TestamentCombatEngineTests
{
    [Test]
    public void CreateCombatMovementExecution_CombatMovementInHand_CombatMovementRemovedFromHand()
    {
        // ASSERT

        var combatMovementInstance = new CombatMovementInstance(
           new CombatMovement(
               new CombatMovementSid("1"),
               new CombatMovementCost(0),
               CombatMovementEffectConfig.Create(ArraySegment<IEffect>.Empty)));

        var hero = CreateCombatant(combatMovementInstance);
        var monster = CreateCombatant(combatMovementInstance);

        var roundQueueResolverMock = new Mock<IRoundQueueResolver>();
        roundQueueResolverMock.Setup(x => x.GetCurrentRoundQueue(It.IsAny<IReadOnlyCollection<ICombatant>>()))
            .Returns(new[] { hero });
        var roundQueueResolver = roundQueueResolverMock.Object;

        var combatEngine = new TestamentCombatEngine(roundQueueResolver, Mock.Of<IDice>());
        combatEngine.Initialize(new[]
            {
                // hero
                new FormationSlot(0, 0) { Combatant = hero },
            },
            new[]
            {
                // monster
                new FormationSlot(0, 0) { Combatant = monster },
            });

        // ACT

        combatEngine.CreateCombatMovementExecution(combatMovementInstance);

        // ASSERT

        var factCombatMovements = hero.CombatMovementContainers.Single(x => x.Type == CombatMovementContainerTypes.Hand).GetItems().ToArray();
        factCombatMovements[0].Should().BeNull();
    }

    private static ICombatant CreateCombatant(CombatMovementInstance combatMovementInstance)
    {
        var combatantMock = new Mock<ICombatant>();
        var handContainer = new CombatMovementContainer(CombatMovementContainerTypes.Hand);

        handContainer.AppendMove(combatMovementInstance);

        var poolContainer = new CombatMovementContainer(CombatMovementContainerTypes.Pool);

        combatantMock.Setup(x => x.CombatMovementContainers).Returns(new[]
        {
            handContainer,
            poolContainer
        });

        combatantMock.Setup(x => x.Stats)
            .Returns(new[] {
                Mock.Of<IUnitStat>(x=>x.Type == CombatantStatTypes.HitPoints && x.Value == new StatValue(1)),
                Mock.Of<IUnitStat>(x=>x.Type == CombatantStatTypes.ShieldPoints && x.Value == new StatValue(1))
            });

        return combatantMock.Object;
    }
}