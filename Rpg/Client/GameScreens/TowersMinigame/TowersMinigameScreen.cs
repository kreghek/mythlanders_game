using System;
using System.Collections.Generic;

using Core.Minigames.Towers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client;
using Rpg.Client.Core.Campaigns;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens.Campaign;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.TowersMinigame;

internal class TowersMinigameScreen : GameScreenWithMenuBase
{
    private readonly HeroCampaign _campaign;
    private readonly TowersEngine _towersEngine;
    private TowerRing? _activeRing;

    private readonly IList<ButtonBase> _barButtonList;

    public TowersMinigameScreen(EwarGame game, TowersMinigameScreenTransitionArgs args) : base(game)
    {
        _campaign = args.Campaign;

        _towersEngine = new TowersEngine(new[] {
            new []{ 3, 2, 1 },
            new int[0],
            new int[0] // target bar
        });

        _barButtonList = new List<ButtonBase>();
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        var closeButton = new TextButton("Skip");
        closeButton.OnClick += CloseButton_OnClick;

        return new[]
        {
            closeButton
        };
    }

    protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        var towersContentRect = new Rectangle(contentRect.Left + 10, contentRect.Top + 10, contentRect.Width - 20, contentRect.Height - 20); 

        for (var barIndex = 0; barIndex < _barButtonList.Count; barIndex++)
        {
            var button = _barButtonList[barIndex];

            button.Rect = new Rectangle(
                towersContentRect.Left + barIndex * 100,
                towersContentRect.Top + contentRect.Bottom - 20,
                100,
                20);
            button.Draw(spriteBatch);
        }
    }

    protected override void UpdateContent(GameTime gameTime)
    {
        base.UpdateContent(gameTime);

        foreach (var button in _barButtonList)
        {
            button.Update(ResolutionIndependentRenderer);
        }
    }

    protected override void InitializeContent()
    {
        for (var barIndex = 0; barIndex < _towersEngine.Bars.Count; barIndex++)
        {
            var bar = _towersEngine.Bars[barIndex];
            var barButton = new TextButton("");
            barButton.OnClick += (s, e) =>
            {
                if (_activeRing is null)
                {
                    _activeRing = bar.PullOf();
                }
                else
                {
                    bar.PutOn(_activeRing);
                    _activeRing = null;
                }
            };

            _barButtonList.Add(barButton);
        }
    }

    private void CloseButton_OnClick(object? sender, EventArgs e)
    {
        _campaign.CompleteCurrentStage();

        ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign, new CampaignScreenTransitionArguments
        {
            Campaign = _campaign
        });
    }
}
