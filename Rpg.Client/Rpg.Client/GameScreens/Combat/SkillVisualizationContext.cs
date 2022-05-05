using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.GameScreens.Combat
{
    internal class SkillVisualizationContext : ISkillVisualizationContext
    {
        private readonly IList<UnitGameObject> _unitGameObjects;

        public SkillVisualizationContext(IList<UnitGameObject> unitGameObjects)
        {
            _unitGameObjects = unitGameObjects;
        }

        public IAnimationManager AnimationManager { get; init; } = null!;
        public SkillExecution Interaction { get; init; } = null!;
        public IList<IInteractionDelivery> InteractionDeliveryList { get; init; } = null!;

        public SoundEffectInstance GetHitSound(GameObjectSoundType soundType)
        {
            return GameObjectContentStorage.GetSkillUsageSound(soundType).CreateInstance();
        }

        public UnitGameObject GetGameObject(ICombatUnit combatUnit)
        {
            return _unitGameObjects.Single(x => x.CombatUnit == combatUnit);
        }

        public AnimationBlocker AddAnimationBlocker()
        {
            return AnimationManager.CreateAndUseBlocker();
        }

        public GameObjectContentStorage GameObjectContentStorage { get; init; } = null!;
        public ScreenShaker ScreenShaker { get; init; } = null!;
    }
}