using System.Collections.Generic;

using Microsoft.Xna.Framework;
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

        IList<IInteractionDelivery> InteractionDeliveryManager { get; }

        ScreenShaker ScreenShaker { get; }

        IDice Dice { get; }

        AnimationBlocker AddAnimationBlocker();

        UnitGameObject GetGameObject(ICombatUnit combatUnit);

        SoundEffectInstance GetHitSound(GameObjectSoundType soundType);

        IBattlefieldInteractionContext BattlefieldInteractionContext { get; }
    }

    internal interface IBattlefieldInteractionContext
    {
        public Rectangle GetArea(Team side);
    }

    internal sealed class BattlefieldInteractionContext : IBattlefieldInteractionContext
    {
        public Rectangle GetArea(Team side)
        {
            if (side == Team.Cpu)
            {
                return new Rectangle(new Point(100 + 400, 100), new Point(200, 200));
            }
            else
            {
                return new Rectangle(new Point(100, 100), new Point(200, 200));
            }
        }
    }

    internal enum Team
    {
        Player,
        Cpu
    }
}