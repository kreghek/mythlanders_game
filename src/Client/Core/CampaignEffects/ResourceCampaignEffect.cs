using System.Collections.Generic;
using System.Linq;

using Client.GameScreens;

using Core.Props;

namespace Client.Core.CampaignEffects;

internal sealed class ResourceCampaignEffect : ICampaignEffect
{
    public IEnumerable<IProp> Resources { get; }

    public ResourceCampaignEffect(IEnumerable<IProp> resources)
    {
        Resources = resources;
    }

    public void Apply(Globe globe)
    {
        foreach (var resource in Resources)
        {
            globe.Player.Inventory.Add(resource);
        }
    }

    public string GetEffectDisplayText()
    {
        return string.Join(", ", Resources.Select(x => GameObjectHelper.GetLocalizedProp(x.Scheme.Sid)));
    }
}