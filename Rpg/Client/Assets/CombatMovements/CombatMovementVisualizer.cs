using System;
using System.Collections.Generic;
using System.Linq;

using Client.Engine;

using Core.Combats;

using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Client.Assets.CombatMovements;

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
