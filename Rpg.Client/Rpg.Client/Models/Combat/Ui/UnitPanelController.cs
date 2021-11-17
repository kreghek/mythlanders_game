using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.Models.Combat.Ui
{
    internal sealed class UnitPanelController
    {
        private const int PANEL_WIDTH = 128;
        private const int PANEL_HEIGHT = 48;
        private const int BAR_WIDTH = 70;
        private readonly Core.Combat _activeCombat;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private readonly IUiContentStorage _uiContentStorage;

        public UnitPanelController(
            ResolutionIndependentRenderer resolutionIndependentRenderer,
            Core.Combat activeCombat,
            IUiContentStorage uiContentStorage,
            GameObjectContentStorage gameObjectContentStorage)
        {
            _resolutionIndependentRenderer = resolutionIndependentRenderer;
            _activeCombat = activeCombat;
            _uiContentStorage = uiContentStorage;
            _gameObjectContentStorage = gameObjectContentStorage;
        }

        public void Draw(SpriteBatch spriteBatch)
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
                    var panelY = playerIndex * (PANEL_HEIGHT + 20);
                    panelPosition = new Vector2(0, panelY);
                    playerIndex++;
                }
                else
                {
                    var panelY = monsterIndex * (PANEL_HEIGHT + 20);
                    panelPosition = new Vector2(_resolutionIndependentRenderer.VirtualWidth - PANEL_WIDTH,
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
                var hpPercentage = (float)unit.Hp / unit.MaxHp;
                var hpSourceRect = new Rectangle(0, 50, (int)(hpPercentage * BAR_WIDTH), 11);
                spriteBatch.Draw(_uiContentStorage.GetUnitPanelTexture(), hpPosition, hpSourceRect,
                    Color.Lerp(Color.Transparent, Color.White, 0.75f));

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), $"{unit.Hp}/{unit.MaxHp}",
                    hpPosition, Color.Black);

                if (unit.IsPlayerControlled && unit.HasSkillsWithCost)
                {
                    var manaPosition = panelPosition + new Vector2(55, 40);
                    var manaPercentage = (float)unit.ManaPool / unit.ManaPoolSize;
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