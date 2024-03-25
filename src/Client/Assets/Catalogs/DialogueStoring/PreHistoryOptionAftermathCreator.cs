using System;

using Client.Assets.DialogueOptionAftermath.PreHistory;
using Client.GameScreens.PreHistory;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.Catalogs.DialogueStoring;

internal sealed class PreHistoryOptionAftermathCreator : IDialogueOptionAftermathCreator<PreHistoryAftermathContext>
{
    public IDialogueOptionAftermath<PreHistoryAftermathContext> Create(string aftermathTypeSid, string data)
    {
        if (aftermathTypeSid == "Background")
        {
            return new SetBackGroundDialogueOptionAftermath(data);
        }

        if (aftermathTypeSid == "PlayEffect")
        {
            return new PlayEffectDialogueOptionAftermath(data, data);
        }

        if (aftermathTypeSid == "PlayMusic")
        {
            return new PlaySongDialogueOptionAftermath(data);
        }

        if (aftermathTypeSid == "AddMonsterPerk")
        { 
            return new AddMonsterPerkOptionAftermath(data);
        }

        if (aftermathTypeSid == "AddHero")
        {
            return new AddHeroOptionAftermath(data);
        }

        throw new InvalidOperationException($"Type {aftermathTypeSid} is unknown.");
    }
}