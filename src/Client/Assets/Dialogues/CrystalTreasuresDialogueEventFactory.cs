using Client.Assets.Catalogs.Crises;

using JetBrains.Annotations;

namespace Client.Assets.Dialogues;

[UsedImplicitly]
internal sealed class CrystalTreasuresDialogueEventFactory : SmallEventDialogueEventFactoryBase
{
    protected override string DialogueFileSid => CrisesCatalogSids.CrystalTreasures;
    protected override string EventSid => "crystal_treasures";
}