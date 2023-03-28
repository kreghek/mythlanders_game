using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Crises;
using Client.Core.Campaigns;
using Client.Engine;
using Client.GameScreens.Campaign;
using Client.GameScreens.Crisis.Ui;
using Client.GameScreens.Rest.Ui;

using Core.Crises;
using Core.Dices;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;
using Rpg.Client.GameScreens;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.Crisis;

internal sealed class CrisisScreen : GameScreenWithMenuBase
{
    private readonly HeroCampaign _campaign;
    private readonly IUiContentStorage _uiContentStorage;
    private readonly ICrisis _crisis;
    private readonly IList<CrisisAftermathButton> _aftermathButtons;
    private readonly Texture2D _backgroundTexture;
    private readonly Texture2D _cleanScreenTexture;
    private readonly SoundEffectInstance _soundEffectInstance;
    private readonly SoundtrackManager _soundtrackManager;

    public CrisisScreen(TestamentGame game, CrisisScreenTransitionArguments args) : base(game)
    {
        _campaign = args.Campaign;

        _uiContentStorage = Game.Services.GetRequiredService<IUiContentStorage>();
        var dice = Game.Services.GetRequiredService<IDice>();

        var crisesCatalog = game.Services.GetRequiredService<ICrisesCatalog>();

        _crisis = dice.RollFromList(crisesCatalog.GetAll().ToArray());

        _aftermathButtons = new List<CrisisAftermathButton>();

        var spriteName = GetBackgroundSpriteName(_crisis.Sid);

        _backgroundTexture = game.Content.Load<Texture2D>($"Sprites/GameObjects/Crises/{spriteName}");

        _cleanScreenTexture = CreateTexture(game.GraphicsDevice, 1, 1, (_) => new Color(36, 40, 41));

        var effectName = GetBackgroundEffectName(_crisis.Sid);
        _soundEffectInstance = game.Content.Load<SoundEffect>($"Audio/Stories/{effectName}").CreateInstance();

        _soundtrackManager = game.Services.GetRequiredService<SoundtrackManager>();
    }

    private static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Func<int, Color> paint)
    {
        //initialize a texture
        Texture2D texture = new Texture2D(device, width, height);

        //the array holds the color for each pixel in the texture
        Color[] data = new Color[width * height];
        for (int pixel = 0; pixel < data.Count(); pixel++)
        {
            //the function applies the color according to the specified pixel
            data[pixel] = paint(pixel);
        }

        //set the color
        texture.SetData(data);

        return texture;
    }

    private static string GetBackgroundSpriteName(CrisisSid sid)
    {
        return sid.Value switch
        {
            "MagicTrap" => "ElectricTrap",
            "CityHunting" => "CityHunting",
            "InfernalSickness" => "InfernalSickness",
            "Starvation" => "Starvation",
            "Preying" => "Preying",
            _ => "ElectricTrap",
        };
    }

    private static string GetBackgroundEffectName(CrisisSid sid)
    {
        return sid.Value switch
        {
            "MagicTrap" => "ElectricDeathRay",
            "CityHunting" => "CityHunting",
            "InfernalSickness" => "InfernalSickness",
            "Starvation" => "Starvation",
            "Preying" => "SkyThunder",
            _ => "Starvation",
        };
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        return ArraySegment<ButtonBase>.Empty;
    }

    protected override void DrawContentWithoutMenu(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        ResolutionIndependentRenderer.BeginDraw();
        spriteBatch.Begin(
            sortMode: SpriteSortMode.Deferred,
            blendState: BlendState.AlphaBlend,
            samplerState: SamplerState.PointClamp,
            depthStencilState: DepthStencilState.None,
            rasterizerState: RasterizerState.CullNone,
            transformMatrix: Camera.GetViewTransformationMatrix());

        spriteBatch.Draw(_cleanScreenTexture, contentRect, Color.White);

        spriteBatch.Draw(_backgroundTexture, new Vector2(-256, 0), Color.White);

        spriteBatch.Draw(_cleanScreenTexture, new Rectangle(contentRect.Center.X, contentRect.Top, contentRect.Width / 2, contentRect.Height), Color.Lerp(Color.White, Color.Transparent, 0.25f));

        var actionButtonWidth = contentRect.Center.X;
        const int ACTION_BUTTON_HEIGHT = 40;

        const int HEADER_HEIGHT = 100;

        var localizedCrisisDescription = GameObjectHelper.GetLocalized(_crisis.Sid);
        var localizedNormalizedCrisisDescription = StringHelper.LineBreaking(localizedCrisisDescription, 40);
        var descriptionText = _uiContentStorage.GetTitlesFont();
        var descriptionTextSize = descriptionText.MeasureString(localizedNormalizedCrisisDescription);

        spriteBatch.DrawString(descriptionText, localizedNormalizedCrisisDescription,
            new Vector2(contentRect.Center.X, contentRect.Top + ControlBase.CONTENT_MARGIN), Color.Wheat);

        for (var buttonIndex = 0; buttonIndex < _aftermathButtons.Count; buttonIndex++)
        {
            var actionButton = _aftermathButtons[buttonIndex];
            actionButton.Rect = new Rectangle(
                contentRect.Center.X,
                HEADER_HEIGHT + buttonIndex * ACTION_BUTTON_HEIGHT + contentRect.Top + (int)descriptionTextSize.Y + ControlBase.CONTENT_MARGIN,
                actionButtonWidth,
                ACTION_BUTTON_HEIGHT
            );

            actionButton.Draw(spriteBatch);
        }

        spriteBatch.End();
    }

    protected override void InitializeContent()
    {
        var context = new CrisisAftermathContext();

        var aftermaths = _crisis.GetItems().ToArray();
        for (var buttonIndex = 0; buttonIndex < aftermaths.Length; buttonIndex++)
        {
            var aftermath = aftermaths[buttonIndex];
            var aftermathButton = new CrisisAftermathButton(buttonIndex + 1, aftermath.Sid);
            _aftermathButtons.Add(aftermathButton);

            aftermathButton.OnClick += (s, e) =>
            {
                var underConstructionModal = new UnderConstructionModal(
                    _uiContentStorage,
                    ResolutionIndependentRenderer);

                aftermath.Apply(context);

                underConstructionModal.Closed += (_, _) =>
                {
                    ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign,
                        new CampaignScreenTransitionArguments(_campaign));
                };

                AddModal(underConstructionModal, false);
                _campaign.CompleteCurrentStage();
            };
        }

        _soundtrackManager.PlaySilence();
        _soundEffectInstance.Play();
    }

    protected override void UpdateContent(GameTime gameTime)
    {
        base.UpdateContent(gameTime);

        foreach (var actionButton in _aftermathButtons)
        {
            actionButton.Update(ResolutionIndependentRenderer);
        }
    }
}