using System;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;
using Client.Assets.Catalogs.DialogueStoring;
using Client.Assets.DialogueOptionAftermath;
using Client.Core;

using CombatDicesTeam.Dialogues;
using CombatDicesTeam.Dices;

namespace Client.Assets.Catalogs;

internal sealed class DialogueOptionAftermathCreator : IDialogueOptionAftermathCreator, IDialogueEnvironmentEffectCreator
{
    private readonly IDice _dice;

    public DialogueOptionAftermathCreator(IDice dice)
    {
        _dice = dice;
    }

    public IDialogueOptionAftermath<CampaignAftermathContext> Create(string typeSid, string data)
    {
        switch (typeSid)
        {
            case "PlayEffect":
                return new PlayEffectDialogueOptionAftermath(data, data);
            case "PlayMusic":
                return new PlaySongDialogueOptionAftermath(data);
            case "MeetHero":
                return new AddHeroOptionAftermath(data);
            case "ActivateStoryPoint":
                return new ActivateStoryPointOptionAftermath(data);
            case "Trigger":
                return new ActivateStoryPointOptionAftermath(data);
            case "UnlockLocation":
                return new UnlockLocationOptionAftermath(CatalogHelper
                    .GetAllFromStaticCatalog<ILocationSid>(typeof(LocationSids)).Single(x => x.ToString() == data));
            case "SetRelationsToKnown":
                return new ChangeCharacterRelationsOptionAftermath(Enum.Parse<UnitName>(data),
                    CharacterKnowledgeLevel.FullName);
            
            case "DamageSingleRandomHero":
                return new DamageSingleRandomOptionAftermath(_dice);
            case "DamageAllHeroes":
                return new DamageAllHeroesOptionAftermath();
            
            case "AddResources":
                return AddResourceOptionAftermath.CreateFromData(data);
            case "RemoveResources":
                return RemoveResourceOptionAftermath.CreateFromData(data);
            
            default:
                throw new InvalidOperationException($"Type {typeSid} is unknown.");
        }
    }
}