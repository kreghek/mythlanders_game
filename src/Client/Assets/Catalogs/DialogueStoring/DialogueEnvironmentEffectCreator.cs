using System;

using Client.Assets.Catalogs.Dialogues;
using Client.Assets.DialogueOptionAftermath;
using Client.GameScreens.PreHistory;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.Catalogs.DialogueStoring;

internal sealed class DialogueEnvironmentEffectCreator : IDialogueEnvironmentEffectCreator<CampaignAftermathContext>
{
    public IDialogueOptionAftermath<CampaignAftermathContext> Create(string typeSid, string data)
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

internal sealed class PreHistoryDialogueEnvironmentEffectCreator : IDialogueEnvironmentEffectCreator<PreHistoryAftermathContext>
{
    public IDialogueOptionAftermath<PreHistoryAftermathContext> Create(string typeSid, string data)
    {
        if (typeSid == "BackGround")
        {
            return new SetBackGroundDialogueOptionAftermath(data);
        }

        throw new InvalidOperationException($"Type {typeSid} is unknown.");
    }
}

internal sealed class PreHistoryOptionAftermathCreator : IDialogueOptionAftermathCreator<PreHistoryAftermathContext>
{
    public IDialogueOptionAftermath<PreHistoryAftermathContext> Create(string aftermathTypeSid, string data)
    {
        if (aftermathTypeSid == "BackGround")
        {
            return new SetBackGroundDialogueOptionAftermath(data);
        }

        throw new InvalidOperationException($"Type {aftermathTypeSid} is unknown.");
    }
}