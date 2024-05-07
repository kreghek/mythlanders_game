using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core;
using Client.Engine;
using Client.GameScreens;
using Client.GameScreens.TextDialogue.Ui;
using Client.ScreenManagement.Ui.TextEvents;

using CombatDicesTeam.Dialogues;
using CombatDicesTeam.Dices;

using GameClient.Engine.Ui;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Client.ScreenManagement;

internal abstract class TextEventScreenBase<TParagraphConditionContext, TAftermathContext> : GameScreenWithMenuBase
{
    private readonly TextEventScreenArgsBase<TParagraphConditionContext, TAftermathContext> _args;
    private readonly Dialogue<TParagraphConditionContext, TAftermathContext> _currentDialogue;
    private readonly DialogueOptions _dialogueOptions;
    private readonly IDice _dice;
    private readonly GameObjectContentStorage _gameObjectContentStorage;

    private readonly HoverController<DialogueOptionButton> _optionHoverController;
    private readonly IStoryState _storyState;
    private readonly IUiContentStorage _uiContentStorage;

    protected readonly IList<TextParagraphControl<TParagraphConditionContext, TAftermathContext>> TextParagraphControls;
    private bool _currentTextFragmentIsReady;
    private IDialogueContextFactory<TParagraphConditionContext, TAftermathContext> _contextFactory;
    protected DialoguePlayer<TParagraphConditionContext, TAftermathContext>? _dialoguePlayer;
    private bool _isInitialized;
    private KeyboardState _keyboardState;
    private double _pressToContinueCounter;
    protected int CurrentFragmentIndex;

    protected TextEventScreenBase(MythlandersGame game,
        TextEventScreenArgsBase<TParagraphConditionContext, TAftermathContext> args) : base(game)
    {
        TextParagraphControls = new List<TextParagraphControl<TParagraphConditionContext, TAftermathContext>>();
        _dialogueOptions = new DialogueOptions();

        _gameObjectContentStorage = game.Services.GetRequiredService<GameObjectContentStorage>();
        _dice = Game.Services.GetRequiredService<IDice>();
        _uiContentStorage = game.Services.GetRequiredService<IUiContentStorage>();

        var globeProvider = game.Services.GetRequiredService<GlobeProvider>();
        var globe = globeProvider.Globe;
        var player = globe.Player;
        _storyState = player.StoryState;
        _currentDialogue = args.CurrentDialogue;

        _args = args;

        _optionHoverController = new HoverController<DialogueOptionButton>();

        _optionHoverController.Hover += (_, button) =>
        {
            if (button is not null)
            {
                HandleOptionHover(button);
            }
        };

        _optionHoverController.Leave += (_, button) =>
        {
            if (button is not null)
            {
                HandleOptionLeave(button);
            }
        };
    }

    protected DialogueSpeech<TParagraphConditionContext, TAftermathContext> CurrentFragment =>
        _dialoguePlayer is not null
            ? _dialoguePlayer.CurrentTextFragments[CurrentFragmentIndex]
            : throw new Exception();

    protected abstract IDialogueContextFactory<TParagraphConditionContext, TAftermathContext>
        CreateDialogueContextFactory(TextEventScreenArgsBase<TParagraphConditionContext, TAftermathContext> args);

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

