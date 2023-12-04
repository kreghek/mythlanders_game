using System.Collections.Generic;

using Client.Core;

namespace Client.Assets.GlobalEffects;

internal sealed class IncreaseDamageGlobeEvent : IGlobeEvent
{
    public int CombatsLeft { get; }
    public bool IsActive { get; }
    public string Title { get; }

    public IReadOnlyList<GlobeRule> GetRules()
    {
        throw new System.NotImplementedException();
    }

    public void Initialize(Globe globe)
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        throw new System.NotImplementedException();
    }
}
