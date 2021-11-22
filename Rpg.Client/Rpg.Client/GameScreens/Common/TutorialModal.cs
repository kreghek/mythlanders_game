using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.GameScreens.Common
{
    internal class TutorialModal : ModalDialogBase
    {
        private readonly TutorialPageDrawerBase _pageDrawer;

        public TutorialModal(TutorialPageDrawerBase pageDrawer, IUiContentStorage uiContentStorage,
            ResolutionIndependentRenderer resolutionIndependentRenderer) : base(uiContentStorage,
            resolutionIndependentRenderer)
        {
            _pageDrawer = pageDrawer;
        }


        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            _pageDrawer.Draw(spriteBatch, ContentRect);
        }
    }
}