using Client.Core.Campaigns;
using Client.Core.Dialogues;

using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;
using Rpg.Client.Core.Dialogues;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.TextDialogue;

internal sealed record TextDialogueScreenTransitionArgs(
    HeroCampaign CurrentCampaign,
    Dialogue CurrentDialogue,
    DialogueEvent DualogueEvent,
    LocationSid Location) : IScreenTransitionArguments;