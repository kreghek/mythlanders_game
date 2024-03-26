using System;
using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;
using Client.Assets.MonsterPerks;
using Client.Core;
using Client.Engine;
using Client.GameScreens.CommandCenter;
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
    private readonly ICampaignGenerator _campaignGenerator;

    private readonly Texture2D _cleanScreenTexture;
    private readonly IDialogueEnvironmentManager _dialogueEnvironmentManager;
    private readonly GlobeProvider _globeProvider;

    private readonly SoundtrackManager _soundtrackManager;
    private PreHistoryAftermathContext? _aftermathContext;
    private double? _backgroundTransitionCounter;

    private IPreHistoryBackground? _currentBackground;

    private bool _isBackgoundInteractive;
    private IPreHistoryBackground? _nextBackground;

    public PreHistoryScreen(MythlandersGame game, PreHistoryScreenScreenTransitionArguments args) : base(game, args)
    {
        _cleanScreenTexture = CreateTexture(game.GraphicsDevice, 1, 1, _ => new Color(44, 30, 49));

        _soundtrackManager = game.Services.GetRequiredService<SoundtrackManager>();
        _dialogueEnvironmentManager = game.Services.GetRequiredService<IDialogueEnvironmentManager>();
        _globeProvider = game.Services.GetService<GlobeProvider>();
        _campaignGenerator = game.Services.GetService<ICampaignGenerator>();
    }

    protected override IDialogueContextFactory<ParagraphConditionContext, PreHistoryAftermathContext>
        CreateDialogueContextFactory(
            TextEventScreenArgsBase<ParagraphConditionContext, PreHistoryAftermathContext> args)
    {
        var contentRect = new Rectangle(ResolutionIndependentRenderer.VirtualBounds.Location.X,
            ResolutionIndependentRenderer.VirtualBounds.Location.Y,
            ResolutionIndependentRenderer.VirtualBounds.Width,
            ResolutionIndependentRenderer.VirtualBounds.Height);

        var backgrounds = new Dictionary<string, IPreHistoryBackground>
        {
            {
                "AncientRising",
                new StaticScenePreHistoryBackground(
                    Game.Content.Load<Texture2D>("Sprites/GameObjects/PreHistory/AncientRising"), contentRect)
            },
            {
                "Monsters",
                new StaticScenePreHistoryBackground(
                    Game.Content.Load<Texture2D>("Sprites/GameObjects/PreHistory/Monsters"), contentRect)
            },
            {
                "MonstersAttack",
                new StaticScenePreHistoryBackground(
                    Game.Content.Load<Texture2D>("Sprites/GameObjects/PreHistory/MonstersAttack"), contentRect)
            },
            {
                "Hero",
                new StaticScenePreHistoryBackground(
                    Game.Content.Load<Texture2D>("Sprites/GameObjects/PreHistory/Hero"), contentRect)
            },
            {
                "FirstFraction",
                new FractionScenePreHistoryBackground(
                    Game.Content.Load<Texture2D>("Sprites/GameObjects/PreHistory/SelectBlack"),
                    Game.Content.Load<Texture2D>("Sprites/GameObjects/PreHistory/SelectBlackDisabled"),
                    Game.Content.Load<Texture2D>("Sprites/GameObjects/PreHistory/SelectUnited"),
                    Game.Content.Load<Texture2D>("Sprites/GameObjects/PreHistory/SelectUnitedDisabled"))
            },
            {
                "Black",
                new StaticScenePreHistoryBackground(
                    Game.Content.Load<Texture2D>("Sprites/GameObjects/PreHistory/Black"), contentRect)
            },
            {
                "Union",
                new StaticScenePreHistoryBackground(
                    Game.Content.Load<Texture2D>("Sprites/GameObjects/PreHistory/Union"), contentRect)
            },
            {
                "StartHeroes",
                new StaticScenePreHistoryBackground(
                    Game.Content.Load<Texture2D>("Sprites/GameObjects/PreHistory/StartHeroes"), contentRect)
            },
            {
                "Monk",
                new StaticScenePreHistoryBackground(
                    Game.Content.Load<Texture2D>("Sprites/GameObjects/PreHistory/Monk"), contentRect)
            },
            {
                "Swordsman",
                new StaticScenePreHistoryBackground(
                    Game.Content.Load<Texture2D>("Sprites/GameObjects/PreHistory/Swordsman"), contentRect)
            },
            {
                "Hoplite",
                new StaticScenePreHistoryBackground(
                    Game.Content.Load<Texture2D>("Sprites/GameObjects/PreHistory/Hoplite"), contentRect)
            }
        };

        _aftermathContext = new PreHistoryAftermathContext(backgrounds,
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

        var otherCampaignLaunches = _campaignGenerator.CreateSet(_globeProvider.Globe);

        ScreenManager.ExecuteTransition(this, ScreenTransition.CommandCenter,
            new CommandCenterScreenTransitionArguments(otherCampaignLaunches));

        _globeProvider.StoreCurrentGlobe();
    }

    protected override void HandleOptionHover(DialogueOptionButton button)
    {
        base.HandleOptionHover(button);

        _currentBackground?.HoverOption(button.Number - 1);
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
        _currentBackground?.Update(gameTime, _isBackgoundInteractive);

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
        if (_currentBackground is not null)
        {
            if (_backgroundTransitionCounter is not null)
            {
                var progress = (TRANSITION_DURATION_SEC - _backgroundTransitionCounter.Value) / TRANSITION_DURATION_SEC;
                var t = (float)Math.Sin(Math.PI * progress);

                _currentBackground.Draw(spriteBatch, contentRect, 1 - t * 1);
            }
            else
            {
                _currentBackground.Draw(spriteBatch, contentRect, 1);
            }
        }
    }

    private void UpdateTransition(GameTime gameTime)
    {
        var targetBackgroundTexture = _aftermathContext!.GetBackgroundTexture();

        if (targetBackgroundTexture != _currentBackground && _backgroundTransitionCounter is null)
        {
            _nextBackground = targetBackgroundTexture;
            _backgroundTransitionCounter = TRANSITION_DURATION_SEC;
        }

        if (_backgroundTransitionCounter is not null)
        {
            if (_backgroundTransitionCounter > 0)
            {
                _backgroundTransitionCounter -= gameTime.ElapsedGameTime.TotalSeconds;

                if (_backgroundTransitionCounter <= TRANSITION_DURATION_SEC * 0.5)
                {
                    _currentBackground = _nextBackground;

                    if (_backgroundTransitionCounter <= 0)
                    {
                        _backgroundTransitionCounter = null;
                        _nextBackground = null;
                        _isBackgoundInteractive = true;
                    }
                }
            }
        }
    }
}