using System.Collections.Generic;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.Perks
{
    internal class PowerUpAura : IPerk
    {
        public IReadOnlyList<EffectRule> CreateCombatBeginningEffects(IEquipmentEffectContext context)
        {
            return new[]
            {
                SkillRuleFactory.CreatePowerUpAura(SkillDirection.AllFriendly)
            };
        }
    }
}