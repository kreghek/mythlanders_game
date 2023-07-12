﻿using Core.Combats;
using Core.Combats.CombatantStatuses;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.Assets.CombatMovements.Hero.Swordsman;

internal class RiseYourSwordsFactory : CombatMovementFactoryBase
{
    public override CombatMovementIcon CombatMovementIcon => new(4, 2);

    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(1),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new AddCombatantStatusEffect(
                        new AllAllyTargetSelector(),
                        new ModifyEffectsCombatantStatusFactory(
                            new CombatantEffectSid(Sid),
                            new MultipleCombatantTurnEffectLifetimeFactory(1),
                            1))
                })
        );
    }
}