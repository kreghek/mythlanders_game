using System.Collections.Generic;

using Client.Core;
using Client.Core.Campaigns;
using Client.ScreenManagement;

namespace Client.GameScreens.Trade;

internal sealed class TradeScreenTransitionArguments : IScreenTransitionArguments
{
    public TradeScreenTransitionArguments(HeroCampaign currentCampaign, IReadOnlyList<TradeOffer> availableOffers)
    {
        CurrentCampaign = currentCampaign;
        AvailableOffers = availableOffers;
    }

    public IReadOnlyList<TradeOffer> AvailableOffers { get; }

    public HeroCampaign CurrentCampaign { get; }
}