        if (_dialoguePlayer is not null && !_dialoguePlayer.IsEnd)
        {
            DrawSpecificForegroundScreenContent(spriteBatch, contentRect);

            DrawTextBlock(spriteBatch, contentRect);
        }
    }

    protected abstract void DrawSpecificBackgroundScreenContent(SpriteBatch spriteBatch, Rectangle contentRect);

    protected abstract void DrawSpecificForegroundScreenContent(SpriteBatch spriteBatch, Rectangle contentRect);

    protected abstract void HandleDialogueEnd();


    protected virtual void HandleOptionHover(DialogueOptionButton button)
    {
    }

    protected virtual void HandleOptionLeave(DialogueOptionButton button)
    {
    }

    protected virtual void HandleOptionSelection(DialogueOptionButton button)
    {
    }

    protected override void UpdateContent(GameTime gameTime)
    {
        base.UpdateContent(gameTime);

        if (_dialoguePlayer is null)
        {
            _contextFactory = CreateDialogueContextFactory(_args);
            _dialoguePlayer =
                new DialoguePlayer<TParagraphConditionContext, TAftermathContext>(_currentDialogue,
                    _contextFactory);
        }

        if (!_isInitialized)
        {
            if (_dialoguePlayer is not null)
            {
                InitDialogueControls(_dialoguePlayer);

                _isInitialized = true;
            }
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

        if (TextParagraphControls.Any())
        {
            var textFragmentControl = TextParagraphControls[CurrentFragmentIndex];

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

        if (CurrentFragmentIndex == TextParagraphControls.Count - 1 &&
            TextParagraphControls[CurrentFragmentIndex].IsComplete)
        {
            const int OPTION_BUTTON_MARGIN = 5;
            var lastTopButtonPosition =
                TextParagraphControls[CurrentFragmentIndex].Rect.Bottom + OPTION_BUTTON_MARGIN;

            _dialogueOptions.Rect = new Rectangle(PORTRAIT_SIZE, lastTopButtonPosition,
                contentRectangle.Width - PORTRAIT_SIZE + 100,
                contentRectangle.Height - lastTopButtonPosition + 100);
            _dialogueOptions.Draw(spriteBatch);
        }

        if (optionDescription is not null)
        {
            optionDescription.Draw(spriteBatch);
        }

        spriteBatch.End();
    }

    private void InitDialogueControls(DialoguePlayer<TParagraphConditionContext, TAftermathContext> dialoguePlayer)
    {
        TextParagraphControls.Clear();
        CurrentFragmentIndex = 0;

        foreach (var textFragment in dialoguePlayer.CurrentTextFragments)
        {
            var speaker = ConvertSpeakerToUnitName(textFragment.Speaker);
            var textFragmentControl = new TextParagraphControl<TParagraphConditionContext, TAftermathContext>(
                textFragment,
                _gameObjectContentStorage.GetTextSoundEffect(speaker),
                _dice,
                _contextFactory.CreateAftermathContext(),
                _storyState);
            TextParagraphControls.Add(textFragmentControl);
        }

        var optionNumber = 1;
        _dialogueOptions.Options.Clear();

        var context = _contextFactory.CreateParagraphConditionContext();

        foreach (var option in dialoguePlayer.CurrentOptions)
        {
            var optionButton = new DialogueOptionButton(optionNumber, option.TextSid)
            {
                DescriptionSid = option.DescriptionSid
            };

            optionButton.OnClick += (s, _) =>
            {
                dialoguePlayer.SelectOption(option);

                HandleOptionSelection((DialogueOptionButton)s!);

                if (dialoguePlayer.IsEnd)
                {
                    HandleDialogueEnd();
                }
                else
                {
                    _isInitialized = false;
                }
            };

            optionButton.IsEnabled = option.SelectConditions.All(x => x.Check(context));

            optionButton.OnHover += (sender, _) =>
            {
                _optionHoverController.HandleHover((DialogueOptionButton?)sender);
            };

            optionButton.OnLeave += (sender, _) =>
            {
                _optionHoverController.HandleLeave((DialogueOptionButton?)sender);
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

        var currentFragment = TextParagraphControls[CurrentFragmentIndex];
        currentFragment.Update(gameTime);

        var maxFragmentIndex = TextParagraphControls.Count - 1;
        if (IsKeyPressed(Keys.Space) && !TextParagraphControls[CurrentFragmentIndex].IsComplete)
        {
            currentFragment.FastComplete();

            return;
        }

        if (currentFragment.IsComplete)
        {
            if (CurrentFragmentIndex < maxFragmentIndex)
            {
                _currentTextFragmentIsReady = true;
                //TODO Make auto-move to next dialog. Make it disabled in settings by default.
                if (IsKeyPressed(Keys.Space))
                {
                    CurrentFragmentIndex++;
                    _currentTextFragmentIsReady = false;
                }
            }
        }

        if (CurrentFragmentIndex == maxFragmentIndex && TextParagraphControls[CurrentFragmentIndex].IsComplete)
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

            DetectOptionDescription(_dialogueOptions);
        }
    }

    private void DetectOptionDescription(DialogueOptions dialogueOptions)
    {
        var mouse = new MouseState().Position;
        
        foreach (var dialogueOptionButton in dialogueOptions.Options)
        {
            if (dialogueOptionButton.DescriptionSid is null)
            {
                continue;
            }

            if (dialogueOptionButton.Rect.Contains(mouse))
            {
                var (text, _) = SpeechVisualizationHelper.PrepareLocalizedText(dialogueOptionButton.DescriptionSid +
                                                                          "_OptionDescription");
                optionDescription = new TextHint(text);
                break;
            }
        }
    }

    private HintBase? optionDescription;
}