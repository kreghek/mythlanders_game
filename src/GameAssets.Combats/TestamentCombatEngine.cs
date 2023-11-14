using CombatDicesTeam.Combats;
using CombatDicesTeam.Dices;

namespace GameAssets.Combats;

public sealed class TestamentCombatEngine : CombatEngineBase
{
    public TestamentCombatEngine(IRoundQueueResolver roundQueueResolver, IDice dice) : base(dice, roundQueueResolver, new LimitedRoundsCombatStateStrategy(new EliminatingCombatStateStrategy(), 10))
    {
    }

    public override CombatMovementExecution CreateCombatMovementExecution(CombatMovementInstance movement)
    {
        SpendCombatMovementResources(movement);

        var effectContext =
            new EffectCombatContext(CurrentCombatant, Field, Dice, HandleCombatantDamagedToStat,
                HandleSwapFieldPositions, this);

        var effectImposeItems = new List<CombatEffectImposeItem>();

        foreach (var effectInstance in movement.Effects)
        {
            var effectInstanceClosure = effectInstance;

            void EffectInfluenceDelegate(ICombatant materializedTarget)
            {
                effectInstanceClosure.Influence(materializedTarget, effectContext);
            }

            var effectTargets = effectInstance.Selector.GetMaterialized(CurrentCombatant, GetCurrentSelectorContext());

            if (movement.SourceMovement.Tags.HasFlag(CombatMovementTags.Attack))
            {
                foreach (var effectTarget in effectTargets)
                {
                    if (effectTarget == CurrentCombatant)
                    {
                        // Does not defence against yourself.
                        continue;
                    }

                    var targetDefenseMovement = GetAutoDefenseMovement(effectTarget);
                    var targetIsInQueue = CurrentRoundQueue.Any(x => x == effectTarget);

                    if (targetDefenseMovement is not null && targetIsInQueue)
                    {
                        foreach (var autoDefenseEffect in targetDefenseMovement.AutoDefenseEffects)
                        {
                            void AutoEffectInfluenceDelegate(ICombatant materializedTarget)
                            {
                                autoDefenseEffect.Influence(materializedTarget, effectContext);
                            }

                            var autoDefenseEffectTargets =
                                autoDefenseEffect.Selector.GetMaterialized(effectTarget,
                                    GetSelectorContext(effectTarget, CurrentCombatant));

                            var autoDefenseEffectImposeItem =
                                new CombatEffectImposeItem(AutoEffectInfluenceDelegate, autoDefenseEffectTargets);

                            effectImposeItems.Add(autoDefenseEffectImposeItem);
                        }

                        RemoveCombatantFromQueue(effectTarget);
                        var autoHandSlotIndex = DropMovementFromHand(
                            effectTarget.CombatMovementContainers.Single(x =>
                                x.Type == CombatMovementContainerTypes.Hand), targetDefenseMovement);

                        if (autoHandSlotIndex is not null)
                        {
                            DoCombatantUsedMovement(effectTarget, targetDefenseMovement, autoHandSlotIndex.Value);
                        }
                    }
                }
            }

            var effectImposeItem = new CombatEffectImposeItem(EffectInfluenceDelegate, effectTargets);

            // Play auto-defence effects before an attacks.
            effectImposeItems.Add(effectImposeItem);
        }

        void CompleteSkillAction()
        {
        }

        var movementExecution = new CombatMovementExecution(CompleteSkillAction, effectImposeItems);

        return movementExecution;
    }

    protected override bool DetectCombatantIsDead(ICombatant combatant)
    {
        return combatant.Stats.Single(x => Equals(x.Type, CombatantStatTypes.HitPoints)).Value.Current <= 0;
    }

    protected override void PrepareCombatantsToNextRound()
    {
        RestoreHandsOfAllCombatants();
        RestoreShieldsOfAllCombatants();
        RestoreManeuversOfAllCombatants();
    }

    protected override void RestoreStatsOnWait()
    {
        var combatant = CurrentCombatant;
        var stat = combatant.Stats.Single(x => Equals(x.Type, CombatantStatTypes.Resolve));
        var valueToRestore = stat.Value.ActualMax - stat.Value.Current;
        stat.Value.Restore(valueToRestore);
    }

