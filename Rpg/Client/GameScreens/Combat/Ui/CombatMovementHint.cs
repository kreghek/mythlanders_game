using System.Linq;

using Client.GameScreens.Common.SkillEffectDrawers;

using Core.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Common.SkillEffectDrawers;

namespace Client.GameScreens.Combat.Ui;

internal class CombatMovementHint : HintBase
{
    private readonly ISkillEffectDrawer[] _effectDrawers;
    private readonly SpriteFont _nameFont;
    private readonly SpriteFont _font;
    private readonly CombatMovementInstance _combatMovement;

    public CombatMovementHint(CombatMovementInstance skill)
    {
        _nameFont = UiThemeManager.UiContentStorage.GetTitlesFont();
        _font = UiThemeManager.UiContentStorage.GetMainFont();
        _combatMovement = skill;
        _effectDrawers = EffectDrawersCollector.GetDrawersInAssembly(_font).ToArray();
    }

    protected override Point CalcTextureOffset()
    {
        return Point.Zero;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle clientRect, Color contentColor)
    {
        var color = Color.White;

        var skillTitlePosition = clientRect.Location.ToVector2() + new Vector2(5, 15);

        var skillNameText = GameObjectHelper.GetLocalized(_combatMovement.SourceMovement.Sid);

        spriteBatch.DrawString(_nameFont, skillNameText, skillTitlePosition, color);

        var manaCostPosition = skillTitlePosition + new Vector2(0, 10);
        if (_combatMovement.SourceMovement.Cost.HasCost)
        {
            spriteBatch.DrawString(_font,
                string.Format(UiResource.SkillManaCostTemplate, _combatMovement.SourceMovement.Cost.Value),
                manaCostPosition, color);
        }

        var ruleBlockPosition = manaCostPosition + new Vector2(0, 20);
        var skillEffects = _combatMovement.Effects.ToArray();
        for (var ruleIndex = 0; ruleIndex < skillEffects.Length; ruleIndex++)
        {
            var effectToDisplay = skillEffects[ruleIndex];

            var rulePosition = ruleBlockPosition + new Vector2(0, 10) * ruleIndex;

            foreach (var effectDrawer in _effectDrawers)
            {
                if (effectDrawer.Draw(spriteBatch, effectToDisplay, rulePosition))
                {
                    break;
                }
            }
        }
    }
}