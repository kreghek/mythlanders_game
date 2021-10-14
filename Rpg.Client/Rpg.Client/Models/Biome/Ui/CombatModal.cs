﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.Models.Biome.GameObjects;

namespace Rpg.Client.Models.Biome.Ui
{
    internal sealed class CombatModalContext
    { 
        public Globe Globe { get; set; }
        public GlobeNodeGameObject SelectedNodeGameObject { get; set; }
        public Action<GlobeNode> CombatDelegate { get; set; }
        public Action<GlobeNode> AutoCombatDelegate { get; set; }
    }

    internal sealed class CombatModal : ModalDialogBase
    {
        private readonly Globe _globe;
        private readonly GlobeNodeGameObject _nodeGameObject;
        private readonly IUiContentStorage _uiContentStorage;

        private readonly IList<ButtonBase> _buttons;

        public CombatModal(CombatModalContext context, IUiContentStorage uiContentStorage, GraphicsDevice graphicsDevice) : base(uiContentStorage, graphicsDevice)
        {
            _globe = context.Globe;
            _nodeGameObject = context.SelectedNodeGameObject;
            _uiContentStorage = uiContentStorage;

            _buttons = new List<ButtonBase>();

            var combatButton = new TextButton(UiResource.ToTheCombatButtonTitle, _uiContentStorage.GetButtonTexture(), _uiContentStorage.GetMainFont(), Rectangle.Empty);
            combatButton.OnClick += (s, e) =>
            {
                context.CombatDelegate(context.SelectedNodeGameObject.GlobeNode);
            };
            _buttons.Add(combatButton);

            var autocombatButton = new TextButton(UiResource.AutocombatButtonTitle, _uiContentStorage.GetButtonTexture(), _uiContentStorage.GetMainFont(), Rectangle.Empty);
            autocombatButton.OnClick += (s, e) =>
            {
                context.AutoCombatDelegate(context.SelectedNodeGameObject.GlobeNode);
            };
            _buttons.Add(autocombatButton);
        }

        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            var textPosition = ContentRect.Location.ToVector2() + new Vector2(0, 16);

            var node = _nodeGameObject;

            var rm = UiResource.ResourceManager;

            var localizedName = rm.GetString($"{node.GlobeNode.Sid}NodeName");
            var normalizedName = localizedName ?? node.GlobeNode.Sid.ToString();

            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), normalizedName,
                textPosition + new Vector2(5, 15),
                Color.Wheat);

            var dialogMarkerText = node.AvailableDialog is not null ? $"(!) {node.AvailableDialog.Sid}" : string.Empty;
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), dialogMarkerText,
                textPosition + new Vector2(5, 25), Color.Wheat);

            var combatSequenceSizeText = GetCombatSequenceSizeText(node);
            spriteBatch.DrawString(_uiContentStorage.GetMainFont(), combatSequenceSizeText,
                textPosition + new Vector2(5, 35), Color.Wheat);

            DisplayCombatRewards(spriteBatch, node, textPosition, node);

            if (node.GlobeNode.CombatSequence is not null)
            {
                var monsterIndex = 0;
                var roundIndex = 1;

                foreach (var combat in node.GlobeNode.CombatSequence.Combats)
                {
                    foreach (var monster in node.Combat.EnemyGroup.Units)
                    {
                        spriteBatch.DrawString(_uiContentStorage.GetMainFont(),
                            $"(rnd {roundIndex}) {monster.UnitScheme.Name} (lvl{monster.Level})",
                            textPosition + new Vector2(5, 65 + monsterIndex * 10), Color.Wheat);

                        monsterIndex++;
                    }

                    roundIndex++;
                }
            }

            var sumButtonWidth = _buttons.Count * (100 + 5);
            var startXPosition = ContentRect.Center.X - sumButtonWidth / 2;
            for (var buttonIndex = 0; buttonIndex < _buttons.Count; buttonIndex++)
            {
                var button = _buttons[buttonIndex];
                button.Rect = new Rectangle(startXPosition + buttonIndex * (100 + 5), ContentRect.Bottom - (20 + 5), 100, 20);
                button.Draw(spriteBatch);
            }
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            base.UpdateContent(gameTime);

            foreach (var button in _buttons)
            {
                button.Update();
            }
        }

        private static string GetCombatSequenceSizeText(GlobeNodeGameObject node)
        {
            var count = node.GlobeNode.CombatSequence.Combats.Count;
            switch (count)
            {
                case 1:
                    return "Short";

                case 3:
                    return "Medium (+25% XP)";

                case 5:
                    return "Long (+50% XP)";

                default:
                    Debug.Fail("Unknown size");
                    return string.Empty;
            }
        }

        private static string? GetDisplayNameOfEquipment(EquipmentItemType? equipmentType)
        {
            if (equipmentType is null)
            {
                return null;
            }

            var rm = UiResource.ResourceManager;

            var equipmentDisplayName = rm.GetString($"{equipmentType}EquipmentItemDisplayName");

            if (equipmentDisplayName is null)
            {
                return $"{equipmentType} equipment items";
            }

            return equipmentDisplayName;
        }

        private void DisplayCombatRewards(SpriteBatch spriteBatch, GlobeNodeGameObject nodeGameObject,
            Vector2 toolTipPosition, GlobeNodeGameObject node)
        {
            if (node.GlobeNode.CombatSequence is null)
            {
                // No combat - no rewards
                return;
            }

            // TODO Display icons

            DrawSummaryXpAwardLabel(spriteBatch, node, toolTipPosition + new Vector2(5, 55));

            var equipmentType = nodeGameObject.GlobeNode.EquipmentItem;
            if (equipmentType is not null)
            {
                var targetUnitScheme = UnsortedHelpers.GetPlayerPersonSchemeByEquipmentType(equipmentType);

                var playerUnit = _globe.Player.GetAll.Where(x => x != null)
                    .SingleOrDefault(x => x.UnitScheme == targetUnitScheme);

                if (playerUnit is not null)
                {
                    var equipmentTypeText = GetDisplayNameOfEquipment(equipmentType);
                    spriteBatch.DrawString(_uiContentStorage.GetMainFont(), equipmentTypeText,
                        toolTipPosition + new Vector2(5, 45), Color.Wheat);
                }
            }
        }

        private void DrawSummaryXpAwardLabel(SpriteBatch spriteBatch, GlobeNodeGameObject node, Vector2 toolTipPosition)
        {
            var monstersAmount = node.Combat.EnemyGroup.Units.Count();
            var roundsAmount = node.GlobeNode.CombatSequence.Combats.Count;
            var summaryXpLabelPosition = toolTipPosition;

            var totalXpForMonsters = node.Combat.EnemyGroup.Units.Sum(x => x.XpReward);
            var summaryXp = (int)Math.Round(totalXpForMonsters * GetCombatSequenceSizeBonus(node));
            spriteBatch.DrawString(
                _uiContentStorage.GetMainFont(),
                $"Xp Reward: {summaryXp}",
                summaryXpLabelPosition,
                Color.Wheat);
        }

        private static float GetCombatSequenceSizeBonus(GlobeNodeGameObject node)
        {
            var count = node.GlobeNode.CombatSequence.Combats.Count;
            switch (count)
            {
                case 1:
                    return 1;

                case 3:
                    return 1.25f;

                case 5:
                    return 1.5f;

                default:
                    Debug.Fail("Unknown size");
                    return 1;
            }
        }
    }
}
