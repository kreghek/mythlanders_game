using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Client.Assets.Heroes;
using Client.Assets.States.Primitives;
using Client.Core.AnimationFrameSets;
using Client.Engine;
using Client.GameScreens.Combat.GameObjects;

using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using Microsoft.Xna.Framework;

using Rpg.Client.Assets.States.Primitives;
using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;

using DamageEffect = Core.Combats.Effects.DamageEffect;

namespace Client.Assets.CombatMovements;

internal interface ICombatMovementFactory
{
    string Sid { get; }
    CombatMovement CreateMovement();
    IActorVisualizationState CreateVisualization(IActorAnimator actorAnimator, CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext);
}

internal interface ICombatMovementVisualizationContext
{
    UnitGameObject GetCombatActor(Combatant combatant);
}

internal sealed class CombatMovementVisualizationContext : ICombatMovementVisualizationContext
{
    private readonly IReadOnlyCollection<UnitGameObject> _gameObjects;

    public CombatMovementVisualizationContext(IReadOnlyCollection<UnitGameObject> gameObjects)
    {
        _gameObjects = gameObjects;
    }

    public UnitGameObject GetCombatActor(Combatant combatant)
    {
        return _gameObjects.Single(x => x.Combatant == combatant);
    }
}

internal class DieBySwordFactory : ICombatMovementFactory
{
    public string Sid => "Die by sword!";

    public CombatMovement CreateMovement()
    {
        return new CombatMovement("Die by sword!",
                new CombatMovementCost(2),
                CombatMovementEffectConfig.Create(
                    new IEffect[]
                    {
                        new DamageEffect(
                            new ClosestInLineTargetSelector(),
                            DamageType.Normal,
                            Range<int>.CreateMono(2)),
                        new ChangePositionEffect(
                            new SelfTargetSelector(),
                            ChangePositionEffectDirection.ToVanguard
                        )
                    })
            )
        {
            Tags = CombatMovementTags.Attack
        };
    }

    public IActorVisualizationState CreateVisualization(IActorAnimator actorAnimator, CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext)
    {
        var skillAnimationInfo = new SkillAnimationInfo
        {
            Items = new[]
            {
                new SkillAnimationInfoItem
                {
                    Duration = 0.75f,
                    //HitSound = hitSound,
                    Interaction = () =>
                        Interaction(movementExecution.EffectImposeItems),
                    InteractTime = 0
                }
            }
        };

        var startPosition = actorAnimator.GraphicRoot.Position;
        var targetCombatUnit = GetFirstTarget(movementExecution);

        var targetActor = visualizationContext.GetCombatActor(targetCombatUnit);

        var targetPosition = targetActor.Graphics.Root.Position;

        var subStates = new IActorVisualizationState[]
        {
            new MoveToPositionActorState(actorAnimator,
                new SlowDownMoveFunction(actorAnimator.GraphicRoot.Position, targetPosition),
                new LinearAnimationFrameSet(Enumerable.Range(8, 2).ToArray(), 8, CommonConstants.FrameSize.X,
                    CommonConstants.FrameSize.Y, 8)),
            new DirectInteractionState(actorAnimator, skillAnimationInfo,
                new LinearAnimationFrameSet(Enumerable.Range(10, 8).ToArray(), 8, CommonConstants.FrameSize.X,
                    CommonConstants.FrameSize.Y, 8)),
            new MoveToPositionActorState(actorAnimator,
                new SlowDownMoveFunction(actorAnimator.GraphicRoot.Position, startPosition),
                new LinearAnimationFrameSet(new[] { 0 }, 1, CommonConstants.FrameSize.X, CommonConstants.FrameSize.Y, 8)
                    { IsLoop = true })
        };

        var _innerState = new SequentialState(subStates);
        return _innerState;
    }

    private static Combatant GetFirstTarget(CombatMovementExecution movementExecution)
    {
        var firstImoseItem = movementExecution.EffectImposeItems.First();

        var targetCombatUnit = firstImoseItem.MaterializedTargets.First();
        return targetCombatUnit;
    }

    private static void Interaction(IReadOnlyCollection<CombatEffectImposeItem> effectImposeItems)
    {
        foreach (var effectImposeItem in effectImposeItems)
        {
            foreach (var target in effectImposeItem.MaterializedTargets)
            {
                effectImposeItem.ImposeDelegate(target);
            }
        }
    }
}

internal static class CommonConstants
{
    public static Point FrameSize { get; } = new(256, 128);
}

internal interface ICombatMovementVisualizer
{
    IActorVisualizationState GetMovementVisualizationState(string sid, IActorAnimator actorAnimator, CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext);
}

internal sealed class CombatMovementVisualizer : ICombatMovementVisualizer
{
    private readonly IDictionary<string, ICombatMovementFactory> _movementVisualizationDict;

    public CombatMovementVisualizer()
    {
        var movementFactories = LoadFactories<ICombatMovementFactory>();

        _movementVisualizationDict = movementFactories.ToDictionary(x => x.Sid, x => x);
    }

    public IActorVisualizationState GetMovementVisualizationState(string sid, IActorAnimator actorAnimator, CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext)
    {
        return _movementVisualizationDict[sid].CreateVisualization(actorAnimator, movementExecution, visualizationContext);
    }

    private static IReadOnlyCollection<TFactory> LoadFactories<TFactory>()
    {
        var assembly = typeof(TFactory).Assembly;
        var factoryTypes = assembly.GetTypes()
            .Where(x => typeof(TFactory).IsAssignableFrom(x) && x != typeof(TFactory) && !x.IsAbstract);
        var factories = factoryTypes.Select(Activator.CreateInstance);
        return factories.OfType<TFactory>().ToArray();
    }
}

internal sealed class MoveToPositionActorState : IActorVisualizationState
{
    private readonly IAnimationFrameSet _animation;
    private readonly IActorAnimator _animator;
    private readonly Duration _duration;
    private readonly IMoveFunction _moveFunction;

    private double _counter;

    public MoveToPositionActorState(IActorAnimator animator, IMoveFunction moveFunction, IAnimationFrameSet animation,
        Duration? duration = null)
    {
        _animation = animation;
        _animator = animator;
        _moveFunction = moveFunction;
        if (duration is not null)
        {
            _duration = duration;
        }
        else
        {
            _duration = new Duration(0.25);
        }
    }

    public bool CanBeReplaced => false;
    public bool IsComplete { get; private set; }

    public void Cancel()
    {
        if (IsComplete)
        {
        }
    }

    public void Update(GameTime gameTime)
    {
        if (IsComplete)
        {
            return;
        }

        if (_counter == 0)
        {
            _animator.PlayAnimation(_animation);
        }

        if (_counter <= _duration.Seconds)
        {
            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            var t = _counter / _duration.Seconds;

            var currentPosition = _moveFunction.CalcPosition(t);

            //var jumpTopPosition = Vector2.UnitY * -24 * (float)Math.Sin((float)_counter / DURATION * Math.PI);

            //var fullPosition = horizontalPosition + jumpTopPosition;

            _animator.GraphicRoot.Position = currentPosition;
        }
        else
        {
            IsComplete = true;
            _animator.GraphicRoot.Position = _moveFunction.CalcPosition(1);
        }
    }
}