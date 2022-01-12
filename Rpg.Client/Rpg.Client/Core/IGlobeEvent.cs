using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal interface IGlobeEvent
    {
        string Title { get; }
        int CombatsLeft { get; }
        bool IsActive { get; }
        IReadOnlyList<GlobeRule> GetRules();
        void Update();
        void Initialize(Globe globe);
    }
}