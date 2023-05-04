using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core;
using Client.Core.Campaigns;
using Client.Core.Dialogues;
using Client.Engine;
using Client.GameScreens.Campaign;
using Client.GameScreens.TextDialogue.Ui;

using Core.Dices;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.GameScreens.Combat.GameObjects.Background;
using Rpg.Client.GameScreens.Speech.Ui;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.VoiceCombat;

internal sealed record VoiceCombatScreenTransitionArguments(HeroCampaign Campaign) : IScreenTransitionArguments;

internal abstract class DialogueScreenBase : GameScreenWithMenuBase
{
    private const int BACKGROUND_LAYERS_COUNT = 3;
    private const float BACKGROUND_LAYERS_SPEED = 0.1f;

    /// <summary>
    /// Event screen has no background parallax.
    /// </summary>
    private const float BG_CENTER_OFFSET_PERCENTAGE = 0;

    private readonly HeroCampaign _campaign;
    private readonly GameObjectContentStorage _gameObjectContentStorage;
    private readonly IReadOnlyList<IBackgroundObject> _cloudLayerObjects;
    private readonly IReadOnlyList<IBackgroundObject> _foregroundLayerObjects;

    private readonly Texture2D _backgroundTexture;

    protected DialogueScreenBase(TestamentGame game, HeroCampaign campaign) : base(game)
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

    private void DrawGameObjects(SpriteBatch spriteBatch)
    {
        var backgroundType = BackgroundHelper.GetBackgroundType(_campaign.Location);

        var backgrounds = _gameObjectContentStorage.GetCombatBackgrounds(backgroundType);

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

    private void DrawForegroundLayers(SpriteBatch spriteBatch, Texture2D[] backgrounds, int backgroundStartOffset,
        int backgroundMaxOffset)
    {
        var xFloat = backgroundStartOffset +
                     -1 * BG_CENTER_OFFSET_PERCENTAGE * BACKGROUND_LAYERS_SPEED * 2 * backgroundMaxOffset;
        var roundedX = (int)Math.Round(xFloat);

        var position = new Vector2(roundedX, 0);

        var position3d = new Vector3(position, 0);

        var worldTransformationMatrix = Camera.GetViewTransformationMatrix();
        worldTransformationMatrix.Decompose(out var scaleVector, out var _, out var translationVector);

        var matrix = Matrix.CreateTranslation(translationVector + position3d)
                     * Matrix.CreateScale(scaleVector);

        spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: matrix);

        spriteBatch.Draw(backgrounds[3], Vector2.Zero, Color.White);

        foreach (var obj in _foregroundLayerObjects)
        {
            obj.Draw(spriteBatch);
        }

        spriteBatch.End();
    }

    protected override void InitializeContent()
    {
        throw new NotImplementedException();
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

            var position3d = new Vector3(position, 0);

            var worldTransformationMatrix = Camera.GetViewTransformationMatrix();
            worldTransformationMatrix.Decompose(out var scaleVector, out _, out var translationVector);

            var matrix = Matrix.CreateTranslation(translationVector + position3d)
                         * Matrix.CreateScale(scaleVector);

            spriteBatch.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.AlphaBlend,
                samplerState: SamplerState.PointClamp,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone,
                transformMatrix: matrix);

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

    protected enum CombatantPositionSide
    {
        Left,
        Right
    }

    protected const int SPEAKER_FRAME_SIZE = 256;

    protected void DrawSpeakerPortrait(SpriteBatch spriteBatch, UnitName speakerSid, Rectangle contentRect, CombatantPositionSide side)
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

        var col = 0;
        var row = 0;
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

    private static Rectangle GetTargetRect(Rectangle contentRect, CombatantPositionSide side)
    {
        var xPosition = side == CombatantPositionSide.Left ? 0 : contentRect.Right - SPEAKER_FRAME_SIZE;

        return new Rectangle(xPosition, contentRect.Bottom - SPEAKER_FRAME_SIZE, SPEAKER_FRAME_SIZE,
                SPEAKER_FRAME_SIZE);
    }
}

internal class VoiceCombatOptionButton : ButtonBase
{
    private const int MARGIN = 5;
    private readonly SpriteFont _font;
    private readonly string _optionText;

    public VoiceCombatOptionButton(int number, string resourceSid)
    {
        _optionText = $"{number}. {resourceSid}";

        _font = UiThemeManager.UiContentStorage.GetTitlesFont();
        Number = number;
    }

    public int Number { get; }

    public Vector2 GetContentSize()
    {
        var textSize = _font.MeasureString(_optionText) + new Vector2(MARGIN * 2, MARGIN * 2);
        return textSize;
    }

    protected override Point CalcTextureOffset()
    {
        if (_buttonState == UiButtonState.Hover || _buttonState == UiButtonState.Pressed)
        {
            return ControlTextures.OptionHover;
        }

        return ControlTextures.OptionNormal;
    }

    protected Color CalculateTextColor()
    {
        if (_buttonState == UiButtonState.Hover || _buttonState == UiButtonState.Pressed)
        {
            return Color.Wheat;
        }

        return Color.SaddleBrown;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color color)
    {
        var textSize = GetContentSize();
        var heightDiff = contentRect.Height - textSize.Y;
        var textPosition = new Vector2(
            contentRect.Left + MARGIN,
            heightDiff / 2 + contentRect.Top);

        var textColor = CalculateTextColor();
        spriteBatch.DrawString(_font, _optionText, textPosition, textColor);
    }
}

