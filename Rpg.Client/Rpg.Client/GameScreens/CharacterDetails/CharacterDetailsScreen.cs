using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.CharacterDetails
{
    internal sealed class CharacterDetailsScreen : GameScreenWithMenuBase
    {
        private readonly IList<ButtonBase> _buttonList;
        private readonly GlobeProvider _globeProvider;
        private readonly ScreenService _screenService;
        private readonly IUiContentStorage _uiContentStorage;
        private readonly GeneralInfoPanel _generalInfoPanel;
        private readonly SkillsInfoPanel _skillsInfoPanel;
        private readonly PerkInfoPanel _perkInfoPanel;
        private readonly UnitGraphics _unitGraphics;

        public CharacterDetailsScreen(EwarGame game) : base(game)
        {
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            var gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
            _screenService = game.Services.GetService<ScreenService>();

            _buttonList = new List<ButtonBase>();

            _globeProvider = game.Services.GetService<GlobeProvider>();

            _generalInfoPanel = new GeneralInfoPanel(_uiContentStorage.GetPanelTexture(), _screenService.Selected,
                _uiContentStorage.GetMainFont());
            _skillsInfoPanel = new SkillsInfoPanel(_uiContentStorage.GetPanelTexture(), _screenService.Selected,
                _uiContentStorage.GetMainFont());
            _perkInfoPanel = new PerkInfoPanel(_uiContentStorage.GetPanelTexture(), _screenService.Selected,
                _uiContentStorage.GetMainFont());

            _unitGraphics = new UnitGraphics(_screenService.Selected, new Vector2(), gameObjectContentStorage);

            InitSlotAssignmentButtons(_screenService.Selected, _globeProvider.Globe.Player);
        }

        protected override IList<ButtonBase> CreateMenu()
        {
            var backButton = new ResourceTextButton(nameof(UiResource.BackButtonTitle),
                _uiContentStorage.GetButtonTexture(), _uiContentStorage.GetMainFont());
            backButton.OnClick += (_, _) =>
            {
                ScreenManager.ExecuteTransition(this, ScreenTransition.Party);
            };

            return new ButtonBase[] { backButton };
        }

        protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
        {
            ResolutionIndependentRenderer.BeginDraw();
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: Camera.GetViewTransformationMatrix());

            var unitGraphicsRect = GetCellRect(contentRect, col: 0, row: 0);
            _unitGraphics.Root.Position = unitGraphicsRect.Center.ToVector2();
            _unitGraphics.Draw(spriteBatch);

            _generalInfoPanel.Rect = GetCellRect(contentRect, col: 1, row: 0);
            _generalInfoPanel.Draw(spriteBatch);
            
            _skillsInfoPanel.Rect = GetCellRect(contentRect, col: 2, row: 0);
            _skillsInfoPanel.Draw(spriteBatch);
            
            _perkInfoPanel.Rect = GetCellRect(contentRect, col: 2, row: 1);
            _perkInfoPanel.Draw(spriteBatch);

            var actionButtonRect = GetCellRect(contentRect, col: 1, row: 1);
            DrawActionButtons(spriteBatch: spriteBatch, actionButtonRect: actionButtonRect);

            spriteBatch.End();
        }

        private void DrawActionButtons(SpriteBatch spriteBatch, Rectangle actionButtonRect)
        {
            for (var buttonIndex = 0; buttonIndex < _buttonList.Count; buttonIndex++)
            {
                const int BUTTON_WIDTH = 100;
                const int BUTTON_HEIGHT = 20;

                var button = _buttonList[buttonIndex];
                const int BUTTON_MARGIN = 5;
                var offset = new Point(0, (BUTTON_HEIGHT + BUTTON_MARGIN) * buttonIndex);
                var panelLocation = new Point(actionButtonRect.Center.X - BUTTON_WIDTH / 2, actionButtonRect.Top);
                var buttonSize = new Point(BUTTON_WIDTH, BUTTON_HEIGHT);

                button.Rect = new Rectangle(panelLocation + offset, buttonSize);
                button.Draw(spriteBatch);
            }
        }

        private static Rectangle GetCellRect(Rectangle contentRect, int col, int row)
        {
            var gridColumnWidth = contentRect.Width / 3;
            var gridRowHeight = contentRect.Height / 2;
            var position = new Point(contentRect.Left + gridColumnWidth * col, contentRect.Top + gridRowHeight * row);
            var size = new Point(gridColumnWidth, gridRowHeight);
            return new Rectangle(position, size);
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            base.UpdateContent(gameTime);

            foreach (var button in _buttonList.ToArray())
            {
                button.Update(ResolutionIndependentRenderer);
            }
            
            _unitGraphics.Update(gameTime);
        }

        private IEnumerable<GroupSlot> GetAvailableSlots(IEnumerable<GroupSlot> freeSlots)
        {
            if (_globeProvider.Globe.Player.Abilities.Contains(PlayerAbility.AvailableTanks))
            {
                return freeSlots;
            }

            // In the first biome the player can use only first 3 slots.
            // There is no ability to split characters on tank line and dd+support.
            return freeSlots.Where(x => !x.IsTankLine);
        }

        private bool GetIsCharacterInGroup(Unit selectedCharacter)
        {
            return _globeProvider.Globe.Player.Party.GetUnits().Contains(selectedCharacter);
        }

        private void InitSlotAssignmentButtons(Unit character, Player player)
        {
            _buttonList.Clear();

            var isCharacterInGroup = GetIsCharacterInGroup(character);
            if (isCharacterInGroup)
            {
                var reserveButton = new ResourceTextButton(
                    nameof(UiResource.MoveToThePoolButtonTitle),
                    _uiContentStorage.GetButtonTexture(),
                    _uiContentStorage.GetMainFont(),
                    Rectangle.Empty);
                _buttonList.Add(reserveButton);

                reserveButton.OnClick += (_, _) =>
                {
                    player.MoveToPool(character);

                    InitSlotAssignmentButtons(character, player);
                };
            }
            else
            {
                var freeSlots = player.Party.GetFreeSlots();
                var availableSlots = GetAvailableSlots(freeSlots);
                foreach (var slot in availableSlots)
                {
                    var slotButton = new TextButton(slot.Index.ToString(),
                        _uiContentStorage.GetButtonTexture(),
                        _uiContentStorage.GetMainFont(),
                        Rectangle.Empty);

                    _buttonList.Add(slotButton);

                    slotButton.OnClick += (_, _) =>
                    {
                        player.MoveToParty(character, slot.Index);

                        InitSlotAssignmentButtons(character, player);
                    };
                }
            }

            InitUpgradeButtons(character, player);
        }

        private void InitUpgradeButtons(Unit character, Player player)
        {
            var xpAmount = player.Inventory.Single(x => x.Type == EquipmentItemType.ExpiriencePoints).Amount;
            if (xpAmount >= character.LevelUpXpAmount)
            {
                var levelUpButton = new TextButton("Level Up", _uiContentStorage.GetButtonTexture(),
                    _uiContentStorage.GetMainFont(), Rectangle.Empty);

                levelUpButton.OnClick += (_, _) =>
                {
                    player.Inventory.Single(x => x.Type == EquipmentItemType.ExpiriencePoints).Amount -=
                        character.LevelUpXpAmount;
                    character.LevelUp();
                    InitSlotAssignmentButtons(character, player);
                };

                _buttonList.Add(levelUpButton);
            }

            foreach (var equipment in character.Equipments)
            {
                var resourceItem = player.Inventory.Single(x => x.Type == equipment.Scheme.RequiredResourceToLevelUp);
                var equipmentResourceAmount = resourceItem.Amount;
                if (equipmentResourceAmount >= equipment.RequiredResourceAmountToLevelUp)
                {
                    var levelUpButton = new TextButton($"Upgrade {equipment.Scheme.Sid} to level {equipment.Level + 1}",
                        _uiContentStorage.GetButtonTexture(),
                        _uiContentStorage.GetMainFont(), Rectangle.Empty);
                    levelUpButton.OnClick += (_, _) =>
                    {
                        resourceItem.Amount -= equipment.RequiredResourceAmountToLevelUp;
                        equipment.LevelUp();
                        InitSlotAssignmentButtons(character, player);
                    };
                    _buttonList.Add(levelUpButton);
                }
            }
        }
    }
}