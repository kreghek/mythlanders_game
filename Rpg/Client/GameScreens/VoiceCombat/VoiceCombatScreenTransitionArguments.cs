using Client.Core.Campaigns;

using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.VoiceCombat;

internal sealed record VoiceCombatScreenTransitionArguments(HeroCampaign Campaign) : IScreenTransitionArguments;