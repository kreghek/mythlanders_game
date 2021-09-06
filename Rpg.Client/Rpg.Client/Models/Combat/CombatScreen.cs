using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.Models.Combat.GameObjects;
using Rpg.Client.Models.Combat.Ui;
using Rpg.Client.Screens;

namespace Rpg.Client.Models.Combat
{
    public class CombatScreen : GameScreenBase
    {
        private readonly AnimationManager _animationManager;

        private readonly IDice _dice;

        private readonly IList<BaseButton> _enemyAttackList;

        private readonly Game _game;

        private readonly GameObjectContentStorage _gameObjectContentStorage;

        private readonly IList<UnitGameObject> _gameObjects;

        private readonly Globe _globe;

        private readonly GraphicsDevice _graphicsDevice;

        private readonly IUiContentStorage _uiContentStorage;

        private CombatResultPanel _combatResultPanel;

        private CombatSkillPanel _combatSkillsPanel;

        private bool _unitsInitialized;

        public CombatScreen(IScreenManager screenManager, Globe globe, GameObjectContentStorage gameObjectContentStorage,
            IUiContentStorage uiContentStorage, AnimationManager animationManager, Game game, IDice dice,
            GraphicsDevice graphicsDevice)
            : base(screenManager)
        {
            _gameObjects = new List<UnitGameObject>();
            _enemyAttackList = new List<BaseButton>();

            _globe = globe;
            _gameObjectContentStorage = gameObjectContentStorage;
            _uiContentStorage = uiContentStorage;
            _animationManager = animationManager;
            _game = game;
            _dice = dice;
            _graphicsDevice = graphicsDevice;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawGameObjects(spriteBatch);

            DrawHud(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.Exit();

            var combat = GetActiveCombat();

            if (!_unitsInitialized)
            {
                combat.Initialize();
                combat.StartRound();

                var playerUnits = combat.Units.Where(x => x.Unit.IsPlayerControlled);

                var index = 0;
                foreach (var unit in playerUnits)
                {
                    var position = new Vector2(100, index * 128 + 100);
                    var gameObject = new UnitGameObject(unit, position, _gameObjectContentStorage);
                    _gameObjects.Add(gameObject);
                    index++;
                }

                var cpuUnits = combat.Units.Where(x => !x.Unit.IsPlayerControlled);

                index = 0;
                foreach (var unit in cpuUnits)
                {
                    var position = new Vector2(400, index * 128 + 100);
                    var gameObject = new UnitGameObject(unit, position, _gameObjectContentStorage);
                    _gameObjects.Add(gameObject);

                    var iconButton = new IconBaseButton(
                        _uiContentStorage.GetButtonTexture(),
                        _uiContentStorage.GetButtonTexture(),
                        new Rectangle(position.ToPoint(), new Point(32, 32)));
                    iconButton.OnClick += (s, e) =>
                    {
                        var attackerUnitGameObject = _gameObjects.Single(x => x.Unit == combat.CurrentUnit);

                        var blocker = new AnimationBlocker();
                        _animationManager.AddBlocker(blocker);

                        attackerUnitGameObject.Attack(gameObject, blocker, _combatSkillsPanel.SelectedCard);

                        blocker.Released += (s, e) =>
                        {
                            var isEnd = combat.NextUnit();
                            if (isEnd)
                                combat.StartRound();
                        };
                    };
                    _enemyAttackList.Add(iconButton);

                    index++;
                }

                _combatSkillsPanel = new CombatSkillPanel(_uiContentStorage)
                {
                    Unit = combat.CurrentUnit
                };

                _unitsInitialized = true;
            }
            else
            {
                // check combat was finished
                if (!combat.Finished)
                {
                    foreach (var unitModel in _gameObjects)
                    {
                        unitModel.IsActive = combat.CurrentUnit == unitModel.Unit;

                        unitModel.Update(gameTime);
                    }

                    if (combat.CurrentUnit is not null)
                    {
                        if (combat.CurrentUnit.Unit.IsPlayerControlled)
                        {
                            if (!_animationManager.HasBlockers)
                            {
                                if (_combatSkillsPanel is not null)
                                    _combatSkillsPanel.Update();

                                if (_combatSkillsPanel?.SelectedCard is not null)
                                    foreach (var button in _enemyAttackList)
                                    {
                                        button.Update();
                                    }
                            }
                        }
                        else
                        {
                            if (!_animationManager.HasBlockers)
                            {
                                var attackerUnitGameObject = _gameObjects.Single(x => x.Unit == combat.CurrentUnit);

                                var blocker = new AnimationBlocker();
                                _animationManager.AddBlocker(blocker);

                                var targetPlayerObject =
                                    _gameObjects.FirstOrDefault(x => x.Unit.Unit.IsPlayerControlled);

                                var combatCard = attackerUnitGameObject.Unit.CombatCards.First();

                                attackerUnitGameObject.Attack(targetPlayerObject, blocker, combatCard);

                                blocker.Released += (s, e) =>
                                {
                                    var isEnd = combat.NextUnit();
                                    if (isEnd)
                                        combat.StartRound();
                                };
                            }
                        }
                    }
                    else
                    {
                        // Unit in queue is killed.

                        var isEnd = combat.NextUnit();
                        if (isEnd)
                            combat.StartRound();
                    }
                }
                else
                {
                    foreach (var unitModel in _gameObjects)
                    {
                        unitModel.IsActive = false;

                        unitModel.Update(gameTime);
                    }

                    if (_combatResultPanel is null)
                    {
                        var enemyUnitsAreDead = combat.Units.Any(x => x.Unit.IsDead && !x.Unit.IsPlayerControlled);

                        _combatResultPanel = new CombatResultPanel(_uiContentStorage);
                        if (enemyUnitsAreDead)
                            _combatResultPanel.Initialize("Win");
                        else
                            _combatResultPanel.Initialize("Fail");

                        _combatResultPanel.Closed += CombatResultPanel_Closed;
                    }

                    _combatResultPanel.Update(gameTime);
                }
            }
        }

        private void CombatResultPanel_Closed(object? sender, EventArgs e)
        {
            _animationManager.DropBlockers();

            var bossWasDefeat = false;
            var finalBossWasDefeat = false;

            var combat = GetActiveCombat();

            if (_combatResultPanel.Result == "Win")
            {
                combat.Biom.Level++;

                if (combat.Combat.IsBossLevel)
                {
                    combat.Biom.IsComplete = true;
                    bossWasDefeat = true;

                    if (combat.Biom.IsFinalBiom)
                        finalBossWasDefeat = true;
                }

                var aliveUnits = combat.Units.Where(x => x.Unit.IsPlayerControlled && !x.Unit.IsDead).ToArray();
                var monsters = combat.Units.Where(x => !x.Unit.IsPlayerControlled && x.Unit.IsDead).ToArray();

                foreach (var unit in aliveUnits)
                {
                    unit.Unit.GainXp(5 * (combat.Combat.Level * 2) * monsters.Length / aliveUnits.Length);
                }
            }
            else
            {
                if (combat.Combat.IsBossLevel)
                    combat.Biom.Level = 0;
            }

            _globe.UpdateNodes(_dice);

            if (bossWasDefeat)
            {
                if (finalBossWasDefeat)
                    ScreenManager.ExecuteTransition(this, ScreenTransition.Map);
                else
                    ScreenManager.ExecuteTransition(this, ScreenTransition.EndGame);
            }
            else
            {
                ScreenManager.ExecuteTransition(this, ScreenTransition.Biom);
            }
        }

        private ActiveCombat GetActiveCombat()
        {
            return _globe.ActiveCombat ?? throw new InvalidOperationException(nameof(_globe.ActiveCombat) + " is null");
        }

        private void DrawGameObjects(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            var list = _gameObjects.ToArray();
            foreach (var gameObject in list)
            {
                gameObject.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        private void DrawHud(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            var combat = GetActiveCombat();

            if (combat.CurrentUnit is not null)
            {
                if (combat.CurrentUnit.Unit.IsPlayerControlled && !_animationManager.HasBlockers)
                {
                    if (_combatSkillsPanel is not null)
                        _combatSkillsPanel.Draw(spriteBatch, _graphicsDevice);

                    if (_combatSkillsPanel?.SelectedCard is not null)
                        foreach (var button in _enemyAttackList)
                        {
                            button.Draw(spriteBatch);
                        }
                }

                if (_combatResultPanel is not null)
                    _combatResultPanel.Draw(spriteBatch, _graphicsDevice);
            }

            spriteBatch.End();
        }
    }
}