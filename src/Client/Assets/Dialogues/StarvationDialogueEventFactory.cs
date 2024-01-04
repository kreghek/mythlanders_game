using JetBrains.Annotations;

namespace Client.Assets.Dialogues;

[UsedImplicitly]
internal sealed class StarvationDialogueEventFactory : SmallEventDialogueEventFactoryBase
{
    protected override string DialogueFileSid => "starvation";
    protected override string EventSid => "Starvation";
}