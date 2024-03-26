using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.PreHistory;

interface IPreHistoryBackground
{
    void Update(GameTime gameTime, bool isInteractive);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="spriteBatch"></param>
    /// <param name="contentRect"></param>
    /// <param name="transition">0..1</param>
    void Draw(SpriteBatch spriteBatch, Rectangle contentRect, double transition);

    void SelectOption(int index);

    void HoverOption(int? index);
}