using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.Imposers;
using Core.Combats.TargetSelectors;

namespace Text.Client;

internal static class CombatantFactory
{
    public static IReadOnlyCollection<FormationSlot> CreateHeroes()
    {
        var swordsmanHero = CreateSwordsman();

        return new[] { new FormationSlot(0, 1) { Combatant = swordsmanHero } };
    }

    private static Combatant CreateSwordsman()
    {
        var heroSequence = new CombatMovementSequence();

        heroSequence.Items.Add(new CombatMovement("Die by sword!",
            new CombatMovementCost(2),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffect(
                        new ClosestInLineTargetSelector(), 
                        new InstantaneousEffectImposer(),
                        DamageType.Normal,
                        Range<int>.CreateMono(2))
                })
            )
        { 
            Tags = CombatMovementTags.Attack
        }
        );

        heroSequence.Items.Add(new CombatMovement("Im  so strong",
            new CombatMovementCost(2),
            new CombatMovementEffectConfig(
                new IEffect[]
                {
                    new ChangeStatEffect(new SelfTargetSelector(),
                        new InstantaneousEffectImposer(), 
                        UnitStatType.Defense, 
                        3, 
                        typeof(ToNextCombatantTurnEffectLifetime))
                },
                new IEffect[]
                {
                    new ChangeStatEffect(new SelfTargetSelector(),
                        new InstantaneousEffectImposer(), 
                        UnitStatType.Defense, 
                        1, 
                        typeof(ToEndOfCurrentRoundEffectLifetime))
                })
            )
        {
            Tags = CombatMovementTags.AutoDefense
        }
        );

        heroSequence.Items.Add(new CombatMovement("Hit from shoulder",
            new CombatMovementCost(3),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffect(
                        new ClosestInLineTargetSelector(),
                        new InstantaneousEffectImposer(),
                        DamageType.Normal,
                        Range<int>.CreateMono(3))
                })
            )
        {
            Tags = CombatMovementTags.Attack
        }
        );

        var hero = new Combatant(heroSequence)
        {
            Sid = "Berimir",
            IsPlayerControlled = true
        };
        return hero;
    }

    public static IReadOnlyCollection<FormationSlot> CreateMonsters()
    {
        var chaser = new ThiefChaserFactory();

        return new[]
        {
            new FormationSlot(0, 1) { Combatant = chaser.Create("Chaser") },
            new FormationSlot(1, 2) { Combatant = chaser.Create("Guard") },
            new FormationSlot(0, 2) { Combatant = chaser.Create("Tommy") }
        };
    }
}