    protected override void SpendManeuverResources()
    {
        CurrentCombatant.Stats.Single(x => Equals(x.Type, CombatantStatTypes.Maneuver)).Value.Consume(1);
    }

    private static int? DropMovementFromHand(ICombatMovementContainer hand, CombatMovementInstance movement)
    {
        var handItems = hand.GetItems().ToArray();
        for (var i = 0; i < handItems.Length; i++)
        {
            if (handItems[i] != movement)
            {
                continue;
            }

            hand.SetMove(null, i);
            return i;
        }

        return null;
    }

    private static CombatMovementInstance? GetAutoDefenseMovement(ICombatant target)
    {
        return target.CombatMovementContainers.Single(x => x.Type == CombatMovementContainerTypes.Hand).GetItems()
            .FirstOrDefault(x =>
                x != null && x.SourceMovement.Tags.HasFlag(CombatMovementTags.AutoDefense));
    }

    private static CombatMovementInstance? PopNextPoolMovement(ICombatMovementContainer pool)
    {
        var move = pool.GetItems().FirstOrDefault();
        if (move is not null)
        {
            pool.RemoveAt(0);
        }

        return move;
    }

    private void RestoreCombatantHand(ICombatant combatant)
    {
        var hand = combatant.CombatMovementContainers.Single(x => x.Type == CombatMovementContainerTypes.Hand);
        var pool = combatant.CombatMovementContainers.Single(x => x.Type == CombatMovementContainerTypes.Pool);

        var handItems = hand.GetItems().ToArray();

        for (var handSlotIndex = 0; handSlotIndex < handItems.Length; handSlotIndex++)
        {
            if (handItems[handSlotIndex] is null)
            {
                var nextMove = PopNextPoolMovement(pool);
                if (nextMove is null)
                {
                    break;
                }

                hand.SetMove(nextMove, handSlotIndex);
                DoCombatMovementAddToContainer(combatant: combatant, nextMove: nextMove, handSlotIndex: handSlotIndex);
            }
        }
    }

    private void RestoreHandsOfAllCombatants()
    {
        foreach (var combatant in AllCombatantList)
        {
            RestoreCombatantHand(combatant);
        }
    }

    private void RestoreManeuversOfAllCombatants()
    {
        RestoreStatOfAllCombatants(CombatantStatTypes.Maneuver);
    }

    private void RestoreShieldsOfAllCombatants()
    {
        RestoreStatOfAllCombatants(CombatantStatTypes.ShieldPoints);
    }

    private void RestoreStatOfAllCombatants(ICombatantStatType statType)
    {
        var combatants = AllCombatantList.Where(x => !x.IsDead);
        foreach (var combatant in combatants)
        {
            var stat = combatant.Stats.SingleOrDefault(x => Equals(x.Type, statType));

            if (stat is not null)
            {
                var valueToRestore = stat.Value.ActualMax - stat.Value.Current;
                stat.Value.Restore(valueToRestore);
            }
        }
    }

    private void SpendCombatantMovementInHand(CombatMovementInstance movement)
    {
        var hand = CurrentCombatant.CombatMovementContainers.Single(x => x.Type == CombatMovementContainerTypes.Hand);
        var movementSlotIndexInHand = DropMovementFromHand(
            hand,
            movement);

        if (movementSlotIndexInHand is not null)
        {
            DoCombatantUsedMovement(CurrentCombatant, movement, movementSlotIndexInHand.Value);
        }
    }

    private void SpendCombatantResolve(CombatMovementCost combatMovementCost)
    {
        var unitStat = CurrentCombatant.Stats.SingleOrDefault(x => Equals(x.Type, CombatantStatTypes.Resolve));

        if (unitStat is not null)
        {
            unitStat.Value.Consume(combatMovementCost.Amount.Current);
        }
    }

    private void SpendCombatMovementResources(CombatMovementInstance movement)
    {
        SpendCombatantResolve(movement.Cost);

        SpendCombatantMovementInHand(movement);
    }
}