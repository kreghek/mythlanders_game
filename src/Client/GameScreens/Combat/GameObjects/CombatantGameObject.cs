﻿using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.CombatMovements;
using Client.Core;
using Client.Engine;
using Client.GameScreens.Combat.GameObjects.CommonStates;

using CombatDicesTeam.Combats;

using GameAssets.Combats.CombatantStatuses;

using GameClient.Engine;
using GameClient.Engine.CombatVisualEffects;
using GameClient.Engine.MoveFunctions;

using Microsoft.Xna.Framework;

namespace Client.GameScreens.Combat.GameObjects;

internal sealed class CombatantGameObject
{
    private readonly IList<IActorVisualizationState> _actorStateEngineList;
    private readonly CombatantGraphicsConfigBase _combatantGraphicsConfig;
    private readonly CombatantPositionSide _combatantSide;
    private readonly GameObjectContentStorage _gameObjectContentStorage;

    public CombatantGameObject(ICombatant combatant, CombatantGraphicsConfigBase combatantGraphicsConfig,
        FieldCoords formationCoords, ICombatantPositionProvider unitPositionProvider,
        GameObjectContentStorage gameObjectContentStorage,
        CombatantPositionSide combatantSide)
    {
        _actorStateEngineList = new List<IActorVisualizationState>();

        _combatantGraphicsConfig = combatantGraphicsConfig;

        var startPosition = unitPositionProvider.GetPosition(formationCoords, combatantSide);
        var spriteSheetId = Enum.Parse<UnitName>(combatant.ClassSid, ignoreCase: true);
        Graphics = new UnitGraphics(spriteSheetId, _combatantGraphicsConfig,
            combatantSide == CombatantPositionSide.Heroes,
            startPosition, gameObjectContentStorage);

        Animator = new ActorAnimator(Graphics);

        Combatant = combatant;
        _gameObjectContentStorage = gameObjectContentStorage;
        _combatantSide = combatantSide;

        // TODO Call ShiftShape from external combat core
        // combatant.Unit.SchemeAutoTransition += Unit_SchemeAutoTransition;
        // combatant.PositionChanged += CombatUnit_PositionChanged;
    }

    public IActorAnimator Animator { get; }

    public ICombatant Combatant { get; }

    public int CombatantSize { get; } = 32;

    public UnitGraphics Graphics { get; }

    public Vector2 InteractionPoint => Graphics.Root.RootNode.Position - _combatantGraphicsConfig.InteractionPoint;
    public Vector2 LaunchPoint => Graphics.Root.RootNode.Position - _combatantGraphicsConfig.LaunchPoint;

    public Vector2 MeleeHitOffset => Graphics.Root.RootNode.Position +
                                     new Vector2(
                                         _combatantSide == CombatantPositionSide.Heroes
                                             ? _combatantGraphicsConfig.MeleeHitXOffset
                                             : -_combatantGraphicsConfig.MeleeHitXOffset, 0);

    public Vector2 StatsPanelOrigin => Graphics.Root.RootNode.Position - _combatantGraphicsConfig.StatsPanelOrigin;

    private CombatantVisualIdleState VisualIdleState =>
        Combatant.Statuses.Any(x => x is DefensiveStanceCombatantStatusWrapper)
            ? CombatantVisualIdleState.DefenseStance
            : CombatantVisualIdleState.Idle;

    public void AddStateEngine(IActorVisualizationState actorStateEngine)
    {
        foreach (var state in _actorStateEngineList.ToArray())
        {
            if (state.CanBeReplaced)
            {
                _actorStateEngineList.Remove(state);
            }
        }

        _actorStateEngineList.Add(actorStateEngine);
    }

    public void AnimateDamageImpact()
    {
        if (VisualIdleState == CombatantVisualIdleState.DefenseStance)
        {
            AddStateEngine(new DefenseState(Graphics));
        }
        else
        {
            AddStateEngine(new WoundState(Graphics));
        }
    }

    public CorpseGameObject CreateCorpse(GameObjectContentStorage gameObjectContentStorage,
        ICombatVisualEffectManager combatVisualEffectManager, AudioSettings audioSettings)
    {
        var deathAnimation = _combatantGraphicsConfig.GetDeathAnimation(gameObjectContentStorage,
            combatVisualEffectManager, audioSettings, Animator.GraphicRoot.Position);

        var corpse = new CorpseGameObject(Graphics, deathAnimation);
        if (_combatantGraphicsConfig.RemoveShadowOnDeath)
        {
            Graphics.RemoveShadowOfCorpse();
        }

        return corpse;
    }

    public void MoveToFieldCoords(Vector2 targetPosition)
    {
        var animationSid = CalcMoveAnimation(Animator.GraphicRoot.Position, targetPosition);
        AddStateEngine(new MoveToPositionActorState(Animator,
            new SlowDownMoveFunction(Animator.GraphicRoot.Position, targetPosition),
            Graphics.GetAnimationInfo(animationSid), new Duration(0.5)));
    }

    public void Update(GameTime gameTime)
    {
        HandleEngineStates(gameTime);

        Graphics.Update(gameTime);
    }

    internal float GetZIndex()
    {
        return Graphics.Root.RootNode.Position.Y;
    }

    private static PredefinedAnimationSid CalcMoveAnimation(Vector2 currentPosition, Vector2 targetPosition)
    {
        var combatantCoords = currentPosition;
        var targetCoords = targetPosition;

        var lineDiff = targetCoords.Y - combatantCoords.Y;
        var columnDiff = targetCoords.X - combatantCoords.X;

        if (columnDiff > 0 && lineDiff == 0)
        {
            return PredefinedAnimationSid.MoveBackward;
        }

        return PredefinedAnimationSid.MoveForward;
    }

    private void HandleEngineStates(GameTime gameTime)
    {
        if (!_actorStateEngineList.Any())
        {
            return;
        }

        var activeStateEngine = _actorStateEngineList.First();
        activeStateEngine.Update(gameTime);

        if (!activeStateEngine.IsComplete)
        {
            return;
        }

        _actorStateEngineList.Remove(activeStateEngine);

        if (!_actorStateEngineList.Any())
        {
            AddStateEngine(new IdleActorVisualizationState(Graphics, VisualIdleState));
        }
    }

    // public void ShiftShape(UnitName spriteSheetId, UnitGraphicsConfigBase graphicsConfig)
    // {
    //     var shapeShiftBlocker = _animationManager.CreateAndUseBlocker();
    //     var deathSound = _gameObjectContentStorage.GetDeathSound(e.SourceScheme.Name);
    //     AddStateEngine(new ShapeShiftState(Graphics, deathSound.CreateInstance(), shapeShiftBlocker));
    //
    //     shapeShiftBlocker.Released += (_, _) =>
    //     {
    //         Graphics.SwitchSourceUnit(Combatant.Unit);
    //         AddStateEngine(new UnitIdleState(Graphics, Combatant.State));
    //     };
    // }
}