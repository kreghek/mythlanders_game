using System.Collections.Generic;

using Rpg.Client.Assets.DialogueEventRequirements;
using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Assets.Dialogues.MainStory
{
    internal sealed class SlavicMain3DialogueEvent : MainDialogueFactoryBase
    {
        protected override string Sid => "SlavicMain3";
        protected override IReadOnlyCollection<ITextEventRequirement>? CreateRequirements(IEventCatalog eventCatalog)
        {
            return new ITextEventRequirement[]
                {
                    new LocationEventRequirement(new[]
                    {
                        GlobeNodeSid.Battleground
                    }),
                    new RequiredEventsCompletedEventRequirement(eventCatalog, new[]{ "SlavicMain2" })
                };
        }
    }
}
