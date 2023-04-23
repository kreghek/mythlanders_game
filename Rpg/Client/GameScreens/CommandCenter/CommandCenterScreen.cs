using System;
using System.Collections.Generic;

using Client.Core;
using Client.Core.Campaigns;
using Client.GameScreens.Campaign;
using Client.GameScreens.CommandCenter.Ui;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.CommandCenter;

internal class CommandCenterScreen : GameScreenWithMenuBase
{
    private readonly IReadOnlyList<HeroCampaign> _campaigns;

    private readonly ButtonBase[] _commandButtons = new ButtonBase[4];
    private readonly Texture2D[] _commandCenterSegmentTexture;

    private readonly Texture2D _mapBackgroundTexture;

    private IReadOnlyList<CampaignPanel>? _availableCampaignPanels;

    public CommandCenterScreen(TestamentGame game, CommandCenterScreenTransitionArguments args) : base(game)
    {
        _campaigns = args.AvailableCampaigns;

        _mapBackgroundTexture = Game.Content.Load<Texture2D>("Sprites/GameObjects/Map/Map");
        _commandCenterSegmentTexture = new[]
        {
            Game.Content.Load<Texture2D>("Sprites/GameObjects/CommandCenter/CommandCenter1"),
            Game.Content.Load<Texture2D>("Sprites/GameObjects/CommandCenter/CommandCenter2"),
            Game.Content.Load<Texture2D>("Sprites/GameObjects/CommandCenter/CommandCenter3"),
            Game.Content.Load<Texture2D>("Sprites/GameObjects/CommandCenter/CommandCenter4")
        };
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        return ArraySegment<ButtonBase>.Empty;
    }

    protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        if (_availableCampaignPanels is null)
        {
            throw new InvalidOperationException("Screen is not initialized");
        }

        ResolutionIndependentRenderer.BeginDraw();
        spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: Camera.GetViewTransformationMatrix());

        spriteBatch.Draw(_mapBackgroundTexture,
            new Rectangle(
                contentRect.Left + ControlBase.CONTENT_MARGIN,
                (contentRect.Top + (contentRect.Height / 8)) + ControlBase.CONTENT_MARGIN,
                contentRect.Width - ControlBase.CONTENT_MARGIN * 2,
                (contentRect.Height / 2) - ControlBase.CONTENT_MARGIN * 2),
            Color.White);

        const int CAMPAIGN_CONTROL_WIDTH = 200;
        var fullCampaignWidth = (CAMPAIGN_CONTROL_WIDTH + ControlBase.CONTENT_MARGIN) * 3;
        var campaignOffsetX = (contentRect.Width - fullCampaignWidth) / 2;

        for (var campaignIndex = 0; campaignIndex < _availableCampaignPanels.Count; campaignIndex++)
        {
            var panel = _availableCampaignPanels[campaignIndex];
            panel.Rect = new Rectangle(
                campaignOffsetX + contentRect.Left + ControlBase.CONTENT_MARGIN + 200 * campaignIndex,
                contentRect.Top + ControlBase.CONTENT_MARGIN,
                200,
                panel.Hover ? 200 : 100);
            panel.Draw(spriteBatch);
        }

        for (var i = 0; i < 4; i++)
        {
            spriteBatch.Draw(_commandCenterSegmentTexture[i],
                new Rectangle(
                    (contentRect.Left + ControlBase.CONTENT_MARGIN) + i * (200 + ControlBase.CONTENT_MARGIN),
                    (contentRect.Top + (contentRect.Height / 8)) + ControlBase.CONTENT_MARGIN +
                    (contentRect.Height / 2) - ControlBase.CONTENT_MARGIN * 2,
                    200,
                    200),
                Color.White);

            _commandButtons[i].Rect = new Rectangle(
                (contentRect.Left + ControlBase.CONTENT_MARGIN) + i * (200 + ControlBase.CONTENT_MARGIN),
                (contentRect.Top + (contentRect.Height / 8)) + ControlBase.CONTENT_MARGIN +
                (contentRect.Height / 2) - ControlBase.CONTENT_MARGIN * 2,
                100, 20);

            _commandButtons[i].Draw(spriteBatch);
        }

        spriteBatch.End();
    }

    protected override void InitializeContent()
    {
        var panels = new List<CampaignPanel>();

        var index = 0;

        var campaignTexturesDict = new Dictionary<ILocationSid, Texture2D>
        {
            { LocationSids.Desert, Game.Content.Load<Texture2D>("Sprites/GameObjects/Campaigns/Desert") },
            { LocationSids.Monastery, Game.Content.Load<Texture2D>("Sprites/GameObjects/Campaigns/Monastery") },
            {
                LocationSids.ShipGraveyard,
                Game.Content.Load<Texture2D>("Sprites/GameObjects/Campaigns/ShipGraveyard")
            },
            { LocationSids.Thicket, Game.Content.Load<Texture2D>("Sprites/GameObjects/Campaigns/DarkThinket") },
            { LocationSids.Swamp, Game.Content.Load<Texture2D>("Sprites/GameObjects/Campaigns/GrimSwamp") }
        };

        foreach (var campaign in _campaigns)
        {
            var campaignTexture = campaignTexturesDict[campaign.Location];

            var panel = new CampaignPanel(campaign, campaignTexture);
            panels.Add(panel);
            panel.Selected += (_, _) =>
            {
                ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign,
                    new CampaignScreenTransitionArguments(campaign));
            };

            index++;
        }

        _availableCampaignPanels = panels;

        _commandButtons[0] = new TextButton("Barraks");
        _commandButtons[1] = new TextButton("Armory");
        _commandButtons[2] = new TextButton("Adjutant");
        _commandButtons[3] = new TextButton("Chronicles");
    }

    protected override void UpdateContent(GameTime gameTime)
    {
        base.UpdateContent(gameTime);

        if (_availableCampaignPanels is null)
        {
            throw new InvalidOperationException("Screen is not initialized");
        }

        foreach (var panel in _availableCampaignPanels)
        {
            panel.Update(ResolutionIndependentRenderer);
        }
    }
}