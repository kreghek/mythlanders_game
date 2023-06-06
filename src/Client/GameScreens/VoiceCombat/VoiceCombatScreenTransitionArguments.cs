using Client.Core.Campaigns;
using Client.ScreenManagement;

namespace Client.GameScreens.VoiceCombat;

internal sealed record VoiceCombatScreenTransitionArguments(HeroCampaign Campaign) : IScreenTransitionArguments;