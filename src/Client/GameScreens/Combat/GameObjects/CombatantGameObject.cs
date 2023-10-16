using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.CombatMovements;
using Client.Core;
using Client.Engine;
using Client.GameScreens.Combat.GameObjects.CommonStates;
using Client.GameScreens.Combat.Ui;

using CombatDicesTeam.Combats;

using GameClient.Engine.MoveFunctions;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Combat.GameObjects;

internal sealed class CombatantGameObject
{
    private readonly IList<IActorVisualizationState> _actorStateEngineList;
    private readonly ICamera2DAdapter _camera;
    private readonly CombatantGraphicsConfigBase _combatantGraphicsConfig;
    private readonly CombatantPositionSide _combatantSide;
    private readonly GameObjectContentStorage _gameObjectContentStorage;
    private readonly ICombatantPositionProvider _unitPositionProvider;

    private CombatUnitState _visualIdleState;

    public CombatantGameObject(ICombatant combatant, CombatantGraphicsConfigBase combatantGraphicsConfig,
        FieldCoords formationCoords, ICombatantPositionProvider unitPositionProvider,
        GameObjectContentStorage gameObjectContentStorage,
        ICamera2DAdapter camera,
        CombatantPositionSide combatantSide)
    {
        _actorStateEngineList = new List<IActorVisualizationState>();

        _combatantGraphicsConfig = combatantGraphicsConfig;

        var position = unitPositionProvider.GetPosition(formationCoords, combatantSide);
        var spriteSheetId = Enum.Parse<UnitName>(combatant.ClassSid, ignoreCase: true);
        Graphics = new UnitGraphics(spriteSheetId, _combatantGraphicsConfig,
            combatantSide == CombatantPositionSide.Heroes,
            position, gameObjectContentStorage);

        Animator = new ActorAnimator(Graphics);

        Combatant = combatant;
        _unitPositionProvider = unitPositionProvider;
        _gameObjectContentStorage = gameObjectContentStorage;
        _camera = camera;
        _combatantSide = combatantSide;

        // TODO Call ShiftShape from external combat core
        // combatant.Unit.SchemeAutoTransition += Unit_SchemeAutoTransition;
        // combatant.PositionChanged += CombatUnit_PositionChanged;
    }

    public IActorAnimator Animator { get; }

    public ICombatant Combatant { get; }

    public UnitGraphics Graphics { get; }

    public Vector2 InteractionPoint => Graphics.Root.Position - _combatantGraphicsConfig.InteractionPoint;

    public bool IsActive { get; set; }
    public Vector2 LaunchPoint => Graphics.Root.Position - _combatantGraphicsConfig.LaunchPoint;

    public Vector2 MeleeHitOffset => Graphics.Root.Position +
                                     new Vector2(
                                         _combatantSide == CombatantPositionSide.Heroes
                                             ? _combatantGraphicsConfig.MeleeHitXOffset
                                             : -_combatantGraphicsConfig.MeleeHitXOffset, 0);

    public Vector2 StatsPanelOrigin => Graphics.Root.Position - _combatantGraphicsConfig.StatsPanelOrigin;

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

    public void AnimateWound()
    {
        AddStateEngine(new WoundState(Graphics));
    }

    public CorpseGameObject CreateCorpse()
    {
        var spriteSheetId = Enum.Parse<UnitName>(Combatant.ClassSid, ignoreCase: true);
        var deathSoundEffect = _gameObjectContentStorage.GetDeathSound(spriteSheetId)
            .CreateInstance();

        deathSoundEffect.Play();

        var corpse = new CorpseGameObject(Graphics, _camera, _gameObjectContentStorage);

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

    internal void AnimateShield()
    {
        // TODO Display shield effect.
    }

    internal void ChangeState(CombatUnitState visualIdleState)
    {
        _visualIdleState = visualIdleState;
    }

    internal float GetZIndex()
    {
        return Graphics.Root.Position.Y;
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

        if (activeStateEngine.IsComplete)
        {
            _actorStateEngineList.Remove(activeStateEngine);

            if (!_actorStateEngineList.Any())
            {
                AddStateEngine(new IdleActorVisualizationState(Graphics, CombatUnitState.Idle /*Combatant.State*/));
            }
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

    public event EventHandler? SkillAnimationCompleted;
}