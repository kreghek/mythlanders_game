using Client.Assets.Catalogs.Dialogues;

using Core.Crises;

namespace Client.Assets.Crises;

public interface ICrisis
{
    public EventType EventType { get; }
    public CrisisSid Sid { get; }
    public DialogueEvent Event { get; }
}