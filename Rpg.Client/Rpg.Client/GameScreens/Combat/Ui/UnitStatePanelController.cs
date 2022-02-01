using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal sealed class UnitStatePanelController
    {
        private const int PANEL_WIDTH = 128;
        private const int PANEL_HEIGHT = 48;
        private const int BAR_WIDTH = 70;
        private readonly Core.Combat _activeCombat;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly IUiContentStorage _uiContentStorage;

        public UnitStatePanelController(
            ResolutionIndependentRenderer resolutionIndependentRenderer,
            Core.Combat activeCombat,
            IUiContentStorage uiContentStorage,
            GameObjectContentStorage gameObjectContentStorage)
        {
            _activeCombat = activeCombat;
            _uiContentStorage = uiContentStorage;
            _gameObjectContentStorage = gameObjectContentStorage;
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle contentRectangle)
        {
            var unitList = _activeCombat.Units.ToArray();

            var playerIndex = 0;
            var monsterIndex = 0;

            foreach (var combatUnit in unitList)
            {
                Vector2 panelPosition;

                var unit = combatUnit.Unit;

                if (unit.IsPlayerControlled)
                {
                    var panelY = contentRectangle.Top + playerIndex * (PANEL_HEIGHT + 20);
                    panelPosition = new Vector2(0, panelY);
                    playerIndex++;
                }
                else
                {
                    var panelY = contentRectangle.Top + monsterIndex * (PANEL_HEIGHT + 20);
                    panelPosition = new Vector2(contentRectangle.Width - PANEL_WIDTH,
                        panelY);
                    monsterIndex++;
                }

                spriteBatch.Draw(_uiContentStorage.GetUnitPanelTexture(), panelPosition,
                    new Rectangle(0, 0, PANEL_WIDTH, PANEL_HEIGHT),
                    Color.White);

                var portraitSourceRect = UnsortedHelpers.GetUnitPortraitRect(unit.UnitScheme.Name);
                var portraitPosition = panelPosition + new Vector2(7, 0);
                var portraitDestRect = new Rectangle(portraitPosition.ToPoint(), new Point(32, 32));
                spriteBatch.Draw(_gameObjectContentStorage.GetUnitPortrains(), portraitDestRect, portraitSourceRect,
                    Color.White);

                var unitName = GameObjectHelper.GetLocalized(unit.UnitScheme.Name);
                var unitNamePosition = panelPosition + new Vector2(55, 3);
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                    $"{unitName} {UiResource.MonsterLevelShortText}{unit.Level}", unitNamePosition, Color.White);

                var hpPosition = panelPosition + new Vector2(55, 20);
                var hpPercentage = (float)unit.HitPoints / unit.MaxHitPoints;
                var hpSourceRect = new Rectangle(0, 50, (int)(hpPercentage * BAR_WIDTH), 11);
                spriteBatch.Draw(_uiContentStorage.GetUnitPanelTexture(), hpPosition, hpSourceRect,
                    Color.Lerp(Color.Transparent, Color.White, 0.75f));

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), $"{unit.HitPoints}/{unit.MaxHitPoints}",
                    hpPosition, Color.Black);

                if (unit.IsPlayerControlled && unit.HasSkillsWithCost)
                {
                    var manaPosition = panelPosition + new Vector2(55, 40);
                    var manaPoolValue = Math.Min((float)unit.ManaPool, unit.ManaPoolSize);
                    var manaPercentage = manaPoolValue / unit.ManaPoolSize;
                    var manaSourceRect = new Rectangle(0, 62, (int)(manaPercentage * BAR_WIDTH), 11);
                    spriteBatch.Draw(_uiContentStorage.GetUnitPanelTexture(), manaPosition, manaSourceRect,
                        Color.Lerp(Color.Transparent, Color.White, 0.75f));

                    spriteBatch.DrawString(_uiContentStorage.GetMainFont(), $"{unit.ManaPool}/{unit.ManaPoolSize}",
                        manaPosition, Color.Black);
                }

                var statText = $"Dmg:{unit.Damage} Ar:{unit.Armor}";
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), statText, panelPosition + new Vector2(55, 50),
                    Color.Gray);
            }
        }
    }
}