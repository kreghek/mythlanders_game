using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Combat.Ui.CombatResultModalModels;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal class CombatResultsBiomeProgression : ControlBase
    {
        private readonly AnimatedCountableUnitItemStat _progression;
        private readonly SpriteFont _textFont;

        public CombatResultsBiomeProgression(Texture2D texture,
            SpriteFont textFont,
            AnimatedCountableUnitItemStat progression) : base(texture)
        {
            _textFont = textFont;
            _progression = progression;
        }

        public void Update()
        {
            _progression.Update();
        }

        protected override Color CalculateColor()
        {
            return Color.White;
        }

        protected override void DrawBackground(SpriteBatch spriteBatch, Color color)
        {
            // Do not draw background
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
        {
            const int BIOME_PROGRESS_BLOCK_HEIGHT = 20;

            var biomeProgressRect = new Rectangle(contentRect.X, contentRect.Y, contentRect.Width,
                BIOME_PROGRESS_BLOCK_HEIGHT);
            DrawBiomeProgression(spriteBatch: spriteBatch, biomeProgressRect: biomeProgressRect);
        }

        private void DrawBiomeProgression(SpriteBatch spriteBatch, Rectangle biomeProgressRect)
        {
            var textTemplate = UiResource.CombatResultMonsterDangerIncreasedTemplate;
            if (_progression.Amount < 0)
            {
                textTemplate = UiResource.CombatResultMonsterDangerDecreasedTemplate;
            }

            var biomeProgress = string.Format(textTemplate, _progression.CurrentValue);
            spriteBatch.DrawString(_textFont, biomeProgress, biomeProgressRect.Location.ToVector2(), Color.Wheat);
        }
    }
}