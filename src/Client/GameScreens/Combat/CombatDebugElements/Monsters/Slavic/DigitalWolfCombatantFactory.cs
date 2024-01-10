using Client.Assets.CombatMovements.Monster.Slavic.DigitalWolf;

using CombatDicesTeam.Combats;

using GameAssets.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements.Monsters.Slavic;

public class DigitalWolfCombatantFactory : MonsterCombatantFactoryBase
{
    protected override string ClassSid => "digitalwolf";

    protected override CombatantStatsConfig CombatantStatsConfig()
    {
        var stats = new CombatantStatsConfig();
        stats.SetValue(CombatantStatTypes.HitPoints, 5);
        stats.SetValue(CombatantStatTypes.ShieldPoints, 3);
        stats.SetValue(CombatantStatTypes.Resolve, 4);
        return stats;
    }

    protected override CombatMovementSequence CombatMovementSequence(int variationIndex)
    {
        var monsterSequence = CreateCombatMoveVariation();
        return monsterSequence;
    }

    private static CombatMovementSequence CreateCombatMoveVariation()
    {
        var list = new[]
        {
            new CyberClawsFactory().CreateMovement(),
            new VelesProtectionFactory().CreateMovement(),
            new EnergeticBiteFactory().CreateMovement(),
            new RegenerativeProtocolFactory().CreateMovement(),
            new FlockAlphaTacticsFactory().CreateMovement()
        };

        var monsterSequence = new CombatMovementSequence();

        for (var i = 0; i < 2; i++)
        {
            foreach (var combatMovement in list)
            {
                monsterSequence.Items.Add(combatMovement);
            }
        }

        return monsterSequence;
    }
}