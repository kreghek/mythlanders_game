using CombatDicesTeam.Dialogues;

namespace Client.Assets.Catalogs.DialogueStoring;

internal interface IDialogueParagraphEffectCreator<in TAftermathContext>
{
    IDialogueOptionAftermath<TAftermathContext> Create(string typeSid, string data);
}