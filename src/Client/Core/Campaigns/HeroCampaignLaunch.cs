using System.Collections.Generic;

using Client.Core.CampaignEffects;

namespace Client.Core.Campaigns;

internal sealed record HeroCampaignLaunch(HeroCampaignLocation Location,
    IReadOnlyCollection<HeroState> Heroes,
    IReadOnlyCollection<ICampaignEffect> Rewards,
    IReadOnlyCollection<ICampaignEffect> Penalties);