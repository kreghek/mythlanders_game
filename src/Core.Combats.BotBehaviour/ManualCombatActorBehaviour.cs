using CombatDicesTeam.Combats;

namespace Core.Combats.BotBehaviour;

public sealed class ManualCombatActorBehaviour : ICombatActorBehaviour
{
    private Action<IIntention>? _intentionDelegate;

    public void Assign(IIntention intention)
    {
        if (_intentionDelegate is null)
        {
            throw new InvalidOperationException();
        }

        _intentionDelegate(intention);
    }

    /// <inheritdoc />
    public void HandleIntention(ICombatantBehaviourData combatData, Action<IIntention> intentionDelegate)
    {
        _intentionDelegate = intentionDelegate;
    }
}