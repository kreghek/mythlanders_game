using CombatDicesTeam.Dialogues;

namespace Client.Assets.Catalogs.DialogueStoring;

internal interface IDialogueEnvironmentEffectCreator<in TAftermathContext>
{
    IDialogueOptionAftermath<TAftermathContext> Create(string typeSid, string data);
}