using Client.Assets.Catalogs.Dialogues;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.Catalogs.DialogueStoring;

internal interface IDialogueOptionAftermathCreator
{
    IDialogueOptionAftermath<CampaignAftermathContext> Create(string aftermathTypeSid, string data);
}