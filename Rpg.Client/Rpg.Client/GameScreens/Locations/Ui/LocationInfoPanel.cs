using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Locations.Ui
{
    internal sealed class LocationInfoPanel : ControlBase
    {
        private readonly Core.Biome _biome;

        private readonly ButtonBase _combatButton;
        private readonly Globe _globe;
        private readonly GlobeNode _globeNode;
        private readonly GlobeNodeSid _nodeSid;
        private readonly Texture2D _panelTexture;
        private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
        private readonly SpriteFont _textFont;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;

        public LocationInfoPanel(GlobeNodeSid nodeSid, Texture2D panelTexture, Texture2D buttonTexture,
            SpriteFont buttonFont, SpriteFont textFont,
            ResolutionIndependentRenderer resolutionIndependentRenderer,
            Core.Biome biome, GlobeNode globeNode, Globe globe, IUnitSchemeCatalog unitSchemeCatalog) : base(
            panelTexture)
        {
            _nodeSid = nodeSid;
            _panelTexture = panelTexture;
            _textFont = textFont;
            _resolutionIndependentRenderer = resolutionIndependentRenderer;
            _biome = biome;
            _globeNode = globeNode;
            _globe = globe;
            _unitSchemeCatalog = unitSchemeCatalog;

            _combatButton =
                new ResourceTextButton(nameof(UiResource.ToTheCombatButtonTitle), buttonTexture, buttonFont);
        }

        public void Update(GameTime gameTime)
        {
            _combatButton.Update(_resolutionIndependentRenderer);
        }

        protected override Color CalculateColor()
        {
            return Color.White;
        }

        protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
        {
            _combatButton.Rect = new Rectangle(contentRect.Left + 5, contentRect.Bottom - 25 - 5,
                contentRect.Width - 5 * 2, 20);

            _combatButton.Draw(spriteBatch);

            DrawBiomeLevel(spriteBatch, contentRect, _biome, _textFont);
        }

        private static void DrawBiomeLevel(SpriteBatch spriteBatch, Rectangle contentRect, Core.Biome biome,
            SpriteFont font)
        {
            var biomeLevelText = $"{UiResource.BiomeLevelText}: {biome.Level}";
            var textSize = font.MeasureString(biomeLevelText);
            const int BIOME_LEVEL_TOP_MARGIN = 5;
            var biomeLevelTextPosition = new Vector2(
                contentRect.Width * 0.5f - textSize.X * 0.5f,
                contentRect.Top + BIOME_LEVEL_TOP_MARGIN);

            spriteBatch.DrawString(font, biomeLevelText,
                biomeLevelTextPosition, Color.White);
        }

        private string GetCombatRewards(GlobeNode node)
        {
            if (node.CombatSequence is null)
            {
                // No combat - no rewards
                return string.Empty;
            }

            // TODO Display icons

            var summaryReward = GetSummaryXpAwardLabel(node);

            var equipmentType = node.EquipmentItem;
            if (equipmentType is not null)
            {
                var targetUnitScheme =
                    UnsortedHelpers.GetPlayerPersonSchemeByEquipmentType(_unitSchemeCatalog, equipmentType);

                var playerUnit = _globe.Player.GetAll()
                    .SingleOrDefault(x => x.UnitScheme == targetUnitScheme);

                if (playerUnit is not null)
                {
                    var equipmentTypeText = GameObjectHelper.GetLocalized(equipmentType);
                    summaryReward += Environment.NewLine + equipmentTypeText;
                }
            }

            return summaryReward;
        }

        private static string GetSummaryXpAwardLabel(GlobeNode node)
        {
            var totalXpForMonsters = node.CombatSequence.Combats.First().EnemyGroup.GetUnits().Sum(x => x.XpReward);
            var combatCount = node.CombatSequence.Combats.Count;
            var summaryXp =
                (int)Math.Round(totalXpForMonsters * LocationsScreenTextHelper.GetCombatSequenceSizeBonus(combatCount));

            return $"{UiResource.XpRewardText}: {summaryXp}";
        }

        public event EventHandler Selected;
    }
}