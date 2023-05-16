using System;
using System.Collections.Generic;
using System.Linq;

using Client.Engine;

using Core.Combats;
using Core.Combats.CombatantEffectLifetimes;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat.Ui;

namespace Client.GameScreens.Combat.Ui;

internal sealed class CombatantQueuePanel : ControlBase
{
    private const int PANEL_WIDTH = 189;
    private const int PANEL_HEIGHT = 48;
    private const int BAR_WIDTH = 118;
    private const int PANEL_BACKGROUND_VERTICAL_OFFSET = 12;
    private const int MARGIN = 10;

    private const int RESOLVE_WIDTH = 12;
    private const int PORTRAIN_WIDTH = 32;
    private readonly CombatCore _activeCombat;

    private readonly IList<(Rectangle, ICombatantEffect)> _effectInfoList =
        new List<(Rectangle, ICombatantEffect)>();

    private readonly GameObjectContentStorage _gameObjectContentStorage;

    private readonly IList<(Rectangle, CombatMovementInstance)> _monsterCombatMoveInfoList =
        new List<(Rectangle, CombatMovementInstance)>();

    private readonly IUiContentStorage _uiContentStorage;
    private HintBase? _combatMoveHint;

    private HintBase? _effectHint;
    private ICombatantEffect? _lastEffectWithHint;
    private CombatMovementInstance? _lastMoveWithHint;

    public CombatantQueuePanel(CombatCore combat,
        IUiContentStorage uiContentStorage,
        GameObjectContentStorage gameObjectContentStorage)
    {
        _activeCombat = combat;
        _uiContentStorage = uiContentStorage;
        _gameObjectContentStorage = gameObjectContentStorage;
    }

    public Point CalcContentSize()
    {
        return new((RESOLVE_WIDTH + PORTRAIN_WIDTH) * (_activeCombat.Combatants.Count + 1), 48);
    }

    public void Update(IResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        var mouse = Mouse.GetState();
        var mousePosition =
            resolutionIndependentRenderer.ConvertScreenToWorldCoordinates(mouse.Position.ToVector2()).ToPoint();

        HandleMonsterCombatMoveHint(mousePosition);

        HandleEffectHint(mousePosition);
    }

    protected override Point CalcTextureOffset()
    {
        return ControlTextures.CombatMove;
    }

    protected override Color CalculateColor()
    {
        return Color.White;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        _monsterCombatMoveInfoList.Clear();
        _effectInfoList.Clear();

        spriteBatch.Draw(UiThemeManager.UiContentStorage.GetModalShadowTexture(), contentRect,
            new Rectangle(ControlTextures.Shadow, new Point(32, 32)), Color.Lerp(Color.White, Color.Transparent, 0.5f));

        for (var index = 0; index < _activeCombat.RoundQueue.Count; index++)
        {
            var combatant = _activeCombat.RoundQueue[index];

            var combatantQueuePosition =
                new Vector2(contentRect.Location.X + (index * (PORTRAIN_WIDTH + RESOLVE_WIDTH + CONTENT_MARGIN)),
                    contentRect.Location.Y + CONTENT_MARGIN);

            var side = combatant.IsPlayerControlled ? Side.Left : Side.Right;
            var portraitDestRect = new Rectangle(combatantQueuePosition.ToPoint() + new Point(RESOLVE_WIDTH, 0),
                new Point(PORTRAIN_WIDTH, PORTRAIN_WIDTH));
            DrawPortrait(spriteBatch, portraitDestRect, combatant, side);

            if (!combatant.IsPlayerControlled && !combatant.IsDead)
            {
                var plannedMove = combatant.Hand.First(x => x is not null);

                if (plannedMove is not null)
                {
                    _monsterCombatMoveInfoList.Add(
                        new ValueTuple<Rectangle, CombatMovementInstance>(portraitDestRect, plannedMove));
                }
            }

            spriteBatch.DrawString(_uiContentStorage.GetTitlesFont(),
                combatant.Stats.Single(x => x.Type == UnitStatType.Resolve).Value.Current.ToString(),
                combatantQueuePosition, Color.White);
        }

        _effectHint?.Draw(spriteBatch);
        _combatMoveHint?.Draw(spriteBatch);
    }

    private static HintBase CreateEffectHint((Rectangle, ICombatantEffect) effectInfo)
    {
        return new EffectHint(effectInfo.Item2)
        {
            Rect = new Rectangle(effectInfo.Item1.Location, new Point(200, 40))
        };
    }

    private static HintBase CreateEffectHint((Rectangle, CombatMovementInstance) moveInfo)
    {
        var hint = new CombatMovementHint(moveInfo.Item2)
        {
            Rect = new Rectangle(moveInfo.Item1.Location, new Point(200, 40))
        };

        hint.Rect = new Rectangle(new Point(moveInfo.Item1.Center.X, moveInfo.Item1.Bottom),
            (hint.ContentSize + new Vector2(CONTENT_MARGIN * 2, CONTENT_MARGIN * 2)).ToPoint());

        return hint;
    }

