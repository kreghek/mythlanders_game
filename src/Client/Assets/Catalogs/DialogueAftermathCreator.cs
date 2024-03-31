using System;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;
using Client.Assets.Catalogs.DialogueStoring;
using Client.Assets.DialogueOptionAftermath.Campaign;
using Client.Core;

using CombatDicesTeam.Dialogues;
using CombatDicesTeam.Dices;

namespace Client.Assets.Catalogs;

internal sealed class DialogueAftermathCreator : IDialogueOptionAftermathCreator<CampaignAftermathContext>, IDialogueParagraphEffectCreator<CampaignAftermathContext>
{
    private readonly IDice _dice;

    public DialogueAftermathCreator(IDice dice)
    {
        _dice = dice;
    }

    public IDialogueOptionAftermath<CampaignAftermathContext> Create(string typeSid, string data)
    {
        return typeSid switch
        {
            "PlayEffect" => new PlayEffectDialogueOptionAftermath(data, data),
            "PlayMusic" => new PlaySongDialogueOptionAftermath(data),
            "MeetHero" => new AddHeroOptionAftermath(data),
            "ActivateStoryPoint" => new ActivateStoryPointOptionAftermath(data),
            "Trigger" => new DialogueEventTriggerOptionAftermath(data),
            "UnlockLocation" => new UnlockLocationOptionAftermath(CatalogHelper
                .GetAllFromStaticCatalog<ILocationSid>(typeof(LocationSids))
                .Single(x => x.ToString() == data)),
            "SetRelationsToKnown" => new ChangeCharacterRelationsOptionAftermath(Enum.Parse<UnitName>(data),
                CharacterKnowledgeLevel.FullName),
            "DamageSingleRandomHero" => new DamageSingleRandomOptionAftermath(_dice),
            "DamageAllHeroes" => new DamageAllHeroesOptionAftermath(),
            "AddResources" => AddResourceOptionAftermath.CreateFromData(data),
            "RemoveResources" => RemoveResourceOptionAftermath.CreateFromData(data),
            _ => throw new InvalidOperationException($"Type {typeSid} is unknown.")
        };
    }
}