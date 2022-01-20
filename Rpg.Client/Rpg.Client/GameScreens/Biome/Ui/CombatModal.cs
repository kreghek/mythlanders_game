using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Biome.GameObjects;

namespace Rpg.Client.GameScreens.Biome.Ui
{
    internal sealed class CombatModalContext
    {
        public Action<GlobeNode> AutoCombatDelegate { get; set; }
        public Action<GlobeNode> CombatDelegate { get; set; }
        public Globe Globe { get; set; }
        public GlobeNodeGameObject SelectedNodeGameObject { get; set; }
    }

    internal sealed class CombatModal : ModalDialogBase
    {
        private readonly IList<ButtonBase> _buttons;
        private readonly Globe _globe;
        private readonly GlobeNodeGameObject _nodeGameObject;
        private readonly IUiContentStorage _uiContentStorage;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;

        public CombatModal(CombatModalContext context, IUiContentStorage uiContentStorage,
            ResolutionIndependentRenderer resolutionIndependentRenderer,
            IUnitSchemeCatalog unitSchemeCatalog) : base(uiContentStorage,
            resolutionIndependentRenderer)
        {
            _globe = context.Globe;
            _nodeGameObject = context.SelectedNodeGameObject;
            _uiContentStorage = uiContentStorage;
            _unitSchemeCatalog = unitSchemeCatalog;
            _buttons = new List<ButtonBase>();

            var combatButton = new TextButton(UiResource.ToTheCombatButtonTitle, _uiContentStorage.GetButtonTexture(),
                _uiContentStorage.GetMainFont(), Rectangle.Empty);
            combatButton.OnClick += (s, e) =>
            {
                context.CombatDelegate(context.SelectedNodeGameObject.GlobeNode);
            };
            _buttons.Add(combatButton);

            AddAutoCombatButtonIfAvailable(context);
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            if (_nodeGameObject.GlobeNode.CombatSequence is null)
            {
                Debug.Fail("Combat sequence is required to be assigned.");
                Close();
                return;
            }

            const int MARGIN = 5;
            const int HERO_GROUP_INFO_HEIGHT = 20 + MARGIN;
            const int BUTTONS_HEIGHT = 20 + MARGIN;
            const int IMAGE_HEIGHT = 64;

            var imageRect = new Rectangle(ContentRect.Location, new Point(ContentRect.Width, IMAGE_HEIGHT));
            DrawLocationImage(spriteBatch, imageRect);

            var shortInfoRect = new Rectangle(ContentRect.Left, imageRect.Bottom + MARGIN, ContentRect.Width,
                ContentRect.Height - (IMAGE_HEIGHT + HERO_GROUP_INFO_HEIGHT + BUTTONS_HEIGHT + MARGIN * 2));
            DrawCombatShortInfo(spriteBatch, shortInfoRect);

            var heroGroupRect = new Rectangle(ContentRect.Left, shortInfoRect.Bottom + MARGIN, ContentRect.Width,
                HERO_GROUP_INFO_HEIGHT);
            DrawHeroGroupInfo(spriteBatch, heroGroupRect);

            var buttonGroupRect = new Rectangle(ContentRect.Left, heroGroupRect.Bottom + MARGIN, ContentRect.Width,
                BUTTONS_HEIGHT);
            DrawButtons(spriteBatch, buttonGroupRect);
        }

        protected override void UpdateContent(GameTime gameTime,
            ResolutionIndependentRenderer? resolutionIndependenceRenderer = null)
        {
            base.UpdateContent(gameTime, resolutionIndependenceRenderer);

            foreach (var button in _buttons)
            {
                button.Update(resolutionIndependenceRenderer);
            }
        }

        private void AddAutoCombatButtonIfAvailable(CombatModalContext context)
        {
            var isAutoCombatAvailable = CheckAllSlavicLocationUnlocked();

            if (isAutoCombatAvailable)
            {
                var autocombatButton = new ResourceTextButton(
                    nameof(UiResource.AutocombatButtonTitle),
                    _uiContentStorage.GetButtonTexture(),
                    _uiContentStorage.GetMainFont(),
                    Rectangle.Empty);

                autocombatButton.OnClick += (s, e) =>
                {
                    context.AutoCombatDelegate(context.SelectedNodeGameObject.GlobeNode);
                };
                _buttons.Add(autocombatButton);
            }
        }

        private bool CheckAllSlavicLocationUnlocked()
        {
            // Display autocombat button if all slavic locations are unlocked.
            var lockedSlavicNodes = _globe.Biomes.First(x => x.Type == BiomeType.Slavic).Nodes
                .Where(x => !x.IsAvailable);

            return !lockedSlavicNodes.Any();
        }

        private void DisplayCombatRewards(SpriteBatch spriteBatch, GlobeNodeGameObject nodeGameObject,
            Vector2 rewardBlockPosition, GlobeNodeGameObject node)
        {
            if (node.GlobeNode.CombatSequence is null)
            {
                // No combat - no rewards
                return;
            }

            // TODO Display icons

            DrawSummaryXpAwardLabel(
                spriteBatch,
                node,
                rewardBlockPosition + new Vector2(5, 55));

            DrawEquipmentRewards(
                spriteBatch: spriteBatch,
                nodeGameObject: nodeGameObject,
                toolTipPosition: rewardBlockPosition);
        }

