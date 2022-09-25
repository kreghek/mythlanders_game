using System;
using System.Collections.Generic;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal interface IGlobeEvent
    {
        int CombatsLeft { get; }
        bool IsActive { get; }
        string Title { get; }

        IReadOnlyCollection<EffectRule> CreateCombatBeginningEffects()
        {
            return Array.Empty<EffectRule>();
        }

        IReadOnlyList<GlobeRule> GetRules();
        void Initialize(Globe globe);
        void Update();
    }
}