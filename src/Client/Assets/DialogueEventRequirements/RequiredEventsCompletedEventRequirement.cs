using System.Linq;

using Client.Assets.Catalogs.Dialogues;
using Client.Core;

namespace Client.Assets.DialogueEventRequirements;

internal sealed class RequiredEventsCompletedEventRequirement : IDialogueEventRequirement
{
    private readonly IEventCatalog _eventCatalog;
    private readonly string[] _requiredEvents;

    public RequiredEventsCompletedEventRequirement(IEventCatalog eventCatalog, string[] requiredEvents)
    {
        _eventCatalog = eventCatalog;
        _requiredEvents = requiredEvents;
    }

    public bool IsApplicableFor(IDialogueEventRequirementContext context)
    {
        foreach (var eventSid in _requiredEvents)
        {
            var eventInfo = _eventCatalog.Events.SingleOrDefault(x => x.Sid == eventSid);

            if (eventInfo?.Completed != true)
            {
                return false;
            }
        }

        return true;
    }
}