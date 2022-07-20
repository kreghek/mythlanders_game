using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Map.GameObjects;

namespace Rpg.Client.GameScreens.Map.Ui
{
    internal sealed class LocationInfoModal : ModalDialogBase
    {
        private readonly IList<ButtonBase> _buttons;
        private readonly Globe _globe;
        private readonly GlobeNodeMarkerGameObject _nodeGameObject;
        private readonly IUiContentStorage _uiContentStorage;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;

        public LocationInfoModal(LocationInfoModalContext context, IUiContentStorage uiContentStorage,
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
                context.CombatDelegate(context.SelectedNodeGameObject.GlobeNode, context.AvailableEvent);
            };
            _buttons.Add(combatButton);

            AddAutoCombatButtonIfAvailable(context);
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            if (_nodeGameObject.GlobeNode.AssignedCombats is null)
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

        private void AddAutoCombatButtonIfAvailable(LocationInfoModalContext context)
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
                    context.AutoCombatDelegate(context.SelectedNodeGameObject.GlobeNode, context.AvailableEvent);
                };
                _buttons.Add(autocombatButton);
            }
        }

        private bool CheckAllSlavicLocationUnlocked()
        {
            return _globe.Player.Abilities.Contains(PlayerAbility.AvailableAutocombats);
        }

        private void DisplayCombatRewards(SpriteBatch spriteBatch, GlobeNodeMarkerGameObject nodeGameObject,
            Vector2 rewardBlockPosition, GlobeNodeMarkerGameObject node)
        {
            if (node.GlobeNode.AssignedCombats is null)
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

            var dialogueMarkerText = GetLocalizedEventTitle() ?? String.Empty;
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), dialogueMarkerText,
                textPosition + new Vector2(5, 25), Color.Wheat);

            var combatCount = _nodeGameObject.GlobeNode.AssignedCombats.Combats.Count;
            var combatSequenceSizeText = BiomeScreenTextHelper.GetCombatSequenceSizeText(combatCount);
            spriteBatch.DrawString(
                _uiContentStorage.GetMainFont(),
                $"{UiResource.CombatLengthLabel} {combatSequenceSizeText}",
                textPosition + new Vector2(5, 35), Color.Wheat);

            DisplayCombatRewards(spriteBatch, _nodeGameObject, textPosition, _nodeGameObject);

            var monsterIndex = 0;

            var uniqueMonsterSchemes = _nodeGameObject.GlobeNode.AssignedCombats.Combats
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

        private string? GetLocalizedEventTitle()
        {
            if (_nodeGameObject.AvailableEvent?.Sid is null)
            {
                return String.Empty;
            }

            return _nodeGameObject.AvailableEvent is not null
                            ? StoryResources.ResourceManager.GetString(_nodeGameObject.AvailableEvent.Sid)
                            : string.Empty;
        }

        private void DrawEquipmentRewards(SpriteBatch spriteBatch,
            GlobeNodeMarkerGameObject nodeGameObject, Vector2 toolTipPosition)
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

        private void DrawSummaryXpAwardLabel(SpriteBatch spriteBatch, GlobeNodeMarkerGameObject node,
            Vector2 toolTipPosition)
        {
            var totalXpForMonsters = node.GlobeNode.AssignedCombats.Combats.SelectMany(x => x.EnemyGroup.GetUnits())
                .Sum(x => x.XpReward);
            var combatCount = node.GlobeNode.AssignedCombats.Combats.Count;
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