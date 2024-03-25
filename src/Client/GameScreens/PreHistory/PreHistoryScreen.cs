using System;
using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;
using Client.Core;
using Client.Engine;
using Client.GameScreens.CommandCenter;
using Client.ScreenManagement;
using Client.ScreenManagement.Ui.TextEvents;

using CombatDicesTeam.Dialogues;
using CombatDicesTeam.Dices;
using CombatDicesTeam.Engine.Ui;

using GameClient.Engine.RectControl;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.PreHistory;

internal sealed class PreHistoryScreen : TextEventScreenBase<ParagraphConditionContext, PreHistoryAftermathContext>
{
    private const int BACKGROUND_WIDTH = 1024;
    private const int BACKGROUND_HEIGHT = 512;

    private const double TRANSITION_DURATION_SEC = 1.25;

    private readonly Texture2D _cleanScreenTexture;
    private readonly SoundtrackManager _soundtrackManager;
    private readonly IDialogueEnvironmentManager _dialogueEnvironmentManager;
    private readonly GlobeProvider _globeProvider;
    private readonly ICampaignGenerator _campaignGenerator;

    private readonly PongRectangleControl _pongBackground;

    private PreHistoryAftermathContext? _aftermathContext;

    private Texture2D? _currentBackgroundTexture;
    private Texture2D? _nextBackgroundTexture;
    private double? _backgroundTransitionCounter;

    public PreHistoryScreen(MythlandersGame game, PreHistoryScreenScreenTransitionArguments args) : base(game, args)
    {
        _cleanScreenTexture = CreateTexture(game.GraphicsDevice, 1, 1, _ => new Color(36, 40, 41));

        _soundtrackManager = game.Services.GetRequiredService<SoundtrackManager>();
        _dialogueEnvironmentManager = game.Services.GetRequiredService<IDialogueEnvironmentManager>();
        _globeProvider = game.Services.GetService<GlobeProvider>();
        _campaignGenerator = game.Services.GetService<ICampaignGenerator>();


        var contentRect = new Rectangle(ResolutionIndependentRenderer.VirtualBounds.Location.X,
            ResolutionIndependentRenderer.VirtualBounds.Location.Y,
            ResolutionIndependentRenderer.VirtualBounds.Width,
            ResolutionIndependentRenderer.VirtualBounds.Height);

        var mapRect = new Rectangle(
            contentRect.Left + ControlBase.CONTENT_MARGIN,
            (contentRect.Top + (contentRect.Height / 8)) + ControlBase.CONTENT_MARGIN,
            contentRect.Width - ControlBase.CONTENT_MARGIN * 2,
            (contentRect.Height / 2) - ControlBase.CONTENT_MARGIN * 2);

        var mapPongRandomSource = new PongRectangleRandomSource(new LinearDice(), 3f);

        _pongBackground = new PongRectangleControl(new Point(BACKGROUND_WIDTH, BACKGROUND_HEIGHT),
            mapRect,
            mapPongRandomSource);
    }

    protected override IDialogueContextFactory<ParagraphConditionContext, PreHistoryAftermathContext> CreateDialogueContextFactory(TextEventScreenArgsBase<ParagraphConditionContext, PreHistoryAftermathContext> args)
    {
        _aftermathContext = new PreHistoryAftermathContext(Game.Content, Game.Services.GetRequiredService<IDialogueEnvironmentManager>(), Game.Services.GetService<GlobeProvider>().Globe.Player);

        return new PreHistoryDialogueContextFactory(_aftermathContext, Game.Services.GetRequiredService<GlobeProvider>().Globe.Player);
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        return ArraySegment<ButtonBase>.Empty;
    }

    protected override void DrawSpecificBackgroundScreenContent(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: Camera.GetViewTransformationMatrix());

        spriteBatch.Draw(_cleanScreenTexture, contentRect, Color.White);

        DrawBackgroundBasedOnTransition(spriteBatch);

        spriteBatch.Draw(_cleanScreenTexture,
            new Rectangle(contentRect.Center.X, contentRect.Top, contentRect.Width / 2, contentRect.Height),
            Color.Lerp(Color.White, Color.Transparent, 0.25f));

        spriteBatch.End();
    }

    private void DrawBackgroundBasedOnTransition(SpriteBatch spriteBatch)
    {
        if (_currentBackgroundTexture is not null)
        {
            if (_backgroundTransitionCounter is not null)
            {
                var progress = (TRANSITION_DURATION_SEC - _backgroundTransitionCounter.Value) / TRANSITION_DURATION_SEC;
                var t = (float)Math.Sin(Math.PI * progress);

                spriteBatch.Draw(_currentBackgroundTexture,
                    _pongBackground.GetRects()[0],
                    Color.Lerp(Color.White, Color.Transparent, t));
            }
            else
            {
                spriteBatch.Draw(_currentBackgroundTexture,
                    _pongBackground.GetRects()[0],
                    Color.White);
            }
        }
    }

    protected override void DrawSpecificForegroundScreenContent(SpriteBatch spriteBatch, Rectangle contentRect)
    {
    }

    protected override void InitializeContent()
    {
        _soundtrackManager.PlaySilence();
    }

    protected override void UpdateSpecificScreenContent(GameTime gameTime)
    {
        _pongBackground.Update(gameTime.ElapsedGameTime.TotalSeconds);
        UpdateTransition(gameTime);
    }

    private void UpdateTransition(GameTime gameTime)
    {
        var targetBackgroundTexture = _aftermathContext!.GetBackgroundTexture();

        if (targetBackgroundTexture != _currentBackgroundTexture && _backgroundTransitionCounter is null)
        {
            _nextBackgroundTexture = targetBackgroundTexture;
            _backgroundTransitionCounter = TRANSITION_DURATION_SEC;
        }

        if (_backgroundTransitionCounter is not null)
        {
            if (_backgroundTransitionCounter > 0)
            {
                _backgroundTransitionCounter -= gameTime.ElapsedGameTime.TotalSeconds;

                if (_backgroundTransitionCounter <= TRANSITION_DURATION_SEC * 0.5)
                {
                    _currentBackgroundTexture = _nextBackgroundTexture;

                    if (_backgroundTransitionCounter <= 0)
                    {
                        _backgroundTransitionCounter = null;
                        _nextBackgroundTexture = null;
                    }
                }
            }
        }
    }

    protected override void HandleDialogueEnd()
    {
        _dialogueEnvironmentManager.Clean();

        var otherCampaignLaunches = _campaignGenerator.CreateSet(_globeProvider.Globe);

        ScreenManager.ExecuteTransition(this, ScreenTransition.CommandCenter,
            new CommandCenterScreenTransitionArguments(otherCampaignLaunches));

        _globeProvider.StoreCurrentGlobe();
    }

    private static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Func<int, Color> paint)
    {
        //initialize a texture
        var texture = new Texture2D(device, width, height);

        //the array holds the color for each pixel in the texture
        var data = new Color[width * height];
        for (var pixel = 0; pixel < data.Length; pixel++)
        {
            //the function applies the color according to the specified pixel
            data[pixel] = paint(pixel);
        }

        //set the color
        texture.SetData(data);

        return texture;
    }
}