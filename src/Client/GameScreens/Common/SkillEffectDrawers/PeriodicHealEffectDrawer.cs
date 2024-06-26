using CombatDicesTeam.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common.SkillEffectDrawers;

internal class PeriodicHealEffectDrawer : ISkillEffectDrawer
{
    private readonly SpriteFont _font;

    public PeriodicHealEffectDrawer(SpriteFont font)
    {
        _font = font;
    }

    public bool Draw(SpriteBatch spriteBatch, IEffectInstance effectToDisplay,
        Vector2 position)
    {
        //if (effectToDisplay is not PeriodicHealEffect periodicHealEffect)
        //{
        //    return false;
        //}

        //var heal = periodicHealEffect.CalculateHeal();
        //var ruleDirectionText = SkillEffectDrawerHelper.GetLocalized(direction);

        //spriteBatch.DrawString(_font,
        //    string.Format(UiResource.PeriodicHealEffectRuleText, heal.Min, heal.Max,
        //        ruleDirectionText, periodicHealEffect.EffectLifetime.GetTextDescription()),
        //    position, Color.Wheat);

        return true;
    }
}