using System;
using System.Collections.Generic;

using Client.Assets;
using Client.Assets.Catalogs.Dialogues;
using Client.Core;
using Client.Engine;
using Client.GameScreens.Combat.GameObjects.Background;
using Client.GameScreens.Common;
using Client.GameScreens.TextDialogue.Tutorial;
using Client.ScreenManagement;

using CombatDicesTeam.Dialogues;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.TextDialogue;

internal class TextDialogueScreen : TextEventScreenBase
{
    private const int BACKGROUND_LAYERS_COUNT = 3;

    private readonly Texture2D _backgroundTexture;
    private readonly IReadOnlyList<IBackgroundObject> _cloudLayerObjects;
    private readonly IReadOnlyList<IBackgroundObject> _foregroundLayerObjects;
    private readonly GameObjectContentStorage _gameObjectContentStorage;
    private readonly ILocationSid _globeLocation;
    private readonly GlobeProvider _globeProvider;
    private readonly Player _player;
    private readonly Random _random;
    private readonly IUiContentStorage _uiContentStorage;

    private double _counter;
    private int _frameIndex;

    public TextDialogueScreen(MythlandersGame game, TextDialogueScreenTransitionArgs args) : base(game, args)
    {
        _random = new Random();

        var globeProvider = game.Services.GetService<GlobeProvider>();

        var globe = globeProvider.Globe ?? throw new InvalidOperationException();
        _player = globe.Player ?? throw new InvalidOperationException();

        _uiContentStorage = game.Services.GetService<IUiContentStorage>();
        _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();

        _globeLocation = args.Location;

        var bgofSelector = Game.Services.GetService<BackgroundObjectFactorySelector>();

        var backgroundObjectFactory = bgofSelector.GetBackgroundObjectFactory(_globeLocation);

        _cloudLayerObjects = backgroundObjectFactory.CreateCloudLayerObjects();
        _foregroundLayerObjects = backgroundObjectFactory.CreateForegroundLayerObjects();

        var data = new[] { Color.White };
        _backgroundTexture = new Texture2D(game.GraphicsDevice, 1, 1);
        _backgroundTexture.SetData(data);

        var soundtrackManager = Game.Services.GetService<SoundtrackManager>();

        soundtrackManager.PlaySilence();

        _globeProvider = globeProvider;
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        return ArraySegment<ButtonBase>.Empty;
    }

    protected override void DrawSpecificBackgroundScreenContent(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        DrawGameObjects(spriteBatch);
    }

    protected override void DrawSpecificForegroundScreenContent(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        if (!_dialoguePlayer.IsEnd)
        {
            DrawCurrentSpeakerPortrait(spriteBatch);
        }
    }

    protected override void InitializeContent()
    {
    }

    protected override void UpdateContent(GameTime gameTime)
    {
        base.UpdateContent(gameTime);

        CheckTutorial();
    }

    protected override void UpdateSpecificScreenContent(GameTime gameTime)
    {
        UpdateBackgroundObjects(gameTime);

        UpdateSpeaker(gameTime);
    }

    private void CheckTutorial()
    {
        if (_player.HasAbility(PlayerAbility.SkipTutorials))
        {
            return;
        }

        if (_player.HasAbility(PlayerAbility.ReadSideQuestTutorial))
        {
            return;
        }

        _player.AddPlayerAbility(PlayerAbility.ReadSideQuestTutorial);

        var tutorialModal = new TutorialModal(new EventTutorialPageDrawer(_uiContentStorage), _uiContentStorage,
            ResolutionIndependentRenderer, _player);
        AddModal(tutorialModal, isLate: false);
    }

    private static UnitName ConvertSpeakerToUnitName(IDialogueSpeaker speaker)
    {
        var speakerName = speaker.ToString();
        return Enum.Parse<UnitName>(speakerName!);
    }

    private void DrawBackgroundLayers(SpriteBatch spriteBatch, Texture2D[] backgrounds)
    {
        for (var i = 0; i < BACKGROUND_LAYERS_COUNT; i++)
        {
            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: Camera.GetViewTransformationMatrix());

            spriteBatch.Draw(backgrounds[i], Vector2.Zero, Color.White);

            if (i == 0 /*Cloud layer*/)
            {
                foreach (var obj in _cloudLayerObjects)
                {
                    obj.Draw(spriteBatch);
                }
            }

            spriteBatch.End();
        }
    }

    private void DrawCurrentSpeakerPortrait(SpriteBatch spriteBatch)
    {
        const int SPEAKER_FRAME_SIZE = 256;

        var currentFragment = CurrentFragment;
        var speaker = currentFragment.Speaker;

        if (DialogueSpeakers.Get(UnitName.Environment).Equals(speaker))
        {
            // This text describes environment. There is no speaker.
            return;
        }

        spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: Camera.GetViewTransformationMatrix());

        var name = ConvertSpeakerToUnitName(speaker);

        spriteBatch.Draw(_gameObjectContentStorage.GetCharacterFaceTexture(name),
            new Rectangle(0, ResolutionIndependentRenderer.VirtualBounds.Height - SPEAKER_FRAME_SIZE,
                SPEAKER_FRAME_SIZE,
                SPEAKER_FRAME_SIZE),
            new Rectangle(0, 0, SPEAKER_FRAME_SIZE,
                SPEAKER_FRAME_SIZE),
            Color.White);

        spriteBatch.End();
    }

    private void DrawForegroundLayers(SpriteBatch spriteBatch, Texture2D[] backgrounds)
    {
        spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: Camera.GetViewTransformationMatrix());

        spriteBatch.Draw(backgrounds[3], Vector2.Zero, Color.White);

        foreach (var obj in _foregroundLayerObjects)
        {
            obj.Draw(spriteBatch);
        }

        spriteBatch.End();
    }

    private void DrawGameObjects(SpriteBatch spriteBatch)
    {
        var backgroundType = LocationHelper.GetLocationTheme(_globeLocation);

        var backgrounds = _gameObjectContentStorage.GetCombatBackgrounds(backgroundType);

        DrawBackgroundLayers(spriteBatch, backgrounds);

        DrawForegroundLayers(spriteBatch, backgrounds);

        spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: Camera.GetViewTransformationMatrix());
        spriteBatch.Draw(_backgroundTexture, ResolutionIndependentRenderer.VirtualBounds,
            Color.Lerp(Color.Transparent, Color.Black, 0.5f));
        spriteBatch.End();
    }

    private void UpdateBackgroundObjects(GameTime gameTime)
    {
        foreach (var obj in _foregroundLayerObjects)
        {
            obj.Update(gameTime);
        }

        foreach (var obj in _cloudLayerObjects)
        {
            obj.Update(gameTime);
        }
    }

    private void UpdateSpeaker(GameTime gameTime)
    {
        const int SPEAKER_FRAME_COUNT = 4;
        const double SPEAKER_FRAME_DURATION = 0.25;

        var currentFragment = _textParagraphControls[_currentFragmentIndex];
        if (!currentFragment.IsComplete)
        {
            _counter += gameTime.ElapsedGameTime.TotalSeconds;

            if (_counter > SPEAKER_FRAME_DURATION)
            {
                _frameIndex = _random.Next(SPEAKER_FRAME_COUNT);
                if (_frameIndex > SPEAKER_FRAME_COUNT - 1)
                {
                    _frameIndex = 0;
                }

                _counter = 0;
            }
        }
        else
        {
            _frameIndex = 0;
        }
    }
}