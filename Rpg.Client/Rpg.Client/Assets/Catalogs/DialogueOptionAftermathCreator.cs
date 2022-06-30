using System;

using Rpg.Client.Assets.DialogueOptionAftermath;
using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Assets.Catalogs
{
    internal sealed class DialogueOptionAftermathCreator : IDialogueOptionAftermathCreator
    {
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;

        public DialogueOptionAftermathCreator(IUnitSchemeCatalog unitSchemeCatalog)
        {
            _unitSchemeCatalog = unitSchemeCatalog;
        }

        public IOptionAftermath Create(string aftermathTypeSid, string data)
        {
            IOptionAftermath? aftermath = null;

            if (aftermathTypeSid == "MeetHero")
            {
                var heroNameStr = data;
                var heroName = Enum.Parse<UnitName>(heroNameStr);
                aftermath = new AddHeroOptionAftermath(_unitSchemeCatalog.Heroes[heroName]);
            }
            else if (aftermathTypeSid == "ActivateStoryPoint")
            {
                var spId = data;
                aftermath = new AddStoryPointOptionAftermath(spId);
            }
            else if (aftermathTypeSid == "UnlockLocation")
            {
                var sid = data;
                var locationId = Enum.Parse<GlobeNodeSid>(sid);
                aftermath = new UnlockLocationOptionAftermath(locationId);
            }

            if (aftermath is null)
            {
                throw new InvalidOperationException($"Type {aftermathTypeSid} is unknown.");
            }

            return aftermath;
        }
    }
}