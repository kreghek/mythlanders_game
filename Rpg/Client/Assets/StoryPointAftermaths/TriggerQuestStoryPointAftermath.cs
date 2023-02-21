using System.Linq;

using Client.Core.Dialogues;

using Rpg.Client.Core;

namespace Client.Assets.StoryPointAftermaths;

internal sealed class TriggerQuestStoryPointAftermath : IStoryPointAftermath
{
    private readonly string _eventSid;
    private readonly DialogueEventTrigger _trigger;
    private readonly IEventCatalog _eventCatalog;

    public TriggerQuestStoryPointAftermath(string eventSid, DialogueEventTrigger trigger, IEventCatalog eventCatalog)
    {
        _eventSid = eventSid;
        _trigger = trigger;
        _eventCatalog = eventCatalog;
    }

    public void Apply()
    {
        var questEvent = _eventCatalog.Events.Single(x => x.Sid == _eventSid);
        questEvent.Trigger(_trigger);
    }
}