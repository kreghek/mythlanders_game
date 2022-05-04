using System;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Assets;
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

                DrawTurnState(spriteBatch, panelPosition, combatUnit, side);

                if (HasMana(unit))
                {
                    DrawManaBar(spriteBatch, panelPosition, combatUnit);
                }

                if (_tempShowEffects)
                {
                    DrawEffects(spriteBatch, panelPosition, combatUnit, side);
                }
            }
        }

        private void DrawEffects(SpriteBatch spriteBatch, Vector2 panelPosition, ICombatUnit combatUnit, Side side)
        {
            const int EFFECT_SIZE = 16;
            const int EFFECTS_MARGIN = 2;
            const int EFFECTS_DURATION_OFFSET = 2;
            
            var effects = _activeCombat.EffectProcessor.GetCurrentEffect(combatUnit).ToArray();

            for (var index = 0; index < effects.Length; index++)
            {
                var effect = effects[index];

                var effectPosition = panelPosition + new Vector2(PANEL_WIDTH + index * (EFFECT_SIZE + EFFECTS_MARGIN), 0);

                if (side == Side.Right)
                {
                    effectPosition += new Vector2(-148, 0);
                }

                var effectRect = new Rectangle(effectPosition.ToPoint(), new Point(EFFECT_SIZE, EFFECT_SIZE));
                var effectSourceRect = GetEffectSourceRect(effect);

                spriteBatch.Draw(_uiContentStorage.GetEffectIconsTexture(), effectRect, effectSourceRect, Color.White);

                if (effect is PeriodicEffectBase periodicEffect)
                {
                    for (var i = -1; i <= 1; i++)
                    {
                        for (var j = -1; j <= 1; j++)
                        {
                            spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                                periodicEffect.Duration.ToString(),
                                effectPosition + new Vector2(EFFECT_SIZE - EFFECTS_DURATION_OFFSET,
                                    EFFECT_SIZE - EFFECTS_DURATION_OFFSET) + new Vector2(i, j), Color.Black);
                        }
                    }

                    spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                        periodicEffect.Duration.ToString(),
                        effectPosition + new Vector2(EFFECT_SIZE - EFFECTS_DURATION_OFFSET,
                            EFFECT_SIZE - EFFECTS_DURATION_OFFSET), Color.White);
                }
            }
        }

        private static Rectangle GetEffectSourceRect(EffectBase effect)
        {
            const int EFFECT_SIZE = 16;
            const int COL_COUNT = 2;

            var index = GetEffectSourceBaseOneIndex(effect) - 1;
            
            Debug.Assert(index >= 0);

            var col = index % COL_COUNT;
            var row = index / COL_COUNT;

            return new Rectangle(col * EFFECT_SIZE, row * EFFECT_SIZE, EFFECT_SIZE, EFFECT_SIZE);
        }

        private static int GetEffectSourceBaseOneIndex(EffectBase effect)
        {
            if (effect.Visualization is null)
            {
                return int.MinValue;
            }

            var visualization = (EffectVisualization)effect.Visualization;
            return visualization.BasedOneIndex;
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
            var targetCombatUnit = combatUnit.Target;
            if (targetCombatUnit is not null)
            {
                var portraitSourceRect = UnsortedHelpers.GetUnitPortraitRect(targetCombatUnit.Unit.UnitScheme.Name);

                var targetPortraintPosition = panelPosition + new Vector2(-30, 0);
                spriteBatch.Draw(_gameObjectContentStorage.GetUnitPortrains(),
                    new Rectangle(targetPortraintPosition.ToPoint(), new Point(16, 16)),
                    portraitSourceRect,
                    Color.White);

                if (combatUnit.TargetSkill is not null)
                {
                    var targetSkill = combatUnit.TargetSkill;

                    var impactText = string.Empty;

                    foreach (var rule in targetSkill.Skill.Rules)
                    {
                        var effect = rule.EffectCreator.Create(combatUnit);

                        if (effect is DamageEffect damageEffect)
                        {
                            var damageRange = damageEffect.CalculateDamage();
                            if (damageRange.Min != damageRange.Max)
                            {
                                impactText = $"{damageRange.Min}-{damageRange.Max}";
                            }
                            else
                            {
                                impactText = $"{damageRange.Min}";
                            }

                            if (targetCombatUnit.Unit.HitPoints <= damageRange.Min)
                            {
                                impactText += "  X";
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(impactText))
                    {
                        for (var i = -1; i <= 1; i++)
                        {
                            for (var j = -1; j <= 1; j++)
                            {
                                spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                                    impactText, panelPosition + new Vector2(-(30 - 24), 6) + new Vector2(i, j),
                                    Color.LightGray);
                            }
                        }

                        spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                            impactText, panelPosition + new Vector2(-(30 - 24), 6),
                            Color.Maroon);
                    }
                }
            }
        }

        private void DrawTurnState(SpriteBatch spriteBatch, Vector2 panelPosition, CombatUnit combatUnit, Side side)
        {
            if (side == Side.Left)
            {
                var markerPosition = panelPosition + new Vector2(17, 33);

                var portraitDestRect = new Rectangle(markerPosition.ToPoint(), new Point(10, 10));

                var color = Color.LightCyan;
                if (combatUnit.IsWaiting)
                {
                    //spriteBatch.Draw(_uiContentStorage.GetUnitStatePanelTexture(), portraitDestRect, new Rectangle(0, 85, 2, 2),
                    //Color.White);
                }
                else
                {
                    color = Color.LightGray;
                }

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), $"[{combatUnit.Unit.UnitScheme.Resolve}]",
                    markerPosition, color);
            }
            else
            {
                var markerPosition = panelPosition + new Vector2(17 + 146, 33);

                var portraitDestRect = new Rectangle(markerPosition.ToPoint(), new Point(10, 10));

                var color = Color.LightCyan;
                if (combatUnit.IsWaiting)
                {
                    //spriteBatch.Draw(_uiContentStorage.GetUnitStatePanelTexture(), portraitDestRect, new Rectangle(0, 85, 2, 2),
                    //Color.White);
                }
                else
                {
                    color = Color.LightGray;
                }

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), $"[{combatUnit.Unit.UnitScheme.Resolve}]",
                    markerPosition, color);

                //spriteBatch.Draw(_uiContentStorage.GetUnitStatePanelTexture(), panelPosition + new Vector2(146, 0),
                //    new Rectangle(0, 83, 42, 32),
                //    Color.White);

                //var portraitSourceRect = UnsortedHelpers.GetUnitPortraitRect(unit.UnitScheme.Name);
                //var portraitPosition = panelPosition + new Vector2(7, 0);
                //var portraitDestRect = portraitPosition;
                //var effect = side == Side.Right ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                //spriteBatch.Draw(_gameObjectContentStorage.GetUnitPortrains(), portraitDestRect + new Vector2(146, 0),
                //    portraitSourceRect,
                //    Color.White,
                //    rotation: 0, origin: Vector2.Zero, scale: 1, effect, layerDepth: 0);
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

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                    $"{unit.ShieldPoints.Current}/{unit.ShieldPoints.ActualBase}",
                    hpPosition + new Vector2(3, 0) + new Vector2(0, 10),
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

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                    $"{unit.ShieldPoints.Current}/{unit.ShieldPoints.ActualBase}",
                    hpPosition + new Vector2(109, 0) - new Vector2(textSize.X, 0) + new Vector2(0, 10),
                    Color.LightCyan);
            }
        }

        private void DrawUnitName(SpriteBatch spriteBatch, Vector2 panelPosition, Unit unit, Side side)
        {
            var unitName = GameObjectHelper.GetLocalized(unit.UnitScheme.Name);

            if (side == Side.Left)
            {
                var unitNamePosition = panelPosition + new Vector2(46, 0);
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), unitName, unitNamePosition, Color.White);
            }
            else
            {
                var textSize = _uiContentStorage.GetMainFont().MeasureString(unitName);

                var unitNamePosition = panelPosition + new Vector2(146 - textSize.X, 0);
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), unitName, unitNamePosition, Color.White);
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