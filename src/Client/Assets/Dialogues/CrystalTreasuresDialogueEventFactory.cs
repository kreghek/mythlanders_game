using Client.Assets.Catalogs.Crises;

using JetBrains.Annotations;

namespace Client.Assets.Dialogues;

[UsedImplicitly]
internal sealed class CrystalTreasuresDialogueEventFactory : SmallEventDialogueEventFactoryBase
{
    protected override string DialogueFileSid => "crystal_treasures";
    protected override string EventSid => CrisesCatalogSids.CrystalTreasures;
}