    private void DrawCombatantHitPointsBar(SpriteBatch spriteBatch, Combatant combatant, Vector2 panelPosition,
        Vector2 backgroundOffset, Side side)
    {
        var hpPosition = panelPosition + backgroundOffset +
                         (side == Side.Left ? new Vector2(46, 22) : new Vector2(26, 22));
        var hpValue = combatant.Stats.Single(x => x.Type == UnitStatType.HitPoints).Value;
        var hpPercentage = hpValue.GetShare();
        var hpSourceRect = new Rectangle(0, 49, (int)(hpPercentage * BAR_WIDTH), 20);
        var effect = side == Side.Right ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        spriteBatch.Draw(_uiContentStorage.GetUnitStatePanelTexture(), hpPosition, hpSourceRect, Color.White,
            rotation: 0, origin: Vector2.Zero, scale: 1, effect, layerDepth: 0);

        var text = $"{hpValue.Current}/{hpValue.ActualMax}";
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

            var spValue = combatant.Stats.Single(x => x.Type == UnitStatType.ShieldPoints).Value;
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                $"{spValue.Current}/{spValue.ActualMax}",
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

            var spValue = combatant.Stats.Single(x => x.Type == UnitStatType.ShieldPoints).Value;

            spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                $"{spValue.Current}/{spValue.ActualMax}",
                hpPosition + new Vector2(109, 0) - new Vector2(textSize.X, 0) + new Vector2(0, 10),
                Color.LightCyan);
        }
    }

    private void DrawEffects(SpriteBatch spriteBatch, Vector2 panelPosition, Combatant combatant, Side side)
    {
        const int EFFECT_SIZE = 16;
        const int EFFECTS_MARGIN = 2;
        const int EFFECTS_DURATION_OFFSET = 2;

        var effects = combatant.Effects.ToArray();

        for (var index = 0; index < effects.Length; index++)
        {
            var effect = effects[index];

            var effectPosition =
                panelPosition + new Vector2(PANEL_WIDTH + index * (EFFECT_SIZE + EFFECTS_MARGIN), 0);

            if (side == Side.Right)
            {
                effectPosition += new Vector2(-148, 0);
            }

            var effectRect = new Rectangle(effectPosition.ToPoint(), new Point(EFFECT_SIZE, EFFECT_SIZE));
            var effectSourceRect = GetEffectSourceRect(effect);

            spriteBatch.Draw(_uiContentStorage.GetEffectIconsTexture(), effectRect, effectSourceRect, Color.White);

            if (effect.Lifetime is MultipleCombatantTurnEffectLifetime periodicEffect)
            {
                DrawLifetime(spriteBatch, EFFECT_SIZE, EFFECTS_DURATION_OFFSET, effectPosition, periodicEffect);
            }

            _effectInfoList.Add(new ValueTuple<Rectangle, ICombatantEffect>(effectRect, effect));
        }
    }

    private void DrawLifetime(SpriteBatch spriteBatch, int effectSize, int effectsDurationOffset,
        Vector2 effectPosition, MultipleCombatantTurnEffectLifetime periodicEffect)
    {
        for (var i = -1; i <= 1; i++)
        {
            for (var j = -1; j <= 1; j++)
            {
                if (i != j)
                {
                    DrawLifetimeInner(spriteBatch, effectSize, effectsDurationOffset, effectPosition,
                        periodicEffect, new Vector2(i, j), Color.DarkGray);
                }
            }
        }

        DrawLifetimeInner(spriteBatch, effectSize, effectsDurationOffset, effectPosition, periodicEffect,
            Vector2.Zero, Color.White);
    }

    private void DrawLifetimeInner(SpriteBatch spriteBatch, int effectSize, int effectsDurationOffset,
        Vector2 effectPosition, MultipleCombatantTurnEffectLifetime periodicEffect, Vector2 offset, Color color)
    {
        spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
            periodicEffect.Counter.ToString(),
            effectPosition + new Vector2(effectSize - effectsDurationOffset,
                effectSize - effectsDurationOffset) + offset, color);
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

    private void DrawPortrait(SpriteBatch spriteBatch, Rectangle portraitDestRect, Combatant combatant, Side side)
    {
        var unitName = GetUnitName(combatant.ClassSid);
        var portraitSourceRect = UnsortedHelpers.GetUnitPortraitRect(unitName);
        var effect = side == Side.Right ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        spriteBatch.Draw(_gameObjectContentStorage.GetUnitPortrains(),
            portraitDestRect,
            portraitSourceRect,
            Color.White,
            effects: effect,
            rotation: 0, origin: Vector2.Zero, layerDepth: 0);
    }

    //private void DrawTargets(SpriteBatch spriteBatch, Vector2 panelPosition, CombatUnit combatUnit)
    //{
    //    if (combatUnit.TargetSlot is null)
    //    {
    //        return;
    //    }

    //    var unitList = _activeCombat.Units.ToArray();
    //    var targetCombatUnit = unitList.Single(x =>
    //        x.SlotIndex == combatUnit.TargetSlot.SlotIndex &&
    //        x.Unit.IsPlayerControlled == combatUnit.TargetSlot.IsPlayerSide);
    //    if (targetCombatUnit is not null)
    //    {
    //        var portraitSourceRect = UnsortedHelpers.GetUnitPortraitRect(targetCombatUnit.Unit.UnitScheme.Name);

    //        var targetPortraintPosition = panelPosition + new Vector2(-30, 0);
    //        spriteBatch.Draw(_gameObjectContentStorage.GetUnitPortrains(),
    //            new Rectangle(targetPortraintPosition.ToPoint(), new Point(16, 16)),
    //            portraitSourceRect,
    //            Color.White);

    //        if (combatUnit.TargetSkill is not null)
    //        {
    //            var targetSkill = combatUnit.TargetSkill;

    //            var impactText = string.Empty;

    //            foreach (var rule in targetSkill.Skill.Rules)
    //            {
    //                var effect = rule.EffectCreator.Create(combatUnit);

    //                if (effect is DamageEffect damageEffect)
    //                {
    //                    var damageRange = damageEffect.CalculateDamage();
    //                    if (damageRange.Min != damageRange.Max)
    //                    {
    //                        impactText = $"{damageRange.Min}-{damageRange.Max}";
    //                    }
    //                    else
    //                    {
    //                        impactText = $"{damageRange.Min}";
    //                    }

    //                    if (targetCombatUnit.HitPoints.Current <= damageRange.Min)
    //                    {
    //                        impactText += "  X";
    //                    }
    //                }
    //            }

    //            if (!string.IsNullOrWhiteSpace(impactText))
    //            {
    //                for (var i = -1; i <= 1; i++)
    //                {
    //                    for (var j = -1; j <= 1; j++)
    //                    {
    //                        spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
    //                            impactText, panelPosition + new Vector2(-(30 - 24), 6) + new Vector2(i, j),
    //                            Color.LightGray);
    //                    }
    //                }

    //                spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
    //                    impactText, panelPosition + new Vector2(-(30 - 24), 6),
    //                    Color.Maroon);
    //            }
    //        }
    //    }
    //}

    private void DrawTurnState(SpriteBatch spriteBatch, Vector2 panelPosition, Combatant combatant, Side side)
    {
        Vector2 markerPosition;
        if (side == Side.Left)
        {
            markerPosition = panelPosition + new Vector2(17, 33);
        }
        else
        {
            markerPosition = panelPosition + new Vector2(17 + 146, 33);
        }

        var color = Color.LightCyan;
        //if (!combatant.IsWaiting)
        //{
        //    color = Color.LightGray;
        //}

        var resolveValue = combatant.Stats.Single(x => x.Type == UnitStatType.Resolve).Value.ActualMax;
        spriteBatch.DrawString(_uiContentStorage.GetMainFont(), $"[{resolveValue}]",
            markerPosition, color);
    }

    private void DrawUnitName(SpriteBatch spriteBatch, Vector2 panelPosition, Combatant combatant, Side side)
    {
        //var unitName = GameObjectHelper.GetLocalized(combatant.UnitScheme.Name);
        var unitName = combatant.Sid;

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

    private static int GetEffectSourceBaseOneIndex(ICombatantEffect effect)
    {
        return 0;
    }

    private static Rectangle GetEffectSourceRect(ICombatantEffect effect)
    {
        const int EFFECT_SIZE = 16;
        const int COL_COUNT = 3;

        var index = GetEffectSourceBaseOneIndex(effect) - 1;

        //Debug.Assert(index >= 0);

        var col = index % COL_COUNT;
        var row = index / COL_COUNT;

        return new Rectangle(col * EFFECT_SIZE, row * EFFECT_SIZE, EFFECT_SIZE, EFFECT_SIZE);
    }

    private static UnitName GetUnitName(string classSid)
    {
        return Enum.Parse<UnitName>(classSid, ignoreCase: true);
    }

    private void HandleEffectHint(Point mousePosition)
    {
        var effectListSnapshotList = _effectInfoList.ToArray();
        var effectHintFound = false;
        foreach (var effectInfo in effectListSnapshotList)
        {
            if (effectInfo.Item1.Contains(mousePosition))
            {
                effectHintFound = true;
                if (_lastEffectWithHint != effectInfo.Item2)
                {
                    _lastEffectWithHint = effectInfo.Item2;
                    _effectHint = CreateEffectHint(effectInfo);
                }
            }
        }

        if (!effectHintFound)
        {
            _lastEffectWithHint = null;
            _effectHint = null;
        }
    }

    private void HandleMonsterCombatMoveHint(Point mousePosition)
    {
        var effectListSnapshotList = _monsterCombatMoveInfoList.ToArray();
        var effectHintFound = false;
        foreach (var effectInfo in effectListSnapshotList)
        {
            if (effectInfo.Item1.Contains(mousePosition))
            {
                effectHintFound = true;
                if (_lastMoveWithHint != effectInfo.Item2 || _combatMoveHint is null)
                {
                    _lastMoveWithHint = effectInfo.Item2;
                    _combatMoveHint = CreateEffectHint(effectInfo);
                }
            }
        }

        if (!effectHintFound)
        {
            _lastEffectWithHint = null;
            _combatMoveHint = null;
        }
    }

    private enum Side
    {
        Left,
        Right
    }
}