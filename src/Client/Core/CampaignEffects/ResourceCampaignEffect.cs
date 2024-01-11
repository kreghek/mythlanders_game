using System.Collections.Generic;
using System.Linq;

using Client.GameScreens;

using Core.Props;

namespace Client.Core.CampaignEffects;

internal sealed class ResourceCampaignEffect : ICampaignEffect
{
    private readonly IReadOnlyCollection<IProp> _resources;

    public ResourceCampaignEffect(IEnumerable<IProp> resources)
    {
        _resources = resources.ToArray();
    }

    public void Apply(Globe globe)
    {
        foreach (var resource in _resources)
        {
            globe.Player.Inventory.Add(resource);   
        }
    }

    public string GetEffectDisplayText()
    {
        return string.Join(", ", _resources.Select(x=> GameObjectHelper.GetLocalizedProp(x.Scheme.Sid)));
    }
}