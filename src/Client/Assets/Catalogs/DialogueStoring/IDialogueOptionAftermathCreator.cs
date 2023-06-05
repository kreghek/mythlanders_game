using Rpg.Client.Core.Dialogues;

namespace Client.Assets.Catalogs.DialogueStoring;

internal interface IDialogueOptionAftermathCreator
{
    IDialogueOptionAftermath Create(string aftermathTypeSid, string data);
}