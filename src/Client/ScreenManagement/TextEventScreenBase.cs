using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core;
using Client.Engine;
using Client.GameScreens;
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
    private readonly DialogueOptions _dialogueOptions;
    protected readonly DialoguePlayer<TParagraphConditionContext, TAftermathContext> _dialoguePlayer;
    private readonly IDice _dice;
    private readonly GameObjectContentStorage _gameObjectContentStorage;
    private readonly IStoryState _storyState;
    private readonly IUiContentStorage _uiContentStorage;

    protected readonly IList<TextParagraphControl<TParagraphConditionContext, TAftermathContext>> TextParagraphControls;
    private readonly TextEventScreenArgsBase<TParagraphConditionContext, TAftermathContext> _args;
    protected int CurrentFragmentIndex;
    private bool _currentTextFragmentIsReady;
    private bool _isInitialized;
    private KeyboardState _keyboardState;
    private double _pressToContinueCounter;

    private HoverController<DialogueOptionButton> _optionHoverController;

    protected abstract IDialogueContextFactory<TParagraphConditionContext, TAftermathContext> CreateDialogueContextFactory(TextEventScreenArgsBase<TParagraphConditionContext, TAftermathContext> args);


    protected TextEventScreenBase(MythlandersGame game, TextEventScreenArgsBase<TParagraphConditionContext, TAftermathContext> args) : base(game)
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

        _dialoguePlayer = new DialoguePlayer<TParagraphConditionContext, TAftermathContext>(args.CurrentDialogue,
                CreateDialogueContextFactory(args));
        _args = args;

        _optionHoverController = new HoverController<DialogueOptionButton>();

        _optionHoverController.Hover += (sender, button) =>
        {
            if (button is not null)
            {
                HandleOptionHover(button);
            }
        };

        _optionHoverController.Leave += (sender, button) =>
        {
            if (button is not null)
            {
                HandleOptionLeave(button);
            }
        };
    }

    protected virtual void HandleOptionHover(DialogueOptionButton button)
    {
        
    }
    
    protected virtual void HandleOptionLeave(DialogueOptionButton button)
    {
        
    }

    protected DialogueSpeech<TParagraphConditionContext, TAftermathContext> CurrentFragment =>
        _dialoguePlayer.CurrentTextFragments[CurrentFragmentIndex];

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

        spriteBatch.End();
    }

    protected abstract void HandleDialogueEnd();

    private void InitDialogueControls()
    {
        TextParagraphControls.Clear();
        CurrentFragmentIndex = 0;
        foreach (var textFragment in _dialoguePlayer.CurrentTextFragments)
        {
            var speaker = ConvertSpeakerToUnitName(textFragment.Speaker);
            var textFragmentControl = new TextParagraphControl<TParagraphConditionContext, TAftermathContext>(
                textFragment,
                _gameObjectContentStorage.GetTextSoundEffect(speaker),
                _dice,
                CreateDialogueContextFactory(_args).CreateAftermathContext(),
                _storyState);
            TextParagraphControls.Add(textFragmentControl);
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
        }
    }
}