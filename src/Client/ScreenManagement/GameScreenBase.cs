using System.Collections.Generic;
using System.Linq;

using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.ScreenManagement;

/// <summary>
/// This is the base class for all game scenes.
/// </summary>
internal abstract class GameScreenBase : IScreen
{
    private readonly IList<IModalWindow> _modals;

    private bool _isInitialized;

    protected GameScreenBase(TestamentGame game)
    {
        Game = game;

        ScreenManager = game.Services.GetService<IScreenManager>();

        Camera = Game.Services.GetService<ICamera2DAdapter>();
        ResolutionIndependentRenderer = Game.Services.GetService<IResolutionIndependentRenderer>();

        _modals = new List<IModalWindow>();
    }

    protected TestamentGame Game { get; }
    protected IScreenManager ScreenManager { get; }
    protected ICamera2DAdapter Camera { get; }
    protected IResolutionIndependentRenderer ResolutionIndependentRenderer { get; }

    protected void AddModal(IModalWindow modal, bool isLate)
    {
        _modals.Add(modal);
        if (!isLate)
        {
            modal.Show();
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (_isInitialized)
        {
            DrawContent(spriteBatch);
        }

        DrawModals(spriteBatch);
    }

    protected abstract void DrawContent(SpriteBatch spriteBatch);

    protected abstract void InitializeContent();

    protected abstract void UpdateContent(GameTime gameTime);

    private void UpdateModals(GameTime gameTime)
    {
        foreach (var modal in _modals)
        {
            if (modal.IsVisible)
            {
                modal.Update(gameTime, ResolutionIndependentRenderer);
                break;
            }
        }
    }

    private void DrawModals(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: Camera.GetViewTransformationMatrix());

        foreach (var modal in _modals)
        {
            if (modal.IsVisible)
            {
                modal.Draw(spriteBatch);
                break;
            }
        }

        spriteBatch.End();
    }

    public IScreen? TargetScreen { get; set; }

    public void Update(GameTime gameTime)
    {
        if (!_modals.Any(x => x.IsVisible))
        {
            if (_isInitialized)
            {
                UpdateContent(gameTime);
            }
            else
            {
                InitializeContent();
                _isInitialized = true;
            }
        }

        UpdateModals(gameTime);
    }
}