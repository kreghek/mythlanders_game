using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;
using Client.Core;
using Client.Engine;
using Client.GameScreens;
using Client.ScreenManagement.Ui.TextEvents;

using CombatDicesTeam.Dialogues;
using CombatDicesTeam.Dices;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Client.ScreenManagement;

internal abstract class TextEventScreenBase : GameScreenWithMenuBase
{
    private readonly IDialogueEnvironmentManager _dialogueEnvironmentManager;
    private readonly DialogueOptions _dialogueOptions;
    protected readonly DialoguePlayer<ParagraphConditionContext, CampaignAftermathContext> _dialoguePlayer;
    private readonly IDice _dice;
    private readonly IEventCatalog _eventCatalog;
    private readonly GameObjectContentStorage _gameObjectContentStorage;
    private readonly GlobeProvider _globeProvider;
    private readonly IStoryState _storyState;

    protected readonly IList<TextParagraphControl> _textParagraphControls;
    private readonly IUiContentStorage _uiContentStorage;
    protected int _currentFragmentIndex;
    private bool _currentTextFragmentIsReady;
    private bool _isInitialized;
    private KeyboardState _keyboardState;
    private double _pressToContinueCounter;

    protected abstract IDialogueContextFactory<ParagraphConditionContext, CampaignAftermathContext> DialogueContextFactory
    {
        get;
    }

    protected TextEventScreenBase(MythlandersGame game, TextEventScreenArgsBase args) : base(game)
    {
        _textParagraphControls = new List<TextParagraphControl>();
        _dialogueOptions = new DialogueOptions();

        _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();
        _dice = Game.Services.GetService<IDice>();
        
        var globeProvider = game.Services.GetService<GlobeProvider>();
        var globe = globeProvider.Globe ?? throw new InvalidOperationException();
        var player = globe.Player ?? throw new InvalidOperationException();
        _storyState = player.StoryState;

        _dialoguePlayer =
            new DialoguePlayer<ParagraphConditionContext, CampaignAftermathContext>(args.CurrentDialogue,
                DialogueContextFactory);
       
    }

    protected DialogueSpeech<ParagraphConditionContext, CampaignAftermathContext> CurrentFragment =>
        _dialoguePlayer.CurrentTextFragments[_currentFragmentIndex];

    protected override IList<ButtonBase> CreateMenu()
    {
        return ArraySegment<ButtonBase>.Empty;
    }

    protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        if (!_isInitialized)
        {
            return;
        }

        DrawSpecificBackgroundScreenContent(spriteBatch, contentRect);

