﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.GameScreens.Bestiary;
using Rpg.Client.GameScreens.Biome;
using Rpg.Client.GameScreens.CharacterDetails;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.GameScreens.Credits;
using Rpg.Client.GameScreens.EndGame;
using Rpg.Client.GameScreens.Event;
using Rpg.Client.GameScreens.Hero;
using Rpg.Client.GameScreens.Map;
using Rpg.Client.GameScreens.Party;
using Rpg.Client.GameScreens.Speech;
using Rpg.Client.GameScreens.Title;

namespace Rpg.Client.ScreenManagement
{
    internal class ScreenManager : IScreenManager
    {
        private const double TRANSITION_DURATION = 1;
        private readonly EwarGame _game;
        private readonly Texture2D _transitionTexture;
        private bool _screenChanged;

        private double? _transitionCounter;

        public ScreenManager(EwarGame game)
        {
            _game = game;
            var colors = new[] { Color.Black };
            _transitionTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            _transitionTexture.SetData(colors);
        }

        public IScreen? ActiveScreen { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (ActiveScreen is not null)
            {
                ActiveScreen.Draw(spriteBatch);
            }

            DrawTransition(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            if (ActiveScreen is null)
            {
                return;
            }

            ActiveScreen.Update(gameTime);

            if (ActiveScreen.TargetScreen is not null)
            {
                if (_transitionCounter is null)
                {
                    _transitionCounter = 0;
                }
            }

            if (_transitionCounter is not null)
            {
                if (_transitionCounter.Value < TRANSITION_DURATION)
                {
                    _transitionCounter += gameTime.ElapsedGameTime.TotalSeconds;

                    if (_transitionCounter.Value > TRANSITION_DURATION / 2 && !_screenChanged)
                    {
                        _screenChanged = true;

                        ActiveScreen = ActiveScreen.TargetScreen;
                    }
                }
                else
                {
                    _transitionCounter = null;
                    _screenChanged = false;
                }
            }
        }

        private IScreen CreateScreenToTransit(ScreenTransition targetTransition)
        {
            return targetTransition switch
            {
                ScreenTransition.Title => new TitleScreen(_game),
                ScreenTransition.Map => new MapScreen(_game),
                ScreenTransition.Biome => new BiomeScreen(_game),
                ScreenTransition.Party => new PartyScreen(_game),
                ScreenTransition.Hero => new HeroScreen(_game),
                ScreenTransition.Event => new SpeechScreen(_game),
                ScreenTransition.Combat => new CombatScreen(_game),
                ScreenTransition.Bestiary => new BestiaryScreen(_game),
                ScreenTransition.Credits => new CreditsScreen(_game),
                ScreenTransition.EndGame => new EndGameScreen(_game),
                _ => throw new ArgumentException("Unknown transition", nameof(targetTransition))
            };
        }

        private void DrawTransition(SpriteBatch spriteBatch)
        {
            if (_transitionCounter is null)
            {
                return;
            }

            spriteBatch.Begin();

            var t = _transitionCounter.Value / TRANSITION_DURATION;

            if (t < 0.5)
            {
                var t2 = t * 2;
                spriteBatch.Draw(_transitionTexture,
                    new Rectangle(
                        0,
                        0,
                        (int)(_game.GraphicsDevice.Viewport.Width * t2),
                        _game.GraphicsDevice.Viewport.Height),
                    Color.White);
            }
            else
            {
                var t2 = (t - 0.5) * 2;
                spriteBatch.Draw(_transitionTexture,
                    new Rectangle(
                        (int)(_game.GraphicsDevice.Viewport.Width * t2),
                        0,
                        _game.GraphicsDevice.Viewport.Width,
                        _game.GraphicsDevice.Viewport.Height),
                    Color.White);
            }

            spriteBatch.End();
        }

        public void ExecuteTransition(IScreen currentScreen, ScreenTransition targetTransition)
        {
            var targetScreen = CreateScreenToTransit(targetTransition);
            currentScreen.TargetScreen = targetScreen;
        }
    }
}