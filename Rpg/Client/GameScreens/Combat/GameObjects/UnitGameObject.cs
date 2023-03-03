using System;
using System.Collections.Generic;
using System.Linq;

using Client.GameScreens.Combat.GameObjects;

using Core.Combats;
using Core.Dices;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.GameObjects.CommonStates;
using Rpg.Client.GameScreens.Combat.Ui;

namespace Rpg.Client.GameScreens.Combat.GameObjects
{
    internal sealed class UnitGameObject : EwarRenderableBase
    {
        private readonly IList<IUnitStateEngine> _actorStateEngineList;
        private readonly AnimationManager _animationManager;
        private readonly Camera2D _camera;
        private readonly IDice _dice;
        private readonly bool _isPlayerSide;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly ScreenShaker _screenShaker;
        private readonly IUnitPositionProvider _unitPositionProvider;

        public UnitGameObject(Combatant combatant, UnitGraphicsConfigBase combatantGraphicsConfig, FieldCoords formationCoords, IUnitPositionProvider unitPositionProvider,
            GameObjectContentStorage gameObjectContentStorage,
            Camera2D camera, ScreenShaker screenShaker, AnimationManager animationManager,
            IDice dice,
            bool isPlayerSide)
        {
            _actorStateEngineList = new List<IUnitStateEngine>();

            var actorGraphicsConfig = combatantGraphicsConfig;
            
            var position = unitPositionProvider.GetPosition(formationCoords, isPlayerSide);
            var spriteSheetId = Enum.Parse<UnitName>(combatant.ClassSid, ignoreCase: true);
            Graphics = new UnitGraphics(spriteSheetId, actorGraphicsConfig, isPlayerSide, position, gameObjectContentStorage);

            Combatant = combatant;
            _unitPositionProvider = unitPositionProvider;
            Position = position;
            _gameObjectContentStorage = gameObjectContentStorage;
            _camera = camera;
            _screenShaker = screenShaker;
            _animationManager = animationManager;
            _dice = dice;
            _isPlayerSide = isPlayerSide;

            // TODO Call ShiftShape from external combat core
            // combatant.Unit.SchemeAutoTransition += Unit_SchemeAutoTransition;
            // combatant.PositionChanged += CombatUnit_PositionChanged;
        }

        public Combatant Combatant { get; }

        public UnitGraphics Graphics { get; }

        public Vector2 InteractionPoint => Position - Vector2.UnitY * 64;

        public bool IsActive { get; set; }
        public Vector2 LaunchPoint => Position - Vector2.UnitY * 64;

        public void AnimateWound()
        {
            AddStateEngine(new WoundState(Graphics));
        }

        public CorpseGameObject CreateCorpse()
        {
            var spriteSheetId = Enum.Parse<UnitName>(Combatant.ClassSid);
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            HandleEngineStates(gameTime);

            Graphics.Update(gameTime);
        }

        public AnimationBlocker PlaybackCombatMovement(CombatMovementExecution movementExecution)
        {
            var mainAnimationBlocker = _animationManager.CreateAndUseBlocker();
            
            foreach (var imposeItem in movementExecution.EffectImposeItems)
            foreach (var target in imposeItem.MaterializedTargets)
                imposeItem.ImposeDelegate(target);

            movementExecution.CompleteDelegate();

            return mainAnimationBlocker;
        }
        
        public void PlaybackCombatMovement(UnitGameObject target,
            IList<IInteractionDelivery> interactionDeliveryList, IVisualizedSkill skill, SkillExecution action,
            IList<UnitGameObject> unitGameObjects)
        {
            var context = new SkillVisualizationContext(unitGameObjects)
            {
                Interaction = action,
                AnimationManager = _animationManager,
                ScreenShaker = _screenShaker,
                InteractionDeliveryManager = interactionDeliveryList,
                GameObjectContentStorage = _gameObjectContentStorage,
                Dice = _dice
            };

            var mainAnimationBlocker = _animationManager.CreateAndUseBlocker();

            var actorStateEngine = skill.CreateState(this, target, mainAnimationBlocker, context);

            if (!actorStateEngine.IsComplete)
            {
                mainAnimationBlocker.Released += (_, _) =>
                {
                    CompleteSkillExecution(action);
                };

                AddStateEngine(actorStateEngine);
            }
            else
            {
                throw new InvalidOperationException("The completed state is cause of a hang-error.");
            }

            void CompleteSkillExecution(SkillExecution action)
            {
                action.SkillComplete();
                SkillAnimationCompleted?.Invoke(this, EventArgs.Empty);
            }
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
                worldTransformationMatrix.Decompose(out var scaleVector, out var _, out var translationVector);

                var matrix = Matrix.CreateTranslation(translationVector + shakeVector3d)
                             * Matrix.CreateScale(scaleVector);

                spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
                    blendState: BlendState.AlphaBlend,
                    samplerState: SamplerState.PointClamp,
                    depthStencilState: DepthStencilState.None,
                    rasterizerState: RasterizerState.CullNone,
                    transformMatrix: matrix,
                    effect: allWhite);
            }
            else
            {
                spriteBatch.End();

                var shakeVector = _screenShaker.GetOffset().GetValueOrDefault(Vector2.Zero);
                var shakeVector3d = new Vector3(shakeVector, 0);

                var worldTransformationMatrix = _camera.GetViewTransformationMatrix();
                worldTransformationMatrix.Decompose(out var scaleVector, out var _, out var translationVector);

                var matrix = Matrix.CreateTranslation(translationVector + shakeVector3d)
                             * Matrix.CreateScale(scaleVector);

                spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
                    blendState: BlendState.AlphaBlend,
                    samplerState: SamplerState.PointClamp,
                    depthStencilState: DepthStencilState.None,
                    rasterizerState: RasterizerState.CullNone,
                    transformMatrix: matrix);
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

        internal float GetZIndex()
        {
            return Graphics.Root.Position.Y;
        }

        private void AddStateEngine(IUnitStateEngine actorStateEngine)
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

        public void ChangeFieldPosition(FieldCoords fieldCoords)
        {
            var position = _unitPositionProvider.GetPosition(fieldCoords, _isPlayerSide);
            Graphics.ChangePosition(position);
            Position = position;
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

        private CombatUnitState _visualIdleState;

        internal void ChangeState(CombatUnitState visualIdleState)
        {
            _visualIdleState = visualIdleState;
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
}