using CombatDicesTeam.Combats;
using CombatDicesTeam.Dices;

using Moq;

using NUnit.Framework;

namespace GameAssets.Combats.Tests;

public class TestamentCombatEngineTests
{
    [Test]
    public void CreateCombatMovementExecution_CombatMovementInHand_CombatMovementRemovedFromHand()
    {
        // ASSERT

        var heroMock = new Mock<ICombatant>();
        var handContainer = new CombatMovementContainer(CombatMovementContainerTypes.Hand);
        var combatMovementInstance = new CombatMovementInstance(
            new CombatMovement(
                new CombatMovementSid("1"), 
                new CombatMovementCost(0), 
                CombatMovementEffectConfig.Create(ArraySegment<IEffect>.Empty)));
        handContainer.AppendMove(combatMovementInstance);
        heroMock.Setup(x => x.CombatMovementContainers).Returns(new[]
        {
            handContainer
        });
        
        
        var hero = heroMock.Object;
        
        var combatEngine = new TestamentCombatEngine(Mock.Of<IDice>());
        combatEngine.Initialize(new[]
            {
                // hero
                new FormationSlot(0, 0) { Combatant = hero },
            },
            new[]
            {
                // monster
                new FormationSlot(0, 0) { Combatant = Mock.Of<ICombatant>() },
            });

        // ACT

        combatEngine.CreateCombatMovementExecution(combatMovementInstance);
    }
}