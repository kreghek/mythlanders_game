using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rpg.Client.Engine;

namespace Rpg.Client.Models.Common
{
    internal class TutorialModal : ModalDialogBase
    {
        private readonly TutorialPageDrawerBase _pageDrawer;

        public TutorialModal(TutorialPageDrawerBase pageDrawer, IUiContentStorage uiContentStorage,
            GraphicsDevice graphicsDevice) : base(uiContentStorage, graphicsDevice)
        {
            _pageDrawer = pageDrawer;
        }


        protected override void DrawContent(SpriteBatch spriteBatch)
        {
            _pageDrawer.Draw(spriteBatch, ContentRect);
        }
    }
}