using System;
using System.Collections.Generic;

using Client.Engine;
using Client.ScreenManagement;

using Core.Crises;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Crisis;

internal sealed class CrisisScreen : CampaignTextEventScreenBase
{
    private readonly Texture2D _backgroundTexture;
    private readonly Texture2D _cleanScreenTexture;
    private readonly SoundtrackManager _soundtrackManager;

    public CrisisScreen(MythlandersGame game, CrisisScreenTransitionArguments args) : base(game, args)
    {
        var spriteName = GetBackgroundSpriteName(args.Crisis.Sid);

        _backgroundTexture = game.Content.Load<Texture2D>($"Sprites/GameObjects/Crises/{spriteName}");

        _cleanScreenTexture = CreateTexture(game.GraphicsDevice, 1, 1, _ => new Color(36, 40, 41));

        _soundtrackManager = game.Services.GetRequiredService<SoundtrackManager>();
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        return ArraySegment<ButtonBase>.Empty;
    }

    protected override void DrawSpecificBackgroundScreenContent(SpriteBatch spriteBatch, Rectangle contentRect)
    {
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

        spriteBatch.End();
    }

    protected override void DrawSpecificForegroundScreenContent(SpriteBatch spriteBatch, Rectangle contentRect)
    {
    }

    protected override void InitializeContent()
    {
        _soundtrackManager.PlaySilence();
    }

    protected override void UpdateSpecificScreenContent(GameTime gameTime)
    {
    }

    private static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Func<int, Color> paint)
    {
        //initialize a texture
        var texture = new Texture2D(device, width, height);

        //the array holds the color for each pixel in the texture
        var data = new Color[width * height];
        for (var pixel = 0; pixel < data.Length; pixel++)
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
            "Bandits" => "Bandits",
            "DesertStorm" => "DesertStorm",
            "FireCaster" => "FireCaster",

            "Cultists" => "Cultists",
            "Drone" => "Drone",
            "Tavern" => "Tavern",
            "Treasures" => "Treasures",

            _ => "ElectricTrap"
        };
    }
}