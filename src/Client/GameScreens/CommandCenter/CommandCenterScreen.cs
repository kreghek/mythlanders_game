using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets;
using Client.Assets.StageItems;
using Client.Core;
using Client.Core.Campaigns;
using Client.Engine;
using Client.GameScreens.Bestiary;
using Client.GameScreens.Campaign;
using Client.GameScreens.CommandCenter.Ui;
using Client.ScreenManagement;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Dices;
using CombatDicesTeam.Engine.Ui;

using Core;

using GameClient.Engine.RectControl;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame;

namespace Client.GameScreens.CommandCenter;

internal class CommandCenterScreen : GameScreenWithMenuBase
{
    private readonly IReadOnlyList<HeroCampaignLaunch> _campaignLaunches;

    //private readonly ButtonBase[] _commandButtons = new ButtonBase[4];
    //private readonly Texture2D[] _commandCenterSegmentTexture;
    private readonly IDice _dice;
    private readonly GlobeProvider _globeProvider;
    private readonly IDictionary<ILocationSid, Vector2> _locationCoords;

    private readonly Texture2D _mapBackgroundTexture;

    private readonly PongRectangleControl _mapPong;

    private IReadOnlyList<ICampaignPanel>? _availableCampaignPanels;

    private double _locationOnMapCounter;

    public CommandCenterScreen(MythlandersGame game, CommandCenterScreenTransitionArguments args) : base(game)
    {
        _dice = game.Services.GetRequiredService<IDice>();
        _globeProvider = game.Services.GetRequiredService<GlobeProvider>();

        _campaignLaunches = args.AvailableCampaigns;

        _mapBackgroundTexture = Game.Content.Load<Texture2D>("Sprites/GameObjects/Map/Map");
        // _commandCenterSegmentTexture = new[]
        // {
        //     Game.Content.Load<Texture2D>("Sprites/GameObjects/CommandCenter/CommandCenter1"),
        //     Game.Content.Load<Texture2D>("Sprites/GameObjects/CommandCenter/CommandCenter2"),
        //     Game.Content.Load<Texture2D>("Sprites/GameObjects/CommandCenter/CommandCenter3"),
        //     Game.Content.Load<Texture2D>("Sprites/GameObjects/CommandCenter/CommandCenter4")
        // };

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

        _mapPong = new PongRectangleControl(new Point(_mapBackgroundTexture.Width, _mapBackgroundTexture.Height),
            mapRect,
            mapPongRandomSource);

        _locationCoords = InitLocationCoords();
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

        DrawBackgroundMap(spriteBatch);

        DrawCampaigns(spriteBatch, contentRect);

        //DrawBase(spriteBatch, contentRect);

        spriteBatch.End();
    }

    protected override void InitializeContent()
    {
        SaveGameProgress();

        _availableCampaignPanels = CreateCampaignPanels();

        // _commandButtons[0] = new ResourceTextButton(nameof(UiResource.BarraksButtonTitle));
        // _commandButtons[0].OnClick += (_, _) =>
        // {
        //     ScreenManager.ExecuteTransition(this, ScreenTransition.Barracks, null!);
        // };
        //
        // _commandButtons[1] = new ResourceTextButton(nameof(UiResource.ArmoryButtonTitle));
        // _commandButtons[2] = new ResourceTextButton(nameof(UiResource.AdjutantButtonTitle));
        // _commandButtons[2].OnClick += (_, _) =>
        // {
        //     ScreenManager.ExecuteTransition(this, ScreenTransition.Bestiary,
        //         new BestiaryScreenTransitionArguments(ScreenTransition.CommandCenter,
        //             new CommandCenterScreenTransitionArguments(_campaignLaunches)));
        // };
        // _commandButtons[3] = new ResourceTextButton(nameof(UiResource.ChroniclesButtonTitle));
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

        _locationOnMapCounter += gameTime.ElapsedGameTime.TotalSeconds * 10;

        // foreach (var commandButton in _commandButtons)
        // {
        //     commandButton.Update(ResolutionIndependentRenderer);
        // }
    }

