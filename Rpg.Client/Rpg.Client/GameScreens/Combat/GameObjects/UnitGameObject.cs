using System;
using System.Collections.Generic;
using System.Linq;

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
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly ScreenShaker _screenShaker;

        public UnitGameObject(CombatUnit combatUnit, Vector2 position,
            GameObjectContentStorage gameObjectContentStorage,
            Camera2D camera, ScreenShaker screenShaker, AnimationManager animationManager,
            IDice dice)
        {
            _actorStateEngineList = new List<IUnitStateEngine>();

            Graphics = new UnitGraphics(combatUnit.Unit, position, gameObjectContentStorage);

            CombatUnit = combatUnit;
            Position = position;
            _gameObjectContentStorage = gameObjectContentStorage;
            _camera = camera;
            _screenShaker = screenShaker;
            _animationManager = animationManager;
            _dice = dice;

            combatUnit.Unit.SchemeAutoTransition += Unit_SchemeAutoTransition;
        }

        public CombatUnit CombatUnit { get; }

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
            var deathSoundEffect = _gameObjectContentStorage.GetDeathSound(CombatUnit.Unit.UnitScheme.Name)
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

        public void UseSkill(UnitGameObject target,
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

            mainAnimationBlocker.Released += (_, _) =>
            {
                action.SkillComplete();
                SkillAnimationCompleted?.Invoke(this, EventArgs.Empty);
            };

            AddStateEngine(actorStateEngine);
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
                    AddStateEngine(new UnitIdleState(Graphics, CombatUnit.State));
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

        private void Unit_SchemeAutoTransition(object? sender, AutoTransitionEventArgs e)
        {
            var shapeShiftBlocker = _animationManager.CreateAndUseBlocker();
            var deathSound = _gameObjectContentStorage.GetDeathSound(e.SourceScheme.Name);
            AddStateEngine(new ShapeShiftState(Graphics, deathSound.CreateInstance(), shapeShiftBlocker));

            shapeShiftBlocker.Released += (_, _) =>
            {
                Graphics.SwitchSourceUnit(CombatUnit.Unit);
                AddStateEngine(new UnitIdleState(Graphics, CombatUnit.State));
            };
        }

        public event EventHandler? SkillAnimationCompleted;
    }
}