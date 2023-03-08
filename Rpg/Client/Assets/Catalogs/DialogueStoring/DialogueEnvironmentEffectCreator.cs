using System;

using Client.Assets.DialogueEventEnviroment;
using Client.Core.Dialogues;

namespace Client.Assets.Catalogs.DialogueStoring;

internal sealed class DialogueEnvironmentEffectCreator : IDialogueEnvironmentEffectCreator
{
    public IDialogueEnvironmentEffect Create(string typeSid, string data)
    {
        if (typeSid == "PlayEffect")
        {
            return new PlayEffectEnviromentCommand(data, data);
        }

        if (typeSid == "PlayMusic")
        {
            return new PlaySongEnviromentCommand(data);
        }

        throw new InvalidOperationException($"Type {typeSid} is unknown.");
    }
}