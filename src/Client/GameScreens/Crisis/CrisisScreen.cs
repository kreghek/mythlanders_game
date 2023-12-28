using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Client.Assets.Catalogs.Crises;
using Client.Core;
using Client.Engine;
using Client.ScreenManagement;

using CombatDicesTeam.Dices;

using Core.Crises;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Crisis;

internal sealed class CrisisScreen : TextEventScreenBase
{
    private readonly Texture2D _backgroundTexture;
    private readonly Texture2D _cleanScreenTexture;
    private readonly SoundEffectInstance _soundEffectInstance;
    private readonly SoundtrackManager _soundtrackManager;

    public CrisisScreen(TestamentGame game, CrisisScreenTransitionArguments args) : base(game, args)
    {
        var dice = Game.Services.GetRequiredService<IDice>();

        var crisesCatalog = game.Services.GetRequiredService<ICrisesCatalog>();

        var availableCrises = crisesCatalog.GetAll().Where(x => x.EventType == args.EventType).ToArray();
        if (!availableCrises.Any())
        {
            throw new InvalidOperationException($"There are no available micro-events of type {args.EventType}.");
        }

        var crisis = dice.RollFromList(availableCrises);

        var eventCatalog = game.Services.GetRequiredService<IEventCatalog>();
        var smallEvent = eventCatalog.Events.First(x => x.Sid == crisis.EventSid);

        var currentDialogueSid = smallEvent.GetDialogSid();
        var crisisDialogue = eventCatalog.GetDialogue(currentDialogueSid);
       

        var spriteName = GetBackgroundSpriteName(crisis.Sid);

        _backgroundTexture = game.Content.Load<Texture2D>($"Sprites/GameObjects/Crises/{spriteName}");

        _cleanScreenTexture = CreateTexture(game.GraphicsDevice, 1, 1, _ => new Color(36, 40, 41));

        var effectName = GetBackgroundEffectName(crisis.Sid);
        _soundEffectInstance = game.Content.Load<SoundEffect>($"Audio/Stories/{effectName}").CreateInstance();

        _soundtrackManager = game.Services.GetRequiredService<SoundtrackManager>();
    }

    protected override IList<ButtonBase> CreateMenu()
    {
        return ArraySegment<ButtonBase>.Empty;
    }

    protected override void DrawSpecificBackgroundScreenContent(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        spriteBatch.Draw(_cleanScreenTexture, contentRect, Color.White);

        spriteBatch.Draw(_backgroundTexture, new Vector2(-256, 0), Color.White);

        spriteBatch.Draw(_cleanScreenTexture,
            new Rectangle(contentRect.Center.X, contentRect.Top, contentRect.Width / 2, contentRect.Height),
            Color.Lerp(Color.White, Color.Transparent, 0.25f));
    }

    protected override void DrawSpecificForegroundScreenContent(SpriteBatch spriteBatch, Rectangle contentRect)
    {
        
    }

    protected override void InitializeContent()
    {
        _soundtrackManager.PlaySilence();
        _soundEffectInstance.Play();
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
            "Treasures" => "Treasures",

            _ => "ElectricTrap"
        };
    }
}