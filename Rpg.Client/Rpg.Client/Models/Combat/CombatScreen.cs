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
    internal class CombatScreen : GameScreenBase
    {
        private readonly ActiveCombat? _combat;
        private readonly IList<UnitGameObject> _gameObjects;
        private readonly GameObjectContentStorage _gameObjectContentStorage;
        private readonly IUiContentStorage _uiContentStorage;
        private CombatSkillPanel _combatSkillsPanel;
        private bool _unitsInitialized;

        public CombatScreen(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            var globe = game.Services.GetService<Globe>();
            _combat = globe.ActiveCombat;

            _gameObjects = new List<UnitGameObject>();

            _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
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
            if (_combatSkillsPanel is not null)
            {
                _combatSkillsPanel.Draw(SpriteBatch, Game.GraphicsDevice);
            }
            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Game.Exit();

            if (!_unitsInitialized)
            {
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
                foreach (var unitModel in _gameObjects)
                {
                    unitModel.IsActive = _combat.CurrentUnit == unitModel.Unit;
                }

                if (_combatSkillsPanel is not null)
                {
                    _combatSkillsPanel.Update();
                }
            }
        }
    }
}
