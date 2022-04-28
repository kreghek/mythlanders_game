using Rpg.Client.Core.Skills;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.GameScreens.Combat
{
    internal interface IVisualizedSkill: ISkill
    {
        
        SkillVisualization Visualization { get; }

        IUnitStateEngine CreateState(UnitGameObject targetGameObject,
            ISkillVisualizationContext context);
    }
}