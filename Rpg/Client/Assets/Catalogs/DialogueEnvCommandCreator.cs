using System;

using Client.Assets.DialogueEventEnviroment;
using Client.Core.Dialogues;

namespace Client.Assets.Catalogs;

internal sealed class DialogueEnvCommandCreator
{
    public IDialogueEventTextFragmentEnvironmentCommand Create(string typeSid, string data)
    {
        if (typeSid == "PlayEffect")
        {
            return new PlayEffectEnviromentCommand(typeSid, data);
        }
        else if (typeSid == "PlayMusic")
        {
            return new PlaySongEnviromentCommand(data);
        }

        throw new InvalidOperationException($"Type {typeSid} is unknown.");
    }
}