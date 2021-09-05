using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Models.Biom;
using Rpg.Client.Models.Combat;
using Rpg.Client.Models.EndGame;
using Rpg.Client.Models.Event;
using Rpg.Client.Models.Map;
using Rpg.Client.Models.Title;

namespace Rpg.Client.Screens
{
    public class ScreenManager : IScreenManager
    {
        public BiomScreen? BiomScreen { get; set; }

        public CombatScreen? CombatScreen { get; set; }

        public EndGameScreen? EndGameScreen { get; set; }

        public EventScreen? EventScreen { get; set; }

        public MapScreen? MapScreen { get; set; }

        public IScreen? ActiveScreen { get; set; }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (ActiveScreen is not null)
                ActiveScreen.Draw(gameTime, spriteBatch);
        }

        public void ExecuteTransition(IScreen currentScreen, ScreenTransition targetTransition)
        {
            var targetScreen = CreateScreenToTransit(targetTransition);
            currentScreen.TargetScreen = targetScreen;
        }

        public TitleScreen? StartScreen { get; set; }

        public void Update(GameTime gameTime)
        {
            if (ActiveScreen is null)
                return;

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
                ScreenTransition.Title => StartScreen,
                ScreenTransition.Map => MapScreen,
                ScreenTransition.Biom => BiomScreen,
                ScreenTransition.Event => EventScreen,
                ScreenTransition.Combat => CombatScreen,
                ScreenTransition.EndGame => EndGameScreen,
                _ => throw new ArgumentException("Unknown transition", nameof(targetTransition))
            };
        }
    }
}