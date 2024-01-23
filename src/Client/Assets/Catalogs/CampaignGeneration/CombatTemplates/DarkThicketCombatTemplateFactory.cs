using Client.Core;

using CombatDicesTeam.Combats;

using JetBrains.Annotations;

namespace Client.Assets.Catalogs.CampaignGeneration.CombatTemplates;

[UsedImplicitly]
internal sealed class DarkThicketCombatTemplateFactory : LocationSpecificCombatTemplateFactory
{
    protected override ILocationSid LocationSid => LocationSids.Thicket;

    protected override MonsterCombatantPrefab[] GetLevel0()
    {
        return new MonsterCombatantPrefab[]
        {
            new("DigitalWolf", 0, new FieldCoords(0, 1)),
            new("Aspid", 0, new FieldCoords(1, 1))
        };
    }

    protected override MonsterCombatantPrefab[] GetLevel1()
    {
        return new MonsterCombatantPrefab[]
        {
            new("Aspid", 0, new FieldCoords(0, 1)),
            new("DigitalWolf", 0, new FieldCoords(0, 2))
        };
    }

    protected override MonsterCombatantPrefab[] GetLevel2()
    {
        return new MonsterCombatantPrefab[]
        {
            new("Aspid", 0, new FieldCoords(0, 1)),
            new("DigitalWolf", 0, new FieldCoords(0, 2)),
            new("DigitalWolf", 0, new FieldCoords(1, 1))
        };
    }
}