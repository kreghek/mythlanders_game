using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.Imposers;
using Core.Combats.TargetSelectors;

namespace Text.Client;

public class ThiefChaserFactory
{
    public Combatant Create(string sid)
    {
        var monsterSequence = new CombatMovementSequence();
        
        monsterSequence.Items.Add(new CombatMovement("Crossing hit",
            new CombatMovementCost(1),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffect(
                        new ClosestInLineTargetSelector(),
                        new InstantaneousEffectImposer(),
                        DamageType.Normal,
                        Range<int>.CreateMono(1)),
                    new DamageEffect(
                        new ClosestInLineTargetSelector(),
                        new InstantaneousEffectImposer(),
                        DamageType.Normal,
                        Range<int>.CreateMono(1))
                })
            )
        );

        monsterSequence.Items.Add(new CombatMovement("Chasing",
                new CombatMovementCost(1),
                CombatMovementEffectConfig.Create(
                    new IEffect[]
                    {
                        new DamageEffect(
                            new ClosestInLineTargetSelector(),
                            new InstantaneousEffectImposer(),
                            DamageType.Normal,
                            Range<int>.CreateMono(1)),
                        new ChangeCurrentStatEffect(
                            new ClosestInLineTargetSelector(),
                            new InstantaneousEffectImposer(),
                            UnitStatType.Resolve,
                            Range<int>.CreateMono(2))
                    })
            )
        );
        
        monsterSequence.Items.Add(new CombatMovement("Guardian promise",
                new CombatMovementCost(1),
                new CombatMovementEffectConfig(
                    new IEffect[]
                    {
                        new ChangeStatEffect(
                            new SelfTargetSelector(),
                            new InstantaneousEffectImposer(), 
                            UnitStatType.Defense, 
                            3, 
                            typeof(ToNextCombatantTurnEffectLifetime))
                    },
                    new IEffect[]
                    {
                        new ChangeStatEffect(
                            new SelfTargetSelector(),
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
        
        monsterSequence.Items.Add(new CombatMovement("Afterlife Whirlwind",
                new CombatMovementCost(1),
                CombatMovementEffectConfig.Create(
                    new IEffect[]
                    {
                        new DamageEffect(
                            new AllVanguardTargetSelector(),
                            new InstantaneousEffectImposer(),
                            DamageType.Normal,
                            Range<int>.CreateMono(1))
                    })
            )
        );
        
        var monster = new Combatant(monsterSequence) { Sid = sid, IsPlayerControlled = false };

        return monster;
    }
}