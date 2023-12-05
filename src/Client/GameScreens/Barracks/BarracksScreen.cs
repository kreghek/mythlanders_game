using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs;
using Client.Core;
using Client.Engine;
using Client.GameScreens.Barracks.Ui;
using Client.ScreenManagement;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Barracks;

internal class BarracksScreen: GameScreenWithMenuBase
{
    private readonly GlobeProvider _globeProvider;
    private HeroState? _selectedHero;

    private IReadOnlyCollection<HeroListItem> _heroList = null!;

    public BarracksScreen(TestamentGame game) : base(game)
    {
        _globeProvider = game.Services.GetRequiredService<GlobeProvider>();
    }

    protected override void InitializeContent()
    {
        var catalog = Game.Services.GetRequiredService<ICombatantGraphicsCatalog>();
        var uiContentStorage =Game.Services.GetRequiredService<IUiContentStorage>();
        _heroList = _globeProvider.Globe.Player.Heroes.Select(x =>
            new HeroListItem(x, catalog, Game.Content, uiContentStorage.GetMainFont())).ToArray();
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        var backButton = new ResourceTextButton(nameof(UiResource.BackButtonTitle));

        backButton.OnClick += (_, _) =>
        {
            //TODO Pass available campaigns back
            ScreenManager.ExecuteTransition(this, ScreenTransition.CommandCenter, null);
        };

        return new ButtonBase[] { backButton };
    }

    protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        var heroes = _globeProvider.Globe.Player.Heroes;

        var heroListRect = new Rectangle(contentRect.Left, contentRect.Top, contentRect.Width, 50);
        DrawHeroList(heroes, spriteBatch, heroListRect);
    }

    private void DrawHeroList(IReadOnlyCollection<HeroState> heroes, SpriteBatch spriteBatch, Rectangle heroListRect)
    {
        for (var index = 0; index < _heroList.ToArray().Length; index++)
        {
            var heroListItem = _heroList.ToArray()[index];
            heroListItem.Rect = new Rectangle(heroListRect.X + index * 100, heroListRect.Top, 100, 32);
            heroListItem.Draw(spriteBatch);
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
}