        if (!_dialoguePlayer.IsEnd)
        {
            DrawSpecificForegroundScreenContent(spriteBatch, contentRect);

            DrawTextBlock(spriteBatch, contentRect);
        }
    }

    protected abstract void DrawSpecificBackgroundScreenContent(SpriteBatch spriteBatch, Rectangle contentRect);

    protected abstract void DrawSpecificForegroundScreenContent(SpriteBatch spriteBatch, Rectangle contentRect);

    protected override void UpdateContent(GameTime gameTime)
    {
        base.UpdateContent(gameTime);

        if (!_isInitialized)
        {
            InitDialogueControls();

            _isInitialized = true;
        }
        else
        {
            UpdateSpecificScreenContent(gameTime);

            UpdateTextHud(gameTime);
        }

        _keyboardState = Keyboard.GetState();
    }

    protected abstract void UpdateSpecificScreenContent(GameTime gameTime);


    private static UnitName ConvertSpeakerToUnitName(IDialogueSpeaker speaker)
    {
        var speakerName = speaker.ToString();
        return Enum.Parse<UnitName>(speakerName!, true);
    }

    private void DrawTextBlock(SpriteBatch spriteBatch, Rectangle contentRectangle)
    {
        spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: Camera.GetViewTransformationMatrix());

        const int PORTRAIT_SIZE = 256;

        if (_textParagraphControls.Any())
        {
            var textFragmentControl = _textParagraphControls[_currentFragmentIndex];

            var textFragmentSize = textFragmentControl.CalculateSize().ToPoint();

            const int SPEECH_MARGIN = 50;
            var sumOptionHeight = _dialogueOptions.GetHeight();
            var fragmentHeight = textFragmentSize.Y + SPEECH_MARGIN + sumOptionHeight;
            var fragmentPosition = new Point(PORTRAIT_SIZE, contentRectangle.Bottom - fragmentHeight);
            textFragmentControl.Rect = new Rectangle(fragmentPosition, textFragmentSize);
            textFragmentControl.Draw(spriteBatch);

            if (_currentTextFragmentIsReady)
            {
                // TODO Move to separated control
                var isVisible = (float)Math.Sin(_pressToContinueCounter) > 0;

                if (isVisible)
                {
                    spriteBatch.DrawString(_uiContentStorage.GetTitlesFont(), UiResource.DialogueContinueText,
                        new Vector2(PORTRAIT_SIZE, contentRectangle.Bottom - 25), Color.White);
                }
            }
        }

        if (_currentFragmentIndex == _textParagraphControls.Count - 1 &&
            _textParagraphControls[_currentFragmentIndex].IsComplete)
        {
            const int OPTION_BUTTON_MARGIN = 5;
            var lastTopButtonPosition =
                _textParagraphControls[_currentFragmentIndex].Rect.Bottom + OPTION_BUTTON_MARGIN;

            _dialogueOptions.Rect = new Rectangle(PORTRAIT_SIZE, lastTopButtonPosition,
                contentRectangle.Width - PORTRAIT_SIZE + 100,
                contentRectangle.Height - lastTopButtonPosition + 100);
            _dialogueOptions.Draw(spriteBatch);
        }

        spriteBatch.End();
    }

    protected abstract void HandleDialogueEnd();

    private void InitDialogueControls()
    {
        _textParagraphControls.Clear();
        _currentFragmentIndex = 0;
        foreach (var textFragment in _dialoguePlayer.CurrentTextFragments)
        {
            var speaker = ConvertSpeakerToUnitName(textFragment.Speaker);
            var textFragmentControl = new TextParagraphControl(
                textFragment,
                _gameObjectContentStorage.GetTextSoundEffect(speaker),
                _dice,
                DialogueContextFactory.CreateAftermathContext(),
                _storyState);
            _textParagraphControls.Add(textFragmentControl);
        }

        var optionNumber = 1;
        _dialogueOptions.Options.Clear();
        foreach (var option in _dialoguePlayer.CurrentOptions)
        {
            var optionButton = new DialogueOptionButton(optionNumber, option.TextSid);
            optionButton.OnClick += (_, _) =>
            {
                _dialoguePlayer.SelectOption(option);

                if (_dialoguePlayer.IsEnd)
                {
                    HandleDialogueEnd();
                }
                else
                {
                    _isInitialized = false;
                }
            };

            _dialogueOptions.Options.Add(optionButton);
            optionNumber++;
        }
    }

    private bool IsKeyPressed(Keys checkKey)
    {
        return Keyboard.GetState().IsKeyUp(checkKey) && _keyboardState.IsKeyDown(checkKey);
    }

    private void UpdateTextHud(GameTime gameTime)
    {
        _pressToContinueCounter += gameTime.ElapsedGameTime.TotalSeconds * 10f;

        var currentFragment = _textParagraphControls[_currentFragmentIndex];
        currentFragment.Update(gameTime);

        var maxFragmentIndex = _textParagraphControls.Count - 1;
        if (IsKeyPressed(Keys.Space) && !_textParagraphControls[_currentFragmentIndex].IsComplete)
        {
            currentFragment.FastComplete();

            return;
        }

        if (currentFragment.IsComplete)
        {
            if (_currentFragmentIndex < maxFragmentIndex)
            {
                _currentTextFragmentIsReady = true;
                //TODO Make auto-move to next dialog. Make it disabled in settings by default.
                if (IsKeyPressed(Keys.Space))
                {
                    _currentFragmentIndex++;
                    _currentTextFragmentIsReady = false;
                }
            }
        }

        if (_currentFragmentIndex == maxFragmentIndex && _textParagraphControls[_currentFragmentIndex].IsComplete)
        {
            _dialogueOptions.Update(ResolutionIndependentRenderer);

            if (IsKeyPressed(Keys.D1))
            {
                _dialogueOptions.SelectOption(1);
            }
            else if (IsKeyPressed(Keys.D2))
            {
                _dialogueOptions.SelectOption(2);
            }
            else if (IsKeyPressed(Keys.D3))
            {
                _dialogueOptions.SelectOption(3);
            }
            else if (IsKeyPressed(Keys.D4))
            {
                _dialogueOptions.SelectOption(4);
            }
        }
    }
}