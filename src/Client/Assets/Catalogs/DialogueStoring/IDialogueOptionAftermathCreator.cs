using Client.Assets.Catalogs.Dialogues;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.Catalogs.DialogueStoring;

internal interface IDialogueOptionAftermathCreator<in TAftermathContext>
{
    IDialogueOptionAftermath<TAftermathContext> Create(string aftermathTypeSid, string data);
}