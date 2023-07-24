using System.Collections.Generic;
using System.Linq;

using Core.Combats;

using GameAssets.Combats;

namespace Client.GameScreens.Combat;

public sealed class CombatUnitBehaviourData : ICombatActorBehaviourData
{
    public CombatUnitBehaviourData(CombatEngineBase combat)
    {
        CurrentActor =
            new CombatUnitBehaviourDataActor(
                GetHand(combat.CurrentCombatant).Where(x => x is not null)
                    .Select(skill => new CombatActorBehaviourDataSkill(skill!)).ToArray());

        Actors = combat.Field.HeroSide.GetAllCombatants().Concat(combat.Field.MonsterSide.GetAllCombatants())
            .Where(actor => actor != combat.CurrentCombatant).Select(actor =>
                new CombatUnitBehaviourDataActor(
                    GetHand(actor).Where(x => x is not null).Select(skill => new CombatActorBehaviourDataSkill(skill!))
                        .ToArray()))
            .ToArray();
    }

    public CombatUnitBehaviourDataActor CurrentActor { get; }
    public IReadOnlyCollection<CombatUnitBehaviourDataActor> Actors { get; }

    private IReadOnlyList<CombatMovementInstance?> GetHand(ICombatant combatant)
    {
        return combatant.CombatMovementContainers.Single(x => x.Type == CombatMovementContainerTypes.Hand).GetItems()
            .ToArray();
    }
}