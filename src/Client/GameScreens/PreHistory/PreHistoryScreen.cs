using System;
using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;
using Client.Assets.MonsterPerks;
using Client.Core;
using Client.Engine;
using Client.ScreenManagement;
using Client.ScreenManagement.Ui.TextEvents;

using CombatDicesTeam.Dialogues;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.PreHistory;

internal sealed class PreHistoryScreen : TextEventScreenBase<ParagraphConditionContext, PreHistoryAftermathContext>
{
    private const double TRANSITION_DURATION_SEC = 1.25;

    private readonly Texture2D _cleanScreenTexture;
    private readonly StateCoordinator _coordinator;
    private readonly IDialogueEnvironmentManager _dialogueEnvironmentManager;
    private readonly GlobeProvider _globeProvider;

    private readonly SoundtrackManager _soundtrackManager;
    private PreHistoryAftermathContext? _aftermathContext;
    private double? _sceneTransitionCounter;

    private IPreHistoryScene? _currentScene;
    private IPreHistoryScene? _nextScene;

    private bool _isBackgoundInteractive;

    public PreHistoryScreen(MythlandersGame game, PreHistoryScreenScreenTransitionArguments args) : base(game, args)
    {
        _cleanScreenTexture = CreateTexture(game.GraphicsDevice, 1, 1, _ => new Color(44, 30, 49));

        _soundtrackManager = game.Services.GetRequiredService<SoundtrackManager>();
        _dialogueEnvironmentManager = game.Services.GetRequiredService<IDialogueEnvironmentManager>();
        _globeProvider = game.Services.GetService<GlobeProvider>();

        _coordinator = game.Services.GetService<StateCoordinator>();
    }

    protected override IDialogueContextFactory<ParagraphConditionContext, PreHistoryAftermathContext>
        CreateDialogueContextFactory(
            TextEventScreenArgsBase<ParagraphConditionContext, PreHistoryAftermathContext> args)
    {
        var contentRect = new Rectangle(ResolutionIndependentRenderer.VirtualBounds.Location.X,
            ResolutionIndependentRenderer.VirtualBounds.Location.Y,
            ResolutionIndependentRenderer.VirtualBounds.Width,
            ResolutionIndependentRenderer.VirtualBounds.Height);

        var scenes = PreHistoryScenes.Create(Game.Content, contentRect);

        _aftermathContext = new PreHistoryAftermathContext(scenes,
            Game.Services.GetRequiredService<IDialogueEnvironmentManager>(),
            Game.Services.GetService<GlobeProvider>().Globe.Player,
            Game.Services.GetService<MonsterPerkCatalog>());

        return new PreHistoryDialogueContextFactory(_aftermathContext,
            Game.Services.GetRequiredService<GlobeProvider>().Globe.Player);
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

        DrawBackgroundBasedOnTransition(spriteBatch, contentRect);

        spriteBatch.End();
    }

    protected override void DrawSpecificForegroundScreenContent(SpriteBatch spriteBatch, Rectangle contentRect)
    {
    }

    protected override void HandleDialogueEnd()
    {
        _dialogueEnvironmentManager.Clean();

        _globeProvider.StoreCurrentGlobe();

        _coordinator.MakeStartTransition(this);
    }

    protected override void HandleOptionHover(DialogueOptionButton button)
    {
        base.HandleOptionHover(button);

        _currentScene?.HoverOption(button.Number - 1);
    }

    protected override void HandleOptionSelection(DialogueOptionButton button)
    {
        base.HandleOptionSelection(button);

        _isBackgoundInteractive = false;
    }

    protected override void InitializeContent()
    {
        _soundtrackManager.PlaySilence();
    }

    protected override void UpdateSpecificScreenContent(GameTime gameTime)
    {
        _currentScene?.Update(gameTime, _isBackgoundInteractive);

        UpdateTransition(gameTime);
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

    private void DrawBackgroundBasedOnTransition(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        if (_currentScene is not null)
        {
            if (_sceneTransitionCounter is not null)
            {
                var progress = (TRANSITION_DURATION_SEC - _sceneTransitionCounter.Value) / TRANSITION_DURATION_SEC;
                var t = (float)Math.Sin(Math.PI * progress);

                _currentScene.Draw(spriteBatch, contentRect, 1 - t * 1);
            }
            else
            {
                _currentScene.Draw(spriteBatch, contentRect, 1);
            }
        }
    }

    private void UpdateTransition(GameTime gameTime)
    {
        var targetBackgroundTexture = _aftermathContext!.GetBackgroundTexture();

        if (targetBackgroundTexture != _currentScene && _sceneTransitionCounter is null)
        {
            _nextScene = targetBackgroundTexture;
            _sceneTransitionCounter = TRANSITION_DURATION_SEC;
        }

        if (_sceneTransitionCounter is not null)
        {
            if (_sceneTransitionCounter > 0)
            {
                _sceneTransitionCounter -= gameTime.ElapsedGameTime.TotalSeconds;

                if (_sceneTransitionCounter <= TRANSITION_DURATION_SEC * 0.5)
                {
                    _currentScene = _nextScene;

                    if (_sceneTransitionCounter <= 0)
                    {
                        _sceneTransitionCounter = null;
                        _nextScene = null;
                        _isBackgoundInteractive = true;
                    }
                }
            }
        }
    }
}