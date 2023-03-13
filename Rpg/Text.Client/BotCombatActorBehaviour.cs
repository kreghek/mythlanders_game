using Core.Combats;

namespace Text.Client;

internal sealed class BotCombatActorBehaviour : ICombatActorBehaviour
{
    void ICombatActorBehaviour.HandleIntention(ICombatActorBehaviourData combatData, Action<IIntention> intentionDelegate)
    {
        var firstSkill = combatData.CurrentActor.Skills.First();

        var skillIntention = new UseCombatMovementIntention(firstSkill.CombatMovement);

        intentionDelegate(skillIntention);
    }
}
