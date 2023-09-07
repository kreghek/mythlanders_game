using CombatDicesTeam.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common.SkillEffectDrawers;

internal class PeriodicDamageEffectDrawer : ISkillEffectDrawer
{
    private readonly SpriteFont _font;

    public PeriodicDamageEffectDrawer(SpriteFont font)
    {
        _font = font;
    }

    public bool Draw(SpriteBatch spriteBatch, IEffectInstance effectToDisplay,
        Vector2 position)
    {
        //if (effectToDisplay is not PeriodicDamageEffect attackEffect)
        //{
        //    return false;
        //}

        //var damage = attackEffect.CalculateDamage();

        //var ruleDirectionText = SkillEffectDrawerHelper.GetLocalized(direction);
        //spriteBatch.DrawString(_font,
        //    string.Format(UiResource.PeriodicDamageEffectRuleText, damage.Min, damage.Max, ruleDirectionText,
        //        attackEffect.EffectLifetime.GetTextDescription()),
        //    position, Color.Wheat);

        return true;
    }
}