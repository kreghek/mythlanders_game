using System;
using System.Linq;

using Client.Assets.DialogueOptionAftermath.PreHistory;
using Client.Core;
using Client.GameScreens.PreHistory;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.Catalogs.DialogueStoring;

internal sealed class PreHistoryOptionAftermathCreator : IDialogueOptionAftermathCreator<PreHistoryAftermathContext>
{
    public IDialogueOptionAftermath<PreHistoryAftermathContext> Create(string aftermathTypeSid, string data)
    {
        return aftermathTypeSid switch
        {
            "Background" => new SetBackGroundDialogueOptionAftermath(data),
            "PlayEffect" => new PlayEffectDialogueOptionAftermath(data, data),
            "PlayMusic" => new PlaySongDialogueOptionAftermath(data),
            "AddMonsterPerk" => new AddMonsterPerkOptionAftermath(data),
            "AddHero" => new AddHeroOptionAftermath(data),
            "UnlockLocation" => new UnlockLocationOptionAftermath(CatalogHelper
                .GetAllFromStaticCatalog<ILocationSid>(typeof(LocationSids))
                .Single(x => x.ToString() == data)),
            _ => throw new InvalidOperationException($"Type {aftermathTypeSid} is unknown.")
        };
    }
}