using System;
using System.Collections.Generic;
using System.Linq;

using Client.Engine;

using CombatDicesTeam.Combats;

using GameClient.Engine.Animations;

namespace Client.Assets.CombatMovements;

internal sealed class CombatMovementVisualizationProvider : ICombatMovementVisualizationProvider
{
    private readonly IDictionary<CombatMovementSid, ICombatMovementFactory> _movementVisualizationDict;

    public CombatMovementVisualizationProvider()
    {
        var movementFactories = LoadFactories<ICombatMovementFactory>();

        _movementVisualizationDict = movementFactories.ToDictionary(x => (CombatMovementSid)x.Sid, x => x);
    }

    private static CombatMovementScene CreateDefaultMovementVisualizationState(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext)
    {
        var config = new SingleMeleeVisualizationConfig(
            new SoundedAnimation(
                new LinearAnimationFrameSet(new[]
                    {
                        0
                    }, 1, CommonConstants.FrameSize.X,
                    CommonConstants.FrameSize.Y, 8), null),
            new SoundedAnimation(
                new LinearAnimationFrameSet(Enumerable.Range(0, 1).ToArray(), 8, CommonConstants.FrameSize.X,
                    CommonConstants.FrameSize.Y, 8), null),
            new SoundedAnimation(
                new LinearAnimationFrameSet(Enumerable.Range(0, 1).ToArray(), 8, CommonConstants.FrameSize.X,
                    CommonConstants.FrameSize.Y, 8), null),
            new SoundedAnimation(
                new LinearAnimationFrameSet(Enumerable.Range(0, 1).ToArray(), 8, CommonConstants.FrameSize.X,
                    CommonConstants.FrameSize.Y, 8), null),
            new SoundedAnimation(
                new LinearAnimationFrameSet(new[]
                    {
                        0
                    }, 1, CommonConstants.FrameSize.X,
                    CommonConstants.FrameSize.Y, 8)
                {
                    IsLooping = true
                }, null));

        return CommonCombatVisualization.CreateSingleMeleeVisualization(actorAnimator, movementExecution,
            visualizationContext,
            config);
    }

    private static IReadOnlyCollection<TFactory> LoadFactories<TFactory>()
    {
        var assembly = typeof(TFactory).Assembly;
        var factoryTypes = assembly.GetTypes()
            .Where(x => typeof(TFactory).IsAssignableFrom(x) && x != typeof(TFactory) && !x.IsAbstract);
        var factories = factoryTypes.Select(Activator.CreateInstance);
        return factories.OfType<TFactory>().ToArray();
    }

    /// <inheritdoc />
    public CombatMovementIcon GetMoveIcon(CombatMovementSid sid)
    {
        if (!_movementVisualizationDict.TryGetValue(sid, out var factory))
        {
            return new CombatMovementIcon(0, 0);
        }

        return factory.CombatMovementIcon;
    }

    /// <inheritdoc />
    public IReadOnlyList<CombatMovementEffectDisplayValue> ExtractCombatMovementValues(
        CombatMovementInstance combatMovementInstance)
    {
        if (!_movementVisualizationDict.TryGetValue(combatMovementInstance.SourceMovement.Sid, out var factory))
        {
            return Array.Empty<CombatMovementEffectDisplayValue>();
        }

        return factory.ExtractEffectsValues(combatMovementInstance);
    }

    /// <inheritdoc />
    public CombatMovementScene GetMovementVisualizationState(CombatMovementSid sid, IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext)
    {
        if (_movementVisualizationDict.TryGetValue(sid, out var factory))
        {
            return factory.CreateVisualization(actorAnimator, movementExecution, visualizationContext);
        }

        // Defensive code
        return CreateDefaultMovementVisualizationState(
            actorAnimator,
            movementExecution,
            visualizationContext);
    }
}