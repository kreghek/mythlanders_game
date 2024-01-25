using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.CombatMovements;
using Client.Engine;

using CombatDicesTeam.Combats;

using GameAssets.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Client.GameScreens.Combat.Ui;

internal sealed class CombatantQueuePanel : ControlBase
{
    private const int RESOLVE_WIDTH = 12;
    private const int PORTRAIT_WIDTH = 32;
    private readonly CombatEngineBase _activeCombat;
    private readonly ICombatantThumbnailProvider _combatantThumbnailProvider;
    private readonly ICombatMovementVisualizationProvider _combatMovementVisualizationProvider;
    private readonly IList<(Rectangle, ICombatantStatus)> _effectInfoList =
        new List<(Rectangle, ICombatantStatus)>();

    private readonly IList<(Rectangle, CombatMovementInstance, ICombatant)> _monsterCombatMoveInfoList =
        new List<(Rectangle, CombatMovementInstance, ICombatant)>();

    private readonly IUiContentStorage _uiContentStorage;
    private HintBase? _combatMoveHint;

    private HintBase? _effectHint;
    private ICombatantStatus? _lastEffectWithHint;
    private CombatMovementInstance? _lastMoveWithHint;

    public CombatantQueuePanel(CombatEngineBase combat,
        IUiContentStorage uiContentStorage,
        ICombatantThumbnailProvider combatantThumbnailProvider,
        ICombatMovementVisualizationProvider combatMovementVisualizationProvider)
    {
        _activeCombat = combat;
        _uiContentStorage = uiContentStorage;
        _combatantThumbnailProvider = combatantThumbnailProvider;
        _combatMovementVisualizationProvider = combatMovementVisualizationProvider;
    }

    public Point CalcContentSize()
    {
        return new((RESOLVE_WIDTH + PORTRAIT_WIDTH) * (_activeCombat.CurrentCombatants.Count + 1), 48);
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

        for (var index = 0; index < _activeCombat.CurrentRoundQueue.Count; index++)
        {
            var combatant = _activeCombat.CurrentRoundQueue[index];

            var combatantQueuePosition =
                new Vector2(contentRect.Location.X + (index * (PORTRAIT_WIDTH + RESOLVE_WIDTH + CONTENT_MARGIN)),
                    contentRect.Location.Y + CONTENT_MARGIN);

            var side = combatant.IsPlayerControlled ? Side.Left : Side.Right;
            var portraitDestRect = new Rectangle(combatantQueuePosition.ToPoint() + new Point(RESOLVE_WIDTH, 0),
                new Point(PORTRAIT_WIDTH, PORTRAIT_WIDTH));
            DrawCombatantThumbnail(spriteBatch, portraitDestRect, combatant, side);

            if (!combatant.IsPlayerControlled && !combatant.IsDead)
            {
                var plannedMove = combatant.CombatMovementContainers
                    .Single(x => x.Type == CombatMovementContainerTypes.Hand).GetItems().First(x => x is not null);

                if (plannedMove is not null)
                {
                    _monsterCombatMoveInfoList.Add(
                        new ValueTuple<Rectangle, CombatMovementInstance, ICombatant>(portraitDestRect, plannedMove, combatant));
                }
            }

            spriteBatch.DrawString(_uiContentStorage.GetTitlesFont(),
                combatant.Stats.Single(x => x.Type == CombatantStatTypes.Resolve).Value.Current.ToString(),
                combatantQueuePosition, Color.White);
        }

        _effectHint?.Draw(spriteBatch);
        _combatMoveHint?.Draw(spriteBatch);
    }

    private static HintBase CreateEffectHint((Rectangle, ICombatantStatus) effectInfo)
    {
        return new EffectHint(effectInfo.Item2)
        {
            Rect = new Rectangle(effectInfo.Item1.Location, new Point(200, 40))
        };
    }

    private HintBase CreateEffectHint((Rectangle, CombatMovementInstance, ICombatant) moveInfo)
    {
        var currentActorResolveValue =
            moveInfo.Item3.Stats.Single(x => ReferenceEquals(x.Type, CombatantStatTypes.Resolve)).Value;
        
        var hint = new CombatMovementHint(moveInfo.Item2, currentActorResolveValue, _combatMovementVisualizationProvider)
        {
            Rect = new Rectangle(moveInfo.Item1.Location, new Point(200, 40))
        };

        hint.Rect = new Rectangle(new Point(moveInfo.Item1.Center.X, moveInfo.Item1.Bottom),
            (hint.ContentSize + new Vector2(CONTENT_MARGIN * 2, CONTENT_MARGIN * 2)).ToPoint());

        return hint;
    }

    private void DrawCombatantThumbnail(SpriteBatch spriteBatch, Rectangle portraitDestRect, ICombatant combatant,
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