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
}