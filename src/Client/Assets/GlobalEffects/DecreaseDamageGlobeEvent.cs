using System.Collections.Generic;
using System.Linq;

using Client.Assets.StoryPointJobs;
using Client.Core;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;

namespace Client.Assets.GlobalEffects;

internal sealed class DecreaseDamageGlobeEvent : IGlobeEvent
{
    public DecreaseDamageGlobeEvent()
    {
        ExpirationConditions = new[]
        {
            new Job(new JobScheme(JobScopeCatalog.Global, JobTypeCatalog.WinCampaigns, new JobGoalValue(1)),
                "WinCampaigns", "CommonJobInProgressPattern", "CommonJobCompletePattern")
        };
    }
    
    public string TitleSid => "DecreaseDamage";
    public void Start(Globe globe)
    {
        foreach (var hero in globe.Player.Heroes.Units)
        {
            var statusFactory = new CombatStatusFactory(source =>
                new ModifyEffectsCombatantStatus(new CombatantStatusSid(TitleSid),
                    new OwnerBoundCombatantEffectLifetime(), source, -1));
            hero.AddCombatStatus(TitleSid, statusFactory);
        }
    }

    public void Finish(Globe globe)
    {
        foreach (var hero in globe.Player.Heroes.Units)
        {
            hero.RemoveCombatStatus(TitleSid);
        }
    }

    public IReadOnlyCollection<IJob> ExpirationConditions { get; }
    
    public IReadOnlyCollection<IJob>? CurrentJobs => ExpirationConditions;
    public bool IsComplete => ExpirationConditions.All(x => x.IsComplete);
    public void HandleCompletion()
    {
        
    }
    
    public int Order { get; } = 1;
}