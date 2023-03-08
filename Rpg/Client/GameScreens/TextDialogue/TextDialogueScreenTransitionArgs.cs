using Client.Core.Campaigns;
using Client.Core.Dialogues;

using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Client.GameScreens.TextDialogue;

internal sealed record TextDialogueScreenTransitionArgs(
    HeroCampaign Campaign,
    Dialogue CurrentDialogue,
    DialogueEvent DialogueEvent,
    LocationSid Location) : CampaignScreenTransitionArgumentsBase(Campaign);