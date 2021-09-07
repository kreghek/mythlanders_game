﻿using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.Screens;

namespace Rpg.Client.Models.Map
{
    public class MapScreen : GameScreenBase
    {
        private readonly IList<TextBaseButton> _biomButtons;
        private readonly Globe _globe;
        private readonly IUiContentStorage _uiContentStorage;

        private readonly IDice _dice;

        private bool _isNodeModelsCreated;
    
        public MapScreen(IScreenManager screenManager, Globe globe, IUiContentStorage uiContentStorage, IDice dice) : base(screenManager)
        {
            _globe = globe;
            _uiContentStorage = uiContentStorage;
            _dice = dice;
            _biomButtons = new List<TextBaseButton>();
        }
    
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
    
            if (_isNodeModelsCreated)
            {
                var index = 0;
                foreach (var button in _biomButtons)
                {
                    button.Rect = new Rectangle(100, 100 + index * 30, 200, 25);
                    button.Draw(spriteBatch);
                    index++;
                }
            }
    
            spriteBatch.End();
        }
    
        public override void Update(GameTime gameTime)
        {
            if (!_globe.IsNodeInitialied)
            {
                _globe.UpdateNodes(_dice);
                _globe.IsNodeInitialied = true;
            }
            else
            {
                if (!_isNodeModelsCreated)
                {
                    foreach (var biom in _globe.Bioms.Where(x => x.IsAvailable).ToArray())
                    {
                        var button = new TextBaseButton(biom.Type.ToString(), _uiContentStorage.GetButtonTexture(),
                            _uiContentStorage.GetMainFont(), Rectangle.Empty);
                        button.OnClick += (s, e) =>
                        {
                            _globe.CurrentBiom = biom;
    
                            ScreenManager.ExecuteTransition(this, ScreenTransition.Biom);
                        };
                        _biomButtons.Add(button);
                    }
    
                    _isNodeModelsCreated = true;
                }
                else
                {
                    foreach (var button in _biomButtons)
                    {
                        button.Update();
                    }
                }
            }
        }
    }
}