using System;
using System.Collections.Generic;

using Client.Core;

namespace Client.Assets.GlobalEffects;

internal sealed class DecreaseDamageGlobeEvent : IGlobeEvent
{
    public int CombatsLeft { get; }
    public bool IsActive { get; }
    public string Title => "Decrease damage on 2 campaigns";

    public IReadOnlyList<GlobeRule> GetRules()
    {
        throw new NotImplementedException();
    }

    public void Initialize(Globe globe)
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        throw new NotImplementedException();
    }
}