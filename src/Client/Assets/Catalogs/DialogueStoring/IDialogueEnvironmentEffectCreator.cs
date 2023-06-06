using Client.Core.Dialogues;

namespace Client.Assets.Catalogs.DialogueStoring;

internal interface IDialogueEnvironmentEffectCreator
{
    IDialogueEnvironmentEffect Create(string typeSid, string data);
}