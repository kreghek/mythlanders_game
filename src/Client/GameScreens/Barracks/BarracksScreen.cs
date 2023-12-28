using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs;
using Client.Assets.CombatMovements;
using Client.Core;
using Client.Engine;
using Client.GameScreens.Barracks.Ui;
using Client.ScreenManagement;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Barracks;

internal class BarracksScreen : GameScreenWithMenuBase
{
    private const int GRID_CELL_MARGIN = 5;
    private readonly ICombatMovementVisualizationProvider _combatMovementVisualizer;

    private readonly GlobeProvider _globeProvider;
    private readonly IUiContentStorage _uiContentStorage;

    private EquipmentsInfoPanel _equipmentPanel = null!;

    private IReadOnlyCollection<HeroListItem> _heroList = null!;
    private PerkInfoPanel _perkInfoPanel = null!;
    private HeroState? _selectedHero;
    private SkillsInfoPanel _skillsInfoPanel = null!;
    private StatsInfoPanel _statsInfoPanel = null!;

    public BarracksScreen(TestamentGame game) : base(game)
    {
        _globeProvider = game.Services.GetRequiredService<GlobeProvider>();

        _uiContentStorage = game.Services.GetRequiredService<IUiContentStorage>();

        _combatMovementVisualizer = game.Services.GetRequiredService<ICombatMovementVisualizationProvider>();
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        var backButton = new ResourceTextButton(nameof(UiResource.BackButtonTitle));

        backButton.OnClick += (_, _) =>
        {
            //TODO Pass available campaigns back
            ScreenManager.ExecuteTransition(this, ScreenTransition.CommandCenter, null!);
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

        var heroListRect = new Rectangle(contentRect.Left, contentRect.Top, contentRect.Width, 50);
        DrawHeroList(spriteBatch, heroListRect);

        if (_selectedHero is not null)
        {
            DrawHeroDetails(spriteBatch, contentRect);
        }

        spriteBatch.End();
    }

    protected override void InitializeContent()
    {
        var catalog = Game.Services.GetRequiredService<ICombatantGraphicsCatalog>();
        var uiContentStorage = Game.Services.GetRequiredService<IUiContentStorage>();
        _heroList = _globeProvider.Globe.Player.Heroes.Units.Select(x =>
            new HeroListItem(x, catalog, Game.Content, uiContentStorage.GetMainFont())).ToArray();

        foreach (var heroListItem in _heroList)
        {
            heroListItem.OnClick += HeroListItem_OnClick;
        }
    }

    protected override void UpdateContent(GameTime gameTime)
    {
        base.UpdateContent(gameTime);

        foreach (var heroListItem in _heroList)
        {
            heroListItem.Update(ResolutionIndependentRenderer);
        }
    }

    private void DrawHeroDetails(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        _statsInfoPanel.Rect = GetCellRect(contentRect, col: 1, row: 0);
        _statsInfoPanel.Draw(spriteBatch);

        _skillsInfoPanel.Rect = GetCellRect(contentRect, col: 2, row: 0);
        _skillsInfoPanel.Draw(spriteBatch);

        _equipmentPanel.Rect = GetCellRect(contentRect, col: 0, row: 1);
        _equipmentPanel.Draw(spriteBatch);

        _perkInfoPanel.Rect = GetCellRect(contentRect, col: 2, row: 1);
        _perkInfoPanel.Draw(spriteBatch);
    }

    private void DrawHeroList(SpriteBatch spriteBatch, Rectangle heroListRect)
    {
        for (var index = 0; index < _heroList.ToArray().Length; index++)
        {
            var heroListItem = _heroList.ToArray()[index];
            heroListItem.Rect = new Rectangle(heroListRect.X + index * 100, heroListRect.Top, 100, 32);
            heroListItem.Draw(spriteBatch);
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

    private void HeroListItem_OnClick(object? sender, EventArgs e)
    {
        var item = (HeroListItem?)sender;

        if (item is null)
        {
            return;
        }

        _selectedHero = item.Hero;
        _statsInfoPanel = new StatsInfoPanel(_selectedHero);
        _skillsInfoPanel = new SkillsInfoPanel(_selectedHero, _uiContentStorage.GetMainFont(),
            _combatMovementVisualizer, _uiContentStorage);
        _perkInfoPanel = new PerkInfoPanel(_selectedHero);
        _equipmentPanel = new EquipmentsInfoPanel(_selectedHero, _uiContentStorage.GetEquipmentTextures(),
            ResolutionIndependentRenderer);
    }
}