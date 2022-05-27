using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.GameScreens.Common.SkillEffectDrawers
{
    internal interface ISkillEffectDrawer
    {
        bool Draw(SpriteBatch spriteBatch, object effectToDisplay, SkillDirection direction, Vector2 position);
    }
}