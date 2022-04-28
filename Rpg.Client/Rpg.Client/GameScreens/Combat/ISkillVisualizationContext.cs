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

        IList<IInteractionDelivery> interactionDeliveryList { get; }

        SoundEffectInstance GetHitSound(GameObjectSoundType);
    }
}