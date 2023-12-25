using System;

using Client.Assets.Catalogs.Dialogues;
using Client.Assets.DialogueOptionAftermath;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.Catalogs.DialogueStoring;

internal sealed class DialogueEnvironmentEffectCreator : IDialogueEnvironmentEffectCreator
{
    public IDialogueOptionAftermath<AftermathContext> Create(string typeSid, string data)
    {
        if (typeSid == "PlayEffect")
        {
            return new PlayEffectDialogueOptionAftermath(data, data);
        }

        if (typeSid == "PlayMusic")
        {
            return new PlaySongDialogueOptionAftermath(data);
        }

        throw new InvalidOperationException($"Type {typeSid} is unknown.");
    }
}