using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.CombatMovements;
using Client.Core;
using Client.Engine;

using CombatDicesTeam.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Barracks.Ui;

internal sealed class SkillsInfoPanel : PanelBase
{
    private const int ICON_SIZE = 32;
    private const int MARGIN = 5;
    private readonly SpriteFont _mainFont;

    private readonly IList<EntityIconButton<CombatMovement>> _skillList;

    public SkillsInfoPanel(HeroState hero, SpriteFont mainFont, ICombatMovementVisualizationProvider combatMovementVisualizer, IUiContentStorage uiContentStorage)
    {
        _skillList = new List<EntityIconButton<CombatMovement>>();
        var heroAvailableMovements = hero.AvailableMovements.ToArray();
        for (var index = 0; index < heroAvailableMovements.Length; index++)
        {
            var movement = heroAvailableMovements[index];

            var icon = combatMovementVisualizer.GetMoveIcon(movement.Sid);
            var iconRect = UnsortedHelpers.GetIconRect(icon);
            var iconData = new IconData(uiContentStorage.GetCombatMoveIconsTexture(), iconRect);

            var skillIconButton = new EntityIconButton<CombatMovement>(iconData, movement);
            _skillList.Add(skillIconButton);

            skillIconButton.OnClick += SkillIconButton_OnClick;
        }

        _mainFont = mainFont;
    }

    protected override string TitleResourceId => nameof(UiResource.HeroSkillsInfoTitle);

    protected override Color CalculateColor()
    {
        return Color.White;
    }

    protected override void DrawPanelContent(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        for (var index = 0; index < _skillList.Count; index++)
        {
            var skillButton = _skillList[index];

            skillButton.Rect = new Rectangle(
                contentRect.Location + new Point(MARGIN, MARGIN + index * (ICON_SIZE + MARGIN)),
                new Point(ICON_SIZE, ICON_SIZE));
            skillButton.Draw(spriteBatch);

            var skillNameText = GameObjectHelper.GetLocalized(skillButton.Entity.Sid);
            spriteBatch.DrawString(_mainFont, skillNameText,
                skillButton.Rect.Location.ToVector2() + new Vector2(ICON_SIZE + MARGIN, 0), Color.Wheat);

            DrawMovementCost(spriteBatch: spriteBatch, skillButton: skillButton);
        }
    }

    private void DrawMovementCost(SpriteBatch spriteBatch, EntityIconButton<CombatMovement> skillButton)
    {
        if (skillButton.Entity.Cost.Amount.Current <= 0)
        {
            return;
        }

        var manaCostText =
            string.Format(UiResource.ManaCostLabelTemplate, skillButton.Entity.Cost.Amount.Current);
        spriteBatch.DrawString(_mainFont, manaCostText,
            skillButton.Rect.Location.ToVector2() + new Vector2(ICON_SIZE + MARGIN, 20), Color.Cyan);
    }

    private void SkillIconButton_OnClick(object? sender, EventArgs e)
    {
    }
}