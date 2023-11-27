using System.Linq;

using Client.Core;
using Client.Engine;

using CombatDicesTeam.Combats;

using GameClient.Engine.Animations;

using Microsoft.Xna.Framework;

namespace Client.Assets.CombatMovements;

/// <summary>
/// Auxiliary class to group methods to work with animations.
/// </summary>
internal static class AnimationHelper
{
    /// <summary>
    /// Convert animation from content to game engine animation.
    /// </summary>
    /// <param name="spredsheetAnimationData">Content animation.</param>
    /// <param name="animation">Animation name.</param>
    public static IAnimationFrameSet ConvertToAnimation(SpriteAtlasAnimationData spredsheetAnimationData,
        string animation)
    {
        var spriteSheetAnimationDataCycles = spredsheetAnimationData.Cycles[animation];

        return new LinearAnimationFrameSet(
            spriteSheetAnimationDataCycles.Frames,
            spriteSheetAnimationDataCycles.Fps,
            spredsheetAnimationData.TextureAtlas.RegionWidth,
            spredsheetAnimationData.TextureAtlas.RegionHeight, 8)
        {
            IsLooping = spriteSheetAnimationDataCycles.IsLooping
        };
    }

    private static ICombatant? GetFirstTargetOrDefault(CombatMovementExecution movementExecution,
        ICombatant actorCombatant)
    {
        var firstImposeItem =
            movementExecution.EffectImposeItems.FirstOrDefault(x =>
                x.MaterializedTargets.All(t => t != actorCombatant));

        var targetCombatUnit = firstImposeItem?.MaterializedTargets.FirstOrDefault(t => t != actorCombatant);
        return targetCombatUnit;
    }

    public static Vector2 GetTargetPositionByCombatMovementCombatant(CombatMovementExecution movementExecution,
        ICombatMovementVisualizationContext visualizationContext)
    {
        var targetCombatant =
            GetFirstTargetOrDefault(movementExecution, visualizationContext.ActorGameObject.Combatant);

        Vector2 targetPosition;
        if (targetCombatant is not null)
        {
            targetPosition = visualizationContext.GetCombatActor(targetCombatant).InteractionPoint;
        }
        else
        {
            targetPosition = visualizationContext.BattlefieldInteractionContext.GetArea(Team.Cpu).Center.ToVector2();
        }

        return targetPosition;
    }
}