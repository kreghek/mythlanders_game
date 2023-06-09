﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

using Client.Core;
using Client.Engine;
using Client.ScreenManagement;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common;

internal sealed class SettingsModal : ModalDialogBase
{
    private const int BUTTON_HEIGHT = 20;

    private const int BUTTON_WIDTH = 100;

    private readonly IList<ButtonBase> _buttons;
    private readonly IScreen _currentScreen;
    private readonly Game _game;
    private readonly GameSettings _gameSettings;
    private readonly GlobeProvider _globeProvider;
    private readonly IResolutionIndependentRenderer _resolutionIndependentRenderer;

    public SettingsModal(IUiContentStorage uiContentStorage,
        IResolutionIndependentRenderer resolutionIndependentRenderer, Game game,
        IScreen currentScreen, bool isGameStarted = true) : base(uiContentStorage,
        resolutionIndependentRenderer)
    {
        _resolutionIndependentRenderer = resolutionIndependentRenderer;
        _game = game;
        _currentScreen = currentScreen;

        _gameSettings = game.Services.GetService<GameSettings>();

        _buttons = new List<ButtonBase>
        {
            CreateSwitchFullScreenButton(),
            CreateSwitchResolutionButton(),
            CreateSwitchMusicButton(),
            CreateSwitchLanguageButton()
        };

        if (_gameSettings.Mode == GameMode.Demo)
        {
            if (isGameStarted)
            {
                // Fast restart available only in the demo game.
                var fastRestartButton = CreateFastRestartButton();
                _buttons.Add(fastRestartButton);
            }
        }

        _globeProvider = game.Services.GetService<GlobeProvider>();

        if (isGameStarted)
        {
            var exitGameButton = new ResourceTextButton(nameof(UiResource.ExitGameButtonTitle));
            exitGameButton.OnClick += (_, _) =>
            {
                game.Exit();
            };
            _buttons.Add(exitGameButton);
        }
    }

    protected override ModalTopSymbol? TopSymbol => ModalTopSymbol.Gears;


    protected override void DrawContent(SpriteBatch spriteBatch)
    {
        var index = 0;
        foreach (var button in _buttons)
        {
            button.Rect = new Rectangle(
                _resolutionIndependentRenderer.VirtualBounds.Center.X - BUTTON_WIDTH / 2,
                150 + index * 30,
                BUTTON_WIDTH,
                BUTTON_HEIGHT);
            button.Draw(spriteBatch);

            index++;
        }
    }

    protected override void UpdateContent(GameTime gameTime,
        IResolutionIndependentRenderer resolutionIndependenceRenderer)
    {
        base.UpdateContent(gameTime, resolutionIndependenceRenderer);

        foreach (var button in _buttons)
        {
            button.Update(_resolutionIndependentRenderer);
        }
    }

    private ButtonBase CreateFastRestartButton()
    {
        var fastRestartButton = new ResourceTextButton(nameof(UiResource.RestartButtonTitle));
        fastRestartButton.OnClick += FastRestartButton_OnClick;

        return fastRestartButton;
    }

    private ButtonBase CreateSwitchFullScreenButton()
    {
        var switchFullScreenButton = new ResourceTextButton(nameof(UiResource.SwitchFullScreenButtonTitle));
        switchFullScreenButton.OnClick += SwitchToFullScreenButton_OnClick;

        return switchFullScreenButton;
    }

    private static ButtonBase CreateSwitchLanguageButton()
    {
        var switchLanguageButton = new ResourceTextButton(nameof(UiResource.SwitchLanguageButtonTitle));
        switchLanguageButton.OnClick += SwitchLanguageButton_OnClick;

        return switchLanguageButton;
    }

    private ButtonBase CreateSwitchMusicButton()
    {
        var button = new ResourceTextButton(nameof(UiResource.SwitchMusicButtonTitle));
        button.OnClick += SwitchMusicButton_OnClick;

        return button;
    }

    private ButtonBase CreateSwitchResolutionButton()
    {
        var button = new ResourceTextButton(nameof(UiResource.SwitchResolutionButtonTitle));
        button.OnClick += SwitchResolutionButton_OnClick;

        return button;
    }

    private void FastRestartButton_OnClick(object? sender, EventArgs e)
    {
        _globeProvider.Globe.Player.ClearAbilities();
        _globeProvider.Globe.Player.ClearInventory();

        var screenManager = _game.Services.GetService<IScreenManager>();
        screenManager.ExecuteTransition(_currentScreen, ScreenTransition.Title, null);
    }

    private void InitializeResolutionIndependence()
    {
        _resolutionIndependentRenderer.Initialize();
    }

    private static void SwitchLanguageButton_OnClick(object? sender, EventArgs e)
    {
        var currentLanguage = Thread.CurrentThread.CurrentUICulture;
        if (string.Equals(
                currentLanguage.TwoLetterISOLanguageName,
                "en",
                StringComparison.InvariantCultureIgnoreCase))
        {
            var newCulture = CultureInfo.GetCultureInfo("zh");
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;
        }
        else if (string.Equals(
                     currentLanguage.TwoLetterISOLanguageName,
                     "zh",
                     StringComparison.InvariantCultureIgnoreCase))
        {
            var newCulture = CultureInfo.GetCultureInfo("ru-RU");
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;
        }
        else
        {
            var newCulture = CultureInfo.GetCultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;
        }
    }

    private void SwitchMusicButton_OnClick(object? sender, EventArgs e)
    {
        if ((int)_gameSettings.MusicVolume == 1)
        {
            _gameSettings.MusicVolume = 0;
        }
        else
        {
            _gameSettings.MusicVolume = 1;
        }
    }

    private void SwitchResolutionButton_OnClick(object? sender, EventArgs e)
    {
        var graphicsManager = _game.Services.GetService<GraphicsDeviceManager>();

        var width = _game.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
        var height = _game.GraphicsDevice.Adapter.CurrentDisplayMode.Height;

        InitializeResolutionIndependence();

        graphicsManager.PreferredBackBufferWidth = width;
        graphicsManager.PreferredBackBufferHeight = height;

        graphicsManager.ApplyChanges();
    }

    private void SwitchToFullScreenButton_OnClick(object? sender, EventArgs e)
    {
        var graphicsManager = _game.Services.GetService<GraphicsDeviceManager>();
        graphicsManager.IsFullScreen = !graphicsManager.IsFullScreen;
        if (!graphicsManager.IsFullScreen)
        {
            var width = 848;
            var height = 480;

            InitializeResolutionIndependence();

            graphicsManager.PreferredBackBufferWidth = width;
            graphicsManager.PreferredBackBufferHeight = height;
        }
        else
        {
            var width = _game.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            var height = _game.GraphicsDevice.Adapter.CurrentDisplayMode.Height;

            InitializeResolutionIndependence();

            graphicsManager.PreferredBackBufferWidth = width;
            graphicsManager.PreferredBackBufferHeight = height;
        }

        graphicsManager.ApplyChanges();
    }
}