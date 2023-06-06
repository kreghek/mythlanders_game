using System;
using System.Collections.Generic;

using Client.Core;
using Client.Core.Campaigns;
using Client.GameScreens.Combat.GameObjects.Background;
using Client.ScreenManagement;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.VoiceCombat;

internal abstract class CombatScreenBase : GameScreenWithMenuBase
{
    private const int BACKGROUND_LAYERS_COUNT = 3;
    private const float BACKGROUND_LAYERS_SPEED = 0.1f;

    /// <summary>
    /// Event screen has no background parallax.
    /// </summary>
    private const float BG_CENTER_OFFSET_PERCENTAGE = 0;

    protected const int SPEAKER_FRAME_SIZE = 256;

    private readonly Texture2D _backgroundTexture;

    private readonly HeroCampaign _campaign;
    private readonly IReadOnlyList<IBackgroundObject> _cloudLayerObjects;
    private readonly IReadOnlyList<IBackgroundObject> _foregroundLayerObjects;
    private readonly GameObjectContentStorage _gameObjectContentStorage;

    protected CombatScreenBase(TestamentGame game, HeroCampaign campaign) : base(game)
    {
        _campaign = campaign;

        _gameObjectContentStorage = game.Services.GetService<GameObjectContentStorage>();

        var bgofSelector = Game.Services.GetService<BackgroundObjectFactorySelector>();

        var backgroundObjectFactory = bgofSelector.GetBackgroundObjectFactory(campaign.Location);

        _cloudLayerObjects = backgroundObjectFactory.CreateCloudLayerObjects();
        _foregroundLayerObjects = backgroundObjectFactory.CreateForegroundLayerObjects();

        var data = new[] { TestamentColors.MaxDark };
        _backgroundTexture = new Texture2D(game.GraphicsDevice, 1, 1);
        _backgroundTexture.SetData(data);
    }

    protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        DrawGameObjects(spriteBatch);

        DrawHud(spriteBatch, contentRect);
    }

    protected abstract void DrawHud(SpriteBatch spriteBatch, Rectangle contentRect);

    protected void DrawSpeakerPortrait(SpriteBatch spriteBatch, UnitName speakerSid, Rectangle contentRect,
        CombatantPositionSide side)
    {
        if (speakerSid == UnitName.Environment)
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

        var col = GetPortraitSheet().X;
        var row = GetPortraitSheet().Y;
        // var col = _frameIndex % 2;
        // var row = _frameIndex / 2;

        var targetRect = GetTargetRect(contentRect, side);

        spriteBatch.Draw(_gameObjectContentStorage.GetCharacterFaceTexture(speakerSid),
            targetRect,
            new Rectangle(
                col * SPEAKER_FRAME_SIZE,
                row * SPEAKER_FRAME_SIZE,
                SPEAKER_FRAME_SIZE,
                SPEAKER_FRAME_SIZE),
            Color.White,
            rotation: 0,
            origin: Vector2.Zero,
            effects: side == CombatantPositionSide.Left ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
            layerDepth: 0);

        spriteBatch.End();
    }

    protected virtual Point GetPortraitSheet()
    {
        return new(0, 0);
    }

    protected override void UpdateContent(GameTime gameTime)
    {
        base.UpdateContent(gameTime);

        UpdateBackgroundObjects(gameTime);
    }

    private void DrawBackgroundLayers(SpriteBatch spriteBatch, Texture2D[] backgrounds, int backgroundStartOffset,
        int backgroundMaxOffset)
    {
        for (var i = 0; i < BACKGROUND_LAYERS_COUNT; i++)
        {
            var xFloat = backgroundStartOffset + BG_CENTER_OFFSET_PERCENTAGE * (BACKGROUND_LAYERS_COUNT - i - 1) *
                BACKGROUND_LAYERS_SPEED * backgroundMaxOffset;
            var roundedX = (int)Math.Round(xFloat);
            var position = new Vector2(roundedX, 0);

            //TODO Repair parallax
            var position3d = new Vector3(position, 0);

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

    private void DrawForegroundLayers(SpriteBatch spriteBatch, Texture2D[] backgrounds, int backgroundStartOffset,
        int backgroundMaxOffset)
    {
        var xFloat = backgroundStartOffset +
                     -1 * BG_CENTER_OFFSET_PERCENTAGE * BACKGROUND_LAYERS_SPEED * 2 * backgroundMaxOffset;
        var roundedX = (int)Math.Round(xFloat);

        var position = new Vector2(roundedX, 0);

        // TODO Restore parallax
        var position3d = new Vector3(position, 0);

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
        var theme = LocationHelper.GetLocationTheme(_campaign.Location);

        var backgrounds = _gameObjectContentStorage.GetCombatBackgrounds(theme);

        const int BG_START_OFFSET = -100;
        const int BG_MAX_OFFSET = 200;

        DrawBackgroundLayers(spriteBatch, backgrounds, BG_START_OFFSET, BG_MAX_OFFSET);

        DrawForegroundLayers(spriteBatch, backgrounds, BG_START_OFFSET, BG_MAX_OFFSET);

        spriteBatch.Begin(sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: Camera.GetViewTransformationMatrix());
        spriteBatch.Draw(_backgroundTexture, ResolutionIndependentRenderer.VirtualBounds,
            Color.Lerp(Color.Transparent, Color.White, 0.5f));
        spriteBatch.End();
    }

    private static Rectangle GetTargetRect(Rectangle contentRect, CombatantPositionSide side)
    {
        var xPosition = side == CombatantPositionSide.Left ? 0 : contentRect.Right - SPEAKER_FRAME_SIZE;

        return new Rectangle(xPosition, contentRect.Bottom - SPEAKER_FRAME_SIZE, SPEAKER_FRAME_SIZE,
            SPEAKER_FRAME_SIZE);
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

    protected enum CombatantPositionSide
    {
        Left,
        Right
    }
}