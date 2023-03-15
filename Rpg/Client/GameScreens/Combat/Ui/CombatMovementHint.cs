using System.Linq;

using Client.GameScreens.Common.SkillEffectDrawers;

using Core.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
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

        var combatMoveTitlePosition = clientRect.Location.ToVector2() + new Vector2(5, 15);

        var combatMoveTitle = GameObjectHelper.GetLocalized(_combatMovement.SourceMovement.Sid);

        spriteBatch.DrawString(_nameFont, combatMoveTitle, combatMoveTitlePosition, color);

        var manaCostPosition = combatMoveTitlePosition + new Vector2(0, 15);
        if (_combatMovement.SourceMovement.Cost.HasCost)
        {
            spriteBatch.DrawString(_font,
                string.Format(UiResource.SkillManaCostTemplate, _combatMovement.SourceMovement.Cost.Value),
                manaCostPosition, color);
        }

        var combatMoveDescription =
            StringHelper.LineBreaking(GameObjectHelper.GetLocalizedDescription(_combatMovement.SourceMovement.Sid), 60);
        var descriptionBlockPosition = manaCostPosition + new Vector2(0, 20);
        spriteBatch.DrawString(_font, combatMoveDescription, descriptionBlockPosition, Color.Wheat);
    }
}