        private void DrawButtons(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            var sumButtonWidth = _buttons.Count * (100 + 5);
            var startXPosition = contentRect.Center.X - sumButtonWidth / 2;

            for (var buttonIndex = 0; buttonIndex < _buttons.Count; buttonIndex++)
            {
                var button = _buttons[buttonIndex];
                button.Rect = new Rectangle(startXPosition + buttonIndex * (100 + 5), contentRect.Top,
                    100, 20);
                button.Draw(spriteBatch);
            }
        }

        private void DrawCombatShortInfo(SpriteBatch spriteBatch, Rectangle contentRectangle)
        {
            var textPosition = contentRectangle.Location.ToVector2();

            var localizedName = GameObjectHelper.GetLocalized(_nodeGameObject.GlobeNode.Sid);

            spriteBatch.DrawString(_uiContentStorage.GetTitlesFont(), localizedName,
                textPosition + new Vector2(5, 5),
                Color.Wheat);

            var dialogMarkerText = _nodeGameObject.AvailableEvent is not null
                ? $"{_nodeGameObject.AvailableEvent.Title}"
                : string.Empty;
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), dialogMarkerText,
                textPosition + new Vector2(5, 25), Color.Wheat);

            var combatCount = _nodeGameObject.GlobeNode.CombatSequence.Combats.Count;
            var combatSequenceSizeText = BiomeScreenTextHelper.GetCombatSequenceSizeText(combatCount);
            spriteBatch.DrawString(
                _uiContentStorage.GetMainFont(),
                $"{UiResource.CombatLengthLabel} {combatSequenceSizeText}",
                textPosition + new Vector2(5, 35), Color.Wheat);

            DisplayCombatRewards(spriteBatch, _nodeGameObject, textPosition, _nodeGameObject);

            var monsterIndex = 0;

            var uniqueMonsterSchemes = _nodeGameObject.GlobeNode.CombatSequence.Combats
                .SelectMany(combatSource => combatSource.EnemyGroup.GetUnits()).Select(x => x.UnitScheme).Distinct()
                .ToArray();
            foreach (var monsterScheme in uniqueMonsterSchemes)
            {
                var unitName = monsterScheme.Name;
                var localizedUnitName = GameObjectHelper.GetLocalized(unitName);

                spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                    localizedUnitName,
                    textPosition + new Vector2(5, 65 + monsterIndex * 10), Color.Wheat);

                monsterIndex++;
            }
        }

        private void DrawEquipmentRewards(SpriteBatch spriteBatch,
            GlobeNodeGameObject nodeGameObject, Vector2 toolTipPosition)
        {
            var equipmentType = nodeGameObject.GlobeNode.EquipmentItem;
            if (equipmentType is null)
            {
                return;
            }

            var targetUnitScheme =
                UnsortedHelpers.GetPlayerPersonSchemeByEquipmentType(_unitSchemeCatalog, equipmentType);

            var playerUnit = _globe.Player.GetAll().SingleOrDefault(x => x.UnitScheme == targetUnitScheme);

            if (playerUnit is null)
            {
                return;
            }

            var equipmentTypeText = GameObjectHelper.GetLocalized(equipmentType);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), equipmentTypeText,
                toolTipPosition + new Vector2(5, 45), Color.Wheat);
        }

        private void DrawHeroGroupInfo(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            const int MARGIN = 5;

            var playerPartyUnits = _globe.Player.Party.GetUnits().ToArray();
            for (var unitIndex = 0; unitIndex < playerPartyUnits.Length; unitIndex++)
            {
                var unit = playerPartyUnits[unitIndex];
                var unitName = unit.UnitScheme.Name;
                var name = GameObjectHelper.GetLocalized(unitName);
                var position = new Vector2(contentRect.Left + unitIndex * (100 + MARGIN), contentRect.Top);
                spriteBatch.DrawString(_uiContentStorage.GetMainFont(), name, position, Color.Wheat);
            }
        }

        private void DrawLocationImage(SpriteBatch spriteBatch, Rectangle imageRect)
        {
            //throw new NotImplementedException();
        }

        private void DrawSummaryXpAwardLabel(SpriteBatch spriteBatch, GlobeNodeGameObject node, Vector2 toolTipPosition)
        {
            var totalXpForMonsters = node.GlobeNode.CombatSequence.Combats.SelectMany(x => x.EnemyGroup.GetUnits())
                .Sum(x => x.XpReward);
            var combatCount = node.GlobeNode.CombatSequence.Combats.Count;
            var summaryXp =
                (int)Math.Round(totalXpForMonsters * BiomeScreenTextHelper.GetCombatSequenceSizeBonus(combatCount));
            spriteBatch.DrawString(
                _uiContentStorage.GetMainFont(),
                $"Xp: {summaryXp}",
                toolTipPosition,
                Color.Wheat);
        }
    }
}