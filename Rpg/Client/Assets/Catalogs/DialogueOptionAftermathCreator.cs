using System;

using Client.Assets.DialogueEventEnviroment;
using Client.Assets.DialogueOptionAftermath;
using Client.Core.Dialogues;

using Rpg.Client.Assets.Catalogs;
using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Client.Assets.Catalogs;

internal sealed class DialogueEnvCommandCreator
{
    public IDialogueEventTextFragmentEnvironmentCommand Create(string typeSid, string data)
    {
        if (typeSid == "PlayEffect")
        { 
            return new PlayEffectEnviromentCommand(typeSid, data);
        } else if (typeSid == "PlayMusic")
        {
            return new PlaySongEnviromentCommand(data);
        }

        throw new InvalidOperationException($"Type {typeSid} is unknown.");
    }
}

internal sealed class DialogueOptionAftermathCreator : IDialogueOptionAftermathCreator
{
    private readonly IUnitSchemeCatalog _unitSchemeCatalog;

    public DialogueOptionAftermathCreator(IUnitSchemeCatalog unitSchemeCatalog)
    {
        _unitSchemeCatalog = unitSchemeCatalog;
    }

    public IDialogueOptionAftermath Create(string typeSid, string data)
    {
        IDialogueOptionAftermath? aftermath = null;

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
            var sid = data;
            var locationId = Enum.Parse<LocationSid>(sid);
            aftermath = new UnlockLocationOptionAftermath(locationId);
        }
        else if (typeSid == "Trigger")
        {
            var trigger = data;
            aftermath = new DialogueEventTriggerOptionAftermath(trigger);
        }

        if (aftermath is null)
        {
            throw new InvalidOperationException($"Type {typeSid} is unknown.");
        }

        return aftermath;
    }
}