using System.Collections.Generic;

using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.GameScreens.Combat
{
    internal interface ISkillVisualizationContext
    {
        IAnimationManager AnimationManager { get; }

        GameObjectContentStorage GameObjectContentStorage { get; }

        SkillExecution Interaction { get; }

        IList<IInteractionDelivery> InteractionDeliveryList { get; }

        ScreenShaker ScreenShaker { get; }
        AnimationBlocker AddAnimationBlocker();

        UnitGameObject GetGameObject(ICombatUnit combatUnit);

        SoundEffectInstance GetHitSound(GameObjectSoundType soundType);
    }
}