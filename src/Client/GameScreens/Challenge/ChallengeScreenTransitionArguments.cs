using System.Collections.Generic;

using Client.Core;
using Client.Core.Campaigns;

namespace Client.GameScreens.Challenge;

internal sealed record ChallengeScreenTransitionArguments
    (HeroCampaign Campaign, IReadOnlyCollection<IJob> Jobs) : CampaignScreenTransitionArgumentsBase(Campaign);