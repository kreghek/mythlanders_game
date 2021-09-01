using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.Models.Biom;
using Rpg.Client.Models.Combat.GameObjects;
using Rpg.Client.Models.Combat.Ui;
using Rpg.Client.Models.Map;
using Rpg.Client.Screens;

namespace Rpg.Client.Models.Combat
{
    internal class CombatScreen : GameScreenBase
    {
        private readonly ActiveCombat _combat;
        private readonly IList<UnitGameObject> _gameObjects;
        private readonly IList<ButtonBase> _enemyAttackList;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly IUiContentStorage _uiContentStorage;
        private readonly AnimationManager _animationManager;
        private CombatSkillPanel _combatSkillsPanel;
        private CombatResultPanel _combatResultPanel;
        private bool _unitsInitialized;

        public CombatScreen(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            var globe = game.Services.GetService<Globe>();
            _combat = globe.ActiveCombat ?? throw new InvalidOperationException(nameof(globe.ActiveCombat) + " is null");

            _gameObjects = new List<UnitGameObject>();
            _enemyAttackList = new List<ButtonBase>();

            _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            _animationManager = game.Services.GetService<AnimationManager>();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            var list = _gameObjects.ToArray();
            foreach (var gameObject in list)
            {
                gameObject.Draw(SpriteBatch);
            }

            SpriteBatch.End();

            SpriteBatch.Begin();

            if (_combat.CurrentUnit is not null)
            {
                if (_combat.CurrentUnit.Unit.IsPlayerControlled && !_animationManager.HasBlockers)
                {
                    if (_combatSkillsPanel is not null)
                    {
                        _combatSkillsPanel.Draw(SpriteBatch, Game.GraphicsDevice);
                    }

                    if (_combatSkillsPanel?.SelectedCard is not null)
                    {
                        foreach (var button in _enemyAttackList)
                        {
                            button.Draw(SpriteBatch);
                        }
                    }
                }

                if (_combatResultPanel is not null)
                {
                    _combatResultPanel.Draw(SpriteBatch, Game.GraphicsDevice);
                }
            }

            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Game.Exit();

            if (!_unitsInitialized)
            {
                _combat.Initialize();
                _combat.StartRound();

                var playerUnits = _combat.Units.Where(x => x.Unit.IsPlayerControlled);

                var index = 0;
                foreach (var unit in playerUnits)
                {
                    var position = new Vector2(100, index * 128 + 100);
                    var gameObject = new UnitGameObject(unit, position, _gameObjectContentStorage);
                    _gameObjects.Add(gameObject);
                    index++;
                }

                var cpuUnits = _combat.Units.Where(x => !x.Unit.IsPlayerControlled);

                index = 0;
                foreach (var unit in cpuUnits)
                {
                    var position = new Vector2(400, index * 128 + 100);
                    var gameObject = new UnitGameObject(unit, position, _gameObjectContentStorage);
                    _gameObjects.Add(gameObject);

                    var iconButton = new IconButton(_uiContentStorage.GetButtonTexture(), _uiContentStorage.GetButtonTexture(), new Rectangle(position.ToPoint(), new Point(32, 32)));
                    iconButton.OnClick += (s, e) =>
                      {
                          var attackerUnitGameObject = _gameObjects.Single(x => x.Unit == _combat.CurrentUnit);

                          var blocker = new AnimationBlocker();
                          _animationManager.AddBlocker(blocker);

                          attackerUnitGameObject.Attack(gameObject, blocker, _combatSkillsPanel.SelectedCard);

                          blocker.Released += (s, e) =>
                          {
                              bool isEnd = _combat.NextUnit();
                              if (isEnd)
                              {
                                  _combat.StartRound();
                              }
                          };
                      };
                    _enemyAttackList.Add(iconButton);

                    index++;
                }

                _combatSkillsPanel = new CombatSkillPanel(_uiContentStorage)
                {
                    Unit = _combat.CurrentUnit
                };

                _unitsInitialized = true;
            }
            else
            {
                // check combat was finished
                if (!_combat.Finished)
                {
                    foreach (var unitModel in _gameObjects)
                    {
                        unitModel.IsActive = _combat.CurrentUnit == unitModel.Unit;

                        unitModel.Update(gameTime);
                    }

                    if (_combat.CurrentUnit is not null)
                    {

                        if (_combat.CurrentUnit.Unit.IsPlayerControlled)
                        {
                            if (!_animationManager.HasBlockers)
                            {
                                if (_combatSkillsPanel is not null)
                                {
                                    _combatSkillsPanel.Update();
                                }

                                if (_combatSkillsPanel?.SelectedCard is not null)
                                {
                                    foreach (var button in _enemyAttackList)
                                    {
                                        button.Update();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!_animationManager.HasBlockers)
                            {
                                var attackerUnitGameObject = _gameObjects.Single(x => x.Unit == _combat.CurrentUnit);

                                var blocker = new AnimationBlocker();
                                _animationManager.AddBlocker(blocker);

                                var targetPlayerObject = _gameObjects.FirstOrDefault(x => x.Unit.Unit.IsPlayerControlled);

                                var combatCard = attackerUnitGameObject.Unit.CombatCards.First();

                                attackerUnitGameObject.Attack(targetPlayerObject, blocker, combatCard);

                                blocker.Released += (s, e) =>
                                {
                                    bool isEnd = _combat.NextUnit();
                                    if (isEnd)
                                    {
                                        _combat.StartRound();
                                    }
                                };
                            }
                        }
                    }
                    else
                    {
                        // Unit in queue is killed.

                        bool isEnd = _combat.NextUnit();
                        if (isEnd)
                        {
                            _combat.StartRound();
                        }
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
                        var enemyUnitsAreDead = _combat.Units.Any(x=> x.Unit.IsDead && !x.Unit.IsPlayerControlled);

                        _combatResultPanel = new CombatResultPanel(_uiContentStorage);
                        if (enemyUnitsAreDead)
                        {
                            _combatResultPanel.Initialize("Win");
                        }
                        else
                        {
                            _combatResultPanel.Initialize("Fail");
                        }
                        _combatResultPanel.Closed += CombatResultPanel_Closed;
                    }

                    _combatResultPanel.Update(gameTime);
                }
            }
        }

        private void CombatResultPanel_Closed(object? sender, System.EventArgs e)
        {
            _animationManager.DropBlockers();
            var globe = Game.Services.GetService<Globe>();

            if (_combatResultPanel.Result == "Win")
            {
                globe.Player.Souls += 10;
                _combat.Biom.Level++;

                if (_combat.Combat.IsBossLevel)
                {
                    _combat.Biom.IsComplete = true;
                }

                var aliveUnits = _combat.Units.Where(x => x.Unit.IsPlayerControlled && !x.Unit.IsDead).ToArray();
                var monsters = _combat.Units.Where(x => !x.Unit.IsPlayerControlled && x.Unit.IsDead).ToArray();

                foreach (var unit in aliveUnits)
                {
                    unit.Unit.GainXp(5 * (_combat.Combat.Level * 2) * monsters.Length / aliveUnits.Length);
                }
            }
            else
            {
                if (_combat.Combat.IsBossLevel)
                {
                    _combat.Biom.Level = 0;
                }
            }

            var dice = Game.Services.GetService<IDice>();
            globe.UpdateNodes(dice);

            TargetScreen = new BiomScreen(Game, SpriteBatch);
        }
    }
}
