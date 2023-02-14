using Rpg.Client.Core.Campaigns;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.NotImplementedStage;

internal sealed record NotImplementedStageScreenTransitionArguments(HeroCampaign Campaign) : IScreenTransitionArguments;