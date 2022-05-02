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

        GameObjectContentStorage GameObjectContentStorage { get; }

        Action Interaction { get; }

        IList<IInteractionDelivery> InteractionDeliveryList { get; }

        ScreenShaker ScreenShaker { get; }

        SoundEffectInstance GetHitSound(GameObjectSoundType soundType);
    }
}