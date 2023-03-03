using System;
using System.Collections.Generic;

using Client.Core.Campaigns;
using Client.GameScreens.Campaign;

using Core.Combats;
using Core.Dices;
using Core.Minigames.Match3;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.Match3;

internal class Match3MinigameScreen : GameScreenWithMenuBase
{
    private readonly HeroCampaign _campaign;
    private readonly Match3Engine _match3Engine;
    private readonly Matrix<ButtonBase> _gemButtonMatrix;

    public Match3MinigameScreen(TestamentGame game, Match3MiniGameScreenTransitionArguments args) : base(game)
    {
        _campaign = args.Campaign;
        var dice = game.Services.GetRequiredService<IDice>();

        var initialMatrix = new Matrix<GemColor>(8, 8);

        for (var colIndex = 0; colIndex < initialMatrix.Width; colIndex++)
        {
            for (var rowIndex = 0; rowIndex < initialMatrix.Height; rowIndex++)
            {
                initialMatrix[colIndex, rowIndex] = dice.RollFromList(new[] { GemColor.Green, GemColor.Red, GemColor.Blue });
            }
        }

        _match3Engine = new Match3Engine(initialMatrix, new GemSource());

        _gemButtonMatrix = new Matrix<ButtonBase>(8, 8);
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

        for (var colIndex = 0; colIndex < _gemButtonMatrix.Width; colIndex++)
        {
            for (var rowIndex = 0; rowIndex < _gemButtonMatrix.Height; rowIndex++)
            {
                var button = _gemButtonMatrix[colIndex, rowIndex];
                button.Rect = new Rectangle(colIndex * 20, rowIndex * 20, 20, 20);
                button.Draw(spriteBatch);
            }
        }

        spriteBatch.End();
    }

    protected override void UpdateContent(GameTime gameTime)
    {
        base.UpdateContent(gameTime);

        for (var colIndex = 0; colIndex < _gemButtonMatrix.Width; colIndex++)
        {
            for (var rowIndex = 0; rowIndex < _gemButtonMatrix.Height; rowIndex++)
            {
                var button = _gemButtonMatrix[colIndex, rowIndex];
                button.Update(ResolutionIndependentRenderer);
            }
        }
    }

    private Coords? _selectedFirstGem;

    protected override void InitializeContent()
    {
        for (var colIndex = 0; colIndex < _gemButtonMatrix.Width; colIndex++)
        {
            for (var rowIndex = 0; rowIndex < _gemButtonMatrix.Height; rowIndex++)
            {
                var coords = new Coords(colIndex, rowIndex);

                var button = new TextButton(((int)_match3Engine.Field[colIndex, rowIndex]).ToString());

                button.OnClick += (s, e) =>
                {
                    if (_selectedFirstGem is null)
                    {
                        _selectedFirstGem = coords;
                    }
                    else
                    {
                        _match3Engine.Swap(_selectedFirstGem, coords);
                        _selectedFirstGem = null;
                    }
                };

                _gemButtonMatrix[colIndex, rowIndex] = button;
            }
        }
    }

    private void CloseButton_OnClick(object? sender, EventArgs e)
    {
        _campaign.CompleteCurrentStage();

        ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign,
            new CampaignScreenTransitionArguments(_campaign));
    }
}
