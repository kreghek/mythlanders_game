using Core.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common.SkillEffectDrawers;

internal class LifeDrawEffectDrawer : ISkillEffectDrawer
{
    private readonly SpriteFont _font;

    public LifeDrawEffectDrawer(SpriteFont font)
    {
        _font = font;
    }

    public bool Draw(SpriteBatch spriteBatch, IEffectInstance effectToDisplay,
        Vector2 position)
    {
        //if (effectToDisplay is not LifeDrawEffect lifeDrawEffect)
        //{
        //    return false;
        //}

        //var damage = lifeDrawEffect.CalculateDamage();
        //var ruleDirectionText = SkillEffectDrawerHelper.GetLocalized(direction);

        //spriteBatch.DrawString(_font,
        //    string.Format(UiResource.LifeDrawEffectRuleText, damage.Min, damage.Max,
        //        ruleDirectionText),
        //    position, Color.Wheat);

        return true;
    }
}