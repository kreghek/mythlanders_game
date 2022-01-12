using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal interface IGlobeEvent
    {
        int CombatsLeft { get; }
        bool IsActive { get; }
        string Title { get; }
        IReadOnlyList<GlobeRule> GetRules();
        void Initialize(Globe globe);
        void Update();
    }
}