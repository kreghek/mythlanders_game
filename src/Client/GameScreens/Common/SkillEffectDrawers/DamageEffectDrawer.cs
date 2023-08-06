using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;

using JetBrains.Annotations;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common.SkillEffectDrawers;

[UsedImplicitly]
internal class DamageEffectDrawer : ISkillEffectDrawer
{
    private readonly SpriteFont _font;

    public DamageEffectDrawer(SpriteFont font)
    {
        _font = font;
    }

    public bool Draw(SpriteBatch spriteBatch, IEffectInstance effectToDisplay,
        Vector2 position)
    {
        if (effectToDisplay is not DamageEffectInstance attackEffect)
        {
            return false;
        }

        var damageRange = attackEffect.Damage;

        var targetSelectorText = SkillEffectDrawerHelper.GetLocalized(effectToDisplay.Selector);
        spriteBatch.DrawString(_font,
            string.Format(UiResource.DamageEffectRuleText, damageRange.Min, damageRange.Max, targetSelectorText),
            position,
            Color.Wheat);

        return true;
    }
}