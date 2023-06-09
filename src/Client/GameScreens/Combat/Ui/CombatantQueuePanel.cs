﻿using System;
using System.Collections.Generic;
using System.Linq;

using Client.Engine;

using Core.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Client.GameScreens.Combat.Ui;

internal sealed class CombatantQueuePanel : ControlBase
{
    private const int RESOLVE_WIDTH = 12;
    private const int PORTRAIN_WIDTH = 32;
    private readonly CombatCore _activeCombat;
    private readonly ICombatantThumbnailProvider _combatantThumbnailProvider;

    private readonly IList<(Rectangle, ICombatantEffect)> _effectInfoList =
        new List<(Rectangle, ICombatantEffect)>();

    private readonly IList<(Rectangle, CombatMovementInstance)> _monsterCombatMoveInfoList =
        new List<(Rectangle, CombatMovementInstance)>();

    private readonly IUiContentStorage _uiContentStorage;
    private HintBase? _combatMoveHint;

    private HintBase? _effectHint;
    private ICombatantEffect? _lastEffectWithHint;
    private CombatMovementInstance? _lastMoveWithHint;

    public CombatantQueuePanel(CombatCore combat,
        IUiContentStorage uiContentStorage,
        ICombatantThumbnailProvider combatantThumbnailProvider)
    {
        _activeCombat = combat;
        _uiContentStorage = uiContentStorage;
        _combatantThumbnailProvider = combatantThumbnailProvider;
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
            DrawCombatantThumbnail(spriteBatch, portraitDestRect, combatant, side);

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

    private void DrawCombatantThumbnail(SpriteBatch spriteBatch, Rectangle portraitDestRect, Combatant combatant,
        Side side)
    {
        var effect = side == Side.Right ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        spriteBatch.Draw(_combatantThumbnailProvider.Get(combatant.ClassSid),
            portraitDestRect,
            sourceRectangle: null,
            side == Side.Left ? Color.White : Color.Lerp(Color.White, Color.Red, 0.15f),
            effects: effect,
            rotation: 0, origin: Vector2.Zero, layerDepth: 0);
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