internal class VoiceCombatOptions : ControlBase
{
    private const int OPTION_BUTTON_MARGIN = 5;

    public VoiceCombatOptions()
    {
        Options = new List<VoiceCombatOptionButton>();
    }

    public IList<VoiceCombatOptionButton> Options { get; }

    public int GetHeight()
    {
        var sumOptionHeight = Options.Sum(x => CalcOptionButtonSize(x).Y) + OPTION_BUTTON_MARGIN;

        return sumOptionHeight;
    }

    public void SelectOption(int number)
    {
        Options.SingleOrDefault(x => x.Number == number)?.Click();
    }

    public void Update(ResolutionIndependentRenderer resolutionIndependentRenderer)
    {
        foreach (var button in Options)
        {
            button.Update(resolutionIndependentRenderer);
        }
    }

    protected override Point CalcTextureOffset()
    {
        return ControlTextures.PanelBlack;
    }

    protected override Color CalculateColor()
    {
        return Color.White;
    }

    protected override void DrawContent(SpriteBatch spriteBatch, Rectangle contentRect, Color contentColor)
    {
        var lastTopButtonPosition = 0;
        foreach (var button in Options)
        {
            var optionButtonSize = CalcOptionButtonSize(button);
            var optionPosition = new Vector2(OPTION_BUTTON_MARGIN + contentRect.Left,
                lastTopButtonPosition + contentRect.Top).ToPoint();

            button.Rect = new Rectangle(optionPosition, optionButtonSize + new Point(1000, 0));

            button.Draw(spriteBatch);

            lastTopButtonPosition += optionButtonSize.Y;
        }
    }

    private static Point CalcOptionButtonSize(VoiceCombatOptionButton button)
    {
        var contentSize = button.GetContentSize();
        return (contentSize + Vector2.One * CONTENT_MARGIN + Vector2.UnitY * OPTION_BUTTON_MARGIN)
            .ToPoint();
    }
}

internal class VoiceCombatScreen : DialogueScreenBase
{
    private class VoiceCombatant
    {
        public int Hp { get; set; }
        public UnitName Sid { get; }

        public VoiceCombatant(int hp, UnitName sid)
        {
            Hp = hp;
            Sid = sid;
        }
    }

    private readonly HeroCampaign _campaign;

    private readonly VoiceCombatant _leftCombatant;
    private readonly VoiceCombatant _rightCombatant;

    private readonly VoiceCombatOptions _voiceCombatOptions;

    public VoiceCombatScreen(TestamentGame game, VoiceCombatScreenTransitionArguments args) : base(game, args.Campaign)
    {
        _campaign = args.Campaign;

        _leftCombatant = new VoiceCombatant(13, UnitName.Assaulter);
        _rightCombatant = new VoiceCombatant(10, UnitName.ChineseOldman);

        _voiceCombatOptions = new VoiceCombatOptions();

        _heroMoves = new List<VoiceCombatMove> { new VoiceCombatMove("Alert!"), new VoiceCombatMove("TheGoodWeather") };
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        var closeButton = new ResourceTextButton(nameof(UiResource.SkipButtonTitle));
        closeButton.OnClick += CloseButton_OnClick;

        return new[]
        {
            closeButton
        };
    }


    protected override void InitializeContent()
    {
        InitDialogueControls();


    }

    private sealed class VoiceCombatMove {
        public VoiceCombatMove(string textSid)
        {
            TextSid = textSid;
        }

        public string TextSid { get; }
    }

    private readonly IList<VoiceCombatMove> _heroMoves;

    private void InitDialogueControls()
    {

        var optionNumber = 1;
        _voiceCombatOptions.Options.Clear();
        foreach (var move in _heroMoves)
        {
            var optionButton = new VoiceCombatOptionButton(optionNumber, move.TextSid);
            optionButton.OnClick += (_, _) =>
            {
                
            };

            _voiceCombatOptions.Options.Add(optionButton);
            optionNumber++;
        }
    }

    private void CloseButton_OnClick(object? sender, EventArgs e)
    {
        ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign,
            new CampaignScreenTransitionArguments(_campaign));
    }

    protected override void DrawHud(SpriteBatch spriteBatch, Rectangle contentRectangle)
    {
        DrawSpeakerPortrait(spriteBatch, _leftCombatant.Sid, contentRectangle, CombatantPositionSide.Left);
        DrawSpeakerPortrait(spriteBatch, _rightCombatant.Sid, contentRectangle, CombatantPositionSide.Right);

        spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: Camera.GetViewTransformationMatrix());

        _voiceCombatOptions.Rect = new Rectangle(SPEAKER_FRAME_SIZE, contentRectangle.Bottom - _voiceCombatOptions.GetHeight() - 100,
                contentRectangle.Width - SPEAKER_FRAME_SIZE * 2,
                _voiceCombatOptions.GetHeight());
        _voiceCombatOptions.Draw(spriteBatch);

        spriteBatch.End();
    }
}
