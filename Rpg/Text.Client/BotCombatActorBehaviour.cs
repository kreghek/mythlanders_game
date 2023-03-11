using Core.Combats;

namespace Text.Client;

internal sealed class BotCombatActorBehaviour : ICombatActorBehaviour
{
    public void HandleIntention(ICombatActorBehaviourData combatData, Action<IIntention> intentionDelegate)
    {
        var firstSkill = combatData.CurrentActor.Skills.First();

        var skillIntention = new UseCombatMovementIntention(firstSkill.CombatMovement);

        intentionDelegate(skillIntention);
    }

    public void HandleIntention(ICombatActorBehaviourData combatData, Action<IIntention> intentionDelegate)
    {
        throw new System.NotImplementedException();
    }
}
