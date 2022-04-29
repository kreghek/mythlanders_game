using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.GameScreens.Combat
{
    internal interface ISkillVisualizationContext
    {
        IAnimationManager AnimationManager { get; }

        Action Interaction { get; }

        IList<IInteractionDelivery> InteractionDeliveryList { get; }

        SoundEffectInstance GetHitSound(GameObjectSoundType soundType);

        GameObjectContentStorage GameObjectContentStorage { get; }

        ScreenShaker ScreenShaker { get; }
    }
    
    internal class SkillVisualizationContext: ISkillVisualizationContext
    {
        public IAnimationManager AnimationManager { get; set; }
        public Action Interaction { get; set; }
        public IList<IInteractionDelivery> InteractionDeliveryList { get; set; }
        public SoundEffectInstance GetHitSound(GameObjectSoundType soundType)
        {
            return GameObjectContentStorage.GetSkillUsageSound(soundType).CreateInstance();
        }

        public GameObjectContentStorage GameObjectContentStorage { get; set; }
        public ScreenShaker ScreenShaker { get; set; }
    }
}