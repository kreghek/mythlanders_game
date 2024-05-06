using System;
using System.Collections.Generic;

using Client.Core.Campaigns;
using Client.Engine;
using Client.GameScreens.Campaign;
using Client.ScreenManagement;

using Core.Minigames.Towers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.TowersMinigame;

internal class TowersMinigameScreen : GameScreenWithMenuBase
{
    private readonly IList<ButtonBase> _barButtonList;
    private readonly HeroCampaign _campaign;
    private readonly Texture2D _textures;
    private readonly TowersEngine _towersEngine;
    private TowerRing? _activeRing;

    public TowersMinigameScreen(MythlandersGame game, TowersMiniGameScreenTransitionArguments args) : base(game)
    {
        _campaign = args.Campaign;

        _towersEngine = new TowersEngine(new[]
        {
            new[] { 3, 2, 1 },
            new int[0],
            new int[0] // target bar
        });

        _barButtonList = new List<ButtonBase>();

        _textures = game.Content.Load<Texture2D>("Sprites/GameObjects/TowersMinigame");
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
        ResolutionIndependentRenderer.BeginDraw();
        spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: Camera.GetViewTransformationMatrix());

        var towersContentRect = new Rectangle(contentRect.Left + 10, contentRect.Top + 10, contentRect.Width - 20,
            contentRect.Height - 20);

        for (var barIndex = 0; barIndex < _barButtonList.Count; barIndex++)
        {
            spriteBatch.Draw(_textures,
                new Rectangle(towersContentRect.Left + barIndex * 100 + 50, towersContentRect.Top + 10, 10,
                    towersContentRect.Height - 20 - 10 - 10), new Rectangle(0, 0, 24, 128), Color.White);

            var button = _barButtonList[barIndex];

            button.Rect = new Rectangle(
                towersContentRect.Left + barIndex * 100,
                towersContentRect.Bottom - 20 - 10,
                100,
                20);
            button.Draw(spriteBatch);
        }

        spriteBatch.End();
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

    protected override void UpdateContent(GameTime gameTime)
    {
        base.UpdateContent(gameTime);

        foreach (var button in _barButtonList)
        {
            button.Update(ResolutionIndependentRenderer);
        }
    }

    private void CloseButton_OnClick(object? sender, EventArgs e)
    {
        //_campaign.CompleteCurrentStage();

        ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign,
            new CampaignScreenTransitionArguments(_campaign));
    }
}