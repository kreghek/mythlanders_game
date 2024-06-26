﻿using System;

using Client.GameScreens.Barracks;
using Client.GameScreens.Bestiary;
using Client.GameScreens.Campaign;
using Client.GameScreens.Challenge;
using Client.GameScreens.Combat;
using Client.GameScreens.CommandCenter;
using Client.GameScreens.Credits;
using Client.GameScreens.Crisis;
using Client.GameScreens.Demo;
using Client.GameScreens.EndGame;
using Client.GameScreens.Intro;
using Client.GameScreens.Match3;
using Client.GameScreens.NotImplementedStage;
using Client.GameScreens.PreHistory;
using Client.GameScreens.Rest;
using Client.GameScreens.SlidingPuzzles;
using Client.GameScreens.TextDialogue;
using Client.GameScreens.Title;
using Client.GameScreens.TowersMinigame;
using Client.GameScreens.Training;
using Client.GameScreens.VoiceCombat;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.ScreenManagement;

internal class ScreenManager : IScreenManager
{
    private const double TRANSITION_DURATION = 1;
    private readonly MythlandersGame _game;
    private readonly GameSettings _gameSettings;
    private readonly Texture2D _transitionTexture;
    private bool _screenChanged;

    private double? _transitionCounter;

    public ScreenManager(MythlandersGame game, GameSettings gameSettings)
    {
        _game = game;
        _gameSettings = gameSettings;
        var colors = new[] { Color.Black };
        _transitionTexture = new Texture2D(game.GraphicsDevice, 1, 1);
        _transitionTexture.SetData(colors);
    }

    public IScreen? ActiveScreen { get; set; }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (ActiveScreen is not null)
        {
            ActiveScreen.Draw(spriteBatch);
        }

        DrawTransition(spriteBatch);
    }

    public void InitStartScreen()
    {
        if (_gameSettings.Mode == GameMode.Full)
        {
            var startScreen = new IntroScreen(_game);
            ActiveScreen = startScreen;
        }
        else
        {
            var startScreen = new TitleScreen(_game);
            ActiveScreen = startScreen;
        }
    }

    public void Update(GameTime gameTime)
    {
        if (ActiveScreen is null)
        {
            return;
        }

        ActiveScreen.Update(gameTime);

        if (ActiveScreen.TargetScreen is not null)
        {
            if (_transitionCounter is null)
            {
                _transitionCounter = 0;
            }
        }

        if (_transitionCounter is not null)
        {
            if (_transitionCounter.Value < TRANSITION_DURATION)
            {
                _transitionCounter += gameTime.ElapsedGameTime.TotalSeconds;

                if (_transitionCounter.Value > TRANSITION_DURATION / 2 && !_screenChanged)
                {
                    _screenChanged = true;

                    ActiveScreen = ActiveScreen.TargetScreen;
                }
            }
            else
            {
                _transitionCounter = null;
                _screenChanged = false;
            }
        }
    }

    private IScreen CreateScreenToTransit(ScreenTransition targetTransition,
        IScreenTransitionArguments screenTransitionArguments)
    {
        return targetTransition switch
        {
            ScreenTransition.Title => new TitleScreen(_game),
            ScreenTransition.PreHistory => new PreHistoryScreen(_game,
                (PreHistoryScreenScreenTransitionArguments)screenTransitionArguments),
            ScreenTransition.Campaign => new CampaignScreen(_game,
                (CampaignScreenTransitionArguments)screenTransitionArguments),
            ScreenTransition.CommandCenter => new CommandCenterScreen(_game,
                (CommandCenterScreenTransitionArguments)screenTransitionArguments),
            ScreenTransition.Barracks => new BarracksScreen(_game),
            ScreenTransition.Event => new TextDialogueScreen(_game,
                (TextDialogueScreenTransitionArgs)screenTransitionArguments),
            ScreenTransition.Combat => new CombatScreen(_game,
                (CombatScreenTransitionArguments)screenTransitionArguments),
            ScreenTransition.Rest => new RestScreen(_game,
                (RestScreenTransitionArguments)screenTransitionArguments),
            ScreenTransition.Crisis => new CrisisScreen(_game,
                (CrisisScreenTransitionArguments)screenTransitionArguments),
            ScreenTransition.Training => new TrainingScreen(_game,
                (TrainingScreenTransitionArguments)screenTransitionArguments),
            ScreenTransition.SlidingPuzzlesMinigame => new SlidingPuzzlesScreen(_game,
                (SlidingPuzzlesMinigameScreenTransitionArguments)screenTransitionArguments),
            ScreenTransition.Match3Minigame => new Match3MinigameScreen(_game,
                (Match3MiniGameScreenTransitionArguments)screenTransitionArguments),
            ScreenTransition.TowersMinigame => new TowersMinigameScreen(_game,
                (TowersMiniGameScreenTransitionArguments)screenTransitionArguments),
            ScreenTransition.Challenge => new ChallengeScreen(_game,
                (ChallengeScreenTransitionArguments)screenTransitionArguments),
            ScreenTransition.Bestiary => new BestiaryScreen(_game,
                (BestiaryScreenTransitionArguments)screenTransitionArguments),
            ScreenTransition.Credits => new CreditsScreen(_game),
            ScreenTransition.EndGame => new EndGameScreen(_game),
            ScreenTransition.NotImplemented => new NotImplementedStageScreen(_game,
                (NotImplementedStageScreenTransitionArguments)screenTransitionArguments),
            ScreenTransition.VoiceCombat => new VoiceCombatScreen(_game,
                (VoiceCombatScreenTransitionArguments)screenTransitionArguments),
            ScreenTransition.Demo => new DemoScreen(_game),
            _ => throw new ArgumentException("Unknown transition", nameof(targetTransition))
        };
    }

    private void DrawTransition(SpriteBatch spriteBatch)
    {
        if (_transitionCounter is null)
        {
            return;
        }

        spriteBatch.Begin();

        var t = _transitionCounter.Value / TRANSITION_DURATION;

        if (t < 0.5)
        {
            var t2 = t * 2;
            spriteBatch.Draw(_transitionTexture,
                new Rectangle(
                    0,
                    0,
                    (int)(_game.GraphicsDevice.Viewport.Width * t2),
                    _game.GraphicsDevice.Viewport.Height),
                Color.White);
        }
        else
        {
            var t2 = (t - 0.5) * 2;
            spriteBatch.Draw(_transitionTexture,
                new Rectangle(
                    (int)(_game.GraphicsDevice.Viewport.Width * t2),
                    0,
                    _game.GraphicsDevice.Viewport.Width,
                    _game.GraphicsDevice.Viewport.Height),
                Color.White);
        }

        spriteBatch.End();
    }

    public void ExecuteTransition(IScreen currentScreen, ScreenTransition targetTransition,
        IScreenTransitionArguments args)
    {
        var targetScreen = CreateScreenToTransit(targetTransition, args);
        currentScreen.TargetScreen = targetScreen;
    }
}