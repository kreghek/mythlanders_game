using System.Collections.Generic;

using Client.Assets.StoryPointJobs;
using Client.Core;

namespace Client.Assets.GlobalEffects;

internal sealed class CharacterDeepPreyingGlobeEvent : IGlobeEvent
{
    private readonly UnitName _name;

    public CharacterDeepPreyingGlobeEvent(UnitName name)
    {
        _name = name;
        
        ExpirationConditions = new[]
        {
            new Job(new JobScheme(JobScopeCatalog.Global, JobTypeCatalog.WinCampaigns, new JobGoalValue(2)),
                "WinCampaigns", "CommonJobInProgressPattern", "CommonJobCompletePattern")
        };
    }

    public string TitleSid => string.Format(GameObjectResources.CharacterDeepPreyingTitleTemplate, _name);
    public void Start(Globe globe)
    {
        foreach (var heroState in globe.Player.Heroes.Units)
        {
            if (heroState.ClassSid == _name.ToString())
            {
                heroState.DisableToCampaigns();
            }
        }
    }

    public void Finish(Globe globe)
    {
        foreach (var heroState in globe.Player.Heroes.Units)
        {
            if (heroState.ClassSid == _name.ToString())
            {
                heroState.EnableToCampaigns();
            }
        }
    }

    public IReadOnlyCollection<IJob> ExpirationConditions { get; }
}