using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets;
using Client.Assets.Crises;
using Client.Core;
using Client.Core.Campaigns;
using Client.Engine;
using Client.GameScreens.Campaign;
using Client.GameScreens.Crisis.Ui;
using Client.GameScreens.Rest.Ui;
using Client.ScreenManagement;

using CombatDicesTeam.Dices;

using Core.Crises;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Client.GameScreens.Crisis;

internal sealed class CrisisScreen : GameScreenWithMenuBase
{
    private readonly IList<CrisisAftermathButton> _aftermathButtons;
    private readonly Texture2D _backgroundTexture;
    private readonly HeroCampaign _campaign;
    private readonly GlobeProvider _globeProvider;
    private readonly Texture2D _cleanScreenTexture;
    private readonly ICrisis _crisis;
    private readonly SoundEffectInstance _soundEffectInstance;
    private readonly SoundtrackManager _soundtrackManager;
    private readonly IUiContentStorage _uiContentStorage;

    private TextHint? _aftermathHint;
    private ControlBase? _aftermathOnHover;

    public CrisisScreen(TestamentGame game, CrisisScreenTransitionArguments args) : base(game)
    {
        _campaign = args.Campaign;
        _globeProvider= Game.Services.GetRequiredService<GlobeProvider>();

        _uiContentStorage = Game.Services.GetRequiredService<IUiContentStorage>();
        var dice = Game.Services.GetRequiredService<IDice>();

        var crisesCatalog = game.Services.GetRequiredService<ICrisesCatalog>();

        _crisis = dice.RollFromList(crisesCatalog.GetAll().Where(x => x.EventType == args.EventType).ToArray());

        _aftermathButtons = new List<CrisisAftermathButton>();

        var spriteName = GetBackgroundSpriteName(_crisis.Sid);

        _backgroundTexture = game.Content.Load<Texture2D>($"Sprites/GameObjects/Crises/{spriteName}");

        _cleanScreenTexture = CreateTexture(game.GraphicsDevice, 1, 1, _ => new Color(36, 40, 41));

        var effectName = GetBackgroundEffectName(_crisis.Sid);
        _soundEffectInstance = game.Content.Load<SoundEffect>($"Audio/Stories/{effectName}").CreateInstance();

        _soundtrackManager = game.Services.GetRequiredService<SoundtrackManager>();
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

        spriteBatch.Draw(_cleanScreenTexture,
            new Rectangle(contentRect.Center.X, contentRect.Top, contentRect.Width / 2, contentRect.Height),
            Color.Lerp(Color.White, Color.Transparent, 0.25f));

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
                HEADER_HEIGHT + buttonIndex * ACTION_BUTTON_HEIGHT + contentRect.Top + (int)descriptionTextSize.Y +
                ControlBase.CONTENT_MARGIN,
                actionButtonWidth,
                ACTION_BUTTON_HEIGHT
            );

            actionButton.Draw(spriteBatch);
        }

        if (_aftermathHint is not null && _aftermathOnHover is not null)
        {
            var textSize = _uiContentStorage.GetMainFont().MeasureString(_aftermathHint.Text);
            _aftermathHint.Rect = new Rectangle(
                Mouse.GetState().X + 5,
                Mouse.GetState().Y + 5,
                (int)textSize.X + ControlBase.CONTENT_MARGIN * 2 + 5, // this is from text hint draw method
                (int)textSize.Y + ControlBase.CONTENT_MARGIN * 2 + 15);

            _aftermathHint.Draw(spriteBatch);
        }

        spriteBatch.End();
    }

    protected override void InitializeContent()
    {
        var context = new CrisisAftermathContext(_globeProvider.Globe.Player);

        var aftermaths = _crisis.GetItems().ToArray();
        for (var buttonIndex = 0; buttonIndex < aftermaths.Length; buttonIndex++)
        {
            var aftermath = aftermaths[buttonIndex];
            var aftermathButton = new CrisisAftermathButton(buttonIndex + 1, aftermath.Sid);
            _aftermathButtons.Add(aftermathButton);

            aftermathButton.OnClick += (s, e) =>
            {
                aftermath.Apply(context);

                _soundEffectInstance.Stop();
                ScreenManager.ExecuteTransition(this, ScreenTransition.Campaign,
                    new CampaignScreenTransitionArguments(_campaign));
            };

            aftermathButton.OnHover += (s, e) =>
            {
                var hintText = GameObjectResources.ResourceManager.GetString($"{aftermath.Sid.ResourceName}_Hint");

                if (hintText is not null)
                {
                    var normalizedText = StringHelper.LineBreaking(hintText, 60);
                    _aftermathHint = new TextHint(normalizedText);
                    _aftermathOnHover = (ControlBase?)s;
                }
            };

            aftermathButton.OnLeave += (s, e) =>
            {
                _aftermathHint = null;
                _aftermathOnHover = null;
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

    private static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Func<int, Color> paint)
    {
        //initialize a texture
        var texture = new Texture2D(device, width, height);

        //the array holds the color for each pixel in the texture
        var data = new Color[width * height];
        for (var pixel = 0; pixel < data.Count(); pixel++)
        {
            //the function applies the color according to the specified pixel
            data[pixel] = paint(pixel);
        }

        //set the color
        texture.SetData(data);

        return texture;
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
            _ => "Starvation"
        };
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
            "Bandits" => "Bandits",
            "DesertStorm" => "DesertStorm",
            "FireCaster" => "FireCaster",

            "Cultists" => "Cultists",
            "Drone" => "Drone",
            "Tavern" => "Tavern",
            "Treasues" => "Treasures",

            _ => "ElectricTrap"
        };
    }
}