    private List<ICampaignPanel> CreateCampaignPanels()
    {
        var panels = new List<ICampaignPanel>();

        var campaignTexturesDict = new Dictionary<ILocationSid, Texture2D>
        {
            { LocationSids.Desert, LoadCampaignThumbnailImage("Desert") },
            { LocationSids.Monastery, LoadCampaignThumbnailImage("Monastery") },
            { LocationSids.ShipGraveyard, LoadCampaignThumbnailImage("ShipGraveyard") },
            { LocationSids.Thicket, LoadCampaignThumbnailImage("DarkThicket") },
            { LocationSids.Swamp, LoadCampaignThumbnailImage("GrimSwamp") },
            { LocationSids.Battleground, LoadCampaignThumbnailImage("Battleground") }
        };

        var placeholderTexture = LoadCampaignThumbnailImage("Placeholder");

        for (var campaignIndex = 0; campaignIndex < 3; campaignIndex++)
        {
            if (campaignIndex < _campaignLaunches.Count)
            {
                var campaignLaunch = _campaignLaunches[campaignIndex];
                var campaignTexture = campaignTexturesDict[campaignLaunch.Location.Sid];

                var panel = new CampaignPanel(campaignLaunch, campaignTexture,
                    _globeProvider.Globe.Features.HasFeature(GameFeatures.CampaignEffects));
                panels.Add(panel);
                panel.Selected += (_, _) =>
                {
                    var heroStartCoordsOpenList = new List<FieldCoords>
                    {
                        new FieldCoords(0, 0),
                        new FieldCoords(0, 1),
                        new FieldCoords(0, 2),
                        new FieldCoords(1, 0),
                        new FieldCoords(1, 1),
                        new FieldCoords(1, 2)
                    };

                    var initHeroes = new List<(HeroState, FieldCoords)>();
                    foreach (var launchHero in campaignLaunch.Heroes)
                    {
                        var rolledCoords = _dice.RollFromList(heroStartCoordsOpenList);
                        initHeroes.Add((launchHero, rolledCoords));
                        heroStartCoordsOpenList.Remove(rolledCoords);
                    }

                    var campaign = new HeroCampaign(initHeroes, campaignLaunch.Location,
                        campaignLaunch.Rewards, campaignLaunch.Penalties, _dice.Roll(100));

                    var monsterLeaderSids = ExtractmonsterLeadersFromCampaign(campaign);
                    WriteAllCampaignMonsterLeadersToKnown(monsterLeaderSids);

                    ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign,
                        new CampaignScreenTransitionArguments(campaign));
                };
            }
            else
            {
                panels.Add(new PlaceholderCampaignPanel(placeholderTexture));
            }
        }

        Texture2D LoadCampaignThumbnailImage(string textureName)
        {
            return Game.Content.Load<Texture2D>($"Sprites/GameObjects/Campaigns/{textureName}");
        }

