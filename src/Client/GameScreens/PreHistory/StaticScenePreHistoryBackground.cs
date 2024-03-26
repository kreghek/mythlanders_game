using Client.Engine;

using CombatDicesTeam.Dices;
using CombatDicesTeam.Engine.Ui;

using GameClient.Engine.RectControl;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.PreHistory;

internal sealed class StaticScenePreHistoryBackground: IPreHistoryBackground
{
    private readonly Texture2D? _texture;
    
    private readonly PongRectangleControl _pongBackground;
    
    public StaticScenePreHistoryBackground(Texture2D texture, Rectangle screenRectangle)
    {
        _texture = texture;
        
        var contentRect = screenRectangle;

        var mapRect = new Rectangle(
            contentRect.Left + ControlBase.CONTENT_MARGIN,
            (contentRect.Top + (contentRect.Height / 8)) + ControlBase.CONTENT_MARGIN,
            contentRect.Width - ControlBase.CONTENT_MARGIN * 2,
            (contentRect.Height / 2) - ControlBase.CONTENT_MARGIN * 2);

        var mapPongRandomSource = new PongRectangleRandomSource(new LinearDice(), 3f);

        _pongBackground = new PongRectangleControl(new Point(texture.Width, texture.Height),
            mapRect,
            mapPongRandomSource);
    }
    
    public void Update(GameTime gameTime, bool isInteractive)
    {
        _pongBackground.Update(gameTime.ElapsedGameTime.TotalSeconds);
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle contentRect, double transition)
    {
        spriteBatch.Draw(_texture,
            _pongBackground.GetRects()[0],
            Color.Lerp(Color.White, Color.Transparent, (float)transition));
    }

    public void SelectOption(int index)
    {
        
    }

    public void HoverOption(int? index)
    {
        
    }
}