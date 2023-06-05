using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Common
{
    internal abstract class TutorialPageDrawerBase
    {
        protected TutorialPageDrawerBase(IUiContentStorage uiContentStorage)
        {
            UiContentStorage = uiContentStorage;
        }

        internal IUiContentStorage UiContentStorage { get; }

        public abstract void Draw(SpriteBatch spriteBatch, Rectangle contentRect);

        protected static string GetTutorialText(string resourceName)
        {
            var tutorialText =
                StringHelper.LineBreaking(UiResource.ResourceManager.GetString(resourceName) ?? $"#{resourceName}", 65);
            return tutorialText;
        }
    }
}