        return panels;
    }

    private void DrawBackgroundMap(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_mapBackgroundTexture,
            _mapPong.GetRects()[0],
            Color.White);

        DrawLocationConnector(spriteBatch);
    }

    private void DrawBase(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        // for (var i = 0; i < 4; i++)
        // {
        //     spriteBatch.Draw(_commandCenterSegmentTexture[i],
        //         new Rectangle(
        //             (contentRect.Left + ControlBase.CONTENT_MARGIN) + i * (200 + ControlBase.CONTENT_MARGIN),
        //             (contentRect.Top + (contentRect.Height / 8)) + ControlBase.CONTENT_MARGIN +
        //             (contentRect.Height / 2) - ControlBase.CONTENT_MARGIN * 2,
        //             200,
        //             200),
        //         Color.White);
        //
        //     _commandButtons[i].Rect = new Rectangle(
        //         (contentRect.Left + ControlBase.CONTENT_MARGIN) + i * (200 + ControlBase.CONTENT_MARGIN),
        //         (contentRect.Top + (contentRect.Height / 8)) + ControlBase.CONTENT_MARGIN +
        //         (contentRect.Height / 2) - ControlBase.CONTENT_MARGIN * 2,
        //         100, 20);
        //
        //     _commandButtons[i].Draw(spriteBatch);
        // }
    }

    private void DrawCampaigns(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        if (_availableCampaignPanels is null)
        {
            return;
        }

        const int CAMPAIGN_CONTROL_WIDTH = 200;
        const int FULL_CAMPAIGN_WIDTH = (CAMPAIGN_CONTROL_WIDTH + ControlBase.CONTENT_MARGIN) * 3;
        var campaignOffsetX = (contentRect.Width - FULL_CAMPAIGN_WIDTH) / 2;

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
    }

    private void DrawLocationConnector(SpriteBatch spriteBatch)
    {
        var locationOnHover = GetLocationOnHover();
        if (locationOnHover is null)
        {
            return;
        }

        var locationCoords = GetLocationCoordsOnMap(locationOnHover);
        var locationButton = GetLocationButton(locationOnHover);
        var x1 = locationCoords.X + _mapPong.GetRects()[0].X;
        var y1 = locationCoords.Y + _mapPong.GetRects()[0].Y;

        var connectorPoints = GetConnectorPoints(x1, y1, locationButton.Rect.Center.X, locationButton.Rect.Center.Y);

        for (var index = 0; index < connectorPoints.Count - 1; index++)
        {
            var connectorStartPoint = connectorPoints[index];
            var connectorEndPoint = connectorPoints[index + 1];

            var lineT = Math.Sin(_locationOnMapCounter + index * 13);

            spriteBatch.DrawLine(connectorStartPoint.X, connectorStartPoint.Y, connectorEndPoint.X,
                connectorEndPoint.Y, MythlandersColors.MainSciFi, (float)(2 + lineT * 1));
            spriteBatch.DrawCircle(connectorStartPoint.X, connectorStartPoint.Y, (float)(8 + lineT * 2), 4,
                MythlandersColors.MainSciFi);
        }

        var t = Math.Sin(_locationOnMapCounter);
        spriteBatch.DrawCircle(x1, y1, (float)(16 + t * 4), 4, MythlandersColors.MainSciFi);
    }

    private static IReadOnlyCollection<string> ExtractmonsterLeadersFromCampaign(HeroCampaign campaign)
    {
        var combats = campaign.Location.Stages.GetAllNodes().Where(x => x.Payload is CombatStageItem)
            .Select(x => x.Payload).Cast<CombatStageItem>();
        return combats.Select(x => x.Metadata.MonsterLeader.ClassSid).Distinct().ToArray();
    }

    private static IReadOnlyList<Point> GetConnectorPoints(int x1, int y1, int x2, int y2)
    {
        return LineHelper.GetBrokenLine(x1, y1, x2, y2, new LineHelper.BrokenLineOptions
            { MinimalMargin = 24 });
    }

    private ControlBase GetLocationButton(ILocationSid locationOnHover)
    {
        return (ControlBase)_availableCampaignPanels!.Single(x => x.Location == locationOnHover);
    }

    private Point GetLocationCoordsOnMap(ILocationSid locationOnHover)
    {
        return _locationCoords[locationOnHover].ToPoint();
    }

    private ILocationSid? GetLocationOnHover()
    {
        return _availableCampaignPanels?.SingleOrDefault(x => x.Location is not null)?.Location;
    }

    private Dictionary<ILocationSid, Vector2> InitLocationCoords()
    {
        var rnd = new Random(2);
        var values = SidCatalogHelper.GetValues<ILocationSid>(typeof(LocationSids));

        return values.ToDictionary(x => x,
            x => new Vector2(rnd.Next(_mapBackgroundTexture.Width / 4, _mapBackgroundTexture.Width * 3 / 4),
                rnd.Next(_mapBackgroundTexture.Height / 4, _mapBackgroundTexture.Height * 3 / 4)));
    }

    private void SaveGameProgress()
    {
        _globeProvider.StoreCurrentGlobe();
    }

    private void WriteAllCampaignMonsterLeadersToKnown(IEnumerable<string> monsterLeaderClassSids)
    {
        foreach (var sid in monsterLeaderClassSids)
        {
            if (!_globeProvider.Globe.Player.KnownMonsters.Any(x =>
                    string.Equals(x.ClassSid, sid, StringComparison.InvariantCultureIgnoreCase)))
            {
                _globeProvider.Globe.Player.KnownMonsters.Add(new MonsterKnowledge(sid,
                    MonsterKnowledgeLevel.CommonDescription));
            }
        }
    }
}