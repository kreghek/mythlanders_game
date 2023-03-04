using Client.GameScreens.Combat.GameObjects;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.GameScreens.Combat
{
    internal interface IVisualizedSkill : ISkill
    {
        SkillVisualization Visualization { get; }

        IActorVisualizationState CreateState(
            UnitGameObject animatedUnitGameObject,
            UnitGameObject targetUnitGameObject,
            IAnimationBlocker mainAnimationBlocker,
            ISkillVisualizationContext context);
    }
}