using System.Collections.Generic;

using Rpg.Client.Assets.DialogueEventRequirements;
using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Assets.Dialogues.MainStory
{
    internal sealed class GreekMeetsLeonidasDialogueEvent : MainDialogueFactoryBase
    {
        protected override string Sid => "GreekMeetsLeonidas";

        protected override IReadOnlyCollection<ITextEventRequirement>? CreateRequirements(IEventCatalog eventCatalog)
        {
            return new ITextEventRequirement[]
            {
                new LocationEventRequirement(new[]
                {
                    GlobeNodeSid.ShipGraveyard
                })
            };
        }
    }
}