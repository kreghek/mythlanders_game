using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.CharacterDetails.Ui;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.CharacterDetails
{
    internal sealed class CharacterDetailsScreen : GameScreenWithMenuBase
    {
        private const int GRID_CELL_MARGIN = 5;
        private readonly IList<ButtonBase> _buttonList;
        private readonly GeneralInfoPanel _generalInfoPanel;
        private readonly PerkInfoPanel _perkInfoPanel;
        private readonly SkillsInfoPanel _skillsInfoPanel;
        private readonly IUiContentStorage _uiContentStorage;
        private readonly UnitGraphics _unitGraphics;

        public CharacterDetailsScreen(EwarGame game) : base(game)
        {
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            var gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
            var screenService = game.Services.GetService<ScreenService>();

            _buttonList = new List<ButtonBase>();

            var globeProvider = game.Services.GetService<GlobeProvider>();

            _generalInfoPanel = new GeneralInfoPanel(_uiContentStorage.GetPanelTexture(), _uiContentStorage.GetTitlesFont(), screenService.Selected,
                _uiContentStorage.GetMainFont());
            _skillsInfoPanel = new SkillsInfoPanel(_uiContentStorage.GetPanelTexture(), _uiContentStorage.GetTitlesFont(), screenService.Selected,
                _uiContentStorage.GetMainFont());
            _perkInfoPanel = new PerkInfoPanel(_uiContentStorage.GetPanelTexture(), _uiContentStorage.GetTitlesFont(), screenService.Selected,
                _uiContentStorage.GetMainFont());

            _unitGraphics = new UnitGraphics(screenService.Selected, new Vector2(), gameObjectContentStorage);

            InitActionButtons(screenService.Selected, globeProvider.Globe.Player);
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

        protected override void UpdateContent(GameTime gameTime)
        {
            base.UpdateContent(gameTime);

            foreach (var button in _buttonList.ToArray())
            {
                button.Update(ResolutionIndependentRenderer);
            }

            _unitGraphics.Update(gameTime);
        }

        private void DrawActionButtons(SpriteBatch spriteBatch, Rectangle actionButtonRect)
        {
            for (var buttonIndex = 0; buttonIndex < _buttonList.Count; buttonIndex++)
            {
                const int BUTTON_WIDTH = 100;
                const int BUTTON_HEIGHT = 20;

                var button = _buttonList[buttonIndex];
                const int BUTTON_MARGIN = GRID_CELL_MARGIN;
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
            var size = new Point(gridColumnWidth - GRID_CELL_MARGIN, gridRowHeight - GRID_CELL_MARGIN);
            return new Rectangle(position, size);
        }

        private void InitActionButtons(Unit character, Player player)
        {
            _buttonList.Clear();

            InitUpgradeButtons(character, player);

            var slotButton = new ResourceTextButton(nameof(UiResource.FormationButtonTitle),
                _uiContentStorage.GetButtonTexture(),
                _uiContentStorage.GetMainFont());

            _buttonList.Add(slotButton);

            slotButton.OnClick += (_, _) =>
            {
                var formationModal =
                    new FormationModal(_uiContentStorage, character, player, ResolutionIndependentRenderer);
                AddModal(formationModal, isLate: false);
            };
        }

        private void InitUpgradeButtons(Unit character, Player player)
        {
            var xpAmount = player.Inventory.Single(x => x.Type == EquipmentItemType.ExpiriencePoints).Amount;
            if (xpAmount >= character.LevelUpXpAmount)
            {
                var levelUpButton = new ResourceTextButton(nameof(UiResource.LevelUpButtonTitle), _uiContentStorage.GetButtonTexture(),
                    _uiContentStorage.GetMainFont(), Rectangle.Empty);

                levelUpButton.OnClick += (_, _) =>
                {
                    player.Inventory.Single(x => x.Type == EquipmentItemType.ExpiriencePoints).Amount -=
                        character.LevelUpXpAmount;
                    character.LevelUp();
                    InitActionButtons(character, player);
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
                        InitActionButtons(character, player);
                    };
                    _buttonList.Add(levelUpButton);
                }
            }
        }
    }
}