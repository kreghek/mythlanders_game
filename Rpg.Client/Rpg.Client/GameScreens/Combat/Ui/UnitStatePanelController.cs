using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal sealed class UnitStatePanelController
    {
        private const int PANEL_WIDTH = 189;
        private const int PANEL_HEIGHT = 48;
        private const int BAR_WIDTH = 118;
        private const int PANEL_BACKGROUND_VERTICAL_OFFSET = 12;
        private const int MARGIN = 10;
        private readonly Core.Combat _activeCombat;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly bool _tempShowEffects;
        private readonly IUiContentStorage _uiContentStorage;

        public UnitStatePanelController(Core.Combat activeCombat,
            IUiContentStorage uiContentStorage,
            GameObjectContentStorage gameObjectContentStorage,
            bool tempShowEffects)
        {
            _activeCombat = activeCombat;
            _uiContentStorage = uiContentStorage;
            _gameObjectContentStorage = gameObjectContentStorage;
            _tempShowEffects = tempShowEffects;
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

                var side = unit.IsPlayerControlled ? Side.Left : Side.Right;

                if (side == Side.Left)
                {
                    var panelY = contentRectangle.Top +
                                 playerIndex * (PANEL_HEIGHT + PANEL_BACKGROUND_VERTICAL_OFFSET + MARGIN);
                    panelPosition = new Vector2(0, panelY);
                    playerIndex++;
                }
                else
                {
                    var panelY = contentRectangle.Top +
                                 monsterIndex * (PANEL_HEIGHT + PANEL_BACKGROUND_VERTICAL_OFFSET + MARGIN);
                    panelPosition = new Vector2(contentRectangle.Right - PANEL_WIDTH,
                        panelY);
                    monsterIndex++;
                }

                var backgroundOffset = new Vector2(0, PANEL_BACKGROUND_VERTICAL_OFFSET);

                DrawUnitHitPointsBar(spriteBatch, unit, panelPosition, backgroundOffset, side);

                DrawPanelBackground(spriteBatch, panelPosition, backgroundOffset, side);

                DrawUnitPortrait(spriteBatch, panelPosition, unit, side);

                DrawUnitName(spriteBatch, panelPosition, unit, side);

                DrawTargets(spriteBatch, panelPosition, combatUnit);

                if (HasMana(unit))
                {
                    DrawManaBar(spriteBatch, panelPosition, combatUnit);
                }

                if (_tempShowEffects)
                {
                    DrawEffects(spriteBatch, panelPosition, combatUnit);
                }
            }
        }

        private void DrawEffects(SpriteBatch spriteBatch, Vector2 panelPosition, CombatUnit combatUnit)
        {
            var effects = _activeCombat.EffectProcessor.GetCurrentEffect(combatUnit).ToArray();

            for (var index = 0; index < effects.Length; index++)
            {
                var effect = effects[index];

                if (effect is PeriodicEffectBase periodicEffect)
                {
                    spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                        $"{periodicEffect.GetType()} {periodicEffect.Duration} turns",
                        panelPosition + new Vector2(0, index * 10), Color.Aqua);
                }
            }
        }

        private void DrawManaBar(SpriteBatch spriteBatch, Vector2 panelPosition, CombatUnit combatUnit)
        {
            var energyPosition = panelPosition + new Vector2(46, 0);

            var manaPointCount = Math.Min(8, combatUnit.EnergyPool);

            var manaSourceRect = new Rectangle(0, 72, 3, 10);
            for (var i = 0; i < manaPointCount; i++)
            {
                spriteBatch.Draw(_uiContentStorage.GetUnitStatePanelTexture(),
                    energyPosition + new Vector2((3 + 2) * i + 3, 17),
                    manaSourceRect,
                    Color.White);
            }
        }

        private void DrawPanelBackground(SpriteBatch spriteBatch, Vector2 panelPosition, Vector2 backgroundOffset,
            Side side)
        {
            var effect = side == Side.Right ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            spriteBatch.Draw(_uiContentStorage.GetUnitStatePanelTexture(), panelPosition + backgroundOffset,
                new Rectangle(0, 0, PANEL_WIDTH, PANEL_HEIGHT),
                Color.White,
                rotation: 0, origin: Vector2.Zero, scale: 1, effect, layerDepth: 0);
        }

        private void DrawTargets(SpriteBatch spriteBatch, Vector2 panelPosition, CombatUnit combatUnit)
        {
            if (combatUnit.Target is not null)
            {
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                    combatUnit.Target.Unit.UnitScheme.Name.ToString(), panelPosition + new Vector2(-30, 0),
                    Color.LightCyan);
            }
        }

        private void DrawUnitHitPointsBar(SpriteBatch spriteBatch, Unit unit, Vector2 panelPosition,
            Vector2 backgroundOffset, Side side)
        {
            var hpPosition = panelPosition + backgroundOffset +
                             (side == Side.Left ? new Vector2(46, 22) : new Vector2(26, 22));
            var hpPercentage = (float)unit.HitPoints / unit.MaxHitPoints;
            var hpSourceRect = new Rectangle(0, 49, (int)(hpPercentage * BAR_WIDTH), 20);
            var effect = side == Side.Right ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(_uiContentStorage.GetUnitStatePanelTexture(), hpPosition, hpSourceRect, Color.White,
                rotation: 0, origin: Vector2.Zero, scale: 1, effect, layerDepth: 0);

            var text = $"{unit.HitPoints}/{unit.MaxHitPoints}";
            if (side == Side.Left)
            {
                for (var xOffset = -1; xOffset <= 1; xOffset++)
                {
                    for (var yOffset = -1; yOffset <= 1; yOffset++)
                    {
                        spriteBatch.DrawString(_uiContentStorage.GetMainFont(), text,
                            hpPosition + new Vector2(3, 0) + new Vector2(xOffset, yOffset),
                            Color.Black);
                    }
                }

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), text, hpPosition + new Vector2(3, 0),
                    Color.LightCyan);
            }
            else
            {
                var textSize = _uiContentStorage.GetMainFont().MeasureString(text);

                for (var xOffset = -1; xOffset <= 1; xOffset++)
                {
                    for (var yOffset = -1; yOffset <= 1; yOffset++)
                    {
                        spriteBatch.DrawString(_uiContentStorage.GetMainFont(), text,
                            hpPosition + new Vector2(109, 0) - new Vector2(textSize.X, 0) +
                            new Vector2(xOffset, yOffset),
                            Color.Black);
                    }
                }

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), text,
                    hpPosition + new Vector2(109, 0) - new Vector2(textSize.X, 0), Color.LightCyan);
            }
        }

        private void DrawUnitName(SpriteBatch spriteBatch, Vector2 panelPosition, Unit unit, Side side)
        {
            var unitName = GameObjectHelper.GetLocalized(unit.UnitScheme.Name);

            var nameText = $"{unitName} (a{unit.Armor})";

            if (side == Side.Left)
            {
                var unitNamePosition = panelPosition + new Vector2(46, 0);
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), nameText, unitNamePosition, Color.White);
            }
            else
            {
                var textSize = _uiContentStorage.GetMainFont().MeasureString(nameText);

                var unitNamePosition = panelPosition + new Vector2(146 - textSize.X, 0);
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), nameText, unitNamePosition, Color.White);
            }
        }

        private void DrawUnitPortrait(SpriteBatch spriteBatch, Vector2 panelPosition, Unit unit, Side side)
        {
            if (side == Side.Left)
            {
                spriteBatch.Draw(_uiContentStorage.GetUnitStatePanelTexture(), panelPosition,
                    new Rectangle(0, 83, 42, 32),
                    Color.White);

                var portraitSourceRect = UnsortedHelpers.GetUnitPortraitRect(unit.UnitScheme.Name);
                var portraitPosition = panelPosition + new Vector2(7, 0);
                var portraitDestRect = new Rectangle(portraitPosition.ToPoint(), new Point(32, 32));
                spriteBatch.Draw(_gameObjectContentStorage.GetUnitPortrains(), portraitDestRect, portraitSourceRect,
                    Color.White);
            }
            else
            {
                spriteBatch.Draw(_uiContentStorage.GetUnitStatePanelTexture(), panelPosition + new Vector2(146, 0),
                    new Rectangle(0, 83, 42, 32),
                    Color.White);

                var portraitSourceRect = UnsortedHelpers.GetUnitPortraitRect(unit.UnitScheme.Name);
                var portraitPosition = panelPosition + new Vector2(7, 0);
                var portraitDestRect = portraitPosition;
                var effect = side == Side.Right ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                spriteBatch.Draw(_gameObjectContentStorage.GetUnitPortrains(), portraitDestRect + new Vector2(146, 0),
                    portraitSourceRect,
                    Color.White,
                    rotation: 0, origin: Vector2.Zero, scale: 1, effect, layerDepth: 0);
            }
        }

        private static bool HasMana(Unit unit)
        {
            return unit.IsPlayerControlled && unit.HasSkillsWithCost;
        }

        private enum Side
        {
            Left,
            Right
        }
    }
}