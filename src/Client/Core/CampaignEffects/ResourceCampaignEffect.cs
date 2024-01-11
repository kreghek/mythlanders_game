using Client.GameScreens;

using Core.Props;

namespace Client.Core.CampaignEffects;

internal sealed class ResourceCampaignEffect : ICampaignEffect
{
    private readonly IProp _resource;

    public ResourceCampaignEffect(IProp resource)
    {
        _resource = resource;
    }

    public void Apply(Globe globe)
    {
        globe.Player.Inventory.Add(_resource);
    }

    public string GetEffectDisplayText()
    {
        return GameObjectHelper.GetLocalizedProp(_resource.Scheme.Sid);
    }
    
    private static IReadOnlyCollection<IDropTableScheme> CreateCampaignResources(HeroCampaignLocation currentCampaign)
    {
        static IReadOnlyCollection<IDropTableScheme> GetLocationResourceDrop(string sid)
        {
            return new[]
            {
                new DropTableScheme(sid, new IDropTableRecordSubScheme[]
                {
                    new DropTableRecordSubScheme(null, GenericRange<int>.CreateMono(1), sid, 1)
                }, 1)
            };
        }

        switch (currentCampaign.Sid.ToString())
        {
            case nameof(LocationSids.Thicket):
                return GetLocationResourceDrop("snow");

            case nameof(LocationSids.Desert):
                return GetLocationResourceDrop("sand");
        }

        return ArraySegment<IDropTableScheme>.Empty;
    }
}