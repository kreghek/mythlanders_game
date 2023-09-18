using System;

using Client.Assets.Catalogs.Dialogues;
using Client.Assets.Catalogs.DialogueStoring;
using Client.Assets.DialogueOptionAftermath;
using Client.Core;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.Catalogs;

internal sealed class DialogueOptionAftermathCreator : IDialogueOptionAftermathCreator
{
    private readonly IUnitSchemeCatalog _unitSchemeCatalog;

    public DialogueOptionAftermathCreator(IUnitSchemeCatalog unitSchemeCatalog)
    {
        _unitSchemeCatalog = unitSchemeCatalog;
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
            aftermath = new ChangeCharacterRelatationsOptionAftermath(Enum.Parse<UnitName>(unitName),
                CharacterKnowledgeLevel.FullName);
        }

        if (aftermath is null)
        {
            throw new InvalidOperationException($"Type {typeSid} is unknown.");
        }

        return aftermath;
    }
}