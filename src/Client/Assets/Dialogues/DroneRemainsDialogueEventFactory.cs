using Client.Assets.Catalogs.Crises;

using JetBrains.Annotations;

namespace Client.Assets.Dialogues;

[UsedImplicitly]
internal sealed class DroneRemainsDialogueEventFactory : SmallEventDialogueEventFactoryBase
{
    protected override string DialogueFileSid => "drone_remains";
    protected override string EventSid => CrisesCatalogSids.DroneRemains;
}