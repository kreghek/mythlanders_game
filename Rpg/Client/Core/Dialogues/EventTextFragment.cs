using Rpg.Client.Core;

namespace Client.Core.Dialogues;

internal sealed class EventTextFragment
{
    public UnitName Speaker { get; }
    public string TextSid { get; }

    public EventTextFragment(UnitName speaker, string textSid)
    {
        TextSid = textSid;
        Speaker = speaker;
    }
}