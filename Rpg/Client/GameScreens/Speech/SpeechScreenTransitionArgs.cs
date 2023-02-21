using Client.Core.Dialogues;

using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;
using Rpg.Client.Core.Dialogues;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.Speech;

internal sealed record SpeechScreenTransitionArgs(HeroCampaign CurrentCampaign, Dialogue CurrentDialogue,
    DialogueEvent dualogueEvent,
    LocationSid Location) : IScreenTransitionArguments
{
}