using Core.Combats;

namespace Text.Client;

/// <summary>
/// Bot behaviour for text client.
/// </summary>
internal sealed class BotCombatActorBehaviour : ICombatActorBehaviour
{
    /// <inheritdoc/>
    public void HandleIntention(ICombatActorBehaviourData combatData, Action<IIntention> intentionDelegate)
    {
        var firstSkill = combatData.CurrentActor.Skills.First();

        var skillIntention = new UseCombatMovementIntention(firstSkill.CombatMovement);

        intentionDelegate(skillIntention);
    }
}
