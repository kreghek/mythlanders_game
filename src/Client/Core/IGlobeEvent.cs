using System;
using System.Collections.Generic;

using Core.Combats;

namespace Rpg.Client.Core
{
    internal interface IGlobeEvent
    {
        int CombatsLeft { get; }
        bool IsActive { get; }
        string Title { get; }

        IReadOnlyCollection<IEffect> CreateCombatBeginningEffects()
        {
            return Array.Empty<IEffect>();
        }

        IReadOnlyList<GlobeRule> GetRules();
        void Initialize(Globe globe);
        void Update();
    }
}