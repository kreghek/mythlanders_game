using System;

using Client.Assets.Catalogs.Dialogues;
using Client.Assets.Catalogs.DialogueStoring;
using Client.Assets.DialogueOptionAftermath;
using Client.Core;

using CombatDicesTeam.Dialogues;
using CombatDicesTeam.Dices;

namespace Client.Assets.Catalogs;

internal sealed class DialogueOptionAftermathCreator : IDialogueOptionAftermathCreator
{
    private readonly IUnitSchemeCatalog _unitSchemeCatalog;
    private readonly IDice _dice;

    public DialogueOptionAftermathCreator(IUnitSchemeCatalog unitSchemeCatalog, IDice dice)
    {
        _unitSchemeCatalog = unitSchemeCatalog;
        _dice = dice;
    }

    public IDialogueOptionAftermath<AftermathContext> Create(string typeSid, string data)
    {
        IDialogueOptionAftermath<AftermathContext>? aftermath = null;

        if (typeSid == "MeetHero")
        {
            var heroNameStr = data;
            var heroName = Enum.Parse<UnitName>(heroNameStr);
            aftermath = new AddHeroOptionAftermath(_unitSchemeCatalog.Heroes[heroName]);
        }
        else if (typeSid == "ActivateStoryPoint")
        {
            var spId = data;
            aftermath = new ActivateStoryPointOptionAftermath(spId);
        }
        else if (typeSid == "UnlockLocation")
        {
            //var sid = data;
            //var locationId = Enum.Parse<LocationSids>(sid);
            //aftermath = new UnlockLocationOptionAftermath(locationId);
        }
        else if (typeSid == "Trigger")
        {
            var trigger = data;
            aftermath = new DialogueEventTriggerOptionAftermath(trigger);
        }
        else if (typeSid == "SetRelationsToKnown")
        {
            var unitName = data;
            aftermath = new ChangeCharacterRelationsOptionAftermath(Enum.Parse<UnitName>(unitName),
                CharacterKnowledgeLevel.FullName);
        }
        else if (typeSid == "DamageSingleRandomHero")
        {
            aftermath = new DamageSingleRandomOptionAftermath(_dice);
        }
        else if (typeSid == "DamageAllHeroes")
        {
            aftermath = new DamageAllHeroesOptionAftermath();
        }
        else if (typeSid == "AddResources")
        {
            var args = data.Split(' ');
            aftermath = new AddResourceOptionAftermath(args[0], int.Parse(args[1]));
        }

        if (aftermath is null)
        {
            throw new InvalidOperationException($"Type {typeSid} is unknown.");
        }

        return aftermath;
    }
}