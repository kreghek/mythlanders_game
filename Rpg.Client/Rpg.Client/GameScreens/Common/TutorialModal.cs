using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Core;
using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Common
{
    internal class TutorialModal : ModalDialogBase
    {
        private readonly TutorialPageDrawerBase _pageDrawer;
        private readonly ResourceTextButton _skipTutorialButton;

        public TutorialModal(TutorialPageDrawerBase pageDrawer, IUiContentStorage uiContentStorage,
            ResolutionIndependentRenderer resolutionIndependentRenderer, Player player) : base(uiContentStorage,
            resolutionIndependentRenderer)
        {
            _pageDrawer = pageDrawer;

            _skipTutorialButton = new ResourceTextButton(UiResource.SkipTutorialButtonTitle, uiContentStorage.GetButtonTexture(), uiContentStorage.GetMainFont());
            _skipTutorialButton.OnClick += (_, _) =>
            {
                player.SkipTutorial = true;
                Close();
            };
        }


        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            var pageRect = new Rectangle(ContentRect.Location, ContentRect.Size - new Point(0, 20));
            _pageDrawer.Draw(spriteBatch, pageRect);

            _skipTutorialButton.Rect = new Rectangle(ContentRect.X, pageRect.Bottom, 100, 20);
            _skipTutorialButton.Draw(spriteBatch);
        }

        protected override void UpdateContent(GameTime gameTime, ResolutionIndependentRenderer? resolutionIndependenceRenderer = null)
        {
            base.UpdateContent(gameTime, resolutionIndependenceRenderer);

            _skipTutorialButton.Update(resolutionIndependenceRenderer);
        }
    }
}