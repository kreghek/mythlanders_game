using System;
using System.Collections.Generic;

using Client.Assets;
using Client.Core;
using Client.Core.Campaigns;
using Client.Engine;
using Client.GameScreens.Campaign;
using Client.GameScreens.CommandCenter.Ui;
using Client.ScreenManagement;

using CombatDicesTeam.Dices;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.CommandCenter;

internal class CommandCenterScreen : GameScreenWithMenuBase
{
    private readonly IReadOnlyList<HeroCampaign> _campaigns;

    private readonly ButtonBase[] _commandButtons = new ButtonBase[4];
    private readonly Texture2D[] _commandCenterSegmentTexture;

    private readonly Texture2D _mapBackgroundTexture;

    private readonly PongRectangle _mapPong;

    private IReadOnlyList<ICampaignPanel>? _availableCampaignPanels;

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

        const int MENU_HEIGHT = 20;
        var contentRect = new Rectangle(ResolutionIndependentRenderer.VirtualBounds.Location.X,
            ResolutionIndependentRenderer.VirtualBounds.Location.Y + MENU_HEIGHT,
            ResolutionIndependentRenderer.VirtualBounds.Width,
            ResolutionIndependentRenderer.VirtualBounds.Height - MENU_HEIGHT);

        var mapRect = new Rectangle(
            contentRect.Left + ControlBase.CONTENT_MARGIN,
            (contentRect.Top + (contentRect.Height / 8)) + ControlBase.CONTENT_MARGIN,
            contentRect.Width - ControlBase.CONTENT_MARGIN * 2,
            (contentRect.Height / 2) - ControlBase.CONTENT_MARGIN * 2);

        var mapPongRandomSource = new PongRectangleRandomSource(new LinearDice(), 2f);

        _mapPong = new PongRectangle(new Point(_mapBackgroundTexture.Width, _mapBackgroundTexture.Height), mapRect,
            mapPongRandomSource);
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
            _mapPong.GetRect(),
            Color.White);

        const int CAMPAIGN_CONTROL_WIDTH = 200;
        var fullCampaignWidth = (CAMPAIGN_CONTROL_WIDTH + ControlBase.CONTENT_MARGIN) * 3;
        var campaignOffsetX = (contentRect.Width - fullCampaignWidth) / 2;

        for (var campaignIndex = 0; campaignIndex < _availableCampaignPanels.Count; campaignIndex++)
        {
            var panel = _availableCampaignPanels[campaignIndex];
            panel.SetRect(new Rectangle(
                campaignOffsetX + contentRect.Left + ControlBase.CONTENT_MARGIN + 200 * campaignIndex,
                contentRect.Top + ControlBase.CONTENT_MARGIN,
                200,
                panel.Hover ? 200 : 100));
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
        var panels = new List<ICampaignPanel>();

        var campaignTexturesDict = new Dictionary<ILocationSid, Texture2D>
        {
            { LocationSids.Desert, LoadCampaignThumbnailImage("Desert") },
            { LocationSids.Monastery, LoadCampaignThumbnailImage("Monastery") },
            { LocationSids.ShipGraveyard, LoadCampaignThumbnailImage("ShipGraveyard") },
            { LocationSids.Thicket, LoadCampaignThumbnailImage("DarkThinket") },
            { LocationSids.Swamp, LoadCampaignThumbnailImage("GrimSwamp") },
            { LocationSids.Battleground, LoadCampaignThumbnailImage("Battleground") }
        };

        var placeholderTexture = LoadCampaignThumbnailImage("Placeholder");

        for (var campaignIndex = 0; campaignIndex < 3; campaignIndex++)
        {
            if (campaignIndex < _campaigns.Count)
            {
                var campaign = _campaigns[campaignIndex];
                var campaignTexture = campaignTexturesDict[campaign.Location];

                var panel = new CampaignPanel(campaign, campaignTexture);
                panels.Add(panel);
                panel.Selected += (_, _) =>
                {
                    ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign,
                        new CampaignScreenTransitionArguments(campaign));
                };
            }
            else
            {
                panels.Add(new PlaceholderCampaignPanel(placeholderTexture));
            }
        }

        _availableCampaignPanels = panels;

        _commandButtons[0] = new ResourceTextButton(nameof(UiResource.BarraksButtonTitle));
        _commandButtons[1] = new ResourceTextButton(nameof(UiResource.ArmoryButtonTitle));
        _commandButtons[2] = new ResourceTextButton(nameof(UiResource.AdjutantButtonTitle));
        _commandButtons[3] = new ResourceTextButton(nameof(UiResource.ChroniclesButtonTitle));

        Texture2D LoadCampaignThumbnailImage(string textureName)
        {
            return Game.Content.Load<Texture2D>($"Sprites/GameObjects/Campaigns/{textureName}");
        }
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

        _mapPong.Update(gameTime.ElapsedGameTime.TotalSeconds);
    }
}