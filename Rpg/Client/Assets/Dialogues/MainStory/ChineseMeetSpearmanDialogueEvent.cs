using System.Collections.Generic;

using Rpg.Client.Assets.DialogueEventRequirements;
using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Assets.Dialogues.MainStory
{
    internal sealed class ChineseMeetSpearmanDialogueEvent : MainDialogueFactoryBase
    {
        protected override string Sid => "ChineseMeetSpearman";

        protected override IReadOnlyCollection<ITextEventRequirement>? CreateRequirements(IEventCatalog eventCatalog)
        {
            return new ITextEventRequirement[]
            {
                new LocationEventRequirement(new[]
                {
                    LocationSid.Monastery
                }),
                new RequiredEventsCompletedEventRequirement(eventCatalog, new[] { "ChineseMeetMonk" })
            };
        }
    }
}