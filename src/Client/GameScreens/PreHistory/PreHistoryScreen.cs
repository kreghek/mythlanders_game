using System;
using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;
using Client.Core;
using Client.Engine;
using Client.ScreenManagement;
using Client.ScreenManagement.Ui.TextEvents;

using CombatDicesTeam.Dialogues;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.PreHistory;

internal sealed class PreHistoryScreen : TextEventScreenBase<ParagraphConditionContext, PreHistoryAftermathContext>
{
    private readonly Texture2D _cleanScreenTexture;
    private readonly SoundtrackManager _soundtrackManager;
    private PreHistoryAftermathContext? _aftermathContext;

    public PreHistoryScreen(MythlandersGame game, PreHistoryScreenScreenTransitionArguments args) : base(game, args)
    {
        _cleanScreenTexture = CreateTexture(game.GraphicsDevice, 1, 1, _ => new Color(36, 40, 41));

        _soundtrackManager = game.Services.GetRequiredService<SoundtrackManager>();
    }

    protected override IDialogueContextFactory<ParagraphConditionContext, PreHistoryAftermathContext> CreateDialogueContextFactory(TextEventScreenArgsBase<ParagraphConditionContext, PreHistoryAftermathContext> args)
    {
        _aftermathContext = new PreHistoryAftermathContext(Game.Content, Game.Services.GetRequiredService<IDialogueEnvironmentManager>());

        return new PreHistoryDialogueContextFactory(_aftermathContext, Game.Services.GetRequiredService<GlobeProvider>().Globe.Player);
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

        if (_aftermathContext!.GetBackgroundTexture() is not null)
        {
            spriteBatch.Draw(_aftermathContext.GetBackgroundTexture(), new Vector2(-256, 0), Color.White);
        }

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

    protected override void HandleDialogueEnd()
    {
        throw new NotImplementedException();
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
}