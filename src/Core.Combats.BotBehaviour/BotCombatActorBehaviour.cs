using CombatDicesTeam.Combats;

namespace Core.Combats.BotBehaviour;

/// <summary>
/// Bot behaviour for combat.
/// </summary>
public sealed class BotCombatActorBehaviour : ICombatActorBehaviour
{
    private readonly IIntentionFactory _intentionFactory;

    public BotCombatActorBehaviour(IIntentionFactory intentionFactory)
    {
        _intentionFactory = intentionFactory;
    }

    /// <inheritdoc />
    public void HandleIntention(ICombatActorBehaviourData combatData, Action<IIntention> intentionDelegate)
    {
        var firstSkill = combatData.CurrentActor.Skills.First();

        var skillIntention = _intentionFactory.CreateCombatMovement(firstSkill.CombatMovement);

        intentionDelegate(skillIntention);
    }
}