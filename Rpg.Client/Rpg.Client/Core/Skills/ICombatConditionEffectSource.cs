using System;
using System.Collections.Generic;

namespace Rpg.Client.Core.Skills
{
    internal interface ICombatConditionEffectSource : IEffectSource
    {
        IReadOnlyList<EffectRule> CreateCombatBeginningEffects(IEquipmentEffectContext context)
        {
            return Array.Empty<EffectRule>();
        }

        IReadOnlyList<EffectRule> CreateCombatHitPointsChangedEffects(IEquipmentEffectContext context)
        {
            return Array.Empty<EffectRule>();
        }
    }
}