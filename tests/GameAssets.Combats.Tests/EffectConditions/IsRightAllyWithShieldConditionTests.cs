using CombatDicesTeam.Combats;

using GameAssets.Combats.CombatantStatuses;

namespace GameAssets.Combats.EffectConditions.Tests;

[TestFixture()]
public class IsRightAllyWithShieldConditionTests
{
    [Test()]
    public void Check_passes_if_shield_right()
    {
        // ARRANGE

        var sut = new IsRightAllyWithShieldCondition();

        var actorMock = new Mock<ICombatant>();
        var actor = actorMock.Object;

        var allyMock = new Mock<ICombatant>();
        allyMock.SetupGet(x => x.Statuses).Returns(new[] { SystemStatuses.HasShield });
        var ally = allyMock.Object;


        var field = new CombatField();
        field.HeroSide[new FieldCoords(0, 1)].Combatant = actor;
        field.HeroSide[new FieldCoords(0, 2)].Combatant = ally;

        // ACT

        var fact = sut.Check(actor, field);

        // ASSERT

        fact.Should().BeTrue();
    }

    [Test()]
    public void Check_fails_then_shield_left()
    {
        // ARRANGE

        var sut = new IsRightAllyWithShieldCondition();

        var actorMock = new Mock<ICombatant>();
        var actor = actorMock.Object;

        var allyMock = new Mock<ICombatant>();
        allyMock.SetupGet(x => x.Statuses).Returns(new[] { SystemStatuses.HasShield });
        var ally = allyMock.Object;


        var field = new CombatField();
        field.HeroSide[new FieldCoords(0, 2)].Combatant = actor;
        field.HeroSide[new FieldCoords(0, 1)].Combatant = ally;

        // ACT

        var fact = sut.Check(actor, field);

        // ASSERT

        fact.Should().BeFalse();
    }

    [Test()]
    public void Check_fails_then_shield_behind()
    {
        // ARRANGE

        var sut = new IsRightAllyWithShieldCondition();

        var actorMock = new Mock<ICombatant>();
        var actor = actorMock.Object;

        var allyMock = new Mock<ICombatant>();
        allyMock.SetupGet(x => x.Statuses).Returns(new[] { SystemStatuses.HasShield });
        var ally = allyMock.Object;


        var field = new CombatField();
        field.HeroSide[new FieldCoords(0, 1)].Combatant = actor;
        field.HeroSide[new FieldCoords(1, 1)].Combatant = ally;

        // ACT

        var fact = sut.Check(actor, field);

        // ASSERT

        fact.Should().BeFalse();
    }

    [Test()]
    public void Check_fails_then_no_shield()
    {
        // ARRANGE

        var sut = new IsRightAllyWithShieldCondition();

        var actorMock = new Mock<ICombatant>();
        var actor = actorMock.Object;

        var allyMock = new Mock<ICombatant>();
        allyMock.SetupGet(x => x.Statuses).Returns(new[] { SystemStatuses.HasShield });
        var ally = allyMock.Object;


        var field = new CombatField();
        field.HeroSide[new FieldCoords(0, 1)].Combatant = actor;

        // ACT

        var fact = sut.Check(actor, field);

        // ASSERT

        fact.Should().BeFalse();
    }

    [Test()]
    public void Check_fails_then_actor_on_right_edge()
    {
        // ARRANGE

        var sut = new IsRightAllyWithShieldCondition();

        var actorMock = new Mock<ICombatant>();
        var actor = actorMock.Object;

        var allyMock = new Mock<ICombatant>();
        allyMock.SetupGet(x => x.Statuses).Returns(new[] { SystemStatuses.HasShield });
        var ally = allyMock.Object;


        var field = new CombatField();
        field.HeroSide[new FieldCoords(0, 2)].Combatant = actor;

        // ACT

        var fact = sut.Check(actor, field);

        // ASSERT

        fact.Should().BeFalse();
    }
}