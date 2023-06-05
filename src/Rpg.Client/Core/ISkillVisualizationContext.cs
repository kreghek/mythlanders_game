using System.Collections.Generic;

using Microsoft.Xna.Framework.Audio;

using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Combat.GameObjects;

namespace Rpg.Client.Core
{
    internal interface ISkillVisualizationContext
    {
        IAnimationManager AnimationManager { get; }

        IBattlefieldInteractionContext BattlefieldInteractionContext { get; }

        IDice Dice { get; }

        GameObjectContentStorage GameObjectContentStorage { get; }

        SkillExecution Interaction { get; }

        IList<IInteractionDelivery> InteractionDeliveryManager { get; }

        ScreenShaker ScreenShaker { get; }

        AnimationBlocker AddAnimationBlocker();

        UnitGameObject GetGameObject(ICombatUnit combatUnit);

        SoundEffectInstance GetSoundEffect(GameObjectSoundType soundType);
    }
}