using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Models.Biome;
using Rpg.Client.Models.Combat;
using Rpg.Client.Models.EndGame;
using Rpg.Client.Models.Event;
using Rpg.Client.Models.Map;
using Rpg.Client.Models.Party;
using Rpg.Client.Models.Title;

namespace Rpg.Client.Screens
{
    internal class ScreenManager : IScreenManager
    {
        private readonly EwarGame _game;

        public ScreenManager(EwarGame game)
        {
            _game = game;
        }

        public IScreen? ActiveScreen { get; set; }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (ActiveScreen is not null)
            {
                ActiveScreen.Draw(gameTime, spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (ActiveScreen is null)
            {
                return;
            }

            if (ActiveScreen.TargetScreen is not null)
            {
                ActiveScreen = ActiveScreen.TargetScreen;
                ActiveScreen.TargetScreen = null;
            }

            ActiveScreen.Update(gameTime);
        }

        private IScreen CreateScreenToTransit(ScreenTransition targetTransition)
        {
            return targetTransition switch
            {
                ScreenTransition.Title => new TitleScreen(_game),
                ScreenTransition.Map => new MapScreen(_game),
                ScreenTransition.Biome => new BiomeScreen(_game),
                ScreenTransition.Party => new PartyScreen(_game),
                ScreenTransition.Event => new EventScreen(_game),
                ScreenTransition.Combat => new CombatScreen(_game),
                ScreenTransition.EndGame => new EndGameScreen(_game),
                _ => throw new ArgumentException("Unknown transition", nameof(targetTransition))
            };
        }

        public void ExecuteTransition(IScreen currentScreen, ScreenTransition targetTransition)
        {
            var targetScreen = CreateScreenToTransit(targetTransition);
            currentScreen.TargetScreen = targetScreen;
        }
    }
}