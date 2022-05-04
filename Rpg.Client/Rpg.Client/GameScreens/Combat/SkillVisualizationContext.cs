using System.Collections.Generic;

using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.GameScreens.Combat
{
    internal class SkillVisualizationContext : ISkillVisualizationContext
    {
        public IAnimationManager AnimationManager { get; init; } = null!;
        public SkillExecution Interaction { get; init; } = null!;
        public IList<IInteractionDelivery> InteractionDeliveryList { get; init; } = null!;

        public SoundEffectInstance GetHitSound(GameObjectSoundType soundType)
        {
            return GameObjectContentStorage.GetSkillUsageSound(soundType).CreateInstance();
        }

        public GameObjectContentStorage GameObjectContentStorage { get; init; } = null!;
        public ScreenShaker ScreenShaker { get; init; } = null!;
    }
}