using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.PreHistory;

internal interface IPreHistoryBackground
{
    /// <summary>
    /// </summary>
    /// <param name="spriteBatch"></param>
    /// <param name="contentRect"></param>
    /// <param name="transition">0..1</param>
    void Draw(SpriteBatch spriteBatch, Rectangle contentRect, double transition);

    void HoverOption(int? index);

    void SelectOption(int index);
    void Update(GameTime gameTime, bool isInteractive);
}