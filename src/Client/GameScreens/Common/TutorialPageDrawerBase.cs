using Client.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.GameScreens.Common;

internal abstract class TutorialPageDrawerBase
{
    protected TutorialPageDrawerBase(IUiContentStorage uiContentStorage)
    {
        UiContentStorage = uiContentStorage;
    }

    internal IUiContentStorage UiContentStorage { get; }

    public abstract void Draw(SpriteBatch spriteBatch, Rectangle contentRect);
}