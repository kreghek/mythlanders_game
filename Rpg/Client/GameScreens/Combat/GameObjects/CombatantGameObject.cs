using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.CombatMovements;
using Client.Engine;

using Core.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Combat.GameObjects;
using Rpg.Client.GameScreens.Combat.GameObjects.CommonStates;
using Rpg.Client.GameScreens.Combat.Ui;

namespace Client.GameScreens.Combat.GameObjects;

internal sealed class CombatantGameObject : EwarRenderableBase
{
    private readonly IList<IActorVisualizationState> _actorStateEngineList;
    private readonly ICamera2DAdapter _camera;
    private readonly UnitGraphicsConfigBase _combatantGraphicsConfig;
    private readonly CombatantPositionSide _combatantSide;
    private readonly GameObjectContentStorage _gameObjectContentStorage;
    private readonly ScreenShaker _screenShaker;
    private readonly ICombatantPositionProvider _unitPositionProvider;

    private CombatUnitState _visualIdleState;

    public CombatantGameObject(Combatant combatant, UnitGraphicsConfigBase combatantGraphicsConfig,
        FieldCoords formationCoords, ICombatantPositionProvider unitPositionProvider,
        GameObjectContentStorage gameObjectContentStorage,
        ICamera2DAdapter camera, ScreenShaker screenShaker,
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
        Position = position;
        _gameObjectContentStorage = gameObjectContentStorage;
        _camera = camera;
        _screenShaker = screenShaker;
        _combatantSide = combatantSide;

        // TODO Call ShiftShape from external combat core
        // combatant.Unit.SchemeAutoTransition += Unit_SchemeAutoTransition;
        // combatant.PositionChanged += CombatUnit_PositionChanged;
    }

    public IActorAnimator Animator { get; }

    public Combatant Combatant { get; }

    public UnitGraphics Graphics { get; }

    public Vector2 InteractionPoint => Position - _combatantGraphicsConfig.InteractionPoint;

    public bool IsActive { get; set; }
    public Vector2 LaunchPoint => Position - _combatantGraphicsConfig.LaunchPoint;

    public Vector2 StatsPanelOrigin => Position - _combatantGraphicsConfig.StatsPanelOrigin;

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

        var corpse = new CorpseGameObject(Graphics, _camera, _screenShaker, _gameObjectContentStorage);

        MoveIndicatorsToCorpse(corpse);

        return corpse;
    }

    public int? GetCurrentIndicatorIndex()
    {
        var currentIndicatorCount = Children.OfType<TextIndicatorBase>().Count();

        if (currentIndicatorCount == 0)
        {
            return null;
        }

        return currentIndicatorCount - 1;
    }

    public void MoveToFieldCoords(Vector2 targetPosition)
    {
        AddStateEngine(new MoveToPositionActorState(Animator,
            new SlowDownMoveFunction(Animator.GraphicRoot.Position, targetPosition),
            Graphics.GetAnimationInfo(PredefinedAnimationSid.MoveBackward), new Duration(0.5)));

        Graphics.ChangePosition(targetPosition);
        Position = targetPosition;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        HandleEngineStates(gameTime);

        Graphics.Update(gameTime);
    }

    protected override void DoDraw(SpriteBatch spriteBatch, float zindex)
    {
        base.DoDraw(spriteBatch, zindex);

        Graphics.ShowActiveMarker = IsActive;

        if (Graphics.IsDamaged)
        {
            var allWhite = _gameObjectContentStorage.GetAllWhiteEffect();
            spriteBatch.End();

            var shakeVector = _screenShaker.GetOffset().GetValueOrDefault(Vector2.Zero);
            var shakeVector3d = new Vector3(shakeVector, 0);

            var worldTransformationMatrix = _camera.GetViewTransformationMatrix();
            worldTransformationMatrix.Decompose(out var scaleVector, out _, out var translationVector);

            var matrix = Matrix.CreateTranslation(translationVector + shakeVector3d)
                         * Matrix.CreateScale(scaleVector);

            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: _camera.GetViewTransformationMatrix(),
                effect: allWhite);
        }
        else
        {
            spriteBatch.End();

            var shakeVector = _screenShaker.GetOffset().GetValueOrDefault(Vector2.Zero);
            var shakeVector3d = new Vector3(shakeVector, 0);

            var worldTransformationMatrix = _camera.GetViewTransformationMatrix();
            worldTransformationMatrix.Decompose(out var scaleVector, out _, out var translationVector);

            var matrix = Matrix.CreateTranslation(translationVector + shakeVector3d)
                         * Matrix.CreateScale(scaleVector);

            spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: _camera.GetViewTransformationMatrix());
        }

        Graphics.Draw(spriteBatch);

        spriteBatch.End();

        spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: _camera.GetViewTransformationMatrix());
    }

    internal void ChangeState(CombatUnitState visualIdleState)
    {
        _visualIdleState = visualIdleState;
    }

    internal float GetZIndex()
    {
        return Graphics.Root.Position.Y;
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
                AddStateEngine(new UnitIdleState(Graphics, CombatUnitState.Idle /*Combatant.State*/));
            }

            ResetActorRootSpritePosition();
        }
    }

    private void MoveIndicatorsToCorpse(Renderable corpse)
    {
        var indicators = Children.OfType<TextIndicatorBase>().ToArray();
        foreach (var indicator in indicators)
        {
            RemoveChild(indicator);
            corpse.AddChild(indicator);
        }
    }

    private void ResetActorRootSpritePosition()
    {
        Graphics.Root.Position = Position;
    }

    // private void Unit_SchemeAutoTransition(object? sender, AutoTransitionEventArgs e)
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