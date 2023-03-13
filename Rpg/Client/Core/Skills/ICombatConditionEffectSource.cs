using System;
using System.Collections.Generic;

using Core.Combats;

namespace Rpg.Client.Core.Skills
{
    internal interface ICombatConditionEffectSource : IEffectSource
    {
        IReadOnlyList<IEffect> CreateCombatBeginningEffects(IEquipmentEffectContext context)
        {
            return Array.Empty<IEffect>();
        }

        IReadOnlyList<IEffect> CreateCombatHitPointsChangedEffects(IEquipmentEffectContext context)
        {
            return Array.Empty<IEffect>();
        }
    }
}