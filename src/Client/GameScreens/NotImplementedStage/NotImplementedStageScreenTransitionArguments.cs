using Client.Core.Campaigns;
using Client.ScreenManagement;

namespace Client.GameScreens.NotImplementedStage;

internal sealed record NotImplementedStageScreenTransitionArguments(HeroCampaign Campaign) : IScreenTransitionArguments;