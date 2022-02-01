using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal sealed class UnitStatePanelController
    {
        private const int PANEL_WIDTH = 189;
        private const int PANEL_HEIGHT = 48;
        private const int BAR_WIDTH = 118;
        private const int PANEL_BACKGROUND_VERTICAL_OFFSET = 12;
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
                    var panelY = contentRectangle.Top + playerIndex * (PANEL_HEIGHT + PANEL_BACKGROUND_VERTICAL_OFFSET + 20);
                    panelPosition = new Vector2(0, panelY);
                    playerIndex++;
                }
                else
                {
                    var panelY = contentRectangle.Top + monsterIndex * (PANEL_HEIGHT + PANEL_BACKGROUND_VERTICAL_OFFSET + 20);
                    panelPosition = new Vector2(contentRectangle.Width - PANEL_WIDTH,
                        panelY);
                    monsterIndex++;
                }

                var backgroundOffset = new Vector2(0, PANEL_BACKGROUND_VERTICAL_OFFSET);

                DrawUnitHitPointsBar(spriteBatch, panelPosition, unit, backgroundOffset);

                DrawPanelBackground(spriteBatch, panelPosition, backgroundOffset);

                DrawUnitPortrait(spriteBatch, panelPosition, unit);

                DrawUnitName(spriteBatch, panelPosition, unit);

                if (HasMana(unit))
                {
                    DrawManaBar(spriteBatch, panelPosition, unit);
                }

                //var statText = $"Dmg:{unit.Damage} Ar:{unit.Armor}";
                //spriteBatch.DrawString(_uiContentStorage.GetMainFont(), statText, panelPosition + new Vector2(55, 50),
                //    Color.Gray);
            }
        }

        private void DrawManaBar(SpriteBatch spriteBatch, Vector2 panelPosition, Core.Unit unit)
        {
            //var manaPosition = panelPosition + new Vector2(55, 40);
            //var manaPoolValue = Math.Min((float)unit.ManaPool, unit.ManaPoolSize);
            //var manaPercentage = manaPoolValue / unit.ManaPoolSize;
            //var manaSourceRect = new Rectangle(0, 62, (int)(manaPercentage * BAR_WIDTH), 11);
            //spriteBatch.Draw(_uiContentStorage.GetUnitStatePanelTexture(), manaPosition, manaSourceRect,
            //    Color.Lerp(Color.Transparent, Color.White, 0.75f));

            //spriteBatch.DrawString(_uiContentStorage.GetMainFont(), $"{unit.ManaPool}/{unit.ManaPoolSize}",
            //    manaPosition, Color.Black);
        }

        private static bool HasMana(Core.Unit unit)
        {
            return unit.IsPlayerControlled && unit.HasSkillsWithCost;
        }

        private void DrawUnitHitPointsBar(SpriteBatch spriteBatch, Vector2 panelPosition, Core.Unit unit, Vector2 backgroundOffset)
        {
            var hpPosition = panelPosition + backgroundOffset + new Vector2(46, 22);
            var hpPercentage = (float)unit.HitPoints / unit.MaxHitPoints;
            var hpSourceRect = new Rectangle(0, 49, (int)(hpPercentage * BAR_WIDTH), 20);
            spriteBatch.Draw(_uiContentStorage.GetUnitStatePanelTexture(), hpPosition, hpSourceRect,
                Color.Lerp(Color.Transparent, Color.White, 1f));

            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), $"{unit.HitPoints}/{unit.MaxHitPoints}",
                hpPosition, Color.Black);
        }

        private void DrawUnitName(SpriteBatch spriteBatch, Vector2 panelPosition, Core.Unit unit)
        {
            var unitName = GameObjectHelper.GetLocalized(unit.UnitScheme.Name);
            var unitNamePosition = panelPosition + new Vector2(55, 3);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                unitName, unitNamePosition, Color.White);
        }

        private void DrawUnitPortrait(SpriteBatch spriteBatch, Vector2 panelPosition, Core.Unit unit)
        {
            spriteBatch.Draw(_uiContentStorage.GetUnitStatePanelTexture(), panelPosition, new Rectangle(0, 83, 42, 32), Color.White);

            var portraitSourceRect = UnsortedHelpers.GetUnitPortraitRect(unit.UnitScheme.Name);
            var portraitPosition = panelPosition + new Vector2(7, 0);
            var portraitDestRect = new Rectangle(portraitPosition.ToPoint(), new Point(32, 32));
            spriteBatch.Draw(_gameObjectContentStorage.GetUnitPortrains(), portraitDestRect, portraitSourceRect,
                Color.White);
        }

        private void DrawPanelBackground(SpriteBatch spriteBatch, Vector2 panelPosition, Vector2 backgroundOffset)
        {
            spriteBatch.Draw(_uiContentStorage.GetUnitStatePanelTexture(), panelPosition + backgroundOffset,
                                new Rectangle(0, 0, PANEL_WIDTH, PANEL_HEIGHT),
                                Color.White);
        }
    }
}