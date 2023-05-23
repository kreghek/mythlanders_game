﻿using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.CombatMovements.Hero.Swordsman;
using Client.Core.AnimationFrameSets;
using Client.Engine;

using Core.Combats;

namespace Client.Assets.CombatMovements;

internal sealed class CombatMovementVisualizer : ICombatMovementVisualizer
{
    private readonly IDictionary<CombatMovementSid, ICombatMovementFactory> _movementVisualizationDict;

    public CombatMovementVisualizer()
    {
        var movementFactories = LoadFactories<ICombatMovementFactory>();

        _movementVisualizationDict = movementFactories.ToDictionary(x => (CombatMovementSid)x.Sid, x => x);
    }

    private static IReadOnlyCollection<TFactory> LoadFactories<TFactory>()
    {
        var assembly = typeof(TFactory).Assembly;
        var factoryTypes = assembly.GetTypes()
            .Where(x => typeof(TFactory).IsAssignableFrom(x) && x != typeof(TFactory) && !x.IsAbstract);
        var factories = factoryTypes.Select(Activator.CreateInstance);
        return factories.OfType<TFactory>().ToArray();
    }

    public CombatMovementIcon GetMoveIcon(CombatMovementSid sid)
    {
        if (!_movementVisualizationDict.TryGetValue(sid, out var factory))
        {
            return new CombatMovementIcon(0, 0);
        }

        return factory.CombatMovementIcon;
    }

    public CombatMovementScene GetMovementVisualizationState(CombatMovementSid sid, IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext)
    {
        if (!_movementVisualizationDict.TryGetValue(sid, out var factory))
        {
            var config = new SingleMeleeVisualizationConfig(
                new LinearAnimationFrameSet(Enumerable.Range(0, 1).ToArray(), 8, CommonConstants.FrameSize.X,
                    CommonConstants.FrameSize.Y, 8),
                new LinearAnimationFrameSet(new[] { 0 }, 1, CommonConstants.FrameSize.X, CommonConstants.FrameSize.Y, 8)
                    { IsLoop = true });

            return CommonCombatVisualization.CreateSingleMeleeVisualization(actorAnimator, movementExecution,
                visualizationContext,
                config);
        }

        return factory.CreateVisualization(actorAnimator, movementExecution, visualizationContext);
    }
}