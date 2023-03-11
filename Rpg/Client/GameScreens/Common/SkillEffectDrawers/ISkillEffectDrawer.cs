using Core.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ITargetSelector = Client.Core.Skills.ITargetSelector;

namespace Client.GameScreens.Common.SkillEffectDrawers;

internal interface ISkillEffectDrawer
{
    bool Draw(SpriteBatch spriteBatch, IEffectInstance movementEffect, Vector2 drawingPosition);
}