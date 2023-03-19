using System;
using System.Collections.Generic;
using System.Linq;

using Client;
using Client.Engine;
using Client.GameScreens.Common;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rpg.Client.Engine;

namespace Rpg.Client.ScreenManagement;

internal abstract class GameScreenWithMenuBase : GameScreenBase
{
    private readonly ResolutionIndependentRenderer _resolutionIndependentRenderer;
    private readonly SettingsModal _settingsModal;

    private KeyboardState _lastKeyboardState;
    private IList<ButtonBase>? _menuButtons;

    private bool _menuCreated;

    protected GameScreenWithMenuBase(TestamentGame game) : base(game)
    {
        var uiContentStorage = game.Services.GetService<IUiContentStorage>();
        _resolutionIndependentRenderer = Game.Services.GetService<ResolutionIndependentRenderer>();

        _settingsModal = new SettingsModal(uiContentStorage, _resolutionIndependentRenderer, Game, this);
        AddModal(_settingsModal, isLate: true);

        CreateSettingsButton = true;
    }

    public bool CreateSettingsButton { get; set; }

    protected abstract IList<ButtonBase> CreateMenu();

    protected override void DrawContent(SpriteBatch spriteBatch)
    {
        const int MENU_HEIGHT = 20;
        if (_menuButtons is not null && _menuButtons.Any())
        {
            var menuRect = new Rectangle(ResolutionIndependentRenderer.VirtualBounds.Location,
                new Point(ResolutionIndependentRenderer.VirtualBounds.Width, MENU_HEIGHT));

            var contentRect = new Rectangle(ResolutionIndependentRenderer.VirtualBounds.Location.X,
                ResolutionIndependentRenderer.VirtualBounds.Location.Y + MENU_HEIGHT,
                ResolutionIndependentRenderer.VirtualBounds.Width,
                ResolutionIndependentRenderer.VirtualBounds.Height - MENU_HEIGHT);

            DrawContentWithoutMenu(spriteBatch, contentRect);

            DrawMenu(spriteBatch, menuRect, _menuButtons);
        }
        else
        {
            DrawContentWithoutMenu(spriteBatch, ResolutionIndependentRenderer.VirtualBounds);
        }
    }

    protected abstract void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect);

    protected override void UpdateContent(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();

        if (_lastKeyboardState.IsKeyDown(Keys.F12) && keyboardState.IsKeyUp(Keys.F12))
        {
            DisplaySettingsModal();
        }

        _lastKeyboardState = keyboardState;

        if (!_menuCreated)
        {
            _menuButtons = CreateMenu().ToList();

            if (CreateSettingsButton)
            {
                var settingsButton = new ResourceTextButton(nameof(UiResource.SettingsButtonTitle));
                settingsButton.OnClick += SettingsButton_OnClick;
                _menuButtons.Add(settingsButton);
            }

            _menuCreated = true;
        }
        else
        {
            UpdateMenu();
        }
    }

    private void DisplaySettingsModal()
    {
        _settingsModal.Show();
    }

    private void DrawMenu(SpriteBatch spriteBatch, Rectangle menuRect, IList<ButtonBase> menuButtons)
    {
        spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: Camera.GetViewTransformationMatrix());

        for (var index = 0; index < menuButtons.Count; index++)
        {
            var menuButton = menuButtons[index];

            menuButton.Rect = new Rectangle((5 + 100) * index, 0, 100, menuRect.Height);

            menuButton.Draw(spriteBatch);
        }

        spriteBatch.End();
    }

    private void SettingsButton_OnClick(object? sender, EventArgs e)
    {
        DisplaySettingsModal();
    }

    private void UpdateMenu()
    {
        if (_menuButtons is null || !_menuButtons.Any())
        {
            return;
        }

        foreach (var menuButton in _menuButtons)
        {
            menuButton.Update(_resolutionIndependentRenderer);
        }
    }
}