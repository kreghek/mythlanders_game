using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.Dices;
using CombatDicesTeam.GenericRanges;

using Core.Combats.TargetSelectors;

using GameAssets.Combats.CombatMovementEffects;

namespace GameAssets.Combats.Tests;

public class MythlandersCombatEngineTests
{
    [Test]
    public void CreateCombatMovementExecution_AttackToAutoDefence_AutoDefenceEffectsInfluencedOnTarget()
    {
        // ASSERT

        var attackCombatMovementInstance = new CombatMovementInstance(
            new CombatMovement(
                new CombatMovementSid("test"),
                new CombatMovementCost(0),
                CombatMovementEffectConfig.Create(new[]
                {
                    new DamageEffectWrapper(
                        new WeakestEnemyTargetSelector(),
                        DamageType.Normal,
                        GenericRange<int>.CreateMono(1))
                }))
            {
                Tags = CombatMovementTags.Attack
            });

        var autoDefenceMovement = new CombatMovement(
            new CombatMovementSid("test_auto_defence"),
            new CombatMovementCost(0),
            new CombatMovementEffectConfig(ArraySegment<IEffect>.Empty,
                new[]
                {
                    new ChangeStatEffect(
                        new CombatantStatusSid("test_auto_defence"),
                        new SelfTargetSelector(),
                        CombatantStatTypes.Defense,
                        1,
                        new ToEndOfCurrentRoundEffectLifetimeFactory())
                }))
        {
            Tags = CombatMovementTags.AutoDefense
        };
        var autoDefenceCombatMovementInstance = new CombatMovementInstance(
            autoDefenceMovement);

        var hero = CreateCombatant(attackCombatMovementInstance, true);
        var monsterMock = CreateCombatantMock(autoDefenceCombatMovementInstance, false);
        monsterMock.SetupGet(x => x.DebugSid).Returns("monster");

        var roundQueueResolverMock = new Mock<IRoundQueueResolver>();
        roundQueueResolverMock.Setup(x => x.GetCurrentRoundQueue(It.IsAny<IReadOnlyCollection<ICombatant>>()))
            .Returns(new[] { hero, monsterMock.Object });
        var roundQueueResolver = roundQueueResolverMock.Object;

        var combatEngine = new MythlandersCombatEngine(roundQueueResolver, Mock.Of<IDice>());
        combatEngine.Initialize(new[]
            {
                // hero
                new FormationSlot(0, 0) { Combatant = hero }
            },
            new[]
            {
                // monster
                new FormationSlot(0, 0) { Combatant = monsterMock.Object }
            });

        // ACT

        var execution = combatEngine.CreateCombatMovementExecution(attackCombatMovementInstance);

        // ASSERT

        foreach (var effectImposeItem in execution.EffectImposeItems)
        {
            foreach (var materializedTarget in effectImposeItem.MaterializedTargets)
            {
                effectImposeItem.ImposeDelegate(materializedTarget);
            }
        }

        monsterMock.Verify(x =>
            x.AddStatus(It.Is<ICombatantStatus>(status => status.Sid.ToString() == "test_auto_defence"),
                It.IsAny<ICombatantStatusImposeContext>(), It.IsAny<ICombatantStatusLifetimeImposeContext>()));
    }

    [Test]
    public void CreateCombatMovementExecution_CombatMovementInHand_CombatMovementRemovedFromHand()
    {
        // ASSERT

        var combatMovementInstance = new CombatMovementInstance(
            new CombatMovement(
                new CombatMovementSid("test"),
                new CombatMovementCost(0),
                CombatMovementEffectConfig.Create(ArraySegment<IEffect>.Empty)));

        var hero = CreateCombatant(combatMovementInstance, true);
        var monster = CreateCombatant(combatMovementInstance, false);

        var roundQueueResolverMock = new Mock<IRoundQueueResolver>();
        roundQueueResolverMock.Setup(x => x.GetCurrentRoundQueue(It.IsAny<IReadOnlyCollection<ICombatant>>()))
            .Returns(new[] { hero });
        var roundQueueResolver = roundQueueResolverMock.Object;

        var combatEngine = new MythlandersCombatEngine(roundQueueResolver, Mock.Of<IDice>());
        combatEngine.Initialize(new[]
            {
                // hero
                new FormationSlot(0, 0) { Combatant = hero }
            },
            new[]
            {
                // monster
                new FormationSlot(0, 0) { Combatant = monster }
            });

        // ACT

        var _ = combatEngine.CreateCombatMovementExecution(combatMovementInstance);

        // ASSERT

        var factCombatMovements = hero.CombatMovementContainers.Single(x => x.Type == CombatMovementContainerTypes.Hand)
            .GetItems().ToArray();
        factCombatMovements[0].Should().BeNull();
    }

    private static ICombatant CreateCombatant(CombatMovementInstance combatMovementInstance, bool b)
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
            .Returns(new[]
            {
                Mock.Of<ICombatantStat>(x => x.Type == CombatantStatTypes.HitPoints && x.Value == new StatValue(1)),
                Mock.Of<ICombatantStat>(x => x.Type == CombatantStatTypes.ShieldPoints && x.Value == new StatValue(1)),
                Mock.Of<ICombatantStat>(x => x.Type == CombatantStatTypes.Defense && x.Value == new StatValue(0))
            });

        combatantMock.SetupGet(x => x.IsPlayerControlled).Returns(b);

        return combatantMock.Object;
    }

    private static Mock<ICombatant> CreateCombatantMock(CombatMovementInstance combatMovementInstance, bool b)
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
            .Returns(new[]
            {
                Mock.Of<ICombatantStat>(x => x.Type == CombatantStatTypes.HitPoints && x.Value == new StatValue(1)),
                Mock.Of<ICombatantStat>(x => x.Type == CombatantStatTypes.ShieldPoints && x.Value == new StatValue(1)),
                Mock.Of<ICombatantStat>(x => x.Type == CombatantStatTypes.Defense && x.Value == new StatValue(0))
            });

        combatantMock.SetupGet(x => x.IsPlayerControlled).Returns(b);

        return combatantMock;
    }
}