using System.Collections.Generic;
using System.Linq;

using Client;
using Client.Core;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Hero.Ui;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Hero
{
    internal sealed class HeroScreen : GameScreenWithMenuBase
    {
        private const int GRID_CELL_MARGIN = 5;
        private readonly IList<ButtonBase> _buttonList;
        private readonly Unit _hero;
        private readonly Player _player;
        private readonly IUiContentStorage _uiContentStorage;
        private readonly UnitGraphics _unitGraphics;

        private EquipmentsInfoPanel _equipmentPanel = null!;
        private GeneralInfoPanel _generalInfoPanel = null!;
        private PerkInfoPanel _perkInfoPanel = null!;
        private SkillsInfoPanel _skillsInfoPanel = null!;

        public HeroScreen(TestamentGame game) : base(game)
        {
            _uiContentStorage = game.Services.GetService<IUiContentStorage>();
            var gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
            var screenService = game.Services.GetService<ScreenService>();
            var globeProvider = game.Services.GetService<GlobeProvider>();

            _buttonList = new List<ButtonBase>();

            _hero = screenService.Selected;

            _player = globeProvider.Globe.Player;

            //_unitGraphics = new UnitGraphics(_hero,
            //    new Vector2(),
            //    gameObjectContentStorage);

            InitContent();
        }

        protected override IList<ButtonBase> CreateMenu()
        {
            var backButton = new ResourceTextButton(nameof(UiResource.BackButtonTitle));
            backButton.OnClick += (_, _) =>
            {
                ScreenManager.ExecuteTransition(this, ScreenTransition.Party, null);
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

            _equipmentPanel.Rect = GetCellRect(contentRect, col: 0, row: 1);
            _equipmentPanel.Draw(spriteBatch);

            _perkInfoPanel.Rect = GetCellRect(contentRect, col: 2, row: 1);
            _perkInfoPanel.Draw(spriteBatch);

            var actionButtonRect = GetCellRect(contentRect, col: 1, row: 1);
            DrawActionButtons(spriteBatch: spriteBatch, actionButtonRect: actionButtonRect);

            spriteBatch.End();
        }

        protected override void InitializeContent()
        {
        }

        protected override void UpdateContent(GameTime gameTime)
        {
            base.UpdateContent(gameTime);

            foreach (var button in _buttonList.ToArray())
            {
                button.Update(ResolutionIndependentRenderer);
            }

            _unitGraphics.Update(gameTime);

            _equipmentPanel.Update(gameTime);
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

            var slotButton = new ResourceTextButton(nameof(UiResource.FormationButtonTitle));

            _buttonList.Add(slotButton);

            slotButton.OnClick += (_, _) =>
            {
                var formationModal =
                    new FormationModal(_uiContentStorage, character, player, ResolutionIndependentRenderer);
                AddModal(formationModal, isLate: false);
            };
        }

        private void InitContent()
        {
            _generalInfoPanel = new GeneralInfoPanel(_hero);

            _skillsInfoPanel = new SkillsInfoPanel(
                _hero,
                _uiContentStorage.GetCombatPowerIconsTexture(),
                _uiContentStorage.GetMainFont());

            _perkInfoPanel = new PerkInfoPanel(_hero);

            _equipmentPanel = new EquipmentsInfoPanel(
                _hero,
                _uiContentStorage.GetEquipmentTextures(),
                _player,
                ResolutionIndependentRenderer);

            InitActionButtons(_hero,
                _player);
        }

        private void InitUpgradeButtons(Unit character, Player player)
        {
            var xpAmount = player.Inventory.Single(x => x.Type == EquipmentItemType.ExperiencePoints).Amount;
            if (xpAmount >= character.LevelUpXpAmount)
            {
                var levelUpButton = new ResourceTextButton(nameof(UiResource.LevelUpButtonTitle));

                levelUpButton.OnClick += (_, _) =>
                {
                    player.Inventory.Single(x => x.Type == EquipmentItemType.ExperiencePoints).Amount -=
                        character.LevelUpXpAmount;
                    character.LevelUp();

                    InitContent();
                };

                _buttonList.Add(levelUpButton);
            }

            // foreach (var equipment in character.Equipments)
            // {
            //     var resourceItem = player.Inventory.Single(x => x.Type == equipment.Scheme.RequiredResourceToLevelUp);
            //     var equipmentResourceAmount = resourceItem.Amount;
            //     if (equipmentResourceAmount >= equipment.RequiredResourceAmountToLevelUp)
            //     {
            //         var levelUpButton = new TextButton($"Upgrade {equipment.Scheme.Sid} to level {equipment.Level + 1}",
            //             _uiContentStorage.GetButtonTexture(),
            //             _uiContentStorage.GetMainFont(), Rectangle.Empty);
            //         levelUpButton.OnClick += (_, _) =>
            //         {
            //             resourceItem.Amount -= equipment.RequiredResourceAmountToLevelUp;
            //             equipment.LevelUp();
            //             InitActionButtons(character, player);
            //         };
            //         _buttonList.Add(levelUpButton);
            //     }
            // }
        }
    }
}