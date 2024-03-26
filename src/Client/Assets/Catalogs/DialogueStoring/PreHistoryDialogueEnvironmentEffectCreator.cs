using System;

using Client.Assets.DialogueOptionAftermath.PreHistory;
using Client.GameScreens.PreHistory;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.Catalogs.DialogueStoring;

internal sealed class
    PreHistoryDialogueEnvironmentEffectCreator : IDialogueEnvironmentEffectCreator<PreHistoryAftermathContext>
{
    public IDialogueOptionAftermath<PreHistoryAftermathContext> Create(string typeSid, string data)
    {
        if (typeSid == "Background")
        {
            return new SetBackGroundDialogueOptionAftermath(data);
        }

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