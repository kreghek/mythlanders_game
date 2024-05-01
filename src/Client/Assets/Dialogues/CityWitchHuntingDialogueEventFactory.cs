using Client.Assets.Catalogs.Crises;

using JetBrains.Annotations;

namespace Client.Assets.Dialogues;

[UsedImplicitly]
internal sealed class CityWitchHuntingDialogueEventFactory : SmallEventDialogueEventFactoryBase
{
    protected override string DialogueFileSid => "city_witch_hunting";
    protected override string EventSid => CrisesCatalogSids.CityWitchHunting;
}