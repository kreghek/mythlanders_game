using System.Collections.Generic;
using System.Linq;

using CombatDicesTeam.Combats;

using GameAssets.Combats;

namespace Client.GameScreens.Combat;

public sealed class CombatUnitBehaviourData : ICombatantBehaviourData
{
    public CombatUnitBehaviourData(CombatEngineBase combat)
    {
        CurrentActor =
            new CombatantBehaviourData(
                GetHand(combat.CurrentCombatant).Where(x => x is not null)
                    .Select(skill => new CombatantMoveBehaviourData(skill!)).ToArray());

        var allOtherCombatants = combat.Field.HeroSide.GetAllCombatants().Concat(combat.Field.MonsterSide.GetAllCombatants())
            .Where(actor => actor != combat.CurrentCombatant)
            .ToArray();

        Actors = allOtherCombatants
            .Select(actor => CreateCombatantData(actor))
            .ToArray();
    }

    private static CombatantBehaviourData CreateCombatantData(ICombatant actor)
    {
        var currentHandMoves = GetHand(actor);
        var currentHandMovesDataItems = currentHandMoves.Select(skill => new CombatantMoveBehaviourData(skill)).ToList();
        return new CombatantBehaviourData(currentHandMovesDataItems);
    }

    private static IReadOnlyList<CombatMovementInstance> GetHand(ICombatant combatant)
    {
        var handContainer = combatant.CombatMovementContainers.Single(x => x.Type == CombatMovementContainerTypes.Hand);
        return handContainer.GetItems().Where(x => x is not null).Select(x => x!).ToArray();
    }


    public IReadOnlyCollection<CombatantBehaviourData> Actors { get; }
    public CombatantBehaviourData CurrentActor { get; }
}